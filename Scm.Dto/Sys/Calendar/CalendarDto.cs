using Com.Scm.Dto;
using Com.Scm.Enums;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Sys.Calendar
{
    public class CalendarDto : ScmDataDto
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
        /// 日程等级
        /// </summary>
        [Required]
        public int level { get; set; }

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
        /// 参与人列表
        /// </summary>
        public List<long> users { get; set; }

        /// <summary>
        /// 提醒设置
        /// </summary>
        public CalendarRemindEnum remind_type { get; set; }
        public int remind_time { get; set; }

        /// <summary>
        /// 重复方式
        /// </summary>
        public CalendarRepeatEnum repeat_type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int repeat_time { get; set; }
    }
}
