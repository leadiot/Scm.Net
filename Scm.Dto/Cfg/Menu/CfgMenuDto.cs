using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Cfg
{
    /// <summary>
    /// 
    /// </summary>
    public class CfgMenuDto : ScmDataDto
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
        [StringLength(32)]
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