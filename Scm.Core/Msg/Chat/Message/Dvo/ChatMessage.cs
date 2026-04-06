using Com.Scm.Enums;

namespace Com.Scm.Msg.Chat.Message.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class ChatMessage 
    {
        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }
        /// <summary>
        /// 发送人
        /// </summary>
        public string user_id { get; set; }
        /// <summary>
        /// 会话群组
        /// </summary>
        public string chat_id { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public ScmChatMessageTypesEnums types { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string create_time { get; set; }
    }
}
