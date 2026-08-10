using Com.Scm.Dvo;
using Com.Scm.Enums;

namespace Com.Scm.Log.Fe.Dvo
{
    public class ScmLogFeDvo : ScmDvo
    {
        public string date { get; set; }
        /// <summary>
        /// 日志时间
        /// </summary>
        public long time { get; set; }

        /// <summary>
        /// 日志级别：debug | info | warn | error
        /// </summary>
        public ScmLogLevelEnum level { get; set; }
        public string LevelName { get; set; }

        /// <summary>
        /// 日志模块：app | code | http | biz | global
        /// </summary>
        public string category { get; set; }

        /// <summary>
        /// 日志消息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 堆栈（可能为空字符串）
        /// </summary>
        public string stack { get; set; }

        /// <summary>
        /// 发生时的页面路径
        /// </summary>
        public string url { get; set; }
    }
}
