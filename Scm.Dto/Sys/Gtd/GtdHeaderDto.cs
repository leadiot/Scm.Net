using Com.Scm.Dto;
using Com.Scm.Sys.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Gtd
{
    /// <summary>
    /// 待办
    /// </summary>
    public class GtdHeaderDto : ScmDataDto
    {
        /// <summary>
        /// 
        /// </summary>
        public long user_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public long cat_id { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        [StringLength(256)]
        public string title { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public ScmGtdPriorityEnum priority { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [StringLength(1024)]
        public string remark { get; set; }

        /// <summary>
        /// 提醒标识
        /// </summary>
        public ScmGtdRemindEnum remind { get; set; }

        /// <summary>
        /// 表达式
        /// </summary>
        [StringLength(128)]
        public string cron { get; set; }

        /// <summary>
        /// 提示方式
        /// </summary>
        public ScmGtdNoticeEnum notice { get; set; }

        /// <summary>
        /// 上次提醒时间
        /// </summary>
        public long last_time { get; set; }

        /// <summary>
        /// 下次提醒时间
        /// </summary>
        public long next_time { get; set; }

        /// <summary>
        /// 作业状态
        /// </summary>
        public ScmGtdHandleEnum handle { get; set; }
    }
}