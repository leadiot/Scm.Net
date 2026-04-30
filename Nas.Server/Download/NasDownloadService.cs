using Com.Scm.Config;
using Com.Scm.Dsa;
using Com.Scm.Nas.Download.Dvo;
using Com.Scm.Nas.Dto.Download;
using Com.Scm.Service;
using Com.Scm.Utils;

namespace Com.Scm.Nas.Download
{
    /// <summary>
    /// 下载任务对外服务层
    /// 封装 NasDownloadManager，提供面向业务的添加/暂停/删除/查询接口，并通过 NasDownloadDao 持久化任务。
    /// </summary>
    public class NasDownloadService : AppService
    {
        private readonly NasDownloadManager _manager;
        private readonly string _defaultSaveRoot;
        private readonly SugarRepository<NasDownloadDao> _thisRepository;

        /// <summary>
        /// </summary>
        /// <param name="envConfig">环境配置</param>
        /// <param name="repository">下载任务仓储</param>
        /// <param name="maxConcurrent">最大并发任务数</param>
        public NasDownloadService(EnvConfig envConfig,
            SugarRepository<NasDownloadDao> repository,
            int maxConcurrent = 3)
        {
            _EnvConfig = envConfig;
            _thisRepository = repository;
            _defaultSaveRoot = _EnvConfig.GetDataPath(NasEnv.PathDownloads);
            _manager = new NasDownloadManager { MaxConcurrent = maxConcurrent };

            // 注册状态变更回调，同步到数据库
            _manager.OnStatusChanged = OnTaskStatusChanged;

            // 从数据库恢复未完成任务
            RestoreTasksFromDb();
        }

        /// <summary>
        /// 添加并启动下载任务
        /// </summary>
        public async Task<long> Add(NasDownloadAddRequest request)
        {
            var linkType = NasDownloadManager.DetectLinkType(request.Url);
            if (linkType == NasDownloadLinkType.Unknown)
            {
                throw new NotSupportedException($"无法识别的下载链接协议: {request.Url}");
            }

            var saveDir = string.IsNullOrWhiteSpace(request.SavePath)
                ? _defaultSaveRoot
                : request.SavePath;

            var fileName = string.IsNullOrWhiteSpace(request.SaveName)
                ? NasDownloadManager.InferFileName(request.Url)
                : request.SaveName;

            var now = TimeUtils.GetUnixTime();

            // 先保存到数据库，获取自增 ID
            var dao = new NasDownloadDao
            {
                Url = request.Url,
                LinkType = linkType,
                FileName = fileName,
                SavePath = saveDir,
                Threads = Math.Clamp(request.Threads, 1, 16),
                FtpUser = request.FtpUser,
                FtpPassword = request.FtpPassword,
                TotalSize = -1,
                DownloadedSize = 0,
                Progress = 0,
                Status = NasDownloadStatus.Pending,
                FinishTime = 0
            };
            await _thisRepository.InsertAsync(dao);

            // 构建运行时任务
            var task = new NasDownloadTask
            {
                id = dao.id,
                Url = dao.Url,
                LinkType = linkType,
                SaveDir = saveDir,
                FileName = fileName,
                Threads = dao.Threads,
                FtpUser = dao.FtpUser,
                FtpPassword = dao.FtpPassword,
                CreateTime = now
            };

            return _manager.Add(task);
        }

        /// <summary>
        /// 暂停下载任务
        /// </summary>
        public bool Pause(long taskId)
        {
            return _manager.Pause(taskId);
        }

        /// <summary>
        /// 恢复已暂停的下载任务
        /// </summary>
        public bool Resume(long taskId)
        {
            return _manager.Resume(taskId);
        }

        /// <summary>
        /// 删除下载任务（取消正在进行的下载，并从数据库删除记录）
        /// </summary>
        public bool Remove(long taskId)
        {
            var result = _manager.Remove(taskId);
            if (result)
            {
                // 从数据库中物理删除
                _thisRepository.DeleteById(taskId);
            }
            else
            {
                // 任务已不在内存中（已完成/已取消），仍需从 DB 删除
                _thisRepository.DeleteById(taskId);
            }
            return true;
        }

