using System.ComponentModel;

namespace Com.Scm.Enums
{
    public enum TaskTypesEnum
    {
        None = 0,
        [Description("生成报表")]
        Report = 1,
        [Description("数据导出")]
        Export = 2,
        [Description("数据导入")]
        Import = 3
    }
}
