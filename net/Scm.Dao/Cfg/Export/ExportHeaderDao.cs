using Com.Scm.Dao.User;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Cfg.Export
{
    /// <summary>
    /// 导出配置头档
    /// </summary>
    [SqlSugar.SugarTable("scm_cfg_export_header")]
    public class ExportHeaderDao : ScmUserDataDao
    {
        /// <summary>
        /// 系统编码
        /// </summary>
        [Required]
        [StringLength(16)]
        [SugarColumn(Length = 16)]
        public string codes { get; set; }
        /// <summary>
        /// 用户编码
        /// </summary>
        [Required]
        [StringLength(128)]
        [SugarColumn(Length = 128)]
        public string codec { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        [Required]
        [StringLength(64)]
        [SugarColumn(Length = 64)]
        public string names { get; set; }
        /// <summary>
        /// 文件名称
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string file { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<ExportDetailDao> details { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="detail"></param>
        public void Add(ExportDetailDao detail)
        {
            if (details == null)
            {
                details = new List<ExportDetailDao>();
            }
            details.Add(detail);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="items"></param>
        public void Add(IEnumerable<ExportDetailDao> items)
        {
            if (details == null)
            {
                details = new List<ExportDetailDao>();
            }
            details.AddRange(items);
        }
    }
}
