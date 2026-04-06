using Com.Scm.Dvo;

namespace Com.Scm.Sys.FlowData.Rnr
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmFlowDataDvo : ScmDataDvo
    {
        /// <summary>
        /// 流程ID
        /// </summary>
        public long flow_id { get; set; }

        /// <summary>
        /// 结点ID
        /// </summary>
        public long node_id { get; set; }

        /// <summary>
        /// 表格名称
        /// </summary>
        public string table { get; set; }

        /// <summary>
        /// 单据ID
        /// </summary>
        public long order_id { get; set; }

        /// <summary>
        /// 单据编码
        /// </summary>
        public string order_codes { get; set; }

        /// <summary>
        /// 单据图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 审批标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 单据地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 单据地址
        /// </summary>
        public string remark { get; set; }
    }
}