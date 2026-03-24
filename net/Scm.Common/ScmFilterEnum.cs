using System.ComponentModel;

namespace Com.Scm
{
    /// <summary>
    /// 过滤枚举
    /// </summary>
    public enum ScmFilterEnum
    {
        None = 0,
        [Description("包含")]
        Include = 1,
        [Description("排除")]
        Exclude = 2,
    }
}
