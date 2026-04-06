using Com.Scm.Dvo;
using Com.Scm.Enums;
using Com.Scm.Ur.User.Dvo;

namespace Com.Scm.Msg.Notice.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class NoticeDvo : ScmDataDvo
    {
        /// <summary>
        /// 
        /// </summary>
        public ScmHandleEnum handle { get; set; }

        /// <summary>
        /// 发送人
        /// </summary>
        public long send_user { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SimpleUserDvo sender { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long send_time { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        public List<NoticeRecipientDvo> recipients { get; set; }

        /// <summary>
        /// 通知标题
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 通知内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 附件内容支持多个,保存相对位置，例如 /upload/files/1.txt
        /// </summary>
        public int files { get; set; }

        /// <summary>
        /// 是否归档
        /// </summary>
        public bool is_arc { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool is_del { get; set; }

        /// <summary>
        /// 是否已读
        /// </summary>
        public bool unread { get; set; }

        /// <summary>
        /// 引用通知
        /// </summary>
        public long ref_id { get; set; }
    }
}
