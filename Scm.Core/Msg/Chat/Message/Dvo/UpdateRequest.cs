namespace Com.Scm.Msg.Chat.Message.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateRequest : ScmUpdateRequest
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        public List<long> users { get; set; }

        /// <summary>
        /// 群组名称
        /// </summary>
        public string namec { get; set; }
    }
}
