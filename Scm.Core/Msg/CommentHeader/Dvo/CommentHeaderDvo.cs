using Com.Scm.Dvo;
using Com.Scm.Msg.CommentDetail.Dvo;

namespace Com.Scm.Msg.CommentHeader.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class CommentHeaderDvo : ScmDataDvo
    {
        /// <summary>
        /// 引用ID
        /// </summary>
        public long rid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string codec { get; set; }

        /// <summary>
        /// 主题说明
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 评论数量
        /// </summary>
        public int qty { get; set; }

        /// <summary>
        /// 主题评分
        /// </summary>
        public int score { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<CommentDetailDvo> details { get; set; }
    }
}