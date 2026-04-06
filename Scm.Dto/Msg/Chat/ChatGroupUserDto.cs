using Com.Scm.Dto;

namespace Com.Scm.Msg.Chat
{
    /// <summary>
    /// 群组人员
    /// </summary>
    public class ChatGroupUserDto : ScmDataDto
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