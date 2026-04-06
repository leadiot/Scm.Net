using Com.Scm.Dvo;
using Com.Scm.Enums;

namespace Com.Scm.Log.OAuth.Dvo
{
    /// <summary>
    /// 三方登录
    /// </summary>
    public class LogOAuthDvo : ScmDataDvo
    {
        /// <summary>
        /// 用户
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 登录网站
        /// </summary>
        public int src { get; set; } = 0;

        /// <summary>
        /// UnionID
        /// </summary>
        public string union_id { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public string user { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public ScmSexEnum sex { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string refresh_token { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public long expires_in { get; set; }

        /// <summary>
        /// 错误代码
        /// </summary>
        public string err_code { get; set; }

        /// <summary>
        /// 错误描述
        /// </summary>
        public string err_msg { get; set; }
    }
}