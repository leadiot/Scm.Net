using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm
{
    [SugarTable("scm_ver")]
    public class ScmVerDao : ScmDao
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public new long id { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        [Required]
        [StringLength(16)]
        [SugarColumn(Length = 16)]
        public string key { get; set; }

        /// <summary>
        /// 当前版本
        /// </summary>
        public int ver { get; set; }

        /// <summary>
        /// 发行日期
        /// </summary>
        public string date { get; set; }

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
