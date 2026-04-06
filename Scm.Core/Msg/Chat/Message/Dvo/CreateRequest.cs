using Com.Scm.Enums;

namespace Com.Scm.Msg.Chat.Message.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class CreateRequest : ScmUpdateRequest
    {
        /// <summary>
        /// 聊天类型
        /// </summary>
        public ScmChatGroupTypesEnum types { get; set; }

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
