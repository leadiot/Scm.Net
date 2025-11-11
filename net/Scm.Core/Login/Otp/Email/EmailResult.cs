namespace Com.Scm.Login.Otp.Email
{
    public class EmailResult : OtpResult
    {
        public const int ERROR_CODE_SEND_100 = 100;
        public const string ERROR_TEXT_SEND_100 = "不支持的验证码方式！";

        public const int ERROR_CODE_SEND_111 = 111;
        public const string ERROR_TEXT_SEND_111 = "无效的电子邮件！";


        public const int ERROR_CODE_SEND_121 = 121;
        public const string ERROR_TEXT_SEND_121 = "验证码发送过于频繁，请1分钟后重试！";

        public const int ERROR_CODE_SEND_122 = 122;
        public const string ERROR_TEXT_SEND_122 = "验证码发送次数过多，请明天再试！";

        public const int ERROR_CODE_SEND_123 = 123;
        public const string ERROR_TEXT_SEND_123 = "验证码发送失败，请稍后重试！";

        public const int ERROR_CODE_VERIFY_130 = 130;
        public const string ERROR_TEXT_VERIFY_130 = "无效的Key！";

        public const int ERROR_CODE_VERIFY_140 = 140;
        public const string ERROR_TEXT_VERIFY_140 = "无效的验证码！";

        /// <summary>
        /// 无效的验证码，不允许多次验证！
        /// </summary>
        public const int ERROR_CODE_VERIFY_141 = 141;
        public const string ERROR_TEXT_VERIFY_141 = "无效的验证码！";

        /// <summary>
        /// 无效的验证码，验证码状态异常！
        /// </summary>
        public const int ERROR_CODE_VERIFY_142 = 142;
        public const string ERROR_TEXT_VERIFY_142 = "无效的验证码！";

        /// <summary>
        /// 无效的验证码，验证码已过期！
        /// </summary>
        public const int ERROR_CODE_VERIFY_143 = 143;
        public const string ERROR_TEXT_VERIFY_143 = "无效的验证码！";
    }
}
