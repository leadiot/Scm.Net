using Com.Scm.Dao.User;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Cfg.UserTheme
{
    /// <summary>
    /// 用户风格配置
    /// </summary>
    [SqlSugar.SugarTable("scm_cfg_user_theme")]
    public class UserThemeDao : ScmUserDataDao
    {
        /// <summary>
        /// 模式
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 16, IsNullable = true)]
        public string light { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 16, IsNullable = true)]
        public string color { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 16, IsNullable = true)]
        public string lang { get; set; }

        /// <summary>
        /// 布局
        /// </summary>
        [StringLength(32)]
        [SugarColumn(Length = 16, IsNullable = true)]
        public string layout { get; set; }

        /// <summary>
        /// 菜单展示方式
        /// </summary>
        public int menu { get; set; }

        /// <summary>
        /// 标签展示方式
        /// </summary>
        public int tabs { get; set; }

        /// <summary>
        /// 首页展示方式
        /// </summary>
        public int home { get; set; }
    }
}
