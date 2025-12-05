using Com.Scm.Dvo;

namespace Com.Scm.Quartz.Dvo
{
    public class QuartzTaskLogDvo : ScmDvo
    {
        /// <summary>
        /// 任务名
        /// </summary>
        public string task { get; set; }
        /// <summary>
        /// 分组名
        /// </summary>
        public string group { get; set; }
        /// <summary>
        /// 任务开始时间
        /// </summary>
        public DateTime begin_time { get; set; } = DateTime.Now;

        /// <summary>
        /// 任务结束时间
        /// </summary>
        public DateTime? end_time { get; set; } = null;

        /// <summary>
        /// 任务执行结果
        /// </summary>
        public int result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string remark { get; set; }
    }
}
