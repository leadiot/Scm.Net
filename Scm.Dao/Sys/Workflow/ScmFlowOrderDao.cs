using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Flow
{
    /// <summary>
    /// 单据流程表配置信息
    /// </summary>
    [SugarTable("scm_flow_order")]
    public class ScmFlowOrderDao : ScmDataDao
    {
        /// <summary>
        /// 流程ID
        /// </summary>
        public long flow_id { get; set; }

        /// <summary>
        /// 单据代码
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string codec { get; set; }

        /// <summary>
        /// 单据名称
        /// </summary>
        [Required]
        [StringLength(128)]
        [SugarColumn(Length = 128)]
        public string namec { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string icon { get; set; }

        /// <summary>
        /// 审批页面
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string url { get; set; }
    }
}