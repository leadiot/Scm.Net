using Com.Scm.Dvo;

namespace Com.Scm.Sys.Table.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmSysTableHeaderDvo : ScmDataDvo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 系统代码
        /// </summary>
        public string codes { get; set; }

        /// <summary>
        /// 视图编码
        /// </summary>
        public string codec { get; set; }

        /// <summary>
        /// 视图名称
        /// </summary>
        public string names { get; set; }
    }
}