using Com.Scm.Dto;
using Com.Scm.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Feedback
{
    /// <summary>
    /// 
    /// </summary>
    public class FeedbackHeaderDto : ScmDataDto
    {
        /// <summary>
        /// 反馈类型
        /// </summary>
        public FeedbackTypesEnums types { get; set; }

        /// <summary>
        /// 网址
        /// </summary>
        [StringLength(1024)]
        public string url { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [StringLength(128)]
        public string title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [StringLength(1024)]
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
    }
}