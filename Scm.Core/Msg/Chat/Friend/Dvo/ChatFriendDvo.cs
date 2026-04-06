using Com.Scm.Dvo;
using Com.Scm.Enums;

namespace Com.Scm.Msg.Chat.Friend.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class ChatFriendDvo : ScmDataDvo
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
        public string namec { get; set; }
    }
}