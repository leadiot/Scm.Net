using Com.Scm.Dto;

namespace Com.Scm.Cfg.UserTheme
{
    public class UserThemeDto : ScmDataDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 模式
        /// </summary>
        public string light { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string color { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        public string lang { get; set; }

        /// <summary>
        /// 布局
        /// </summary>
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
