using Com.Scm.Dao.User;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Desktop
{
    /// <summary>
    /// 用户桌面图标
    /// </summary>
    [SugarTable("scm_cfg_desktop_icon")]
    public class ScmCfgDesktopIconDao : ScmUserDataDao
    {
        /// <summary>
        /// 权限标识
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string codec { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string namec { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string view { get; set; }

        /// <summary>
        /// 默认宽度
        /// </summary>
        public int width { get; set; }

        /// <summary>
        /// 默认高度
        /// </summary>
        public int height { get; set; }

        /// <summary>
        /// 是否可调整大小
        /// </summary>
        public bool resizable { get; set; }

        /// <summary>
        /// 居中显示
        /// </summary>
        public bool center { get; set; }

        /// <summary>
        /// 指示该项是否在桌面界面中可见。
        /// </summary>
        /// <remarks>用于控制桌面视图中的可见性，通常用于在不同平台或视图模式下切换显示。</remarks>
        public bool show_in_desktop { get; set; }

        /// <summary>
        /// 指示窗口是否显示在任务栏中。
        /// </summary>
        /// <remarks>仅对顶级窗口有效；对非顶级窗口或某些平台可能被忽略。</remarks>
        public bool show_in_taskbar { get; set; }
    }
}
