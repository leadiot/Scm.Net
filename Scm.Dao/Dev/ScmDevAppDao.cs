using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Dev
{
    /// <summary>
    /// App信息
    /// </summary>
    [SugarTable("scm_sys_app")]
    public class ScmDevAppDao : ScmDataDao
    {
        /// <summary>
        /// 应用类型
        /// </summary>
        [Required]
        public int types { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 应用代码
        /// </summary>
        [Required]
        [StringLength(16)]
        [SugarColumn(Length = 16)]
        public string code { get; set; }

        /// <summary>
        /// 应用名称
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string name { get; set; }

        /// <summary>
        /// 主版本号
        /// </summary>
        public int major { get; set; }

        /// <summary>
        /// 子版本号
        /// </summary>
        public int minor { get; set; }

        /// <summary>
        /// 修订版本号
        /// </summary>
        public int patch { get; set; }

        /// <summary>
        /// 构建版本号，默认自增
        /// </summary>
        public int build { get; set; }

        /// <summary>
        /// 应用简介
        /// </summary>
        [StringLength(1024)]
        [SugarColumn(Length = 1024, IsNullable = true)]
        public string remark { get; set; }
    }
}
