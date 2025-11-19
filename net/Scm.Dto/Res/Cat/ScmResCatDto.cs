using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Res.Cat
{
    /// <summary>
    /// 类别
    /// </summary>
    public class ScmResCatDto : ScmDataDto
    {
        /// <summary>
        /// 
        /// </summary>
        public const long SYS_ID = 1000000000000000001L;

        /// <summary>
        /// 应用标识
        /// </summary>
        [Required]
        public long app { get; set; }

        /// <summary>
        /// 层级
        /// </summary>
        public int lv { get; set; }

        /// <summary>
        /// 显示排序
        /// </summary>
        public int od { get; set; }

        /// <summary>
        /// 语言
        /// </summary>
        [Required]
        public int lang { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [StringLength(32)]
        public string codec { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        [StringLength(32)]
        public string namec { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        [StringLength(256)]
        public string image { get; set; }

        /// <summary>
        /// 上级ID
        /// </summary>
        public long pid { get; set; }

        /// <summary>
        /// 顶级ID
        /// </summary>
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
        /// 
        /// </summary>
        public string uri { get; set; }
    }
}