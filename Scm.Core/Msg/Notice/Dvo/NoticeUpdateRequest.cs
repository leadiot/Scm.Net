namespace Com.Scm.Msg.Notice.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class NoticeUpdateRequest : ScmUpdateRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public long send_user { get; set; }
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
        public List<long> recipients { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<NoticeAttachmentDto> files { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public NoticeUpdateEnum operate { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum NoticeUpdateEnum
    {
        /// <summary>
        /// 
        /// </summary>
        None = 0,
        /// <summary>
        /// 草稿
        /// </summary>
        Save = 1,
        /// <summary>
        /// 发送
        /// </summary>
        Send = 2
    }
}
