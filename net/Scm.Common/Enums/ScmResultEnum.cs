using System.ComponentModel;

namespace Com.Scm.Enums
{
    public enum ScmResultEnum
    {
        [Description("默认")]
        None,
        [Description("失败")]
        Failure,
        [Description("成功")]
        Success
    }
}
