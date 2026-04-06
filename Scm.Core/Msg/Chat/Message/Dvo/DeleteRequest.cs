namespace Com.Scm.Msg.Chat.Message.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class DeleteRequest : ScmUpdateRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public long group_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long user { get; set; }
    }
}
