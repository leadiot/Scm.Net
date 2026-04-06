using Com.Scm.Dao;
using Com.Scm.Workflow;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys
{
    /// <summary>
    /// 审批结果
    /// </summary>
    [SugarTable("scm_flow_data_result")]
    public class ScmFlowDataResultDao : ScmDataDao
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
        [SugarColumn(Length = 256, IsNullable = true)]
        public string comment { get; set; }
    }
}