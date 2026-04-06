using Com.Scm.Dvo;

namespace Com.Scm.Sys.FeedbackDetail.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class FeedbackDetailDvo : ScmDataDvo
    {
        /// <summary>
        /// 反馈ID
        /// </summary>
        public long header_id { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        public string content { get; set; }
    }
}