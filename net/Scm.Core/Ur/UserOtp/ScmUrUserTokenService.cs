using Com.Scm.Config;
using Com.Scm.Dsa;
using Com.Scm.Jwt;
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
    public class ScmUrUserTokenService : ApiService
    {
        private readonly JwtContextHolder _contextHolder;
        private readonly SugarRepository<UserDao> _thisRepository;
        private readonly OtpConfig _otpConfig;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRepository"></param>
        /// <returns></returns>
        public ScmUrUserTokenService(JwtContextHolder contextHolder,
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
        [HttpGet("{id}")]
        public async Task<UserOtpDvo> GetAsync()
        {
            var token = _contextHolder.GetToken();

            var userDao = await _thisRepository.GetByIdAsync(token.user_id);
            return ConvertToDvo(userDao);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<UserOtpDvo> UpdateAsync()
        {
            var token = _contextHolder.GetToken();

            var userDao = await _thisRepository.GetByIdAsync(token.user_id);
            userDao.GenerateToken();

            await _thisRepository.UpdateAsync(userDao);
            return ConvertToDvo(userDao);
        }

        private UserOtpDvo ConvertToDvo(UserDao userDao)
        {
            var dvo = new UserOtpDvo();
            dvo.issuer = _otpConfig.Issuer;
            dvo.code_length = _otpConfig.Digits;
            dvo.algorithm = EnumUtils.ToKey(_otpConfig.Algorithm);
            dvo.codec = userDao.codec;
            dvo.namec = userDao.namec;
            dvo.avatar = userDao.avatar;

            var bytes = userDao.DecodeToken();
            var otpSecret = TextUtils.Base32Encode(bytes);
            var template = _otpConfig.Template ?? "";
            dvo.token = template.Replace("{issuer}", Uri.UnescapeDataString(_otpConfig.Issuer))
                .Replace("{account}", Uri.UnescapeDataString(userDao.codec))
                .Replace("{secret}", Uri.UnescapeDataString(otpSecret))
                .Replace("{algorithm}", EnumUtils.ToKey(_otpConfig.Algorithm))
                .Replace("{digits}", _otpConfig.Digits.ToString())
                .Replace("{period}", _otpConfig.Digits.ToString());

            return dvo;
        }

        /// <summary>
        /// 校验是否成功
        /// </summary>
        /// <returns></returns>
        public async Task<bool> VerifyAsync(string code)
        {
            var token = _contextHolder.GetToken();

            var userDao = await _thisRepository.GetByIdAsync(token.user_id);
            var secret = userDao.DecodeToken();

            var totp = new TotpAuth(_otpConfig.Period, _otpConfig.Digits, Otp.OtpHashAlgorithm.SHA1);
            return totp.VerifyCode(secret, code);
        }
    }
}