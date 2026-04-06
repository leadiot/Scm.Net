using Com.Scm.Dvo;

namespace Com.Scm.Cfg
{
    /// <summary>
    /// 
    /// </summary>
    public class CfgMenuDvo : ScmDataDvo
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 菜单ID
        /// </summary>
        public long menu_id { get; set; }

        /// <summary>
        /// 展示名称
        /// </summary>
        public string namec { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public long pid { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 访问频次
        /// </summary>
        public int qty { get; set; }
    }
}