using Com.Scm.Dto;
using Com.Scm.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Msg.Chat
{
    /// <summary>
    /// 
    /// </summary>
    public class ChatFriendDto : ScmDataDto
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
        [StringLength(32)]
        public string namec { get; set; }
    }
}