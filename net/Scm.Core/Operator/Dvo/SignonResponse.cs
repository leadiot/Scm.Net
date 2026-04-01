using Com.Scm.Cfg.UserTheme;
using Com.Scm.Response;

namespace Com.Scm.Operator.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class SignonResponse : ScmApiResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public string user { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string accessToken { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public OperatorInfo userInfo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public UserThemeDto userTheme { get; set; }

        /// <summary>
        /// 用户注册失败，未知错误
        /// </summary>
        public const int ERROR_01 = 1;
        /// <summary>
        /// 账户或密码输入错误，请重试！
        /// </summary>
        public const int ERROR_03 = 3;
        /// <summary>
        /// 账号被冻结，请联系管理员！
        /// </summary>
        public const int ERROR_04 = 4;
        /// <summary>
        /// 限时登录
        /// </summary>
        public const int ERROR_05 = 5;

        #region 密码注册
        /// <summary>
        /// 无效的机构代码
        /// </summary>
        public const int ERROR_10 = 10;
        /// <summary>
        /// 存在相同的机构名称
        /// </summary>
        public const int ERROR_11 = 11;
        /// <summary>
        /// 机构信息注册失败
        /// </summary>
        public const int ERROR_12 = 12;
        /// <summary>
        /// 存在相同的用户名称
        /// </summary>
        public const int ERROR_13 = 13;
        /// <summary>
        /// 无效的登录用户，请重新输入！
        /// </summary>
        public const int ERROR_14 = 14;
        /// <summary>
        /// 用户信息注册失败
        /// </summary>
        public const int ERROR_15 = 15;
        /// <summary>
        /// 无效的登录密码，请重新输入！
        /// </summary>
        public const int ERROR_16 = 16;
        #endregion

        #region 联合注册
        /// <summary>
        /// 无效的联合登录信息
        /// </summary>
        public const int ERROR_41 = 41;
        /// <summary>
        /// 联合登录授权已过期，请重新授权！
        /// </summary>
        public const int ERROR_42 = 42;
        #endregion
    }
}
