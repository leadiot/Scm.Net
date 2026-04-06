using Com.Scm.Dvo;

namespace Com.Scm.Msg.CommentDetail.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class CommentDetailDvo : ScmDataDvo
    {
        /// <summary>
        /// 评论
        /// </summary>
        public string comment { get; set; }

        /// <summary>
        /// 回复ID
        /// </summary>
        public long rid { get; set; }

        /// <summary>
        /// 引用ID
        /// </summary>
        public long pid { get; set; }

        /// <summary>
        /// 评分
        /// </summary>
        public int score { get; set; }
        /// <summary>
        /// 点赞数量
        /// </summary>
        public int likes { get; set; }
        /// <summary>
        /// 回复数量
        /// </summary>
        public int reply { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string namec { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string avatar { get; set; }
    }
}