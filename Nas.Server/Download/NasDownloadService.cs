using Com.Scm.Config;
using Com.Scm.Nas.Download.Dvo;
using Com.Scm.Nas.Dto.Download;
using Com.Scm.Service;

namespace Com.Scm.Nas.Download
{
    /// <summary>
    /// 下载任务对外服务层
    /// 封装 NasDownloadManager，提供面向业务的添加/暂停/删除/查询接口。
    /// </summary>
    public class NasDownloadService : AppService
    {
        private readonly NasDownloadManager _manager;
        private readonly string _defaultSaveRoot;

        /// <summary>
        /// </summary>
        /// <param name="nasRootDir">NAS 根目录（物理路径）</param>
        /// <param name="maxConcurrent">最大并发任务数</param>
        public NasDownloadService(EnvConfig envConfig, int maxConcurrent = 3)
        {
            _EnvConfig = envConfig;
            _defaultSaveRoot = _EnvConfig.GetDataPath(NasEnv.PathDownloads);
            _manager = new NasDownloadManager { MaxConcurrent = maxConcurrent };
        }

        /// <summary>
        /// 添加并启动下载任务
        /// </summary>
        public string Add(NasDownloadAddRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.Url))
                throw new ArgumentException("下载链接不能为空");

            var linkType = NasDownloadManager.DetectLinkType(request.Url);
            if (linkType == NasDownloadLinkType.Unknown)
                throw new NotSupportedException($"无法识别的下载链接协议: {request.Url}");

            var saveDir = string.IsNullOrWhiteSpace(request.SavePath)
                ? _defaultSaveRoot
                : request.SavePath;

            var fileName = string.IsNullOrWhiteSpace(request.SaveName)
                ? NasDownloadManager.InferFileName(request.Url)
                : request.SaveName;

            var task = new NasDownloadTask
            {
                Url = request.Url,
                LinkType = linkType,
                SaveDir = saveDir,
                FileName = fileName,
                Threads = Math.Clamp(request.Threads, 1, 16),
                FtpUser = request.FtpUser,
                FtpPassword = request.FtpPassword
            };

            return _manager.Add(task);
        }

        /// <summary>
        /// 暂停下载任务
        /// </summary>
        public bool Pause(string taskId)
        {
            return _manager.Pause(taskId);
        }

        /// <summary>
        /// 恢复已暂停的下载任务
        /// </summary>
        public bool Resume(string taskId)
        {
            return _manager.Resume(taskId);
        }

        /// <summary>
        /// 删除下载任务（取消正在进行的下载）
        /// </summary>
        public bool Remove(string taskId)
        {
            return _manager.Remove(taskId);
        }

        /// <summary>
        /// 获取所有下载任务列表
        /// </summary>
        public NasDownloadListResponse GetAll()
        {
            var tasks = _manager.GetAll()
                .OrderByDescending(t => t.CreateTime)
                .Select(MapToDto)
                .ToList();

            return new NasDownloadListResponse
            {
                Tasks = tasks,
                Total = tasks.Count
            };
        }

        /// <summary>
        /// 获取单个下载任务
        /// </summary>
        public NasDownloadTaskDto Get(string taskId)
        {
            var task = _manager.Get(taskId);
            return task == null ? null : MapToDto(task);
        }

        /// <summary>
        /// 将运行时任务模型映射为 DTO
        /// </summary>
        private static NasDownloadTaskDto MapToDto(NasDownloadTask task)
        {
            return new NasDownloadTaskDto
            {
                id = task.id,
                Url = task.Url,
                LinkType = task.LinkType,
                FileName = task.FileName,
                SavePath = task.SaveDir,
                TotalSize = task.TotalSize,
                DownloadedSize = task.DownloadedSize,
                Progress = task.Progress,
                Speed = task.Speed,
                Status = task.Status.ToString(),
                ErrorMessage = task.ErrorMessage,
                create_time = task.CreateTime,
                FinishTime = task.FinishTime,
                Threads = task.Threads
            };
        }

        public void Dispose()
        {
            _manager?.Dispose();
        }
    }
}
