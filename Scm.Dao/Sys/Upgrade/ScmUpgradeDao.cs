using Com.Scm.Dao;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys
{
    [SugarTable("scm_sys_upgrade")]
    public class ScmUpgradeDao : ScmDataDao
    {
        public int major { get; set; }

        public int minor { get; set; }

        public int patch { get; set; }

        public int build { get; set; }

        [Required]
        [StringLength(64)]
        [SugarColumn(Length = 64)]
        public string ver_info { get; set; }

        [Required]
        [StringLength(64)]
        [SugarColumn(Length = 64)]
        public string ver_code { get; set; }

        [Required]
        [StringLength(64)]
        [SugarColumn(Length = 64)]
        public string ver_date { get; set; }

        [SugarColumn(ColumnDataType = "tinyint", IsNullable = false)]
        public ScmPhaseEnum phase { get; set; }

        public bool forced { get; set; }

        /// <summary>
        /// 当前版本
        /// </summary>
        public bool current { get; set; }

        [StringLength(512)]
        [SugarColumn(Length = 512, IsNullable = true)]
        public string url { get; set; }

        public int size { get; set; }

        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string hash { get; set; }

        [StringLength(2048)]
        [SugarColumn(Length = 2048, IsNullable = true)]
        public string remark { get; set; }

        [StringLength(64)]
        [SugarColumn(Length = 64, IsNullable = true)]
        public string ver_min { get; set; }

        [StringLength(64)]
        [SugarColumn(Length = 64, IsNullable = true)]
        public string ver_max { get; set; }

        /// <summary>
        /// 本地下载路径
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string file { get; set; }

        public ScmHandleEnum handle { get; set; }

        public ScmResultEnum result { get; set; }
    }
}