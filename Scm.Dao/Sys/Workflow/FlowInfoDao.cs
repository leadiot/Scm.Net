using Com.Scm.Dao;
using Com.Scm.Workflow;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Workflow
{
    /// <summary>
    /// 工作流信息
    /// </summary>
    [SugarTable("scm_flow_info")]
    public class FlowInfoDao : ScmDataDao
    {
        /// <summary>
        /// 适用APP
        /// </summary>
        public long app_id { get; set; }

        /// <summary>
        /// 流程类型
        /// </summary>
        [Required]
        public FlowInfoTypesEnum types { get; set; }

        /// <summary>
        /// 流程名称
        /// </summary>
        [Required]
        [StringLength(256)]
        [SugarColumn(Length = 256)]
        public string title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string icon { get; set; }

        /// <summary>
        /// 审批被拒后状态
        /// </summary>
        [Required]
        public int refused { get; set; }

        /// <summary>
        /// 流程说明
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string summary { get; set; }
    }
}