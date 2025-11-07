using Com.Scm.Email.Config;
using Com.Scm.Enums;
using Com.Scm.Log;
using Com.Scm.Phone.Config;
using Com.Scm.Res.Otp;
using Com.Scm.Ur;
using Com.Scm.Utils;
using SqlSugar;

namespace Com.Scm.Service
{
    public class ScmOtpService : IOtpService
    {
        private readonly ISqlSugarClient _SqlClient;
        private readonly EmailConfig _emailConfig;
        private readonly PhoneConfig _phoneConfig;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlClient"></param>
        /// <param name="emailConfig"></param>
        public ScmOtpService(ISqlSugarClient sqlClient, EmailConfig emailConfig, PhoneConfig phoneConfig)
        {
            _SqlClient = sqlClient;
            _emailConfig = emailConfig;
            _phoneConfig = phoneConfig;
        }

        #region 发送验证码
        /// <summary>
        /// 
        /// </summary>
        /// <param name="types">消息类型</param>
        /// <param name="seq">发送代码，用于判断是否重复发送</param>
        /// <param name="code">接收代码（手机或邮箱等）</param>
        /// <param name="templateCode">模板编码</param>
        /// <returns></returns>
        public async Task<SmsResult> SendSmsAsync(OtpTypesEnum types, string code, string seq, string templateCode)
        {
            if (seq == null)
            {
                seq = "";
            }

            var result = new SmsResult();

            if (types == OtpTypesEnum.Phone)
            {
                if (!TextUtils.IsCellphone(code))
                {
                    result.SetError(SmsResult.ERROR_CODE_SEND_111, SmsResult.ERROR_TEXT_SEND_111);
                    return result;
                }
            }
            else if (types == OtpTypesEnum.Email)
            {
                if (!TextUtils.IsEmail(code))
                {
                    result.SetError(SmsResult.ERROR_CODE_SEND_112, SmsResult.ERROR_TEXT_SEND_112);
                    return result;
                }
            }
            else
            {
                result.SetError(SmsResult.ERROR_CODE_SEND_100, SmsResult.ERROR_TEXT_SEND_100);
                return result;
            }

            // 重复发送校验
            var logSmsDao = await _SqlClient.Queryable<LogOtpDao>()
                .Where(a => a.types == types && a.code == code && a.key == seq && a.row_status == ScmRowStatusEnum.Enabled)
                .OrderBy(a => a.id, OrderByType.Desc)
                .FirstAsync();

            if (logSmsDao == null)
            {
                logSmsDao = new Log.LogOtpDao();
                logSmsDao.key = TextUtils.GuidString();
                logSmsDao.types = types;
                logSmsDao.code = code;
                logSmsDao.seq = seq;
                logSmsDao.handle = ScmHandleEnum.Todo;
                logSmsDao.PrepareCreate(UserDto.SYS_ID);

                await _SqlClient.InsertAsync(logSmsDao);
            }

            var now = DateTime.Now;

            // 频繁发送
            if (TimeUtils.GetUnixTime(now) - logSmsDao.send_time < 1000 * 60)
            {
                result.SetError(SmsResult.ERROR_CODE_SEND_121, SmsResult.ERROR_TEXT_SEND_121);
                return result;
            }
            // 多次发送
            if (logSmsDao.send_qty > 5)
            {
                result.SetError(SmsResult.ERROR_CODE_SEND_122, SmsResult.ERROR_TEXT_SEND_122);
                return result;
            }

            // 设置为发送中
            logSmsDao.sms = ScmUtils.SmsCode();
            logSmsDao.handle = ScmHandleEnum.Doing;
            logSmsDao.PrepareUpdate(UserDto.SYS_ID);
            await _SqlClient.UpdateAsync(logSmsDao);

            // 执行发送
            var handle = false;
            if (types == OtpTypesEnum.Phone)
            {
                handle = SendSmsByPhone(logSmsDao, templateCode);
            }
            else if (types == OtpTypesEnum.Email)
            {
                handle = await SendSmsByEmailAsync(logSmsDao, templateCode);
            }

            // 记录发送结果
            logSmsDao.send_qty += 1;
            logSmsDao.send_time = TimeUtils.GetUnixTime(now);
            logSmsDao.handle = ScmHandleEnum.Done;
            if (handle)
            {
                logSmsDao.expired = TimeUtils.GetUnixTime(now.AddMinutes(10));
                logSmsDao.result = ScmResultEnum.Success;
            }
            else
            {
                logSmsDao.expired = 0;
                logSmsDao.result = ScmResultEnum.Failure;
            }
            logSmsDao.PrepareUpdate(UserDto.SYS_ID);
            await _SqlClient.UpdateAsync(logSmsDao);

            result.Dao = logSmsDao;
            return result;
        }

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="logSmsDao"></param>
        /// <returns></returns>
        private bool SendSmsByPhone(LogOtpDao logSmsDao, string templateCode)
        {
            return PhoneHelper.SendPhone(_phoneConfig, logSmsDao.code, logSmsDao.sms);
        }

