namespace Com.Scm.Otp.Hotp
{
    public class HotpParam : OtpParam
    {
        /// <summary>
        /// 共享密钥
        /// </summary>
        public byte[] Secret { get; set; }
    }
}
