using Com.Scm.Dvo;
using Com.Scm.Enums;
using Com.Scm.Sys.FeedbackDetail.Dvo;

namespace Com.Scm.Sys.FeedbackHeader.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class FeedbackHeaderDvo : ScmDataDvo
    {
        /// <summary>
        /// 反馈类型
        /// </summary>
        public FeedbackTypesEnums types { get; set; }

        /// <summary>
        /// 网址
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 处理状态
        /// </summary>
        public FeedbackHandleEnums handle { get; set; }

        /// <summary>
        /// 解决状态
        /// </summary>
        public int resolve { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<FeedbackDetailDvo> details { get; set; }
    }
}