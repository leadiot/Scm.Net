namespace Com.Scm.Nas.Download
{
    /// <summary>
    /// 下载策略接口，每种协议对应一个实现
    /// </summary>
    public interface IDownloadStrategy
    {
        /// <summary>
        /// 该策略支持的链接类型
        /// </summary>
        NasDownloadLinkType LinkType { get; }

        /// <summary>
        /// 执行下载（异步多线程）
        /// </summary>
        /// <param name="task">下载任务</param>
        /// <param name="cancellationToken">取消令牌</param>
        Task DownloadAsync(NasDownloadTask task, CancellationToken cancellationToken);
    }
}
