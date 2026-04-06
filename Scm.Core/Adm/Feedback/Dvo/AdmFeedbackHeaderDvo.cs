using Com.Scm.Dvo;
using Com.Scm.Enums;

namespace Com.Scm.Adm.Feedback.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class AdmFeedbackHeaderDvo : ScmDataDvo
    {
        /// <summary>
        /// 
        /// </summary>
        public long user_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string user_names { get; set; }

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
        public ScmBoolEnum resolved { get; set; }

        /// <summary>
        /// 是否有新回复
        /// </summary>
        public bool system_reply { get; set; }

        /// <summary>
        /// 是否有客户回复
        /// </summary>
        public bool customer_reply { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<AdmFeedbackDetailDvo> details { get; set; }
    }
}
