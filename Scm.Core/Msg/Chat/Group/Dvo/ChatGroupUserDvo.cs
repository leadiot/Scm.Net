using Com.Scm.Dvo;

namespace Com.Scm.Msg.Chat.Group.Dvo
{
    /// <summary>
    /// 群组人员
    /// </summary>
    public class ChatGroupUserDvo : ScmDataDvo
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        public long group_id { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        public long user_id { get; set; }
    }
}