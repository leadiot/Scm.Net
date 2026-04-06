namespace Com.Scm.Otp.Totp
{
    /// <summary>
    /// 支持的哈希算法
    /// </summary>
    public enum TotpAlgorithm
    {
        /// <summary>
        /// SHA-1算法
        /// </summary>
        SHA1,

        /// <summary>
        /// SHA-256算法
        /// </summary>
        SHA256,

        /// <summary>
        /// SHA-512算法
        /// </summary>
        SHA512
    }
}
