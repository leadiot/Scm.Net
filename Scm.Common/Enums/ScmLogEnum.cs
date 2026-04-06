using System.ComponentModel;

namespace Com.Scm.Enums
{
    /// <summary>
    /// 日志级别枚举
    /// </summary>
    public enum ScmLogLevelEnum
    {
        None = 0,
        [Description("调试")]
        Debug = 1,
        [Description("信息")]
        Info = 2,
        [Description("警告")]
        Warn = 3,
        [Description("错误")]
        Error = 4,
        [Description("致命错误")]
        Fatal = 5
    }

    public enum ScmLogTypesEnum
    {
        None = 0,
        [Description("登录日志")]
        Login = 1,
        [Description("操作日志")]
        Operate = 2,
        [Description("任务日志")]
        Tasks = 3
    }
}