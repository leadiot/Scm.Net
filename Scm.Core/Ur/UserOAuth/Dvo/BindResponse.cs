namespace Com.Scm.Ur.UserOAuth.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class BindResponse : ScmUpdateResponse<string>
    {
        /// <summary>
        /// 无效的联合登录信息
        /// </summary>
        public const int ERROR_01 = 1;
        /// <summary>
        /// 联合登录授权已过期，请重新授权
        /// </summary>
        public const int ERROR_02 = 2;
        /// <summary>
        /// 联合登录已绑定多个账户，不能重复绑定
        /// </summary>
        public const int ERROR_03 = 3;
        /// <summary>
        /// 联合登录已绑定其它账户，不能重复绑定
        /// </summary>
        public const int ERROR_04 = 4;
    }
}
