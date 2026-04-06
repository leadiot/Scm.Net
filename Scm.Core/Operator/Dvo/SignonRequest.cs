using Com.Scm.Enums;
using Com.Scm.Request;

namespace Com.Scm.Operator.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class SignonRequest : ScmRequest
    {
        /// <summary>
        /// 注册模式（表单或三方）
        /// </summary>
        public ScmLoginModeEnum mode { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string user { get; set; }

        /// <summary>
        /// 登录口令
        /// </summary>
        public string pass { get; set; }

        /// <summary>
        /// 系统
        /// </summary>
        //public List<int> systems { get; set; }
        public int system { get; set; }

        #region ByPass
        /// <summary>
        /// 用户全称
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// 电子邮件
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string phone { get; set; }
        #endregion

        #region ByOauth

        /// <summary>
        /// 
        /// </summary>
        public SignonOptEnum opt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SignonOptEnum
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 新建
        /// </summary>
        Create = 1,
        /// <summary>
        /// 绑定
        /// </summary>
        Combine = 2,
        /// <summary>
        /// 关联
        /// </summary>
        Associate = 3
    }
}
