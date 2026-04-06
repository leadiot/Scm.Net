using System.ComponentModel;

namespace Com.Scm.Enums
{
    public enum ScmVerHeaderTypesEnum
    {
        None = 0,
    }

    public enum ScmVerDetailTypesEnum
    {
        None = 0,
        [Description("修复")]
        BugFix = 1,
        [Description("新增")]
        New = 2,
        [Description("移除")]
        Remove = 3,
        [Description("优化")]
        Upgrade
    }
}
