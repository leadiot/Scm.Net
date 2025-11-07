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
        /// 手机登录
        /// </summary>
        ByPhone = 21,
        /// <summary>
        /// 邮件登录
        /// </summary>
        ByEmail = 22,
        /// <summary>
        /// Otp登录
        /// </summary>
        ByHotp = 23,

        /// <summary>
        /// 单点登录
        /// </summary>
        BySso = 30,
        /// <summary>
        /// Oauth
        /// </summary>
        ByOauth = 31,
        /// <summary>
        /// Oidc
        /// </summary>
        ByOidc = 32,
        /// <summary>
        /// Saml
        /// </summary>
        BySaml = 33,
        /// <summary>
        /// Ldap
        /// </summary>
        ByLdap = 34,

        /// <summary>
        /// 令牌登录
        /// </summary>
        ByMut = 40,

        /// <summary>
        /// 识别登录
        /// </summary>
        ByBrv = 50,
    }
}
