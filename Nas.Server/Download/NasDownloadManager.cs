using Com.Scm.Nas.Download.Strategy;
using System.Collections.Concurrent;

namespace Com.Scm.Nas.Download
{
    /// <summary>
    /// 下载管理器
    /// 负责任务的生命周期管理：添加、暂停、恢复、删除，以及并发调度执行。
    /// </summary>
    public class NasDownloadManager : IDisposable
    {
        /// <summary>
        /// 最大并发下载任务数（同时运行的任务数量上限）
        /// </summary>
        public int MaxConcurrent { get; set; } = 3;

        private readonly ConcurrentDictionary<string, NasDownloadTask> _tasks = new();
        private readonly Dictionary<NasDownloadLinkType, IDownloadStrategy> _strategies = new();
        private readonly SemaphoreSlim _semaphore;
        private bool _disposed;

        public NasDownloadManager()
        {
            _semaphore = new SemaphoreSlim(MaxConcurrent, MaxConcurrent);
            RegisterDefaultStrategies();
        }

        /// <summary>
        /// 注册默认策略（HTTP、FTP、File）
        /// </summary>
        private void RegisterDefaultStrategies()
        {
            RegisterStrategy(new HttpDownloadStrategy());
            RegisterStrategy(new FtpDownloadStrategy());
            RegisterStrategy(new FileDownloadStrategy());
        }

        /// <summary>
        /// 注册自定义下载策略
        /// </summary>
        public void RegisterStrategy(IDownloadStrategy strategy)
        {
            _strategies[strategy.LinkType] = strategy;
        }

        /// <summary>
        /// 添加下载任务并立即启动
        /// </summary>
        /// <returns>任务 ID</returns>
        public string Add(NasDownloadTask task)
        {
            _tasks[task.TaskId] = task;
            _ = RunTaskAsync(task);
            return task.TaskId;
        }

        /// <summary>
        /// 暂停下载任务
        /// </summary>
        public bool Pause(string taskId)
        {
            if (!_tasks.TryGetValue(taskId, out var task)) return false;
            if (task.Status != NasDownloadStatus.Downloading) return false;

            task.IsPauseRequested = true;
            task.Cts?.Cancel();
            task.Status = NasDownloadStatus.Paused;
            return true;
        }

        /// <summary>
        /// 恢复已暂停的下载任务
        /// </summary>
        public bool Resume(string taskId)
        {
            if (!_tasks.TryGetValue(taskId, out var task)) return false;
            if (task.Status != NasDownloadStatus.Paused) return false;

            task.IsPauseRequested = false;
            task.Status = NasDownloadStatus.Pending;
            _ = RunTaskAsync(task);
            return true;
        }

        /// <summary>
        /// 删除下载任务（同时取消正在运行的任务）
        /// </summary>
        public bool Remove(string taskId)
        {
            if (!_tasks.TryGetValue(taskId, out var task)) return false;

            task.Cts?.Cancel();
            task.Status = NasDownloadStatus.Cancelled;
            _tasks.TryRemove(taskId, out _);

            // 清理临时分片文件
            CleanupTempFiles(task);
            return true;
        }

        /// <summary>
        /// 获取所有任务快照
        /// </summary>
        public IReadOnlyCollection<NasDownloadTask> GetAll()
        {
            return _tasks.Values.ToList();
        }

        /// <summary>
        /// 获取指定任务
        /// </summary>
        public NasDownloadTask Get(string taskId)
        {
            _tasks.TryGetValue(taskId, out var task);
            return task;
        }

        /// <summary>
        /// 执行任务下载逻辑（受信号量控制并发）
        /// </summary>
        private async Task RunTaskAsync(NasDownloadTask task)
        {
            await _semaphore.WaitAsync();
            try
            {
                if (task.Status == NasDownloadStatus.Cancelled) return;

                task.Cts = new CancellationTokenSource();
                task.Status = NasDownloadStatus.Downloading;
                task.SpeedSnapshotTime = DateTime.Now;
                task.SpeedSnapshotBytes = task.DownloadedSize;

                if (!_strategies.TryGetValue(task.LinkType, out var strategy))
                {
                    throw new NotSupportedException($"不支持的下载协议类型: {task.LinkType}");
                }

                await strategy.DownloadAsync(task, task.Cts.Token);

                if (!task.IsPauseRequested)
                {
                    task.Status = NasDownloadStatus.Completed;
                    task.FinishTime = DateTime.Now;
                    task.Speed = 0;
                }
            }
            catch (OperationCanceledException)
            {
                if (!task.IsPauseRequested)
                {
                    task.Status = NasDownloadStatus.Cancelled;
                }
            }
            catch (Exception ex)
            {
                task.Status = NasDownloadStatus.Failed;
                task.ErrorMessage = ex.Message;
                task.Speed = 0;
                Console.WriteLine($"[NasDownload] 任务 {task.TaskId} 下载失败: {ex.Message}");
            }
            finally
            {
                task.Cts?.Dispose();
                task.Cts = null;
                _semaphore.Release();
            }
        }

        /// <summary>
        /// 解析 URL 对应的链接类型
        /// </summary>
        public static NasDownloadLinkType DetectLinkType(string url)
        {
            if (string.IsNullOrWhiteSpace(url)) return NasDownloadLinkType.Unknown;

            if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) ||
                url.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
                return NasDownloadLinkType.Http;

            if (url.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase) ||
                url.StartsWith("ftps://", StringComparison.OrdinalIgnoreCase))
                return NasDownloadLinkType.Ftp;

            if (url.StartsWith("file://", StringComparison.OrdinalIgnoreCase) ||
                url.StartsWith("/", StringComparison.Ordinal) ||
                (url.Length >= 3 && url[1] == ':'))   // Windows 绝对路径 C:\
                return NasDownloadLinkType.File;

            if (url.StartsWith(NasEnv.VirtualTag, StringComparison.OrdinalIgnoreCase))
                return NasDownloadLinkType.Nas;

            return NasDownloadLinkType.Unknown;
        }

        /// <summary>
        /// 从 URL 推断文件名
        /// </summary>
        public static string InferFileName(string url)
        {
            try
            {
                var uri = new Uri(url);
                var name = Path.GetFileName(uri.LocalPath);
                if (!string.IsNullOrWhiteSpace(name)) return name;
            }
            catch { }

            return $"download_{DateTime.Now:yyyyMMddHHmmss}";
        }

        private static void CleanupTempFiles(NasDownloadTask task)
        {
            try
            {
                var dir = task.SaveDir;
                if (!Directory.Exists(dir)) return;
                var prefix = task.FileName + ".part";
                foreach (var f in Directory.GetFiles(dir, prefix + "*"))
                {
                    try { File.Delete(f); } catch { }
                }
            }
            catch { }
        }

        public void Dispose()
        {
            if (_disposed) return;
            foreach (var task in _tasks.Values)
            {
                task.Cts?.Cancel();
            }
            _semaphore?.Dispose();
            _disposed = true;
        }
    }
}
