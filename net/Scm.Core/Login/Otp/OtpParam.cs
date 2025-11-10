namespace Com.Scm.Otp
{
    public class OtpParam
    {
        /// <summary>
        /// 默认口令长度
        /// </summary>
        public const int DefaultDigits = 6;

        /// <summary>
        /// 口令长度
        /// </summary>
        public int Digits { get; set; } = DefaultDigits;

        public virtual void LoadDefault()
        {
            Digits = DefaultDigits;
        }
    }
}
