using Com.Scm.Dao;
using SqlSugar;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Quartz.Dao
{
    /// <summary>
    /// 任务执行记录
    /// </summary>
    [Description("任务执行记录")]
    [SugarTable("scm_task_quarz_log")]
    public class QuarzTaskLogDao : ScmDao
    {
        /// <summary>
        /// 任务名
        /// </summary>
        [Description("任务名")]
        [StringLength(128)]
        public string task { get; set; }
        /// <summary>
        /// 分组名
        /// </summary>
        [Description("分组名")]
        [StringLength(128)]
        public string group { get; set; }
        /// <summary>
        /// 任务开始时间
        /// </summary>
        [Description("任务开始时间")]
        public DateTime begin_time { get; set; } = DateTime.Now;

        /// <summary>
        /// 任务结束时间
        /// </summary>
        [Description("任务结束时间")]
        [SugarColumn(IsNullable = true)]
        public DateTime? end_time { get; set; } = null;

        /// <summary>
        /// 任务执行结果
        /// </summary>
        [Description("任务执行结果")]
        public int result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(2048)]
        public string remark { get; set; }
    }
}
