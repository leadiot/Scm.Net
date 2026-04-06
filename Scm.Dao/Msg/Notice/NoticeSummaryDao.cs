using Com.Scm.Dao;

namespace Com.Scm.Msg.Notice
{
    /// <summary>
    /// 邮件统计
    /// </summary>
    [SqlSugar.SugarTable("scm_msg_notice_summary")]
    public class NoticeSummaryDao : ScmDataDao
    {
        /// <summary>
        /// 总计
        /// </summary>
        public int total { get; set; }
        /// <summary>
        /// 未读
        /// </summary>
        public int unread { get; set; }
        /// <summary>
        /// 草稿
        /// </summary>
        public int draft { get; set; }
        /// <summary>
        /// 归档
        /// </summary>
        public int archived { get; set; }
        /// <summary>
        /// 删除
        /// </summary>
        public int deleted { get; set; }
    }
}
