using Com.Scm.Dao;
using Com.Scm.Enums;
using SqlSugar;

namespace Com.Scm.Msg.Notice
{
    /// <summary>
    /// 发件人
    /// </summary>
    [SugarTable("scm_msg_notice_sender")]
    public class NoticeSenderDao : ScmDataDao
    {
        /// <summary>
        /// 
        /// </summary>
        public long notice_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 消息状态
        /// </summary>
        public ScmHandleEnum handle { get; set; }

        /// <summary>
        /// 阅读次数
        /// </summary>
        public int read_qty { get; set; }

        /// <summary>
        /// 回复次数
        /// </summary>
        public int reply_qty { get; set; }

        /// <summary>
        /// 是否归档
        /// </summary>
        public bool is_arc { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool is_del { get; set; }
    }
}
