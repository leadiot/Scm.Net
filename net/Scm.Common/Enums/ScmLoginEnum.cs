namespace Com.Scm.Enums
{
    /// <summary>
    /// 登录模式
    /// </summary>
    public enum ScmLoginModeEnum
    {
        None,
        /// <summary>
        /// 口令登录
        /// </summary>
        ByPass = 10,
        /// <summary>
        /// 手机登录
        /// </summary>
        ByPhone = 20,
        /// <summary>
        /// 邮件登录
        /// </summary>
        ByEmail = 30,
        /// <summary>
        /// Otp登录
        /// </summary>
        ByOtp = 40,
        /// <summary>
        /// 联合登录
        /// </summary>
        ByOauth = 50
    }
}
