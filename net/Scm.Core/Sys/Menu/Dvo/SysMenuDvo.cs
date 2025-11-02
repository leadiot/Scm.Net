using Com.Scm.Dvo;
using Com.Scm.Enums;

namespace Com.Scm.Sys.Menu.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class SysMenuDvo : ScmDataDvo
    {
        /// <summary>
        /// 菜单类型
        /// </summary>
        public ScmMenuTypesEnum types { get; set; }

        /// <summary>
        /// 显示语言
        /// </summary>
        public string lang { get; set; }

        /// <summary>
        /// 权限标识
        /// </summary>
        public string codec { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string namec { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public long pid { get; set; }

        /// <summary>
        /// 菜单层级
        /// </summary>
        public int layer { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 重定向
        /// </summary>
        public string redirect { get; set; }

        /// <summary>
        /// Vue文件路径
        /// </summary>
        public string view { get; set; }

        /// <summary>
        /// 菜单图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 高亮
        /// </summary>
        public string active { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string color { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 是否显示
        /// </summary>
        public bool visible { get; set; }
        /// <summary>
        /// 是否全屏
        /// </summary>
        public bool fullpage { get; set; }
        /// <summary>
        /// 是否缓存
        /// </summary>
        public bool keepAlive { get; set; }

        /// <summary>
        /// 接口权限
        /// </summary>
        public List<SysMenuApiUrl> api { get; set; } = new();
    }
}
