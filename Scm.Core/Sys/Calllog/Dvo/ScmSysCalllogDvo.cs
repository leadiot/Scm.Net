using Com.Scm.Dvo;
using Com.Scm.Enums;

namespace Com.Scm.Sys.Calllog.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmSysCalllogDvo : ScmDataDvo
    {
        /// <summary>
        /// 号码
        /// </summary>
        public string number { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        public long date { get; set; }

        /// <summary>
        /// 呼叫类型
        /// </summary>
        public ScmCallTypeEnum type { get; set; }

        /// <summary>
        /// 持续时长
        /// </summary>
        public long duration { get; set; }

        /// <summary>
        /// 操作系统相关参数
        /// </summary>
        public Dictionary<string, string> os_params { get; set; }
    }
}
