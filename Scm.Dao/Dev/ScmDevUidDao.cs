using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Dev
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_sys_uid")]
    public class ScmDevUidDao : ScmDao
    {
        /// <summary>
        /// 键
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32, IsNullable = true)]
        public string k { get; set; }

        /// <summary>
        /// 当前值
        /// </summary>
        [Required]
        public long v { get; set; }

        /// <summary>
        /// 缓存大小
        /// </summary>
        [Required]
        public int c { get; set; } = 1;

        /// <summary>
        /// 缓冲大小
        /// </summary>
        [Required]
        public int b { get; set; } = 0;

        /// <summary>
        /// 数值长度
        /// </summary>
        [Required]
        public int l { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        [Required]
        public long t { get; set; }

        /// <summary>
        /// 前置掩码
        /// </summary>
        [StringLength(8)]
        [SugarColumn(Length = 8, IsNullable = true)]
        public string m { get; set; }

        /// <summary>
        /// 后置掩码
        /// </summary>
        [StringLength(8)]
        [SugarColumn(Length = 8, IsNullable = true)]
        public string p { get; set; }
    }
}