using Com.Scm.Dto;
using Com.Scm.Workflow;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Workflow
{
    public class SysFlowInfoDto : ScmDataDto
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
        public string title { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [Required]
        [StringLength(32)]
        public string icon { get; set; }

        /// <summary>
        /// 审批被拒后状态
        /// </summary>
        [Required]
        public int refused { get; set; } = 1;

        /// <summary>
        /// 流程说明
        /// </summary>
        [StringLength(128)]
        public string summary { get; set; }
    }
}
