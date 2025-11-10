namespace Com.Scm.Otp.Totp
{
    public class TotpParam : OtpParam
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
        /// 时间窗口（秒）
        /// </summary>
        public int TimeStep { get; set; } = DefaultTimeStep;

        /// <summary>
        /// 摘要算法
        /// </summary>
        public TotpAlgorithm Algorithm { get; set; } = DefaultAlgorithm;

        /// <summary>
        /// 容错窗口数量
        /// </summary>
        public int Windows { get; set; } = 1;

        /// <summary>
        /// 共享密钥
        /// </summary>
        public byte[] Secret { get; set; }
        #endregion

        public override void LoadDefault()
        {
            base.LoadDefault();

            TimeStep = DefaultTimeStep;
            Algorithm = DefaultAlgorithm;
            Windows = 1;
        }
    }
}
