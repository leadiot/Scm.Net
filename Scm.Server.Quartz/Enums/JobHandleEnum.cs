using System.ComponentModel;

namespace Com.Scm.Quartz.Enums
{
    public enum JobHandleEnum
    {
        None = 0,
        [Description("初始")]
        Init = 1,
        [Description("暂停")]
        Paused = 2,
        [Description("停止")]
        Stoped = 3,
        [Description("启动")]
        Running = 4
    }
}
