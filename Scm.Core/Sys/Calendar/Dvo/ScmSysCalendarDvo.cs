using Com.Scm.Dvo;
using Com.Scm.Ur.User.Dvo;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Calendar.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class ScmSysCalendarDvo : ScmDataDvo
    {
        /// <summary>
        /// 日程标题
        /// </summary>
        [Required]
        [StringLength(256)]
        public string title { get; set; }

        /// <summary>
        /// 日程类型
        /// </summary>
        [Required]
        public int types { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string types_names { get; set; }

        /// <summary>
        /// 日程等级
        /// </summary>
        [Required]
        public int level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string level_names { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        [Required]
        public long start_time { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        [Required]
        public long end_time { get; set; }

        /// <summary>
        /// 提醒设置
        /// </summary>
        public int remind_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int remind_time { get; set; }

        /// <summary>
        /// 参与人列表
        /// </summary>
        public List<long> user_ids { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<SimpleUserDvo> users { get; set; }
    }
}
