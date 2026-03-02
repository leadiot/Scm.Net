using System.ComponentModel;

namespace Com.Scm.Nas
{
    /// <summary>
    /// 同步方向
    /// </summary>
    public enum NasDirEnums
    {
        None,
        /// <summary>
        /// 仅上传
        /// </summary>
        [Description("仅上传")]
        Upload,
        /// <summary>
        /// 双向同步
        /// </summary>
        [Description("双向同步")]
        Sync,
        /// <summary>
        /// 仅下载
        /// </summary>
        [Description("仅下载")]
        Download
    }
}
