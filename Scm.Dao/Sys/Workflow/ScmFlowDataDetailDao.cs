using Com.Scm.Dao;
using Com.Scm.Workflow;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys
{
    /// <summary>
    /// 审批单据-人员
    /// </summary>
    [SugarTable("scm_flow_data_detail")]
    public class ScmFlowDataDetailDao : ScmDataDao
    {
        /// <summary>
        /// 审批ID
        /// </summary>
        public long header_id { get; set; }

        /// <summary>
        /// 节点ID
        /// </summary>
        public long node_id { get; set; }

        /// <summary>
        /// 审批人ID
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 审批类型（主审、或审、抄送）
        /// </summary>
        public FlowDataDetailEnum types { get; set; }

        /// <summary>
        /// 审批结果
        /// </summary>
        public FlowDataResultEnum wfa_result { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string comment { get; set; }
    }
}