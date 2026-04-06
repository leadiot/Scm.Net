using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Msg.Comment
{
    /// <summary>
    /// 
    /// </summary>
    public class CommentDetailDto : ScmDto
    {
        /// <summary>
        /// 评论
        /// </summary>
        [StringLength(1024)]
        public string comment { get; set; }

        /// <summary>
        /// 回复ID
        /// </summary>
        [Required]
        public long rid { get; set; }

        /// <summary>
        /// 引用ID
        /// </summary>
        [Required]
        public long pid { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        [Required]
        public int score { get; set; }
        /// <summary>
        /// 点赞数量
        /// </summary>
        [Required]
        public int likes { get; set; }
        /// <summary>
        /// 回复数量
        /// </summary>
        [Required]
        public int reply { get; set; }
    }
}