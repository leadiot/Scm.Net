using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Adm.Feedback.Dao
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_sys_feedback_detail")]
    public class AdmFeedbackDetailDao : ScmDataDao
    {
        /// <summary>
        /// 反馈ID
        /// </summary>
        public long header_id { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        [StringLength(1024)]
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