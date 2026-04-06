using Com.Scm.Dvo;

namespace Com.Scm.Flow
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmFlowOrderDvo : ScmDataDvo
    {
        /// <summary>
        /// 流程ID
        /// </summary>
        public long flow_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string codec { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string namec { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 审批页面
        /// </summary>
        public string url { get; set; }
    }
}