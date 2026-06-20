using Com.Scm.Dvo;
using Com.Scm.Enums;

namespace Com.Scm.Sys.Sms.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmSysSmsDvo : ScmDataDvo
    {
        /// <summary>
        /// 电话号码
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 短信内容
        /// </summary>
        public string body { get; set; }

        /// <summary>
        /// 发送日期
        /// </summary>
        public long date { get; set; }

        /// <summary>
        /// 短信类型
        /// </summary>
        public ScmSmsTypeEnum type { get; set; }

        /// <summary>
        /// 操作系统相关参数
        /// </summary>
        public Dictionary<string, string> os_params { get; set; }
    }
}
