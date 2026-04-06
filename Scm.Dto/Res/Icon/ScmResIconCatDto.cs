using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Res.Icon
{
    public class ScmResIconCatDto : ScmDataDto
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
        /// 数量
        /// </summary>
        public int qty { get; set; }
    }
}
