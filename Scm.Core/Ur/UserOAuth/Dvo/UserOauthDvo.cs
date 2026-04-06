using Com.Scm.Dvo;

namespace Com.Scm.Ur.UserOAuth.Dvo
{
    /// <summary>
    /// 三方登录
    /// </summary>
    public class UserOAuthDvo : ScmDataDvo
    {
        /// <summary>
        /// 用户
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 外部应用
        /// </summary>
        public string provider { get; set; }

        /// <summary>
        /// 授权ID
        /// </summary>
        public string auth_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string user { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        /// 登录次数
        /// </summary>
        public int qty { get; set; }
    }
}