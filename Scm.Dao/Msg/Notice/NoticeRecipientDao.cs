using Com.Scm.Dao;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Msg.Notice
{
    /// <summary>
    /// 收件人
    /// </summary>
    [SqlSugar.SugarTable("scm_msg_notice_recipient")]
    public class NoticeRecipientDao : ScmDataDao
    {
        /// <summary>
        /// 通知编号
        /// </summary>
        [Required]
        public long notice_id { get; set; }

        /// <summary>
        /// 用户编号
        /// </summary>
        [Required]
        public long user_id { get; set; }

        /// <summary>
        /// 读取次数
        /// </summary>
        [Required]
        public int read_qty { get; set; }
    }
}
