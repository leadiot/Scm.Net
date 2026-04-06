using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Msg.Chat
{
    /// <summary>
    /// 
    /// </summary>
    [SugarTable("scm_msg_chat_header")]
    public class ChatMsgHeaderDao : ScmDataDao
    {
        /// <summary>
        /// 
        /// </summary>
        public long group_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(64)]
        [SugarColumn(Length = 64, IsNullable = true)]
        public string namec { get; set; }
    }
}
