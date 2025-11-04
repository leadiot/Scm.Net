namespace Com.Scm.Otp.Totp
{
    public class TotpConfig
    {
        public string User { get; set; }
        public string SecretKey { get; set; }
        public string CodeLength { get; set; }
        public OtpHashAlgorithm Algorithm { get; set; }
    }
}
