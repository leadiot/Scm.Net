using Com.Scm.Enums;

namespace Com.Scm.Nas.Download
{
    /// <summary>
    /// 下载任务内部运行时模型
    /// </summary>
    public class NasDownloadTask
    {
        /// <summary>
        /// 任务唯一 ID
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 原始下载链接
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 链接类型
        /// </summary>
        public NasDownloadLinkType LinkType { get; set; }

        /// <summary>
        /// 保存目录（本地物理路径）
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 保存文件名
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// 完整保存路径
        /// </summary>
        public string FullPath => Path.Combine(FilePath, FileName);

        /// <summary>
        /// 并发线程数
        /// </summary>
        public int Threads { get; set; } = 4;

        /// <summary>
        /// FTP 凭据（可选）
        /// </summary>
        public string FtpUser { get; set; }

        /// <summary>
        /// FTP 密码（可选）
        /// </summary>
        public string FtpPassword { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public ScmHandleEnum Handle { get; set; } = ScmHandleEnum.Todo;

        public ScmResultEnum Result { get; set; }

        /// <summary>
        /// 文件总大小（字节，-1 表示未知）
        /// </summary>
        public long TotalSize { get; set; } = -1;

        /// <summary>
        /// 已下载字节数
        /// </summary>
        public long DownloadedSize { get; set; }

        /// <summary>
        /// 当前速度（字节/秒）
        /// </summary>
        public long Speed { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public long CreateTime { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public long FinishTime { get; set; }

        /// <summary>
        /// 用于暂停/取消的 CancellationTokenSource
        /// </summary>
        public CancellationTokenSource Cts { get; set; }

        /// <summary>
        /// 是否暂停标志
        /// </summary>
        public bool IsPauseRequested { get; set; }

        /// <summary>
        /// 下载进度（0~100）
        /// </summary>
        public double Progress =>
            TotalSize > 0 ? Math.Round((double)DownloadedSize / TotalSize * 100, 2) : 0;

        /// <summary>
        /// 速度快照时间（用于计算速度）
        /// </summary>
        public DateTime SpeedSnapshotTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 速度快照字节数
        /// </summary>
        public long SpeedSnapshotBytes { get; set; }

        /// <summary>
        /// 更新下载速度（每秒调用一次）
        /// </summary>
        public void UpdateSpeed()
        {
            var now = DateTime.Now;
            var elapsed = (now - SpeedSnapshotTime).TotalSeconds;
            if (elapsed >= 1.0)
            {
                Speed = (long)((DownloadedSize - SpeedSnapshotBytes) / elapsed);
                SpeedSnapshotTime = now;
                SpeedSnapshotBytes = DownloadedSize;
            }
        }
    }
}
