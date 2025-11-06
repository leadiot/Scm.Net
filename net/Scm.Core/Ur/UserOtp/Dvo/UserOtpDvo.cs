using Com.Scm.Dvo;
using Com.Scm.Enums;

namespace Com.Scm.Ur.UserOtp.Dvo
{
    /// <summary>
    /// 三方登录
    /// </summary>
    public class UserOtpDvo : ScmDvo
    {
        /// <summary>
        /// 登录用户
        /// </summary>
        public string codec { get; set; }

        /// <summary>
        /// 展示名称
        /// </summary>
        public string namec { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string avatar { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public ScmRowStatusEnum status { get; set; }

        /// <summary>
        /// 凭证
        /// </summary>
        public string secret { get; set; }

        /// <summary>
        /// 启用时间
        /// </summary>
        public long time { get; set; }

        /// <summary>
        /// 发行者
        /// </summary>
        public string issuer { get; set; }

        /// <summary>
        /// 密码长度
        /// </summary>
        public int digits { get; set; }

        /// <summary>
        /// 摘要算法
        /// </summary>
        public string algorithm { get; set; }

        /// <summary>
        /// 用于生成二维码的Uri
        /// </summary>
        public string uri { get; set; }
    }
}