using Com.Scm.Dvo;
using Com.Scm.Enums;

namespace Com.Scm.Msg.Chat.Message.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class ChatDetailDvo : ScmDataDvo
    {
        /// <summary>
        /// 发送人
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 接收人
        /// </summary>
        public long group_id { get; set; }

        /// <summary>
        /// 消息类型（文本、图片、音频、文件）
        /// </summary>
        public ScmChatMessageTypesEnums types { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string content { get; set; }
    }
}