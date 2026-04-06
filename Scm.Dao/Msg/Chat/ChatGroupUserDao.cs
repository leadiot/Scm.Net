using Com.Scm.Dao;
using SqlSugar;

namespace Com.Scm.Msg.Chat
{
    /// <summary>
    /// 群组人员
    /// </summary>
    [SugarTable("scm_msg_chat_group_user")]
    public class ChatGroupUserDao : ScmDataDao
    {
        /// <summary>
        /// 群组ID
        /// </summary>
        public long group_id { get; set; }

        /// <summary>
        /// 人员ID
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 上次读取时间
        /// </summary>
        public long last_time { get; set; }
    }
}