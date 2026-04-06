using Com.Scm.Enums;
using Com.Scm.Request;

namespace Com.Scm.Operator.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class SendSmsRequest : ScmRequest
    {
        /// <summary>
        /// 登录模式
        /// </summary>
        public ScmLoginModeEnum mode { get; set; }

        /// <summary>
        /// 手机或者邮件
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 键
        /// </summary>
        public string req { get; set; }
    }
}
