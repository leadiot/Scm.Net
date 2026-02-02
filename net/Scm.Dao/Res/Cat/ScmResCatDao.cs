using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Res.Cat
{
    /// <summary>
    /// 类别
    /// </summary>
    [SugarTable("scm_res_cat")]
    public class ScmResCatDao : ScmDataDao
    {
        /// <summary>
        /// 应用标识
        /// </summary>
        [Required]
        public long app { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        [Required]
        public int lv { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        [Required]
        public int od { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        [Required]
        public int lang { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string codec { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string namec { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = true)]
        public string image { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        [Required]
        public long pid { get; set; }

        /// <summary>
        /// 顶级ID
        /// </summary>
        [Required]
        public long tid { get; set; }

        /// <summary>
        /// 引用数量
        /// </summary>
        [Required]
        public int qty { get; set; }

        /// <summary>
        /// 引用ID
        /// </summary>
        public long ref_id { get; set; }

        /// <summary>
        /// 跳转链接
        /// </summary>
        [StringLength(128)]
        [SugarColumn(Length = 128, IsNullable = true)]
        public string uri { get; set; }
    }
}
