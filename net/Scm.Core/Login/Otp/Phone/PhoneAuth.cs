using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Log;
using Com.Scm.Otp;
using Com.Scm.Phone.Config;
using Com.Scm.Ur;
using Com.Scm.Utils;
using SqlSugar;

namespace Com.Scm.Login.Otp.Phone
{
    public class PhoneAuth : OtpAuth
    {
        private ISqlSugarClient _SqlClient;
        private PhoneParam _PhoneParam;
        private PhoneConfig _PhoneConfig;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sqlClient"></param>
        public PhoneAuth(OtpConfig config, ISqlSugarClient sqlClient) : base(config)
        {
            _PhoneConfig = config.Phone;
            _SqlClient = sqlClient;
            Type = OtpTypesEnum.Phone;
        }

        #region 接口实现
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public override bool Init(OtpParam param)
        {
            _PhoneParam = param as PhoneParam;
            if (_PhoneParam == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        public override OtpResult GenerateCode(string requestId)
        {
            var result = new PhoneResult();

            var code = _PhoneParam.phone;
            if (!TextUtils.IsCellphone(code))
            {
                result.SetFailure(PhoneResult.ERROR_CODE_SEND_111, PhoneResult.ERROR_TEXT_SEND_111);
                return result;
            }
            if (_PhoneConfig == null)
            {
                result.SetFailure(PhoneResult.ERROR_CODE_SEND_112, PhoneResult.ERROR_TEXT_SEND_112);
                return result;
            }

            requestId = requestId ?? "";

            // 重复发送校验
            var logOtpDao = _SqlClient.Queryable<LogOtpDao>()
                .Where(a => a.types == Type && a.code == code && a.seq == requestId && a.row_status == ScmRowStatusEnum.Enabled)
                .OrderBy(a => a.id, OrderByType.Desc)
                .First();

            if (logOtpDao == null)
            {
                logOtpDao = new Log.LogOtpDao();
                logOtpDao.key = TextUtils.GuidString();
                logOtpDao.types = Type;
                logOtpDao.code = code;
                logOtpDao.seq = requestId;
                logOtpDao.handle = ScmHandleEnum.Todo;
                logOtpDao.PrepareCreate(UserDto.SYS_ID);

                _SqlClient.Insert(logOtpDao);
            }

            var now = DateTime.Now;

            // 频繁发送
            if (TimeUtils.GetUnixTime(now) - logOtpDao.send_time < 1000 * 60)
            {
                result.SetFailure(PhoneResult.ERROR_CODE_SEND_121, PhoneResult.ERROR_TEXT_SEND_121);
                return result;
            }

            // 多次发送
            if (logOtpDao.send_qty > 5)
            {
                result.SetFailure(PhoneResult.ERROR_CODE_SEND_122, PhoneResult.ERROR_TEXT_SEND_122);
                return result;
            }

            // 设置为发送中
            logOtpDao.pass = TextUtils.RandomNumber(_PhoneParam.Digits);
            logOtpDao.handle = ScmHandleEnum.Doing;
            logOtpDao.PrepareUpdate(UserDto.SYS_ID);
            _SqlClient.Update(logOtpDao);

            // 执行发送
            var handle = SendSmsByPhone(logOtpDao, _PhoneParam.template);

            // 记录发送结果
            logOtpDao.send_qty += 1;
            logOtpDao.send_time = TimeUtils.GetUnixTime(now);
            logOtpDao.handle = ScmHandleEnum.Done;
            if (handle)
            {
                logOtpDao.expired = TimeUtils.GetUnixTime(now.AddMinutes(10));
                logOtpDao.result = ScmResultEnum.Success;
                result.SetSuccess(logOtpDao.key);
            }
            else
            {
                logOtpDao.expired = 0;
                logOtpDao.result = ScmResultEnum.Failure;
                result.SetFailure(PhoneResult.ERROR_CODE_SEND_123, PhoneResult.ERROR_TEXT_SEND_123);
            }
            logOtpDao.PrepareUpdate(UserDto.SYS_ID);
            _SqlClient.Update(logOtpDao);

            return result;
        }

        /// <summary>
        /// 生成口令（异步）
        /// </summary>
        /// <param name="requestId">请求标识</param>
        /// <returns></returns>
        public override async Task<OtpResult> GenerateCodeAsync(string requestId)
        {
            var result = new PhoneResult();

            var code = _PhoneParam.phone;
            if (!TextUtils.IsCellphone(code))
            {
                result.SetFailure(PhoneResult.ERROR_CODE_SEND_111, PhoneResult.ERROR_TEXT_SEND_111);
                return result;
            }
            if (_PhoneConfig == null)
            {
                result.SetFailure(PhoneResult.ERROR_CODE_SEND_112, PhoneResult.ERROR_TEXT_SEND_112);
                return result;
            }

            requestId = requestId ?? "";

            // 重复发送校验
            var logOtpDao = await _SqlClient.Queryable<LogOtpDao>()
                .Where(a => a.types == Type && a.code == code && a.seq == requestId && a.row_status == ScmRowStatusEnum.Enabled)
                .OrderBy(a => a.id, OrderByType.Desc)
                .FirstAsync();

            if (logOtpDao == null)
            {
                logOtpDao = new Log.LogOtpDao();
                logOtpDao.key = TextUtils.GuidString();
                logOtpDao.types = Type;
                logOtpDao.code = code;
                logOtpDao.seq = requestId;
                logOtpDao.handle = ScmHandleEnum.Todo;
                logOtpDao.PrepareCreate(UserDto.SYS_ID);

                await _SqlClient.InsertAsync(logOtpDao);
            }

            var now = DateTime.Now;

            // 频繁发送
            if (TimeUtils.GetUnixTime(now) - logOtpDao.send_time < 1000 * 60)
            {
                result.SetFailure(PhoneResult.ERROR_CODE_SEND_121, PhoneResult.ERROR_TEXT_SEND_121);
                return result;
            }

            // 多次发送
            if (logOtpDao.send_qty > 5)
            {
                result.SetFailure(PhoneResult.ERROR_CODE_SEND_122, PhoneResult.ERROR_TEXT_SEND_122);
                return result;
            }

            // 设置为发送中
            logOtpDao.pass = TextUtils.RandomNumber(_PhoneParam.Digits);
            logOtpDao.handle = ScmHandleEnum.Doing;
            logOtpDao.PrepareUpdate(UserDto.SYS_ID);
            await _SqlClient.UpdateAsync(logOtpDao);

            // 执行发送
            var handle = await SendSmsByPhoneAsync(logOtpDao, _PhoneParam.template);

            // 记录发送结果
            logOtpDao.send_qty += 1;
            logOtpDao.send_time = TimeUtils.GetUnixTime(now);
            logOtpDao.handle = ScmHandleEnum.Done;
            if (handle)
            {
                logOtpDao.expired = TimeUtils.GetUnixTime(now.AddMinutes(10));
                logOtpDao.result = ScmResultEnum.Success;
                result.SetSuccess(logOtpDao.key);
            }
            else
            {
                logOtpDao.expired = 0;
                logOtpDao.result = ScmResultEnum.Failure;
                result.SetFailure(PhoneResult.ERROR_CODE_SEND_123, PhoneResult.ERROR_TEXT_SEND_123);
            }
            logOtpDao.PrepareUpdate(UserDto.SYS_ID);
            await _SqlClient.UpdateAsync(logOtpDao);

            return result;
        }

        /// <summary>
        /// 验证口令（同步）
        /// </summary>
        /// <param name="key">验证凭证</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public override OtpResult VerifyCode(string key, string code)
        {
            var result = new OtpResult();

            if (!ScmUtils.IsValidCode(key, 32))
            {
                result.SetFailure(PhoneResult.ERROR_CODE_VERIFY_130, PhoneResult.ERROR_TEXT_VERIFY_130);
                return result;
            }

            // 检测校验码是否正确
            var now = DateTime.Now;
            var logSmsDao = _SqlClient.Queryable<LogOtpDao>()
                .Where(a => a.key == key && a.types == Type && a.row_status == ScmRowStatusEnum.Enabled)
                .First();
            if (logSmsDao == null)
            {
                result.SetFailure(PhoneResult.ERROR_CODE_VERIFY_130, PhoneResult.ERROR_TEXT_VERIFY_130);
                return result;
            }

            // 验证码不匹配
            if (logSmsDao.pass != code)
            {
                result.SetFailure(PhoneResult.ERROR_CODE_VERIFY_140, PhoneResult.ERROR_TEXT_VERIFY_140);
                return result;
            }

            // 是否允许多次验证
            if (logSmsDao.verify > 0)
            {
                result.SetFailure(PhoneResult.ERROR_CODE_VERIFY_141, PhoneResult.ERROR_TEXT_VERIFY_141);
                return result;
            }

            // 数据清理
            if (logSmsDao.handle != ScmHandleEnum.Done)
            {
                // 逻辑删除数据
                logSmsDao.row_status = ScmRowStatusEnum.Disabled;
                logSmsDao.PrepareUpdate(UserDto.SYS_ID);
                _SqlClient.Update(logSmsDao);

                result.SetFailure(PhoneResult.ERROR_CODE_VERIFY_142, PhoneResult.ERROR_TEXT_VERIFY_142);
                return result;
            }

            // 数据清理
            if (logSmsDao.IsExpired(now))
            {
                // 逻辑删除数据
                logSmsDao.row_status = ScmRowStatusEnum.Disabled;
                logSmsDao.PrepareUpdate(UserDto.SYS_ID);
                _SqlClient.Update(logSmsDao);

                result.SetFailure(PhoneResult.ERROR_CODE_VERIFY_143, PhoneResult.ERROR_TEXT_VERIFY_143);
                return result;
            }

            // 累加认证次数
            logSmsDao.verify += 1;
            // 逻辑删除数据
            logSmsDao.row_status = ScmRowStatusEnum.Disabled;
            logSmsDao.PrepareUpdate(UserDto.SYS_ID);
            _SqlClient.Update(logSmsDao);
            result.SetSuccess(logSmsDao.pass);

            //result.Dao = logSmsDao;
            return result;
        }

        /// <summary>
        /// 验证口令（异步）
        /// </summary>
        /// <param name="key">验证凭证</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override async Task<OtpResult> VerifyCodeAsync(string key, string code)
        {
            var result = new OtpResult();

            if (!ScmUtils.IsValidCode(key, 32))
            {
                result.SetFailure(PhoneResult.ERROR_CODE_VERIFY_130, PhoneResult.ERROR_TEXT_VERIFY_130);
                return result;
            }

            // 检测校验码是否正确
            var now = DateTime.Now;
            var logSmsDao = await _SqlClient.Queryable<LogOtpDao>()
                .Where(a => a.key == key && a.types == Type && a.row_status == ScmRowStatusEnum.Enabled)
                .FirstAsync();
            if (logSmsDao == null)
            {
                result.SetFailure(PhoneResult.ERROR_CODE_VERIFY_130, PhoneResult.ERROR_TEXT_VERIFY_130);
                return result;
            }

            // 验证码不匹配
            if (logSmsDao.pass != code)
            {
                result.SetFailure(PhoneResult.ERROR_CODE_VERIFY_140, PhoneResult.ERROR_TEXT_VERIFY_140);
                return result;
            }

            // 是否允许多次验证
            if (logSmsDao.verify > 0)
            {
                result.SetFailure(PhoneResult.ERROR_CODE_VERIFY_141, PhoneResult.ERROR_TEXT_VERIFY_141);
                return result;
            }

            // 数据清理
            if (logSmsDao.handle != ScmHandleEnum.Done)
            {
                logSmsDao.row_status = ScmRowStatusEnum.Disabled;
                logSmsDao.PrepareUpdate(UserDto.SYS_ID);
                await _SqlClient.UpdateAsync(logSmsDao);

                result.SetFailure(PhoneResult.ERROR_CODE_VERIFY_142, PhoneResult.ERROR_TEXT_VERIFY_142);
                return result;
            }

            // 数据清理
            if (logSmsDao.IsExpired(now))
            {
                logSmsDao.row_status = ScmRowStatusEnum.Disabled;
                logSmsDao.PrepareUpdate(UserDto.SYS_ID);
                await _SqlClient.UpdateAsync(logSmsDao);

                result.SetFailure(PhoneResult.ERROR_CODE_VERIFY_143, PhoneResult.ERROR_TEXT_VERIFY_143);
                return result;
            }

            logSmsDao.verify += 1;// 累加认证次数
            logSmsDao.row_status = ScmRowStatusEnum.Disabled;
            logSmsDao.PrepareUpdate(UserDto.SYS_ID);
            await _SqlClient.UpdateAsync(logSmsDao);
            result.SetSuccess(logSmsDao.pass);

            //result.Dao = logSmsDao;
            return result;
        }

        /// <summary>
        /// 更新口令
        /// </summary>
        /// <param name="requestId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool ChangeCode(string requestId)
        {
            return true;
        }
        #endregion

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="logSmsDao"></param>
        /// <returns></returns>
        private bool SendSmsByPhone(LogOtpDao logSmsDao, string templateCode)
        {
            return PhoneHelper.SendPhone(_PhoneConfig, logSmsDao.code, logSmsDao.pass);
        }

        private async Task<bool> SendSmsByPhoneAsync(LogOtpDao logSmsDao, string templateCode)
        {
            return await PhoneHelper.SendPhoneAsync(_PhoneConfig, logSmsDao.code, logSmsDao.pass);
        }
    }
}
