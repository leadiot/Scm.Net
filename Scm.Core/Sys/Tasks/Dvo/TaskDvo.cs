using Com.Scm.Dvo;
using Com.Scm.Enums;

namespace Com.Scm.Sys.Tasks.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class TaskDvo : ScmDataDvo
    {
        /// <summary>
        /// 导出类型
        /// </summary>
        public TaskTypesEnum types { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string codes { get; set; }
        /// <summary>
        /// 代码
        /// </summary>
        public string codec { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string names { get; set; }
        /// <summary>
        /// 类名
        /// </summary>
        public string clazz { get; set; }
        /// <summary>
        /// 报文
        /// </summary>
        public string json { get; set; }
        /// <summary>
        /// 文件
        /// </summary>
        public string file { get; set; }

        /// <summary>
        /// 作业状态
        /// </summary>
        public ScmHandleEnum handle { get; set; }
        /// <summary>
        /// 作业结果
        /// </summary>
        public ScmResultEnum result { get; set; }
        /// <summary>
        /// 提示信息
        /// </summary>
        public string message { get; set; }

        /// <summary>
        /// 预计执行时间（起始）
        /// </summary>
        public long need_time_f { get; set; }
        /// <summary>
        /// 预计执行时间（结束）
        /// </summary>
        public long need_time_t { get; set; }
        /// <summary>
        /// 实际执行时间（实际）
        /// </summary>
        public long exec_time_f { get; set; }
        /// <summary>
        /// 实际执行时间（结束）
        /// </summary>
        public long exec_time_t { get; set; }
    }
}