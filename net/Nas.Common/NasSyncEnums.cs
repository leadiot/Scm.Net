using System.ComponentModel;

namespace Com.Scm.Nas
{
    /// <summary>
    /// 同步策略
    /// </summary>
    public enum NasSyncEnums
    {
        None,
        /// <summary>
        /// 仅上传
        /// </summary>
        [Description("仅上传")]
        Upload,
        /// <summary>
        /// 仅下载
        /// </summary>
        [Description("仅下载")]
        Download,
        /// <summary>
        /// 双向同步
        /// </summary>
        [Description("双向同步")]
        Both
    }
}