        /// <summary>
        /// 获取所有下载任务列表（优先内存，再补充数据库已完成任务）
        /// </summary>
        public NasDownloadListResponse GetAll()
        {
            // 内存中活跃任务
            var activeTasks = _manager.GetAll()
                .OrderByDescending(t => t.CreateTime)
                .Select(MapToDto)
                .ToList();

            var activeIds = new HashSet<long>(activeTasks.Select(t => t.id));

            // 从数据库补充已完成/失败/取消的任务（不在内存中的）
            var dbTasks = _thisRepository.GetList(
                    d => d.Status == NasDownloadStatus.Completed
                      || d.Status == NasDownloadStatus.Failed
                      || d.Status == NasDownloadStatus.Cancelled)
                .Where(d => !activeIds.Contains(d.id))
                .OrderByDescending(d => d.create_time)
                .Select(MapDaoToDto)
                .ToList();

            var allTasks = activeTasks.Concat(dbTasks).ToList();

            return new NasDownloadListResponse
            {
                Tasks = allTasks,
                Total = allTasks.Count
            };
        }

        /// <summary>
        /// 获取单个下载任务（优先内存，次选数据库）
        /// </summary>
        public NasDownloadTaskDto Get(long taskId)
        {
            var task = _manager.Get(taskId);
            if (task != null) return MapToDto(task);

            var dao = _thisRepository.GetById(taskId);
            return dao == null ? null : MapDaoToDto(dao);
        }

        /// <summary>
        /// 启动时从数据库恢复 Pending/Downloading 状态的任务
        /// </summary>
        private void RestoreTasksFromDb()
        {
            try
            {
                var pendingStatuses = new[]
                {
                    NasDownloadStatus.Pending,
                    NasDownloadStatus.Downloading
                };

                var daos = _thisRepository.GetList(d => pendingStatuses.Contains(d.Status));
                foreach (var dao in daos)
                {
                    var task = new NasDownloadTask
                    {
                        id = dao.id,
                        Url = dao.Url,
                        LinkType = dao.LinkType,
                        SaveDir = dao.SavePath,
                        FileName = dao.FileName,
                        Threads = dao.Threads,
                        FtpUser = dao.FtpUser,
                        FtpPassword = dao.FtpPassword,
                        TotalSize = dao.TotalSize,
                        DownloadedSize = 0,  // 重新开始下载
                        Status = NasDownloadStatus.Pending,
                        CreateTime = dao.create_time
                    };

                    // 重置数据库状态为 Pending
                    dao.Status = NasDownloadStatus.Pending;
                    dao.PrepareUpdate(0);
                    _thisRepository.Update(dao);

                    _manager.Add(task);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[NasDownload] 恢复任务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 任务状态变更时同步更新数据库
        /// </summary>
        private void OnTaskStatusChanged(NasDownloadTask task, NasDownloadStatus oldStatus, NasDownloadStatus newStatus)
        {
            try
            {
                var dao = _thisRepository.GetById(task.id);
                if (dao == null) return;

                dao.Status = newStatus;
                dao.TotalSize = task.TotalSize;
                dao.DownloadedSize = task.DownloadedSize;
                dao.Progress = task.Progress;
                dao.ErrorMessage = task.ErrorMessage;
                dao.FinishTime = task.FinishTime;
                _thisRepository.Update(dao);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[NasDownload] 更新任务状态失败 id={task.id}: {ex.Message}");
            }
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
                Status = task.Status,
                ErrorMessage = task.ErrorMessage,
                create_time = task.CreateTime,
                FinishTime = task.FinishTime,
                Threads = task.Threads
            };
        }

        /// <summary>
        /// 将数据库 Dao 映射为 DTO
        /// </summary>
        private static NasDownloadTaskDto MapDaoToDto(NasDownloadDao dao)
        {
            return new NasDownloadTaskDto
            {
                id = dao.id,
                Url = dao.Url,
                LinkType = dao.LinkType,
                FileName = dao.FileName,
                SavePath = dao.SavePath,
                TotalSize = dao.TotalSize,
                DownloadedSize = dao.DownloadedSize,
                Progress = dao.Progress,
                Speed = 0,
                Status = dao.Status,
                ErrorMessage = dao.ErrorMessage,
                create_time = dao.create_time,
                FinishTime = dao.FinishTime,
                Threads = dao.Threads
            };
        }

        public void Dispose()
        {
            _manager?.Dispose();
        }
    }
}
