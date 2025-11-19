using Com.Scm.Dao;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Dev.Icon
{
    /// <summary>
    /// 图标分类
    /// </summary>
    [SqlSugar.SugarTable("scm_res_icon_cat")]
    public class ScmDevIconCatDao : ScmDataDao
    {
        /// <summary>
        /// 分类代码
        /// </summary>
        [StringLength(16)]
        public string code { get; set; }

        /// <summary>
        /// 分类名称
        /// </summary>
        [StringLength(32)]
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
