using Com.Scm.Email.Config;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Log;
using Com.Scm.Otp;
using Com.Scm.Res.Otp;
using Com.Scm.Ur;
using Com.Scm.Utils;
using SqlSugar;

namespace Com.Scm.Login.Otp.Email
{
    public class EmailAuth : OtpAuth
    {
        private ISqlSugarClient _SqlClient;
        private EmailParam _EmailParam;
        private EmailConfig _EmailConfig;

        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="config"></param>
        /// <param name="sqlClient"></param>
        public EmailAuth(OtpConfig config, ISqlSugarClient sqlClient) : base(config)
        {
            _EmailConfig = config.Email;
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
            _EmailParam = param as EmailParam;
            if (_EmailParam == null)
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
            var result = new EmailResult();

            var code = _EmailParam.email;
            if (!TextUtils.IsEmail(code))
            {
                result.SetFailure(EmailResult.ERROR_CODE_SEND_111, EmailResult.ERROR_TEXT_SEND_111);
                return result;
            }
            if (_EmailConfig == null)
            {
                result.SetFailure(EmailResult.ERROR_CODE_SEND_112, EmailResult.ERROR_TEXT_SEND_112);
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
                result.SetFailure(EmailResult.ERROR_CODE_SEND_121, EmailResult.ERROR_TEXT_SEND_121);
                return result;
            }

            // 多次发送
            if (logOtpDao.send_qty > 5)
            {
                result.SetFailure(EmailResult.ERROR_CODE_SEND_122, EmailResult.ERROR_TEXT_SEND_122);
                return result;
            }

            // 设置为发送中
            logOtpDao.pass = TextUtils.RandomNumber(Config.Digits);
            logOtpDao.handle = ScmHandleEnum.Doing;
            logOtpDao.PrepareUpdate(UserDto.SYS_ID);
            _SqlClient.Update(logOtpDao);

            // 执行发送
            var handle = SendSmsByEmail(logOtpDao, _EmailParam.template);

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
                result.SetFailure(EmailResult.ERROR_CODE_SEND_123, EmailResult.ERROR_TEXT_SEND_123);
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
            var result = new EmailResult();

            var code = _EmailParam.email;
            if (!TextUtils.IsEmail(code))
            {
                result.SetFailure(EmailResult.ERROR_CODE_SEND_111, EmailResult.ERROR_TEXT_SEND_111);
                return result;
            }
            if (_EmailConfig == null)
            {
                result.SetFailure(EmailResult.ERROR_CODE_SEND_112, EmailResult.ERROR_TEXT_SEND_112);
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
                result.SetFailure(EmailResult.ERROR_CODE_SEND_121, EmailResult.ERROR_TEXT_SEND_121);
                return result;
            }

            // 多次发送
            if (logOtpDao.send_qty > 5)
            {
                result.SetFailure(EmailResult.ERROR_CODE_SEND_122, EmailResult.ERROR_TEXT_SEND_122);
                return result;
            }

            // 设置为发送中
            logOtpDao.pass = TextUtils.RandomNumber(Config.Digits);
            logOtpDao.handle = ScmHandleEnum.Doing;
            logOtpDao.PrepareUpdate(UserDto.SYS_ID);
            await _SqlClient.UpdateAsync(logOtpDao);

            // 执行发送
            var handle = await SendSmsByEmailAsync(logOtpDao, _EmailParam.template);

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
                result.SetFailure(EmailResult.ERROR_CODE_SEND_123, EmailResult.ERROR_TEXT_SEND_123);
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
                result.SetFailure(EmailResult.ERROR_CODE_VERIFY_130, EmailResult.ERROR_TEXT_VERIFY_130);
                return result;
            }

            // 检测校验码是否正确
            var now = DateTime.Now;
            var logSmsDao = _SqlClient.Queryable<LogOtpDao>()
                .Where(a => a.key == key && a.types == Type && a.row_status == ScmRowStatusEnum.Enabled)
                .First();
            if (logSmsDao == null)
            {
                result.SetFailure(EmailResult.ERROR_CODE_VERIFY_130, EmailResult.ERROR_TEXT_VERIFY_130);
                return result;
            }

            // 验证码不匹配
            if (logSmsDao.pass != code)
            {
                result.SetFailure(EmailResult.ERROR_CODE_VERIFY_140, EmailResult.ERROR_TEXT_VERIFY_140);
                return result;
            }

            // 是否允许多次验证
            if (logSmsDao.verify > 0)
            {
                result.SetFailure(EmailResult.ERROR_CODE_VERIFY_141, EmailResult.ERROR_TEXT_VERIFY_141);
                return result;
            }

            // 数据清理
            if (logSmsDao.handle != ScmHandleEnum.Done)
            {
                // 逻辑删除数据
                logSmsDao.row_status = ScmRowStatusEnum.Disabled;
                logSmsDao.PrepareUpdate(UserDto.SYS_ID);
                _SqlClient.Update(logSmsDao);

                result.SetFailure(EmailResult.ERROR_CODE_VERIFY_142, EmailResult.ERROR_TEXT_VERIFY_142);
                return result;
            }

            // 数据清理
            if (logSmsDao.IsExpired(now))
            {
                // 逻辑删除数据
                logSmsDao.row_status = ScmRowStatusEnum.Disabled;
                logSmsDao.PrepareUpdate(UserDto.SYS_ID);
                _SqlClient.Update(logSmsDao);

                result.SetFailure(EmailResult.ERROR_CODE_VERIFY_143, EmailResult.ERROR_TEXT_VERIFY_143);
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
                result.SetFailure(EmailResult.ERROR_CODE_VERIFY_130, EmailResult.ERROR_TEXT_VERIFY_130);
                return result;
            }

