using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Flow
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmFlowOrderDto : ScmDataDto
    {
        /// <summary>
        /// 流程ID
        /// </summary>
        public long flow_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(32)]
        public string codec { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(128)]
        public string namec { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [StringLength(32)]
        public string icon { get; set; }

        /// <summary>
        /// 审批页面
        /// </summary>
        [StringLength(128)]
        public string url { get; set; }
    }
}