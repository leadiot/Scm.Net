using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Feedback
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_msg_feedback_detail")]
    public class FeedbackDetailDao : ScmDataDao
    {
        /// <summary>
        /// 反馈ID
        /// </summary>
        public long header_id { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        [Required]
        [StringLength(1024)]
        [SugarColumn(Length = 1024)]
        public string content { get; set; }

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