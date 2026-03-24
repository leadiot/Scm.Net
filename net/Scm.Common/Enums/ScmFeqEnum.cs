using System.ComponentModel;

namespace Com.Scm.Enums
{
    /// <summary>
    /// 周期处理
    /// </summary>
    public enum ScmFeqEnum
    {
        None = 0,
        [Description("实时")]
        Event = 1,
        [Description("每隔")]
        Every = 2,
        [Description("每到")]
        Fixed = 3,
        [Description("满足")]
        Match = 4,
        [Description("人工")]
        Manual = 5,
    }
}
