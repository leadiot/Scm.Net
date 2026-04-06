using System.ComponentModel;

namespace Com.Scm.Enums
{
    public enum ScmDataTypeEnum
    {
        None = 0,
        [Description("字符串")]
        String = 1,
        [Description("整型")]
        Integer = 2,
        [Description("长整型")]
        Long = 3,
        [Description("单精度")]
        Float = 4,
        [Description("又精度")]
        Double = 5,
        [Description("布尔")]
        Bool = 6,
        [Description("日期")]
        Date = 7,
        [Description("时间")]
        Time = 8,
        [Description("日期时间")]
        DateTime = 9
    }
}
