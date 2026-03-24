using System.ComponentModel;

namespace Com.Scm.Nas
{
    public enum NasFeqEnum
    {
        None = 0,
        [Description("实时")]
        RealTime = 1,
        [Description("每秒")]
        BySeconds = 2,
        [Description("每分")]
        ByMinutes = 3,
        [Description("每时")]
        ByHours = 4,
        [Description("每天")]
        ByDays = 5,
        [Description("每周")]
        ByWeeks = 6,
        [Description("每月")]
        ByMonths = 7,
        [Description("每季")]
        BySeasons = 8,
        [Description("手动")]
        ByManual = 9,
    }
}
