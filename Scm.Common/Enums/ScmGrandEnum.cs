using System.ComponentModel;

namespace Com.Scm.Enums
{
    public enum ScmGrandTypeEnums
    {
        None = 0,

        [Description("测试版")]
        Test = 1,

        [Description("演示版")]
        Demo = 2,

        [Description("限免版")]
        Limit = 3,

        [Description("租赁版")]
        Rent = 4,

        [Description("授权版")]
        Auth = 5,

        [Description("专业版")]
        Pro = 6,
        [Description("旗舰版")]
        Ultimate = 7
    }

    public enum ScmGrandModeEnums
    {
        None = 0,
        Time = 1,
        User = 2,
        Data = 3,
        Amount = 4
    }

    public enum ScmGrandResultEnums
    {
        None = 0,
        /// <summary>
        /// 正常
        /// </summary>
        Normal = 1,
        Soon = 2,
        Near = 3,
        Over = 4
    }
}
