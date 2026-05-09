using Com.Scm.Config;
using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Nas.Download.Dvo;
using Com.Scm.Nas.Dto.Download;
using Com.Scm.Service;
using Com.Scm.Token;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Nas.Download
{
    /// <summary>
    /// 下载任务对外服务层
    /// 封装 NasDownloadManager，提供面向业务的添加/暂停/删除/查询接口，并通过 NasDownloadDao 持久化任务。
    /// </summary>
    public class NasDownloadService : ApiService
    {
        private static NasDownloadManager _Manager;

        private readonly ScmContextHolder _ScmHolder;
        private readonly SugarRepository<NasDownloadDao> _thisRepository;

        /// <summary>
        /// </summary>
        /// <param name="envConfig">环境配置</param>
        /// <param name="repository">下载任务仓储</param>
        /// <param name="maxConcurrent">最大并发任务数</param>
        public NasDownloadService(SugarRepository<NasDownloadDao> repository, EnvConfig envConfig, ScmContextHolder scmHolder)
        {
            _thisRepository = repository;
            _EnvConfig = envConfig;
            _ScmHolder = scmHolder;

            if (_Manager == null)
            {
                _Manager = new NasDownloadManager
                {
                    MaxConcurrent = 3,
                    // 注册状态变更回调，同步到数据库
                    OnStatusChanged = OnTaskStatusChanged
                };
            }
        }

        /// <summary>
        /// 获取所有下载任务列表（优先内存，再补充数据库已完成任务）
        /// </summary>
        public async Task<List<NasDownloadDto>> GetListAsync()
        {
            var daoList = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
                .OrderByDescending(d => d.id)
                .ToListAsync();
            var dtoList = new List<NasDownloadDto>();
            foreach (var dao in daoList)
            {
                dtoList.Add(MapDaoToDto(dao));

                if (dao.handle != ScmHandleEnum.Doing)
                {
                    continue;
                }

                var task = _Manager.Get(dao.id);
                if (task == null)
                {
                    _Manager.Add(MapDaoToTask(dao));
                }
            }
            return dtoList;
        }

        /// <summary>
        /// 获取单个下载任务（优先内存，次选数据库）
        /// </summary>
        [HttpGet("{id}")]
        public async Task<NasDownloadDto> GetAsync(long id)
        {
            var task = _Manager.Get(id);
            if (task != null)
            {
                return MapToDto(task);
            }

            var dao = await _thisRepository.GetByIdAsync(id);
            return dao == null ? null : MapDaoToDto(dao);
        }

        /// <summary>
        /// 添加并启动下载任务
        /// </summary>
        public async Task<long> AddAsync(NasDownloadAddRequest request)
        {
            var userCodes = _ScmHolder.GetToken().user_codes;

            var linkType = NasDownloadManager.DetectLinkType(request.Url);
            if (linkType == NasDownloadLinkType.Unknown)
            {
                throw new NotSupportedException($"无法识别的下载链接协议: {request.Url}");
            }

            var saveDir = _EnvConfig.GetNasDownloadPath(userCodes, request.Path);

            var fileName = string.IsNullOrWhiteSpace(request.Name)
                ? NasDownloadManager.InferFileName(request.Url)
                : request.Name;

            // 先保存到数据库，获取自增 ID
            var dao = new NasDownloadDao
            {
                url = request.Url,
                link_type = linkType,
                file_name = fileName,
                file_path = saveDir,
                threads = Math.Clamp(request.Threads, 1, 16),
                ftp_user = request.FtpUser,
                ftp_pass = request.FtpPassword,
                total_size = -1,
                downloaded_size = 0,
                progress = 0,
                handle = ScmHandleEnum.Doing,
                finish_time = 0
            };
            await _thisRepository.InsertAsync(dao);

            // 构建运行时任务
            return _Manager.Add(MapDaoToTask(dao));
        }

        /// <summary>
        /// 暂停下载任务
        /// </summary>
        [HttpGet("{id}")]
        public bool Pause(long id)
        {
            return _Manager.Pause(id);
        }

        /// <summary>
        /// 恢复已暂停的下载任务
        /// </summary>
        [HttpGet("{id}")]
        public async Task<bool> Resume(long id)
        {
            _Manager.Resume(id);

            var dao = await _thisRepository.GetByIdAsync(id);
            dao.handle = ScmHandleEnum.Doing;
            await _thisRepository.UpdateAsync(dao);

            _Manager.Add(MapDaoToTask(dao));
            return true;
        }

        public async Task<int> RemoveAsync(List<long> ids)
        {
            int count = 0;
            foreach (var taskId in ids)
            {
                if (_Manager.Remove(taskId))
                {
                    count += 1;
                }
            }

            // 同步删除数据库记录
            await _thisRepository.DeleteAsync(d => ids.Contains(d.id));
            return count;
        }

        /// <summary>
        /// 删除下载任务（取消正在进行的下载，并从数据库删除记录）
        /// </summary>
        public async Task<int> StatusAsync(ScmChangeStatusRequest param)
        {
            return await UpdateStatus(_thisRepository, param.ids, param.status);
        }

        /// <summary>
        /// 任务状态变更时同步更新数据库
        /// </summary>
        private void OnTaskStatusChanged(NasDownloadTask task, ScmHandleEnum handle, ScmResultEnum result)
        {
            try
            {
                var dao = _thisRepository.GetById(task.id);
                if (dao == null) return;

                dao.handle = handle;
                dao.result = result;
                dao.total_size = task.TotalSize;
                dao.downloaded_size = task.DownloadedSize;
                dao.progress = task.Progress;
                dao.message = task.ErrorMessage;
                dao.finish_time = task.FinishTime;
                _thisRepository.Update(dao);
            }
            catch (Exception ex)
            {
                LogUtils.Error($"[NasDownload] 更新任务状态失败 id={task.id}: {ex.Message}");
            }
        }

        /// <summary>
        /// 将运行时任务模型映射为 DTO
        /// </summary>
        private static NasDownloadDto MapToDto(NasDownloadTask task)
        {
            return new NasDownloadDto
            {
                id = task.id,
                url = task.Url,
                link_type = task.LinkType,
                file_name = task.FileName,
                file_path = task.FilePath,
                total_size = task.TotalSize,
                downloaded_size = task.DownloadedSize,
                progress = task.Progress,
                speed = task.Speed,
                handle = task.Handle,
                message = task.ErrorMessage,
                create_time = task.CreateTime,
                finish_time = task.FinishTime,
                threads = task.Threads
            };
        }

        /// <summary>
        /// 将数据库 Dao 映射为 DTO
        /// </summary>
        private static NasDownloadDto MapDaoToDto(NasDownloadDao dao)
        {
            return new NasDownloadDto
            {
                id = dao.id,
                url = dao.url,
                link_type = dao.link_type,
                file_name = dao.file_name,
                file_path = dao.file_path,
                total_size = dao.total_size,
                downloaded_size = dao.downloaded_size,
                progress = dao.progress,
                speed = 0,
                handle = dao.handle,
                message = dao.message,
                create_time = dao.create_time,
                finish_time = dao.finish_time,
                threads = dao.threads
            };
        }

        private static NasDownloadTask MapDaoToTask(NasDownloadDao dao)
        {
            return new NasDownloadTask
            {
                id = dao.id,
                Url = dao.url,
                LinkType = dao.link_type,
                FilePath = dao.file_path,
                FileName = dao.file_name,
                Threads = dao.threads,
                FtpUser = dao.ftp_user,
                FtpPassword = dao.ftp_pass,
                TotalSize = dao.total_size,
                DownloadedSize = dao.downloaded_size,
                Handle = dao.handle,
                CreateTime = dao.create_time
            };
        }

        public void Dispose()
        {
            _Manager?.Dispose();
        }
    }
}
