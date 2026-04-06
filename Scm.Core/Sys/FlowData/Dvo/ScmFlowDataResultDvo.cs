using Com.Scm.Dvo;
using Com.Scm.Workflow;

namespace Com.Scm.Sys.FlowData.Dvo
{
    public class ScmFlowDataResultDvo : ScmDataDvo
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
        public string remark { get; set; }
    }
}
