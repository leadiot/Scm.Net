using Com.Scm.Dvo;
using Com.Scm.Enums;

namespace Com.Scm.Ur.User.Dvo
{
    /// <summary>
    /// 简单用户信息
    /// </summary>
    public class SimpleUserDvo : ScmDvo
    {
        /// <summary>
        /// 
        /// </summary>
        public string codes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string codec { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string names { get; set; }

        /// <summary>
        /// 展示姓名
        /// </summary>
        public string namec { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string phone { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public ScmSexEnum sex { get; set; }
    }
}
