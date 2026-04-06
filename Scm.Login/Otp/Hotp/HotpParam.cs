namespace Com.Scm.Otp.Hotp
{
    public class HotpParam : OtpParam
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
        /// 时间窗口（秒）
        /// </summary>
        public int HmacStep { get; set; } = DefaultHmacStep;

        /// <summary>
        /// 摘要算法
        /// </summary>
        public HotpAlgorithm Algorithm { get; set; } = DefaultAlgorithm;

        /// <summary>
        /// 容错窗口数量
        /// </summary>
        public int Windows { get; set; } = 2;

        /// <summary>
        /// 共享密钥
        /// </summary>
        public byte[] Secret { get; set; }
        #endregion

        public override void LoadDefault()
        {
            base.LoadDefault();

            HmacStep = DefaultHmacStep;
            Algorithm = DefaultAlgorithm;
            Windows = 2;
        }
    }
}
