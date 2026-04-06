using Com.Scm.Response;
using Com.Scm.Ur.User.Dvo;

namespace Com.Scm.Msg.Notice.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class ReadEditResponse : ScmApiResponse
    {
        /// <summary>
        /// 
        /// </summary>
        public long send_time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long send_user { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public SimpleUserDvo sender { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<NoticeRecipientDvo> recipients { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<NoticeAttachmentDto> files { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long create_time { get; set; }
    }
}
