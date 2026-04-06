using System.ComponentModel;

namespace Com.Scm.Nas
{
    /// <summary>
    /// 删除策略
    /// </summary>
    public enum NasDssEnum
    {
        None = 0,

        [Description("跳过")]
        Skip = 1,

        [Description("逻辑删除")]
        Remove = 2,

        [Description("物理删除")]
        Delete = 3,
    }
}
