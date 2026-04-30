using System.ComponentModel;

namespace Com.Scm.Nas
{
    /// <summary>
    /// 下载链接类型
    /// </summary>
    public enum NasDownloadLinkType
    {
        /// <summary>
        /// 未知
        /// </summary>
        [Description("未知")]
        Unknown = 0,
        /// <summary>
        /// HTTP/HTTPS
        /// </summary>
        [Description("HTTP")]
        Http = 1,
        /// <summary>
        /// FTP
        /// </summary>
        [Description("FTP")]
        Ftp = 2,
        /// <summary>
        /// 本地文件路径
        /// </summary>
        [Description("本地文件")]
        File = 3,
        /// <summary>
        /// NAS 内部虚拟路径
        /// </summary>
        [Description("NAS路径")]
        Nas = 4,
    }

    /// <summary>
    /// 下载任务状态
    /// </summary>
    public enum NasDownloadStatus
    {
        /// <summary>
        /// 等待中
        /// </summary>
        [Description("等待中")]
        Pending = 0,
        /// <summary>
        /// 下载中
        /// </summary>
        [Description("下载中")]
        Downloading = 1,
        /// <summary>
        /// 已暂停
        /// </summary>
        [Description("已暂停")]
        Paused = 2,
        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Completed = 3,
        /// <summary>
        /// 已失败
        /// </summary>
        [Description("已失败")]
        Failed = 4,
        /// <summary>
        /// 已取消
        /// </summary>
        [Description("已取消")]
        Cancelled = 5,
    }
}
