using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Desktop.Theme
{
    /// <summary>
    /// 桌面主题
    /// </summary>
    [SugarTable("scm_sys_theme_desktop")]
    public class ThemeDesktopDao : ScmDataDao
    {
        /// <summary>
        /// 风格ID
        /// </summary>
        public int style_id { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 主题名称
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string names { get; set; }

        /// <summary>
        /// 主题内容
        /// </summary>
        [Required]
        [StringLength(1024)]
        [SugarColumn(Length = 1024)]
        public string theme { get; set; }

        /// <summary>
        /// 预览内容
        /// </summary>
        public string preview { get; set; }
    }
}
