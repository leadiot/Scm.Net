namespace Com.Scm.Otp
{
    public class OtpConfig
    {
        /// <summary>
        /// 发行者
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// 登录账户
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 共享密钥
        /// </summary>
        public byte[] Secret { get; set; }
        /// <summary>
        /// 代码长度
        /// </summary>
        public int CodeLength { get; set; } = 6;
        /// <summary>
        /// 摘要算法
        /// </summary>
        public OtpHashAlgorithm Algorithm { get; set; } = OtpHashAlgorithm.SHA1;
    }
}
