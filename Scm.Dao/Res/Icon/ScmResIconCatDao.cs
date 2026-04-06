using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Res.Icon
{
    /// <summary>
    /// 图标分类
    /// </summary>
    [SugarTable("scm_res_icon_cat")]
    public class ScmResIconCatDao : ScmDataDao
    {
        /// <summary>
        /// 分类代码
        /// </summary>
        [Required]
        [StringLength(16)]
        [SugarColumn(Length = 16)]
        public string code { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string name { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public long pid { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int qty { get; set; }
    }
}
