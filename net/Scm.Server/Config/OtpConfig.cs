using Com.Scm.Otp;

namespace Com.Scm.Config
{
    public class OtpConfig
    {
        public const string NAME = "Otp";

        /// <summary>
        /// 类型：totp,hotp
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 摘要算法
        /// </summary>
        public OtpHashAlgorithm Algorithm { get; set; } = OtpHashAlgorithm.SHA1;
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
            Type = (Type ?? "").ToLower();
            if (Type != "hotp")
            {
                Type = "totp";
            }

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
    }
}
