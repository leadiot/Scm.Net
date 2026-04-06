using Com.Scm.Dto;
using Com.Scm.Workflow;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmFlowDataResultDto : ScmDataDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long flow_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long data_id { get; set; }

        /// <summary>
        /// 审批结果
        /// </summary>
        public FlowDataResultEnum result { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(256)]
        public string remark { get; set; }
    }
}