using Com.Scm.Config;
using Com.Scm.Otp.Hotp;

namespace Com.Scm.Login.Otp.Hotp
{
    public class HotpConfig
    {
        #region 常量
        /// <summary>
        /// 默认时间步长（秒）
        /// </summary>
        public const int DefaultHmacStep = 3;

        /// <summary>
        /// 默认哈希算法
        /// </summary>
        public const HotpAlgorithm DefaultAlgorithm = HotpAlgorithm.SHA1;
        #endregion

        #region 属性

        /// <summary>
        /// 摘要算法
        /// </summary>
        public HotpAlgorithm Algorithm { get; set; } = HotpAlgorithm.SHA1;

        /// <summary>
        /// 时间窗口（秒）
        /// </summary>
        public int Period { get; set; } = DefaultHmacStep;

        /// <summary>
        /// 容错窗口数量
        /// </summary>
        public int Windows { get; set; } = 2;

        public string Template { get; set; }

        #endregion

        public void Prepare(EnvConfig envConfig)
        {
            LoadDef();
        }

        public void LoadDef()
        {
            //if (string.IsNullOrEmpty(Issuer))
            //{
            //    Issuer = "Scm.Net";
            //}

            //if (string.IsNullOrEmpty(Algorithm))
            //{
            //    Algorithm = "SHA1";
            //}

            //if (Digits < 4 || Digits > 8)
            //{
            //    Digits = 6;
            //}

            if (Period < 1 || Period > 100)
            {
                Period = DefaultHmacStep;
            }

            if (Windows < 0 || Windows > 10)
            {
                Windows = 0;
            }

            if (string.IsNullOrEmpty(Template))
            {
                Template = "otpauth://totp/{issuer}:{account}?secret={secret}&issuer={issuer}&algorithm={algorithm}&digits={digits}&period={period}";
            }
        }
    }
}
