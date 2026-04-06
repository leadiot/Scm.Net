using Com.Scm.Dto;
using Com.Scm.Workflow;

namespace Com.Scm.Sys
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmFlowDataDetailDto : ScmDataDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long header_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 审批类型（主审、或审、抄送）
        /// </summary>
        public FlowDataDetailEnum types { get; set; }
    }
}