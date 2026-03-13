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
        /// 上传
        /// </summary>
        [Description("上传")]
        Upload,
        /// <summary>
        /// 下载
        /// </summary>
        [Description("下载")]
        Download
    }
}
