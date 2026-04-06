using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Cfg.Export
{
    /// <summary>
    /// 导出配置明细
    /// </summary>
    [SugarTable("scm_cfg_export_detail")]
    public class ExportDetailDao : ScmDataDao
    {
        /// <summary>
        /// 
        /// </summary>
        public long export_id { get; set; }
        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }
        /// <summary>
        /// 列名称
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string col { get; set; }
        /// <summary>
        /// 展示名称
        /// </summary>
        [StringLength(64)]
        [SugarColumn(Length = 64, IsNullable = true)]
        public string namec { get; set; }
        /// <summary>
        /// 默认值
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string def { get; set; }
        /// <summary>
        /// 公式
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string fun { get; set; }
    }
}
