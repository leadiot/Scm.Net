using System.ComponentModel;

namespace Com.Scm.Enums
{
    public enum ScmHandleEnum
    {
        None = 0,
        /// <summary>
        /// 初始化
        /// </summary>
        [Description("初始化")]
        Todo = 1,
        /// <summary>
        /// 进行中
        /// </summary>
        [Description("执行中")]
        Doing = 2,
        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Done = 3
    }
}