            // 检测校验码是否正确
            var now = DateTime.Now;
            var logSmsDao = await _SqlClient.Queryable<LogOtpDao>()
                .Where(a => a.key == key && a.types == Type && a.row_status == ScmRowStatusEnum.Enabled)
                .FirstAsync();
            if (logSmsDao == null)
            {
                result.SetFailure(EmailResult.ERROR_CODE_VERIFY_130, EmailResult.ERROR_TEXT_VERIFY_130);
                return result;
            }

            // 验证码不匹配
            if (logSmsDao.pass != code)
            {
                result.SetFailure(EmailResult.ERROR_CODE_VERIFY_140, EmailResult.ERROR_TEXT_VERIFY_140);
                return result;
            }

            // 是否允许多次验证
            if (logSmsDao.verify > 0)
            {
                result.SetFailure(EmailResult.ERROR_CODE_VERIFY_141, EmailResult.ERROR_TEXT_VERIFY_141);
                return result;
            }

            // 数据清理
            if (logSmsDao.handle != ScmHandleEnum.Done)
            {
                logSmsDao.row_status = ScmRowStatusEnum.Disabled;
                logSmsDao.PrepareUpdate(UserDto.SYS_ID);
                await _SqlClient.UpdateAsync(logSmsDao);

                result.SetFailure(EmailResult.ERROR_CODE_VERIFY_142, EmailResult.ERROR_TEXT_VERIFY_142);
                return result;
            }

            // 数据清理
            if (logSmsDao.IsExpired(now))
            {
                logSmsDao.row_status = ScmRowStatusEnum.Disabled;
                logSmsDao.PrepareUpdate(UserDto.SYS_ID);
                await _SqlClient.UpdateAsync(logSmsDao);

                result.SetFailure(EmailResult.ERROR_CODE_VERIFY_143, EmailResult.ERROR_TEXT_VERIFY_143);
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
        /// 发送邮件验证码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="seq"></param>
        /// <param name="templateCode"></param>
        /// <returns></returns>
        public bool SendSmsByEmail(LogOtpDao logOtpDao, string templateCode)
        {
            var headText = "登录验证码";
            var bodyText = logOtpDao.pass;
            var footText = "";
            var file = "";

            // 加载消息模板
            OtpDao resSmsDao = null;
            if (!string.IsNullOrWhiteSpace(templateCode))
            {
                resSmsDao = _SqlClient.Queryable<OtpDao>()
                    .Where(a => a.codec == templateCode && a.types == OtpTypesEnum.Email && a.row_status == ScmRowStatusEnum.Enabled)
                    .First();
            }

            if (resSmsDao == null)
            {
                logOtpDao.sms_id = ScmEnv.DEFAULT_ID;
                logOtpDao.sms_codec = "";
            }
            else
            {
                logOtpDao.sms_id = resSmsDao.id;
                logOtpDao.sms_codec = resSmsDao.codec;

                if (!string.IsNullOrWhiteSpace(resSmsDao.head))
                {
                    headText = resSmsDao.head;
                }

                if (!string.IsNullOrWhiteSpace(resSmsDao.body))
                {
                    bodyText = resSmsDao.body.Replace("{sms}", bodyText);
                }

                footText = resSmsDao.foot;

                file = resSmsDao.file;
            }

            bodyText = _EmailConfig.GetText(file, headText, bodyText, footText);

            var receiver = new EmailAddress { Name = logOtpDao.code, Address = logOtpDao.code };
            return EmailHelper.SendEmail(_EmailConfig, headText, bodyText, receiver);
        }

        /// <summary>
        /// 发送邮件验证码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="seq"></param>
        /// <param name="templateCode"></param>
        /// <returns></returns>
        public async Task<bool> SendSmsByEmailAsync(LogOtpDao logOtpDao, string templateCode)
        {
            var headText = "登录验证码";
            var bodyText = logOtpDao.pass;
            var footText = "";
            var file = "";

            // 加载消息模板
            OtpDao resSmsDao = null;
            if (!string.IsNullOrWhiteSpace(templateCode))
            {
                resSmsDao = await _SqlClient.Queryable<OtpDao>()
                    .Where(a => a.codec == templateCode && a.types == OtpTypesEnum.Email && a.row_status == ScmRowStatusEnum.Enabled)
                    .FirstAsync();
            }

            if (resSmsDao == null)
            {
                logOtpDao.sms_id = ScmEnv.DEFAULT_ID;
                logOtpDao.sms_codec = "";
            }
            else
            {
                logOtpDao.sms_id = resSmsDao.id;
                logOtpDao.sms_codec = resSmsDao.codec;

                if (!string.IsNullOrWhiteSpace(resSmsDao.head))
                {
                    headText = resSmsDao.head;
                }

                if (!string.IsNullOrWhiteSpace(resSmsDao.body))
                {
                    bodyText = resSmsDao.body.Replace("{sms}", bodyText);
                }

                footText = resSmsDao.foot;

                file = resSmsDao.file;
            }

            bodyText = _EmailConfig.GetText(file, headText, bodyText, footText);

            var receiver = new EmailAddress { Name = logOtpDao.code, Address = logOtpDao.code };
            return await EmailHelper.SendEmailAsync(_EmailConfig, headText, bodyText, receiver);
        }
    }
}