        /// <summary>
        /// 发送邮件验证码
        /// </summary>
        /// <param name="code"></param>
        /// <param name="seq"></param>
        /// <param name="templateCode"></param>
        /// <returns></returns>
        public async Task<bool> SendSmsByEmailAsync(LogOtpDao logSmsDao, string templateCode)
        {
            var headText = "登录验证码";
            var bodyText = logSmsDao.sms;
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
                logSmsDao.sms_id = ScmEnv.DEFAULT_ID;
                logSmsDao.sms_codec = "";
            }
            else
            {
                logSmsDao.sms_id = resSmsDao.id;
                logSmsDao.sms_codec = resSmsDao.codec;

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

            bodyText = _emailConfig.GetText(file, headText, bodyText, footText);

            var receiver = new EmailAddress { Name = logSmsDao.code, Address = logSmsDao.code };
            return await EmailHelper.SendEmailAsync(_emailConfig, headText, bodyText, receiver);
        }
        #endregion

        #region 核验验证码
        /// <summary>
        /// 
        /// </summary>
        /// <param name="key">发送代码，用于判断是否重复发送</param>
        /// <param name="sms">验证码</param>
        /// <returns></returns>
        public async Task<SmsResult> VerifySmsAsync(string key, string sms)
        {
            var result = new SmsResult();

            if (!ScmUtils.IsValidCode(key, 32))
            {
                result.SetError(SmsResult.ERROR_CODE_VERIFY_130, SmsResult.ERROR_TEXT_VERIFY_130);
                return result;
            }

            // 检测校验码是否正确
            var now = DateTime.Now;
            var logSmsDao = await _SqlClient.Queryable<LogOtpDao>()
                .Where(a => a.key == key && a.row_status == ScmRowStatusEnum.Enabled)
                .FirstAsync();
            if (logSmsDao == null)
            {
                result.SetError(SmsResult.ERROR_CODE_VERIFY_130, SmsResult.ERROR_TEXT_VERIFY_130);
                return result;
            }
            // 是否允许多次验证
            if (logSmsDao.verify > 0)
            {
                result.SetError(SmsResult.ERROR_CODE_VERIFY_140, SmsResult.ERROR_TEXT_VERIFY_140);
                return result;
            }
            // 验证码不匹配
            if (logSmsDao.sms != sms)
            {
                result.SetError(SmsResult.ERROR_CODE_VERIFY_140, SmsResult.ERROR_TEXT_VERIFY_140);
                return result;
            }

            // 数据清理
            if (logSmsDao.handle == ScmHandleEnum.Done || logSmsDao.IsExpired(now))
            {
                logSmsDao.row_status = ScmRowStatusEnum.Disabled;
                logSmsDao.PrepareUpdate(UserDto.SYS_ID);
                await _SqlClient.UpdateAsync(logSmsDao);

                result.SetError(SmsResult.ERROR_CODE_VERIFY_140, SmsResult.ERROR_TEXT_VERIFY_140);
                return result;
            }

            logSmsDao.verify += 1;// 累加认证次数
            logSmsDao.row_status = ScmRowStatusEnum.Disabled;
            logSmsDao.PrepareUpdate(UserDto.SYS_ID);
            await _SqlClient.UpdateAsync(logSmsDao);

            result.Dao = logSmsDao;
            return result;
        }
        #endregion
    }
}
