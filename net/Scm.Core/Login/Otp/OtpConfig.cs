using Com.Scm.Config;
using Com.Scm.Email.Config;
using Com.Scm.Log;
using Com.Scm.Phone.Config;

namespace Com.Scm.Login.Otp
{
    public class OtpConfig
    {
        public const string NAME = "Otp";

        /// <summary>
        /// 类型：totp,hotp
        /// </summary>
        public OtpTypesEnum Type { get; set; }
        /// <summary>
        /// 摘要算法
        /// </summary>
        public string Algorithm { get; set; } = "SHA1";
        /// <summary>
        /// 发行者
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 代码长度
        /// </summary>
        public int Digits { get; set; } = 6;
        /// <summary>
        /// 步增周期
        /// </summary>
        public int Period { get; set; } = 30;
        /// <summary>
        /// 容错窗口
        /// </summary>
        public int Window { get; set; } = 1;

        /// <summary>
        /// 二维码模板
        /// </summary>
        public string Template { get; set; }

        public void Prepare(EnvConfig envConfig)
        {
            if (string.IsNullOrEmpty(Issuer))
            {
                Issuer = "Scm.Net";
            }

            if (Digits < 4 || Digits > 8)
            {
                Digits = 6;
            }

            if (Period < 30)
            {
                Period = 30;
            }
        }

        public PhoneConfig Phone { get; set; }

        public EmailConfig Email { get; set; }
    }
}
