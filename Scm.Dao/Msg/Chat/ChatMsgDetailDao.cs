using Com.Scm.Dao;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Msg.Chat
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_msg_chat_detail")]
    public class ChatMsgDetailDao : ScmDataDao
    {
        /// <summary>
        /// 接收人
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
        [SugarColumn(Length = 1024, IsNullable = true)]
        public string content { get; set; }
    }
}