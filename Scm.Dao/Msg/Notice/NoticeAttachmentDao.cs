using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Msg.Notice
{
    /// <summary>
    /// 附件
    /// </summary>
    [SugarTable("scm_msg_notice_attachment")]
    public class NoticeAttachmentDao : ScmDataDao
    {
        /// <summary>
        /// 
        /// </summary>
        public long notice_id { get; set; }

        /// <summary>
        /// 文件名称
        /// </summary>
        [Required]
        [StringLength(128)]
        [SugarColumn(Length = 128)]
        public string namec { get; set; }

        /// <summary>
        /// 文件地址
        /// </summary>
        [Required]
        [StringLength(1024)]
        [SugarColumn(Length = 1024)]
        public string url { get; set; }
    }
}
