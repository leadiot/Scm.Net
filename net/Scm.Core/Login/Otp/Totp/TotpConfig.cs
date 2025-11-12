using Com.Scm.Config;
using Com.Scm.Otp.Totp;

namespace Com.Scm.Login.Otp.Totp
{
    public class TotpConfig
    {
        #region 常量
        /// <summary>
        /// 默认时间步长（秒）
        /// </summary>
        public const int DefaultTimeStep = 30;

        /// <summary>
        /// 默认哈希算法
        /// </summary>
        public const TotpAlgorithm DefaultAlgorithm = TotpAlgorithm.SHA1;
        #endregion

        #region 属性
        /// <summary>
        /// 发行者
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 摘要算法
        /// </summary>
        public TotpAlgorithm Algorithm { get; set; } = DefaultAlgorithm;
        /// <summary>
        /// 步增周期
        /// </summary>
        public int Period { get; set; } = DefaultTimeStep;
        /// <summary>
        /// 容错窗口
        /// </summary>
        public int Windows { get; set; } = 1;

        /// <summary>
        /// 二维码模板
        /// </summary>
        public string Template { get; set; }
        #endregion

        public void Prepare(EnvConfig envConfig)
        {
            LoadDef();
        }

        public void LoadDef()
        {
            if (string.IsNullOrEmpty(Issuer))
            {
                Issuer = "Scm.Net";
            }

            //if (string.IsNullOrEmpty(Algorithm))
            //{
            //    Algorithm = "SHA1";
            //}

            if (Period < 30 || Period > 300)
            {
                Period = 30;
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
