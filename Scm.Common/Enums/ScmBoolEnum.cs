using System.ComponentModel;

namespace Com.Scm.Enums
{
    public enum ScmBoolEnum
    {
        [Description("默认")]
        None = 0,

        [Description("否")]
        False = 1,
        
        [Description("是")]
        True = 2,
    }
}
