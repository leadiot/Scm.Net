namespace Com.Scm.Otp.Totp
{
    public class TotpParam : OtpParam
    {
        /// <summary>
        /// 共享密钥
        /// </summary>
        public byte[] Secret { get; set; }
    }
}
