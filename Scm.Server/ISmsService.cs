using Com.Scm.Log;

namespace Com.Scm
{
    public interface ISmsService
    {
        /// <param name="types">消息类型</param>
        /// <param name="code">接收代码（手机或邮箱等）</param>
        /// <param name="seq">发送代码，用于判断是否重复发送</param>
        /// <param name="templateCode">模板编码</param>
        Task<SmsResult> SendSmsAsync(SmsTypesEnum types, string code, string seq, string templateCode);

        /// <param name="types">消息类型</param>
        /// <param name="code">接收代码（手机或邮箱等）</param>
        /// <param name="key">发送代码，用于判断是否重复发送</param>
        /// <param name="sms">验证码</param>
        Task<SmsResult> VerifySmsAsync(string key, string sms);
    }

    public class SmsResult
    {
        public const int ERROR_CODE_SEND_100 = 100;
        public const string ERROR_TEXT_SEND_100 = "不支持的验证码方式！";

        public const int ERROR_CODE_SEND_111 = 111;
        public const string ERROR_TEXT_SEND_111 = "无效的手机号码！";

        public const int ERROR_CODE_SEND_112 = 112;
        public const string ERROR_TEXT_SEND_112 = "无效的电子邮件！";

        public const int ERROR_CODE_SEND_121 = 121;
        public const string ERROR_TEXT_SEND_121 = "验证码发送过于频繁，请1分钟后重试！";

        public const int ERROR_CODE_SEND_122 = 122;
        public const string ERROR_TEXT_SEND_122 = "验证码发送次数过多，请明天再试！";

        public const int ERROR_CODE_VERIFY_130 = 130;
        public const string ERROR_TEXT_VERIFY_130 = "无效的Key！";

        public const int ERROR_CODE_VERIFY_140 = 140;
        public const string ERROR_TEXT_VERIFY_140 = "无效的验证码！";

        public int Code { get; set; }
        public string Text { get; set; }
        public LogSmsDao Dao { get; set; }

        public void SetError(int code, string text)
        {
            this.Code = code;
            this.Text = text;
        }

        public bool HasError()
        {
            return Code != 0;
        }
    }
}
