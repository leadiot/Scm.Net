using Com.Scm.Dao;
using Com.Scm.Enums;
using Com.Scm.Utils;
using SqlSugar;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Log
{
    /// <summary>
    /// 前端日志
    /// </summary>
    [SugarTable("scm_log_fe")]
    public class LogFeDao : ScmDao
    {
        [StringLength(10)]
        [SugarColumn(Length = 10, IsNullable = false)]
        public string date { get; set; }
        /// <summary>
        /// 日志时间
        /// </summary>
        public long time { get; set; }

        /// <summary>
        /// 日志级别：debug | info | warn | error
        /// </summary>
        [StringLength(8)]
        [SugarColumn(Length = 8, IsNullable = false)]
        public ScmLogLevelEnum level { get; set; }

        /// <summary>
        /// 日志模块：app | code | http | biz | global
        /// </summary>
        [StringLength(8)]
        [SugarColumn(Length = 8, IsNullable = false)]
        public string category { get; set; }

        /// <summary>
        /// 日志消息
        /// </summary>
        [StringLength(256)]
        [SugarColumn(Length = 256, IsNullable = false)]
        public string message { get; set; }

        /// <summary>
        /// 堆栈（可能为空字符串）
        /// </summary>
        [StringLength(2048)]
        [SugarColumn(Length = 2048, IsNullable = true)]
        public string stack { get; set; }

        /// <summary>
        /// 发生时的页面路径
        /// </summary>
        [StringLength(1024)]
        [SugarColumn(Length = 8, IsNullable = false)]
        public string url { get; set; }

        public override void PrepareCreate(long userId)
        {
            base.PrepareCreate(userId);

            var time = TimeUtils.GetDateTime(this.time);
            this.date = TimeUtils.FormatDate(time);
        }
    }
}
