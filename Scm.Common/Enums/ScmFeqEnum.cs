using System.ComponentModel;

namespace Com.Scm.Enums
{
    /// <summary>
    /// 周期处理
    /// </summary>
    public enum ScmFeqTypeEnum
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

    public enum ScmFeqUnitEnum
    {
        None = 0,
        [Description("秒")]
        BySeconds = 1,
        [Description("分")]
        ByMinutes = 2,
        [Description("时")]
        ByHours = 3,
        [Description("天")]
        ByDays = 4,
        [Description("周")]
        ByWeeks = 5,
        [Description("月")]
        ByMonths = 6,
        [Description("季")]
        BySeasons = 7,
        [Description("年")]
        ByYears = 8,
    }
}
