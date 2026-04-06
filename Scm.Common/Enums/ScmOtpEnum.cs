namespace Com.Scm.Log
{
    public enum OtpTypesEnum
    {
        None = 0,
        /// <summary>
        /// 短信口令
        /// </summary>
        Phone,
        /// <summary>
        /// 邮件口令
        /// </summary>
        Email,
        /// <summary>
        /// 事件口令
        /// </summary>
        Hotp,
        /// <summary>
        /// 时序口令
        /// </summary>
        Totp,
    }
}
