using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmFlowDataHeaderDto : ScmDataDto
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
        [StringLength(32)]
        public string table { get; set; }

        /// <summary>
        /// 单据ID
        /// </summary>
        public long order_id { get; set; }

        /// <summary>
        /// 单据编码
        /// </summary>
        [StringLength(32)]
        public string order_codes { get; set; }

        /// <summary>
        /// 单据图标
        /// </summary>
        [StringLength(32)]
        public string icon { get; set; }

        /// <summary>
        /// 审批标题
        /// </summary>
        [StringLength(128)]
        public string title { get; set; }

        /// <summary>
        /// 单据地址
        /// </summary>
        [StringLength(128)]
        public string url { get; set; }
    }
}