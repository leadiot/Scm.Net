using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm
{
    [SugarTable("scm_ver")]
    public class ScmVerDao : ScmDao
    {
        /// <summary>
        /// 关键字
        /// </summary>
        [Required]
        [StringLength(16)]
        [SugarColumn(Length = 16)]
        public string key { get; set; }

        /// <summary>
        /// 主版本
        /// </summary>
        public int major { get; set; }

        /// <summary>
        /// 子版本
        /// </summary>
        public int minor { get; set; }

        /// <summary>
        /// 修正版本
        /// </summary>
        public int patch { get; set; }

        /// <summary>
        /// 构建版本
        /// </summary>
        public string build { get; set; }

        /// <summary>
        /// 发行日期
        /// </summary>
        public string release_date { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public long update_time { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public long create_time { get; set; }
    }
}
