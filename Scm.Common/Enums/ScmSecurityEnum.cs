using System.ComponentModel;

namespace Com.Scm.Enums
{
    /// <summary>
    /// 数据安全性
    /// </summary>
    public enum ScmSecurityEnum
    {
        None = 0,
        [Description("所有")]
        All = 1,
        [Description("包含")]
        Include = 2,
        [Description("排除")]
        Exclude = 3
    }
}
