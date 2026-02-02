using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Msg.Comment
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_msg_comment_detail")]
    public class CommentDetailDao : ScmDataDao
    {
        /// <summary>
        /// 主题ID
        /// </summary>
        [Required]
        public long comment_id { get; set; }

        /// <summary>
        /// 评论
        /// </summary>
        [Required]
        [StringLength(1024)]
        [SugarColumn(Length = 1024)]
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