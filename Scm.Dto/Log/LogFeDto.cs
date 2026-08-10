using Com.Scm.Dto;
using System.ComponentModel.DataAnnotations;

namespace Com.Scm.Log
{
    public class LogFeDto : ScmDto
    {
        /// <summary>
        /// 日志时间
        /// </summary>
        public long time { get; set; }

        /// <summary>
        /// 日志级别：debug | info | warn | error
        /// </summary>
        [StringLength(8)]
        public string level { get; set; }

        /// <summary>
        /// 日志模块：app | code | http | biz | global
        /// </summary>
        [StringLength(8)]
        public string category { get; set; }

        /// <summary>
        /// 日志消息
        /// </summary>
        [StringLength(256)]
        public string message { get; set; }

        /// <summary>
        /// 堆栈（可能为空字符串）
        /// </summary>
        [StringLength(2048)]
        public string stack { get; set; }

        /// <summary>
        /// 发生时的页面路径
        /// </summary>
        [StringLength(1024)]
        public string url { get; set; }
    }
}
