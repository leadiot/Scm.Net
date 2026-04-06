using Com.Scm.Dto;
using Com.Scm.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Msg.Chat
{
    /// <summary>
    /// 
    /// </summary>
    public class ChatMsgDetailDto : ScmDataDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long header_id { get; set; }

        /// <summary>
        /// 发送人
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 消息类型（文本、图片、音频、文件）
        /// </summary>
        public ScmChatMessageTypesEnums types { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        [StringLength(1024)]
        public string content { get; set; }
    }
}