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
        /// 凭据登录
        /// </summary>
        ByOtp = 20,
        /// <summary>
        /// 手机登录，后续变为ByToken
        /// </summary>
        ByPhone = 21,
        /// <summary>
        /// 邮件登录，后续变为ByOtp
        /// </summary>
        ByEmail = 22,
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
