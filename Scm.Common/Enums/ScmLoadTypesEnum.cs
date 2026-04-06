using System.ComponentModel;

namespace Com.Scm.Enums
{
    /// <summary>
    /// 客户端数据加载方式
    /// </summary>
    public enum ScmLoadTypesEnum
    {
        None = 0,
        [Description("预加载")]
        PreLoad = 1,
        [Description("热加载")]
        JitLoad = 2
    }
}
