using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Res.Tag
{
    /// <summary>
    /// 标签
    /// </summary>
    [SugarTable("scm_res_tag")]
    public class ScmResTagDao : ScmDataDao
    {
        /// <summary>
        /// 应用标识
        /// </summary>
        public long app { get; set; }

        /// <summary>
        /// 应用标识
        /// </summary>
        [Required]
        [StringLength(32)]
        public string label { get; set; }

        /// <summary>
        /// 引用数量
        /// </summary>
        public int qty { get; set; }
    }
}
