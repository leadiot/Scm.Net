using Com.Scm.Dsa;
using Com.Scm.Exceptions;
using Com.Scm.Jwt;
using Com.Scm.Login.Otp;
using Com.Scm.Otp.Totp;
using Com.Scm.Service;
using Com.Scm.Ur.UserOtp.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Ur.UserOtp
{
    /// <summary>
    /// Otp服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Ur")]
    public class ScmUrUserOtpService : ApiService
    {
        private readonly JwtContextHolder _contextHolder;
        private readonly SugarRepository<UserDao> _thisRepository;
        private readonly OtpConfig _otpConfig;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRepository"></param>
        /// <returns></returns>
        public ScmUrUserOtpService(JwtContextHolder contextHolder,
            SugarRepository<UserDao> userRepository,
            OtpConfig otpConfig)
        {
            _contextHolder = contextHolder;
            _thisRepository = userRepository;
            _otpConfig = otpConfig;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<UserOtpDvo> GetAsync()
        {
            var token = _contextHolder.GetToken();

            var userDao = await _thisRepository.GetByIdAsync(token.user_id);

            var dvo = new UserOtpDvo();
            dvo.status = userDao.otp_status;
            dvo.time = userDao.otp_time;

            return dvo;
        }

        [HttpGet]
        public async Task<UserOtpDvo> GetViewAsync()
        {
            var token = _contextHolder.GetToken();

            var userDao = await _thisRepository.GetByIdAsync(token.user_id);
            return ConvertToDvo(userDao);
        }

        /// <summary>
        /// 更新密钥
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<UserOtpDvo> UpdateAsync()
        {
            var token = _contextHolder.GetToken();

            var userDao = await _thisRepository.GetByIdAsync(token.user_id);
            userDao.GenerateSecret();
            userDao.otp_time = TimeUtils.GetUnixTime();

            await _thisRepository.UpdateAsync(userDao);

            var dvo = new UserOtpDvo();
            dvo.status = userDao.otp_status;
            dvo.time = userDao.otp_time;

            return dvo;
        }

        private UserOtpDvo ConvertToDvo(UserDao userDao)
        {
            var dvo = new UserOtpDvo();
            dvo.issuer = _otpConfig.Issuer;
            dvo.digits = _otpConfig.Digits;
            dvo.algorithm = _otpConfig.Algorithm;
            dvo.codec = userDao.codec;
            dvo.namec = userDao.namec;
            dvo.avatar = userDao.avatar;
            dvo.status = userDao.otp_status;
            dvo.time = userDao.otp_time;

            var bytes = userDao.DecodeSecret();
            if (bytes != null)
            {
                dvo.secret = TextUtils.Base32Encode(bytes);
            }

            var template = _otpConfig.Template ?? "";
            dvo.uri = template.Replace("{issuer}", Uri.UnescapeDataString(_otpConfig.Issuer))
                .Replace("{account}", Uri.UnescapeDataString(userDao.codec))
                .Replace("{secret}", Uri.UnescapeDataString(dvo.secret))
                .Replace("{algorithm}", _otpConfig.Algorithm)
                .Replace("{digits}", _otpConfig.Digits.ToString())
                .Replace("{period}", _otpConfig.Period.ToString());

            return dvo;
        }

        /// <summary>
        /// 口令校验
        /// </summary>
        /// <returns></returns>
        public async Task<bool> VerifyAsync(VerifyRequest request)
        {
            if (request == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(request.code))
            {
                throw new BusinessException("输入口令不能为空！");
            }
            if (!TextUtils.IsNumberic(request.code, 6))
            {
                throw new BusinessException("无效的口令格式！");
            }

            var token = _contextHolder.GetToken();

            var userDao = await _thisRepository.GetByIdAsync(token.user_id);
            var secret = userDao.DecodeSecret();

            var totp = new TotpAuth(null);
            totp.Init(null);
            var result = totp.VerifyCode("0", request.code);
            return result.success;
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="param">逗号分隔</param>
        /// <returns></returns>
        public async Task<UserOtpDvo> StatusAsync(ScmChangeStatusRequest param)
        {
            var token = _contextHolder.GetToken();

            var otpDvo = new UserOtpDvo();

            var userDao = await _thisRepository.GetByIdAsync(token.user_id);
            if (userDao != null)
            {
                userDao.otp_status = param.status;
                if (param.status == Enums.ScmRowStatusEnum.Enabled)
                {
                    if (string.IsNullOrWhiteSpace(userDao.otp_secret))
                    {
                        userDao.GenerateSecret();
                    }
                    userDao.otp_time = TimeUtils.GetUnixTime();
                }

                await _thisRepository.UpdateAsync(userDao);

                otpDvo.status = param.status;
                otpDvo.time = userDao.otp_time;
            }

            return otpDvo;
        }
    }
}