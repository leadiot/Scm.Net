using System.ComponentModel;

namespace Com.Scm.Enums
{
    public enum ScmRowDeleteEnum
    {
        None = 0,
        [Description("未删除")]
        No = 1,
        [Description("已删除")]
        Yes = 2,
    }
}
