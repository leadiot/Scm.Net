using Com.Scm.Dao;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Config
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_sys_config_key")]
    public class ConfigKeyDao : ScmDataDao
    {
        /// <summary>
        /// 分类
        /// </summary>
        public long cat_id { get; set; }
        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }
        /// <summary>
        /// 键
        /// </summary>
        [Required]
        [StringLength(16)]
        [SugarColumn(Length = 16)]
        public string key { get; set; }
        /// <summary>
        /// 值
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string value { get; set; }
        /// <summary>
        /// 终端类型
        /// </summary>
        public ScmClientTypeEnum client { get; set; }
        /// <summary>
        /// 数据类型
        /// </summary>
        public ScmDataTypeEnum date { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string remark { get; set; }
    }
}
