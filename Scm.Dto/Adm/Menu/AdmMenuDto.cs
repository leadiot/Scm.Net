using Com.Scm.Dto;
using Com.Scm.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Adm.Menu
{
    public class AdmMenuDto : ScmDataDto
    {
        public const long HOME_ID = 1000000000000000010L;
        public const long FAV_ID = 1000000000000000020L;

        /// <summary>
        /// 应用
        /// </summary>
        [Required]
        public ScmClientTypeEnum client { get; set; }

        /// <summary>
        /// 菜单类型
        /// </summary>
        [Required]
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
        [Required]
        [StringLength(32)]
        public string namec { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public long pid { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        public string uri { get; set; }

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
        /// 菜单层级
        /// </summary>
        [Required]
        public int layer { get; set; } = 1;

        /// <summary>
        /// 显示排序
        /// </summary>
        [Required]
        public int od { get; set; } = 1;

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool visible { get; set; }

        /// <summary>
        /// 是否使能
        /// </summary>
        public bool enabled { get; set; }

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

    /// <summary>
    /// 接口权限
    /// </summary>
    public class SysMenuApiUrl
    {
        /// <summary>
        /// 权限标识
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 方法
        /// </summary>
        public string method { get; set; }

        /// <summary>
        /// Api url
        /// </summary>
        public string url { get; set; }
    }
}
