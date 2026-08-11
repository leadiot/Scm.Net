using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.I18n
{
    /// <summary>
    /// 全局多语言翻译表
    /// </summary>
    [SugarTable("scm_sys_i18n")]
    public class ScmSysI18nDao : ScmDataDao
    {
        /// <summary>
        /// 所属模块：menu/dic/uom/error/field 等
        /// </summary>
        [Required]
        [StringLength(16)]
        [SugarColumn(Length = 16, IsNullable = false)]
        public string module { get; set; }

        /// <summary>
        /// 翻译键，命名规范 {module}.{business}.{field}
        /// 示例：menu.sys.user.name
        /// </summary>
        [Required]
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = false)]
        public string key { get; set; }

        /// <summary>
        /// 语言代码：zh-cn / en / ja 等
        /// </summary>
        [Required]
        [StringLength(8)]
        [SugarColumn(Length = 8, IsNullable = false)]
        public string lang { get; set; }

        /// <summary>
        /// 翻译文本
        /// </summary>
        [Required]
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = false)]
        public string value { get; set; }
    }
}
