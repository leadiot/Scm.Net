using System.ComponentModel;

namespace Com.Scm.Enums
{
    public enum ScmHandleEnum
    {
        [Description("默认")]
        None = 0,
        /// <summary>
        /// 初始化
        /// </summary>
        [Description("初始化")]
        Todo = 1,
        /// <summary>
        /// 暂停中
        /// </summary>
        [Description("暂停中")]
        Pause = 2,
        /// <summary>
        /// 进行中
        /// </summary>
        [Description("执行中")]
        Doing = 3,
        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Done = 4
    }
}
