using Com.Scm.Dao.User;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Feedback
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_msg_feedback_header")]
    public class FeedbackHeaderDao : ScmUserDataDao
    {
        /// <summary>
        /// 反馈类型
        /// </summary>
        public FeedbackTypesEnums types { get; set; }

        /// <summary>
        /// 网址
        /// </summary>
        [StringLength(1024)]
        [SugarColumn(Length = 1024, IsNullable = true)]
        public string url { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [Required]
        [StringLength(128)]
        [SugarColumn(Length = 128)]
        public string title { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        [StringLength(1024)]
        [SugarColumn(Length = 1024, IsNullable = true)]
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