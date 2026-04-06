using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Msg.Comment
{
    /// <summary>
    /// 
    /// </summary>
    public class CommentHeaderDto : ScmDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string codes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string codec { get; set; }
        /// <summary>
        /// 引用ID
        /// </summary>
        public long rid { get; set; }

        /// <summary>
        /// 主题说明
        /// </summary>
        [StringLength(256)]
        public string remark { get; set; }

        /// <summary>
        /// 评论数量
        /// </summary>
        public int qty { get; set; }

        /// <summary>
        /// 主题评分
        /// </summary>
        [Required]
        public int score { get; set; }
    }
}