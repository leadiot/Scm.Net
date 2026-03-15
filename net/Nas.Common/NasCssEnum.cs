using System.ComponentModel;

namespace Com.Scm.Nas
{
    /// <summary>
    /// 冲突策略
    /// </summary>
    public enum NasCssEnum
    {
        None = 0,

        [Description("保留本地")]
        KeepNative = 1,

        [Description("保留远端")]
        KeepRemote = 2,

        [Description("保留所有")]
        KeepBoth = 3,

        [Description("跳过")]
        Skip = 4
    }
}
