using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Res.Tag
{
    public class ScmResTagDto : ScmDataDto
    {
        /// <summary>
        /// 应用标识
        /// </summary>
        [Required]
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
