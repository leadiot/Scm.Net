using System.ComponentModel;

namespace Com.Scm.Enums
{
    public enum ScmSexEnum
    {
        [Description("默认")]
        None = 0,
        [Description("男")]
        Male = 1,
        [Description("女")]
        Female = 2,
        [Description("保密")]
        Secret = 3,
        [Description("其它")]
        Other = 9,
    }
}
