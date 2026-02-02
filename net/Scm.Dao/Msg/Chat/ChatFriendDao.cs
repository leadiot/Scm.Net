using Com.Scm.Dao;
using Com.Scm.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Msg.Chat
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_msg_chat_friend")]
    public class ChatFriendDao : ScmDataDao
    {
        /// <summary>
        /// 好友ID
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 好友类型（人，群）
        /// </summary>
        public ScmChatFriendTypesEnum types { get; set; }

        /// <summary>
        /// 好友名称
        /// </summary>
        [Required]
        [StringLength(32)]
        [SugarColumn(Length = 32)]
        public string namec { get; set; }
    }
}