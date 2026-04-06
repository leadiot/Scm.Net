using Com.Scm.Dvo;
using Com.Scm.Workflow;

namespace Com.Scm.Sys.FlowInfo.Dvo
{
    /// <summary>
    /// 工作流
    /// </summary>
    public class SysFlowInfoDvo : ScmDataDvo
    {
        /// <summary>
        /// 流程类型
        /// </summary>
        public FlowInfoTypesEnum types { get; set; }

        /// <summary>
        /// 流程名称
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 审批被拒后状态
        /// </summary>
        public int refused { get; set; } = 1;

        /// <summary>
        /// 流程说明
        /// </summary>
        public string summary { get; set; }

        /// <summary>
        /// 具体流程Json
        /// </summary>
        public string flow { get; set; }
    }
}