using Com.Scm.Dao.User;
using Com.Scm.Sys.Enums;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Gtd
{
    /// <summary>
    /// 待办（明细）
    /// </summary>
    [SugarTable("scm_gtd_detail")]
    public class GtdDetailDao : ScmUserDataDao
    {
        /// <summary>
        /// 
        /// </summary>
        public long header_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long time { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [StringLength(256)]
        public string title { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(1024)]
        public string remark { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public ScmGtdPriorityEnum priority { get; set; }

        /// <summary>
        /// 提醒标识
        /// </summary>
        public ScmGtdRemindEnum remind { get; set; }

        /// <summary>
        /// 提示方式
        /// </summary>
        public ScmGtdNoticeEnum notice { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ScmGtdHandleEnum handle { get; set; }
    }
}
