using Com.Scm.Dvo;

namespace Com.Scm.Ur.UserOtp.Dvo
{
    /// <summary>
    /// 三方登录
    /// </summary>
    public class UserOtpDvo : ScmDataDvo
    {
        /// <summary>
        /// 登录用户
        /// </summary>
        public string user { get; set; }

        /// <summary>
        /// 展示名称
        /// </summary>
        public string namec { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        /// 凭证
        /// </summary>
        public string token { get; set; }
    }
}