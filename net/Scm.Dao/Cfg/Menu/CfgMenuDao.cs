using Com.Scm.Dao.User;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Cfg
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_cfg_menu")]
    public class CfgMenuDao : ScmUserDataDao
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public long menu_id { get; set; }

        /// <summary>
        /// 展示名称
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
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