using Com.Scm.Api;
using Com.Scm.Config;
using Com.Scm.Enums;
using Com.Scm.Filters;
using Com.Scm.Nas.Cfg;
using Com.Scm.Nas.Log;
using Com.Scm.Nas.Res;
using Com.Scm.Nas.Sync.Dvo;
using Com.Scm.Service;
using Com.Scm.Token;
using Com.Scm.Ur;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Nas.Sync
{
    /// <summary>
    /// 终端文件同步服务
    /// </summary>
    [NoAuditLog]
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "Scm")]
    public class NasSyncService : AppService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlClient"></param>
        /// <param name="nasConfig"></param>
        /// <param name="resHolder"></param>
        public NasSyncService(ISqlSugarClient sqlClient,
            EnvConfig envConfig,
            IResHolder resHolder)
        {
            _SqlClient = sqlClient;
            _EnvConfig = envConfig;
            _ResHolder = resHolder;
        }

        public SyncResult TestAsync()
        {
            var json = "{\"terminal_id\":0,\"folder_id\":2031909146500141056,\"res_id\":1773255166001,\"dir_id\":1773254941001,\"type\":10,\"kind\":0,\"name\":\"Devices\",\"path\":\"/Public/public/Devices\",\"hash\":\"\",\"size\":0,\"modify_time\":0,\"opt\":1,\"dir\":1,\"src\":null,\"row_status\":0,\"create_user\":0,\"create_names\":null,\"create_time\":1773284008749,\"update_user\":0,\"update_names\":null,\"update_time\":1773284008749,\"id\":0}";
            var dto = json.AsJsonObject<NasLogFileDto>();
            var terminalDao = _ResHolder.GetRes<ScmUrTerminalDao>(2031718705003630592);

            var result = new SyncResult();

            DealDeleteFile(terminalDao, dto, result);

            return result;
        }

        #region 对外接口
        /// <summary>
        /// 数据初始化
        /// </summary>
        /// <returns></returns>
        public async Task<List<SyncResFileDao>> PostInitAsync([FromHeader] string appToken)
        {
            var token = ScmToken.FromAppToken(appToken);
            var terminalDao = _ResHolder.GetRes<ScmUrTerminalDao>(token.terminal_id);
            if (terminalDao == null || terminalDao.IsExpired())
            {
                return null;
            }

            var userDao = _ResHolder.GetRes<UserDao>(terminalDao.user_id);
            if (userDao == null)
            {
                return null;
            }

            var dirList = new List<SyncResFileDao>();
            var dao = CreateSpecialDirDao(terminalDao, NasEnv.NodeDevices, NasEnv.PathDevices, ScmFileKindEnum.Folder);
            dirList.Add(dao);
            dao = CreateSpecialDirDao(terminalDao, NasEnv.NodePublic, NasEnv.PathPublic, ScmFileKindEnum.Folder);
            dirList.Add(dao);
            dao = CreateSpecialDirDao(terminalDao, NasEnv.NodeSecret, NasEnv.PathSecret, ScmFileKindEnum.Folder);
            dirList.Add(dao);
            dao = CreateSpecialDirDao(terminalDao, NasEnv.NodeDownloads, NasEnv.PathDownloads, ScmFileKindEnum.Folder);
            dirList.Add(dao);
            dao = CreateSpecialDirDao(terminalDao, NasEnv.NodeApps, NasEnv.PathApps, ScmFileKindEnum.Folder);
            dirList.Add(dao);

            return dirList;
        }

        /// <summary>
        /// 获取驱动列表
        /// </summary>
        /// <returns></returns>
        public async Task<ScmApiListResponse<NasCfgFolderDto>> GetFolderAsync([FromHeader] string appToken)
        {
            var token = ScmToken.FromAppToken(appToken);
            var terminalDao = _ResHolder.GetRes<ScmUrTerminalDao>(token.terminal_id);
            if (terminalDao == null || terminalDao.IsExpired())
            {
                return null;
            }

            var list = await _SqlClient.Queryable<NasCfgFolderDao>()
                .Where(a => a.terminal_id == terminalDao.id && a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .Select<NasCfgFolderDto>()
                .ToListAsync();

            var response = new ScmApiListResponse<NasCfgFolderDto>();
            response.SetSuccess(list);

            return response;
        }

        /// <summary>
        /// 更新驱动
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<NasCfgFolderDto> PostFolderAsync(NasCfgFolderDto dto, [FromHeader] string appToken)
        {
            var token = ScmToken.FromAppToken(appToken);
            var terminalDao = _ResHolder.GetRes<ScmUrTerminalDao>(token.terminal_id);
            if (terminalDao == null || terminalDao.IsExpired())
            {
                return null;
            }

            dto.path = SyncCfgFolderDao.GetStoragePath(dto.node, dto.path);

            var cfgDao = await _SqlClient.Queryable<SyncCfgFolderDao>()
                .Where(a => a.terminal_id == token.terminal_id && a.name == dto.name)
                .FirstAsync();

            if (cfgDao == null)
            {
                cfgDao = new SyncCfgFolderDao();
                cfgDao.user_id = terminalDao.user_id;
                cfgDao.terminal_id = terminalDao.id;
                cfgDao.name = dto.name;
                cfgDao.path = dto.path;
                cfgDao.PrepareCreate(terminalDao.user_id);
                await _SqlClient.Insertable(cfgDao).ExecuteCommandAsync();
            }
            else
            {
                cfgDao.row_status = Enums.ScmRowStatusEnum.Enabled;
                cfgDao.PrepareUpdate(terminalDao.user_id);
                await _SqlClient.Updateable(cfgDao).ExecuteCommandAsync();
            }

            var list = new List<SyncResFileDao>();
            var dirDao = CreateRecursiveDirDao(terminalDao, dto.path, list);

            // 回写
            cfgDao.res_id = dirDao.id;
            await _SqlClient.Updateable(cfgDao).ExecuteCommandAsync();

            var rPath = GetNativePath(terminalDao, dto.path);
            FileUtils.CreateDir(rPath);

            dto.id = cfgDao.id;
            dto.res_id = dirDao.id;

            return dto;
        }

        /// <summary>
        /// 检查指定HASH是否存在
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        [HttpGet("{hash}")]
        public async Task<bool> GetQueryAsync(string hash)
        {
            var exists = await _SqlClient.Queryable<Sync.SyncResFileDao>()
                .Where(a => a.hash == hash)
                .AnyAsync();

            return true;
        }

        /// <summary>
        /// 获取同步日志（按时间升序排列）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<NasLogFileDto>> GetLogAsync(GetLogRequest request, [FromHeader] string appToken)
        {
            var token = ScmToken.FromAppToken(appToken);
            var terminalDao = _ResHolder.GetRes<ScmUrTerminalDao>(token.terminal_id);
            if (terminalDao == null || terminalDao.IsExpired())
            {
                return null;
            }

            //var driveDao = GetDriveDao(driveId);

            var response = await _SqlClient.Queryable<SyncLogFolderDao, SyncLogFileDao>((a, b) => a.log_id == b.id)
                .Where((a, b) => a.user_id == terminalDao.user_id &&
                    a.folder_id == request.folder_id &&
                    a.row_status == Enums.ScmRowStatusEnum.Enabled &&
                    a.id > request.id)
                .OrderBy(a => a.id, OrderByType.Asc)
                .Select((a, b) => new NasLogFileDto
                {
                    id = a.id,
                    terminal_id = b.terminal_id,
                    folder_id = b.folder_id,
                    res_id = b.res_id,
                    dir_id = b.dir_id,
                    type = b.type,
                    name = b.name,
                    path = b.path,
                    hash = b.hash,
                    size = b.size,
                    modify_time = b.modify_time,
                    opt = b.opt,
                    dir = b.dir,
                    src = b.src
                })
                .ToPageAsync(request.page, request.limit);

            foreach (var item in response.Items)
            {
                item.path = GetClientPath(terminalDao, item.path);
                item.src = GetClientPath(terminalDao, item.src);
            }
            return response;
        }

        /// <summary>
        /// 获取目录列表（根据上级目录）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<NasResFileDto>> GetDirAsync(GetDirRequest request, [FromHeader] string appToken)
        {
            var token = ScmToken.FromAppToken(appToken);
            var terminalDao = _ResHolder.GetRes<ScmUrTerminalDao>(token.terminal_id);
            if (terminalDao == null || terminalDao.IsExpired())
            {
                return null;
            }

            var byPath = request.by_path;// !string.IsNullOrEmpty(request.path);

            var response = await _SqlClient.Queryable<Sync.SyncResFileDao>()
                .Where(a => a.user_id == terminalDao.user_id &&
                    a.type == ScmFileTypeEnum.Dir &&
                    a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .WhereIF(byPath, a => a.path == request.path)
                .WhereIF(!byPath, a => a.dir_id == request.dir_id)
                .OrderBy(a => a.name, OrderByType.Asc)
                .Select<NasResFileDto>()
                .ToPageAsync(request.page, request.limit);

            foreach (var item in response.Items)
            {
                item.path = GetClientPath(terminalDao, item.path);
            }
            return response;
        }

        /// <summary>
        /// 获取文档列表（根据上级目录）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<NasResFileDto>> GetDocAsync(GetDocRequest request, [FromHeader] string appToken)
        {
            var token = ScmToken.FromAppToken(appToken);
            var terminalDao = _ResHolder.GetRes<ScmUrTerminalDao>(token.terminal_id);
            if (terminalDao == null || terminalDao.IsExpired())
            {
                return null;
            }

            var byPath = request.by_path;// !string.IsNullOrEmpty(request.path);

            var response = await _SqlClient.Queryable<Sync.SyncResFileDao>()
                .Where(a => a.user_id == terminalDao.user_id &&
                    a.type == ScmFileTypeEnum.Doc &&
                    a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .WhereIF(byPath, a => a.path == request.path)
                .WhereIF(!byPath, a => a.dir_id == request.dir_id)
                .WhereIF(request.kind != ScmFileKindEnum.None, a => a.kind == request.kind)
                .OrderBy(a => a.name, OrderByType.Asc)
                .Select<NasResFileDto>()
                .ToPageAsync(request.page, request.limit);

            foreach (var item in response.Items)
            {
                item.path = GetClientPath(terminalDao, item.path);
            }
            return response;
        }

        /// <summary>
        /// 获取文件列表（根据上级目录）
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<NasResFileDto>> GetFileAsync(GetDocRequest request, [FromHeader] string appToken)
        {
            var token = ScmToken.FromAppToken(appToken);
            var terminalDao = _ResHolder.GetRes<ScmUrTerminalDao>(token.terminal_id);
            if (terminalDao == null || terminalDao.IsExpired())
            {
                return null;
            }

            var byPath = request.by_path;// !string.IsNullOrEmpty(request.path);

            var response = await _SqlClient.Queryable<Sync.SyncResFileDao>()
                .Where(a => a.user_id == terminalDao.user_id &&
                    a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .WhereIF(byPath, a => a.path == request.path)
                .WhereIF(!byPath, a => a.dir_id == request.dir_id)
                .OrderBy(a => a.type, OrderByType.Asc)
                .OrderBy(a => a.name, OrderByType.Asc)
                .Select<NasResFileDto>()
                .ToPageAsync(request.page, request.limit);

            foreach (var item in response.Items)
            {
                item.path = GetClientPath(terminalDao, item.path);
            }
            return response;
        }

        /// <summary>
        /// 上传同步日志
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="terminalId"></param>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async Task<SyncResult> PostSyncAsync(NasLogFileDto dto, [FromHeader] string appToken)
        {
            if (dto == null)
            {
                LogUtils.Error("PostSync", "上传对象为空！");
                return SyncResult.Failure("上传对象为空！");
            }

            LogUtils.Debug("PostSync", "上传同步日志", dto.ToJsonString());

            var token = ScmToken.FromAppToken(appToken);
            var terminalDao = _ResHolder.GetRes<ScmUrTerminalDao>(token.terminal_id);
            if (terminalDao == null)
            {
                LogUtils.Error("PostSync", "终端信息异常！");
                return SyncResult.Failure("终端信息异常！");
            }
            if (terminalDao.IsExpired())
            {
                LogUtils.Error("PostSync", "终端授权异常！");
                return SyncResult.Failure("终端授权异常！");
            }

            var folderDao = GetCfgFolderDaoById(dto.folder_id);
            if (folderDao == null)
            {
                LogUtils.Error("PostSync", "目录信息异常！");
                return SyncResult.Failure("目录信息异常！");
            }

            dto.path = folderDao.GetStoragePath(dto.path);
            dto.src = folderDao.GetStoragePath(dto.src);

            var result = new SyncResult();
            if (dto.opt == NasOptEnums.Delete)
            {
                DealDeleteFile(terminalDao, dto, result);
                return result;
            }

            if (dto.opt == NasOptEnums.Create)
            {
                await DealCreateFile(terminalDao, dto, result);
                return result;
            }

            if (dto.opt == NasOptEnums.Rename)
            {
                await DealRenameFile(terminalDao, dto, result);
                return result;
            }

            if (dto.opt == NasOptEnums.Move)
            {
                await DealMoveFile(terminalDao, dto, result);
                return result;
            }

            if (dto.opt == NasOptEnums.Copy)
            {
                await DealCopyFile(terminalDao, dto, result);
                return result;
            }

            if (dto.opt == NasOptEnums.Change)
            {
                await DealChangeFile(terminalDao, dto, result);
                return result;
            }

            LogUtils.Error("PostSync", "不支持的操作：" + dto.opt);
            result.SetFailure("不支持的操作：" + dto.opt);
            return result;
        }
        #endregion

        #region 删除文件
        private const string TAG_DELETE_FILE = "删除文件";

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool DealDeleteFile(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Info(TAG_DELETE_FILE, "删除文件", dto.path);

            if (dto == null)
            {
                return false;
            }

            if (dto.type == ScmFileTypeEnum.Dir)
            {
                return DealDeleteDir(token, dto, result);
            }

            if (dto.type == ScmFileTypeEnum.Doc)
            {
                return DealDeleteDoc(token, dto, result);
            }

            LogUtils.Error(TAG_DELETE_FILE, "未知的文件类型：" + dto.type);
            result.SetFailure("未知的文件类型：" + dto.type);
            return false;
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool DealDeleteDoc(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Info(TAG_DELETE_FILE, "删除文档", dto.path);

            var dstFile = GetNativePath(token, dto.path);
            FileUtils.DeleteDoc(dstFile);

            var parentList = new List<SyncResFileDao>();
            var parentDao = GetDirDaoByPath(token, NasUtils.GetParentPath(dto.path), parentList);
            var docDao = GetDocDaoByPath(token.user_id, dto.path);

            var resId = 0L;
            var ver = 0L;
            var dirId = 0L;
            if (docDao != null)
            {
                DealDeleteDocDao(docDao);
                resId = docDao.id;
                ver = docDao.ver;
                dirId = docDao.dir_id;
            }

            AddLogFileByDto(token, dto, resId, dirId, parentList);

            LogUtils.Info(TAG_DELETE_FILE, "文档删除完成", dto.path);
            result.SetSuccess(resId, ver);
            return true;
        }

        /// <summary>
        /// 删除文档数据
        /// </summary>
        /// <param name="dao"></param>
        private void DealDeleteDocDao(Sync.SyncResFileDao dao)
        {
            _SqlClient.Deleteable(dao).ExecuteCommand();
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool DealDeleteDir(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Info(TAG_DELETE_FILE, "删除目录", dto.path);

            var dstFile = GetNativePath(token, dto.path);
            FileUtils.DeleteDir(dstFile);

            var parentList = new List<SyncResFileDao>();
            var dirDao = GetDirDaoByPath(token, dto.path, parentList);
            //var dirDao = GetDirDaoByPath(token.user_id, dto.path);

            var resId = 0L;
            var dirId = 0L;
            var ver = 0L;
            if (dirDao != null)
            {
                DealDeleteDirDao(dirDao);

                DeleteResFileDao(token, dirDao);

                resId = dirDao.id;
                dirId = dirDao.dir_id;
                ver = dirDao.ver;
            }

            AddLogFileByDto(token, dto, resId, dirId, parentList);

            LogUtils.Info(TAG_DELETE_FILE, "目录删除完成", dto.path);
            result.SetSuccess(resId, ver);
            return true;
        }

        /// <summary>
        /// 级联删除目录数据
        /// </summary>
        /// <param name="dao"></param>
        private void DealDeleteDirDao(Sync.SyncResFileDao dao)
        {
            var dirList = _SqlClient.Queryable<Sync.SyncResFileDao>()
                .Where(a => a.type == ScmFileTypeEnum.Dir && a.dir_id == dao.id)
                .ToList();
            foreach (var dir in dirList)
            {
                DealDeleteDirDao(dir);
            }

            _SqlClient.Deleteable<Sync.SyncResFileDao>()
                .Where(a => a.dir_id == dao.id)
                .ExecuteCommand();
        }
        #endregion

        #region 创建文件
        private const string TAG_CREATE_FILE = "创建文件";

        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> DealCreateFile(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Info(TAG_CREATE_FILE, "文件路径", dto.path);

            if (dto.type == ScmFileTypeEnum.Dir)
            {
                return await DealCreateDir(token, dto, result);
            }

            if (dto.type == ScmFileTypeEnum.Doc)
            {
                return await DealCreateDoc(token, dto, result);
            }

            LogUtils.Error(TAG_CREATE_FILE, "未知的文件类型：" + dto.type);
            result.SetFailure("未知的文件类型：" + dto.type);
            return false;
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> DealCreateDir(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Info(TAG_CREATE_FILE, "创建目录", dto.path);

            var path = GetNativePath(token, dto.path);
            LogUtils.Debug(TAG_CREATE_FILE, "目录物理路径", path);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var parentList = new List<SyncResFileDao>();
            var dirDao = CreateRecursiveDirDao(token, dto.path, parentList);

            AddLogFileByDto(token, dto, dirDao.id, dirDao.dir_id, parentList);

            LogUtils.Info(TAG_CREATE_FILE, "目录创建完成", dto.path);
            result.SetSuccess(dirDao.id, dirDao.ver);
            return true;
        }

        /// <summary>
        /// 创建文档
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> DealCreateDoc(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Info(TAG_CREATE_FILE, "创建文档", dto.path);

            if (string.IsNullOrEmpty(dto.src))
            {
                LogUtils.Error(TAG_CREATE_FILE, "上传文档来源路径为空", dto.src);
                return false;
            }

            var tmpFile = _EnvConfig.GetTempPath(dto.src);
            if (!FileUtils.ExistsDoc(tmpFile))
            {
                LogUtils.Error(TAG_CREATE_FILE, "上传文档不存在", tmpFile);
                result.SetFailure($"上传文档不存在！");
                return false;
            }

            var dstFile = GetNativePath(token, dto.path);
            if (!FileUtils.MoveDoc(tmpFile, dstFile, true))
            {
                LogUtils.Error(TAG_CREATE_FILE, "上传文档移动异常！");
                SyncResult.Failure("上传文档移动异常！");
                return false;
            }

            var parentList = new List<SyncResFileDao>();
            var dirDao = CreateRecursiveDirDao(token, NasUtils.GetParentPath(dto.path), parentList);

            var docDao = AddCreateFile(token, dto, dirDao.id);

            AddLogFileByDto(token, dto, docDao.id, dirDao.id, parentList);

            LogUtils.Info(TAG_CREATE_FILE, "文档创建完成", dto.path);
            result.SetSuccess(docDao.id, docDao.ver);
            return true;
        }

        private Sync.SyncResFileDao AddCreateFile(ScmUrTerminalDao token, NasLogFileDto logDto, long dirId)
        {
            var resDao = GetResFileDaoByPath(token.user_id, logDto.path, logDto.type);
            if (resDao == null)
            {
                resDao = logDto.Adapt<Sync.SyncResFileDao>();
                resDao.user_id = token.user_id;
                resDao.dir_id = dirId;
                resDao.PrepareCreate(token.user_id);

                _SqlClient.Insertable(resDao).ExecuteCommand();
            }
            return resDao;
        }
        #endregion

        #region 移动文件
        private const string TAG_MOVE_FILE = "移动文件";

        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> DealMoveFile(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Info(TAG_MOVE_FILE, $"移动文件", $"{dto.src} -> {dto.path}");

            if (dto == null)
            {
                return false;
            }

            if (dto.type == ScmFileTypeEnum.Dir)
            {
                return await DealMoveDir(token, dto, result);
            }

            if (dto.type == ScmFileTypeEnum.Doc)
            {
                return await DealMoveDoc(token, dto, result);
            }

            LogUtils.Error(TAG_MOVE_FILE, "未知的文件类型：" + dto.type);
            result.SetFailure("未知的文件类型：" + dto.type);
            return false;
        }

        /// <summary>
        /// 移动目录
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> DealMoveDir(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Info(TAG_MOVE_FILE, "移动目录", dto.path);

            var srcDir = GetNativePath(token, dto.src);
            if (!FileUtils.ExistsDir(srcDir))
            {
                LogUtils.Error(TAG_MOVE_FILE, "来源目录不存在", srcDir);
                result.SetFailure($"来源目录 {dto.src} 不存在！");
                return false;
            }

            var parentList = new List<SyncResFileDao>();
            var parentDao = CreateRecursiveDirDao(token, NasUtils.GetParentPath(dto.path), parentList);
            var dstDao = GetDirDaoByPath(token.user_id, dto.path);
            if (dstDao != null)
            {
                DeleteResFileDao(token, dstDao);
            }

            var srcDao = GetDirDaoByPath(token.user_id, dto.src);
            if (srcDao != null)
            {
                UpdateResDirDao(srcDao, token, dto.name, dto.path, parentDao.id);
            }
            else
            {
                srcDao = AddResDirDao(token, dto, parentDao.id);
            }

            var dstDir = GetNativePath(token, dto.path);
            DealMoveDirRecursive(token, srcDao, srcDir, dto.src, dstDir, dto.path);

            AddLogFileByDto(token, dto, srcDao.id, srcDao.dir_id, parentList);

            LogUtils.Info(TAG_MOVE_FILE, "目录移动完成", dto.path);
            result.SetSuccess(srcDao.id, srcDao.ver);
            return true;
        }

        /// <summary>
        /// 目录级联移动
        /// </summary>
        /// <param name="token"></param>
        /// <param name="parentDao"></param>
        /// <param name="srcDir"></param>
        /// <param name="srcPath"></param>
        /// <param name="dstDir"></param>
        /// <param name="dstPath"></param>
        /// <returns></returns>
        private bool DealMoveDirRecursive(ScmUrTerminalDao token, SyncResFileDao parentDao, string srcDir, string srcPath, string dstDir, string dstPath)
        {
            if (!Directory.Exists(dstDir))
            {
                Directory.CreateDirectory(dstDir);
            }

            var dirList = Directory.GetDirectories(srcDir);
            foreach (var dir in dirList)
            {
                var name = FileUtils.GetFileName(dir);
                var dstUri = dstPath + NasEnv.WebSeparator + name;

                // 若目标存在，则删除
                var dstDao = GetDirDaoByPath(token.user_id, dstUri);
                if (dstDao != null)
                {
                    DeleteResFileDao(token, dstDao);
                }

                var srcUri = srcPath + NasEnv.WebSeparator + name;
                var dirDao = GetDirDaoByPath(token.user_id, srcUri);
                if (dirDao != null)
                {
                    UpdateResDirDao(dirDao, token, name, dstUri, parentDao.id);
                }
                else
                {
                    dirDao = AddResDirDao(token, parentDao.id, name, dstUri);
                }

                var dstFile = Path.Combine(dstDir, name);
                if (!Directory.Exists(dstFile))
                {
                    Directory.CreateDirectory(dstFile);
                }

                DealMoveDirRecursive(token, dirDao, dir, srcUri, dstFile, dstUri);
            }

            var docList = Directory.GetFiles(srcDir);
            foreach (var doc in docList)
            {
                var name = FileUtils.GetFileName(doc);
                var dstUri = dstPath + NasEnv.WebSeparator + name;

                // 若目标存在，则删除
                var dstDao = GetDocDaoByPath(token.user_id, dstUri);
                if (dstDao != null)
                {
                    DeleteResFileDao(token, dstDao);
                }

                var srcUri = srcPath + NasEnv.WebSeparator + name;
                var docDao = GetDocDaoByPath(token.user_id, srcUri);
                if (docDao != null)
                {
                    var kind = GetFileKind(name);
                    UpdateResFileDao(docDao, token, name, dstUri, kind, parentDao.id);
                }
                else
                {
                    var hash = FileUtils.Sha(doc);
                    var info = new FileInfo(doc);
                    var time = TimeUtils.GetUnixTime(info.LastAccessTimeUtc);
                    docDao = AddResDocDao(token, parentDao.id, dstUri, name, hash, info.Length, time);
                }

                var dstFile = Path.Combine(dstDir, name);
                File.Move(doc, dstFile, true);
            }

            Directory.Delete(srcDir, true);

            return true;
        }

        /// <summary>
        /// 移动文档
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> DealMoveDoc(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Info(TAG_MOVE_FILE, $"移动文档", $"{dto.src} -> {dto.path}");

            var srcFile = GetNativePath(token, dto.src);
            if (!FileUtils.ExistsDoc(srcFile))
            {
                LogUtils.Error(TAG_MOVE_FILE, "来源文档不存在", srcFile);
                result.SetFailure($"来源文档 {dto.src} 不存在！");
                return false;
            }

            var dstFile = GetNativePath(token, dto.path);
            FileUtils.MoveDoc(srcFile, dstFile, true);

            var dstDao = GetDocDaoByPath(token.user_id, dto.path);
            if (dstDao != null)
            {
                DeleteResFileDao(token, dstDao);
            }

            // 创建父级目录
            var parentList = new List<SyncResFileDao>();
            var parentDao = CreateRecursiveDirDao(token, NasUtils.GetParentPath(dto.path), parentList);

            var srcDao = GetDocDaoByPath(token.user_id, dto.src);
            if (srcDao != null)
            {
                UpdateResFileDao(srcDao, token, dto.name, dto.path, dto.kind, parentDao.id);
            }
            else
            {
                srcDao = AddResFileByDto(token, dto, parentDao.id);
            }

            AddLogFileByDto(token, dto, srcDao.id, srcDao.dir_id, parentList);

            LogUtils.Info(TAG_MOVE_FILE, "文档移动完成", dto.path);
            result.SetSuccess(srcDao.id, srcDao.ver);
            return true;
        }
        #endregion

        #region 复件文件
        private const string TAG_COPY_FILE = "复制文件";

        /// <summary>
        /// 复件文件
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> DealCopyFile(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Info(TAG_COPY_FILE, $"复制文件", $"{dto.src} -> {dto.path}");

            if (dto == null)
            {
                return false;
            }

            if (dto.type == ScmFileTypeEnum.Dir)
            {
                return await DealCopyDir(token, dto, result);
            }

            if (dto.type == ScmFileTypeEnum.Doc)
            {
                return await DealCopyDoc(token, dto, result);
            }

            LogUtils.Error(TAG_COPY_FILE, "未知的文件类型：" + dto.type);
            result.SetFailure("未知的文件类型：" + dto.type);
            return false;
        }

        /// <summary>
        /// 复件目录
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> DealCopyDir(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Info(TAG_COPY_FILE, $"复制目录", $"{dto.src} -> {dto.path}");

            var srcPath = GetNativePath(token, dto.src);
            if (!FileUtils.ExistsDir(srcPath))
            {
                LogUtils.Error(TAG_COPY_FILE, "来源目录不存在", srcPath);
                result.SetFailure($"来源目录 {dto.src} 不存在！");
                return false;
            }

            var parentList = new List<SyncResFileDao>();
            var parentDao = CreateRecursiveDirDao(token, NasUtils.GetParentPath(dto.path), parentList);

            var dstDao = GetDirDaoByPath(token.user_id, dto.path);
            if (dstDao != null)
            {
                UpdateResDirDao(dstDao, token, dto.name, dto.path, parentDao.id);
            }
            else
            {
                dstDao = AddResDirDao(token, dto, parentDao.id);
            }

            var dstDir = GetNativePath(token, dto.path);
            DealCopyDirRecursive(token, dstDao, srcPath, dto.src, dstDir, dto.path);

            AddLogFileByDto(token, dto, dstDao.id, dstDao.dir_id, parentList);

            LogUtils.Info(TAG_COPY_FILE, "目录复制完成", dto.path);
            result.SetSuccess(dstDao.id, dstDao.ver);
            return true;
        }

        /// <summary>
        /// 目录级联复制
        /// </summary>
        /// <param name="token"></param>
        /// <param name="parentDao"></param>
        /// <param name="srcDir"></param>
        /// <param name="srcPath"></param>
        /// <param name="dstDir"></param>
        /// <param name="dstPath"></param>
        /// <returns></returns>
        private bool DealCopyDirRecursive(ScmUrTerminalDao token, SyncResFileDao parentDao, string srcDir, string srcPath, string dstDir, string dstPath)
        {
            if (!Directory.Exists(dstDir))
            {
                Directory.CreateDirectory(dstDir);
            }

            var dirList = Directory.GetDirectories(srcDir);
            foreach (var dir in dirList)
            {
                var name = FileUtils.GetFileName(dir);
                var dstUri = dstPath + NasEnv.WebSeparator + name;

                // 若目标存在，则删除
                var dstDao = GetDirDaoByPath(token.user_id, dstUri);
                if (dstDao != null)
                {
                    UpdateResDirDao(dstDao, token, name, dstUri, parentDao.id);
                }
                else
                {
                    dstDao = AddResDirDao(token, parentDao.id, name, dstUri);
                }

                var dstFile = Path.Combine(dstDir, name);
                if (!Directory.Exists(dstFile))
                {
                    Directory.CreateDirectory(dstFile);
                }

                var srcUri = srcPath + NasEnv.WebSeparator + name;
                DealCopyDirRecursive(token, dstDao, dir, srcUri, dstFile, dstUri);
            }

            var docList = Directory.GetFiles(srcDir);
            foreach (var doc in docList)
            {
                var name = FileUtils.GetFileName(doc);
                var dstUri = dstPath + NasEnv.WebSeparator + name;

                var dstDao = GetDocDaoByPath(token.user_id, dstUri);
                if (dstDao != null)
                {
                    var kind = GetFileKind(name);
                    UpdateResFileDao(dstDao, token, name, dstUri, kind, parentDao.id);
                }
                else
                {
                    var hash = FileUtils.Sha(doc);
                    var info = new FileInfo(doc);
                    var time = TimeUtils.GetUnixTime(info.LastAccessTimeUtc);
                    dstDao = AddResDocDao(token, parentDao.id, name, dstUri, hash, info.Length, time);
                }

                var dstFile = Path.Combine(dstDir, name);
                File.Copy(doc, dstFile, true);
            }

            return true;
        }

        /// <summary>
        /// 复制文档
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> DealCopyDoc(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Info(TAG_COPY_FILE, "复制文档", $"{dto.src} -> {dto.path}");

            var srcFile = GetNativePath(token, dto.src);
            if (!FileUtils.ExistsDoc(srcFile))
            {
                LogUtils.Error(TAG_COPY_FILE, "来源文档不存在", srcFile);
                result.SetFailure($"来源文档 {dto.src} 不存在！");
                return false;
            }

            var dstFile = GetNativePath(token, dto.path);
            FileUtils.CopyDoc(srcFile, dstFile, true);

            var parentList = new List<SyncResFileDao>();
            var parentDao = CreateRecursiveDirDao(token, NasUtils.GetParentPath(dto.path), parentList);

            var dstDao = GetDocDaoByPath(token.user_id, dto.path);
            if (dstDao != null)
            {
                dstDao.dir_id = parentDao.id;
                dstDao.name = dto.name;
                dstDao.path = dto.path;
                dstDao.hash = dto.hash;
                dstDao.size = dto.size;
                UpdateResFileDao(token, dstDao);
            }
            else
            {
                // 追加文档记录
                dstDao = AddResFileByDto(token, dto, parentDao.id);
            }

            AddLogFileByDto(token, dto, dstDao.id, parentDao.id, parentList);

            LogUtils.Info(TAG_COPY_FILE, "文档复制完成", dto.path);
            result.SetSuccess(dstDao.id, dstDao.ver);
            return true;
        }
        #endregion

        #region 更名文件
        private const string TAG_RENAME_FILE = "更名文件";

        /// <summary>
        /// 更名文件
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> DealRenameFile(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Info(TAG_RENAME_FILE, "更名文件", $"{dto.src} -> {dto.path}");

            if (dto == null)
            {
                return false;
            }

            if (dto.type == ScmFileTypeEnum.Dir)
            {
                return await DealRenameDir(token, dto, result);
            }

            if (dto.type == ScmFileTypeEnum.Doc)
            {
                return await DealRenameDoc(token, dto, result);
            }

            LogUtils.Error(TAG_RENAME_FILE, "未知的文件类型：" + dto.type);
            result.SetFailure("未知的文件类型：" + dto.type);
            return false;
        }

        /// <summary>
        /// 更名目录
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> DealRenameDir(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Info(TAG_RENAME_FILE, "更名目录", $"{dto.src} -> {dto.path}");

            var srcDir = GetNativePath(token, dto.src);
            if (!FileUtils.ExistsDir(srcDir))
            {
                LogUtils.Error(TAG_RENAME_FILE, "来源目录不存在", srcDir);
                result.SetFailure($"来源目录 {dto.src} 不存在！");
                return false;
            }

            var parentList = new List<SyncResFileDao>();
            var parentDao = CreateRecursiveDirDao(token, NasUtils.GetParentPath(dto.path), parentList);

            var dstDao = GetDirDaoByPath(token.user_id, dto.path);
            if (dstDao != null)
            {
                DeleteResFileDao(token, dstDao);
            }

            var srcDao = GetDirDaoByPath(token.user_id, dto.src);
            if (srcDao != null)
            {
                UpdateResDirDao(srcDao, token, dto.name, dto.path, parentDao.id);
            }
            else
            {
                srcDao = AddResDirDao(token, dto, parentDao.id);
            }

            var dstDir = GetNativePath(token, dto.path);
            DealMoveDirRecursive(token, srcDao, srcDir, dto.src, dstDir, dto.path);

            AddLogFileByDto(token, dto, srcDao.id, srcDao.dir_id, parentList);

            LogUtils.Info(TAG_RENAME_FILE, "目录更名完成", dto.path);
            result.SetSuccess(srcDao.id, srcDao.ver);
            return true;
        }

        /// <summary>
        /// 移动文档
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> DealRenameDoc(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Info(TAG_RENAME_FILE, "更名文档", $"{dto.src} -> {dto.path}");

            var srcFile = GetNativePath(token, dto.src);
            if (!FileUtils.ExistsDoc(srcFile))
            {
                LogUtils.Error(TAG_RENAME_FILE, "来源文档不存在", srcFile);
                result.SetFailure($"来源文档 {dto.src} 不存在！");
                return false;
            }

            var dstFile = GetNativePath(token, dto.path);
            FileUtils.MoveDoc(srcFile, dstFile, true);

            var parentList = new List<SyncResFileDao>();
            var paretnDao = CreateRecursiveDirDao(token, NasUtils.GetParentPath(dto.path), parentList);

            var dstDao = GetDocDaoByPath(token.user_id, dto.path);
            if (dstDao != null)
            {
                DeleteResFileDao(token, dstDao);
            }

            var srcDao = GetDocDaoByPath(token.user_id, dto.src);
            if (srcDao != null)
            {
                UpdateResFileDao(srcDao, token, dto.name, dto.path, dto.kind, paretnDao.id);
            }
            else
            {
                // 追加文档记录
                srcDao = AddResFileByDto(token, dto, paretnDao.id);
            }

            AddLogFileByDto(token, dto, srcDao.id, paretnDao.id, parentList);

            LogUtils.Info(TAG_RENAME_FILE, "文档更名完成", dto.path);
            result.SetSuccess(srcDao.id, srcDao.ver);
            return true;
        }
        #endregion

        #region 更新文件
        private const string TAG_CHANGE_FILE = "更新文件";

        /// <summary>
        /// 更新文件
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> DealChangeFile(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Info(TAG_CHANGE_FILE, "更新文件", dto.path);

            if (dto.type == ScmFileTypeEnum.Dir)
            {
                return await DealChangeDir(token, dto, result);
            }

            if (dto.type == ScmFileTypeEnum.Doc)
            {
                return await DealChangeDoc(token, dto, result);
            }

            LogUtils.Error(TAG_CHANGE_FILE, "未知的文件类型：" + dto.type);
            result.SetFailure("未知的文件类型：" + dto.type);
            return false;
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> DealChangeDoc(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Info(TAG_CHANGE_FILE, "更新文档", dto.path);

            if (string.IsNullOrEmpty(dto.src))
            {
                LogUtils.Error(TAG_CHANGE_FILE, "上传文档来源路径为空", dto.src);
                return false;
            }

            var tmpFile = _EnvConfig.GetTempPath(dto.src);
            if (!FileUtils.ExistsDoc(tmpFile))
            {
                LogUtils.Error(TAG_CHANGE_FILE, "上传文档不存在", tmpFile);
                result.SetFailure($"上传文档不存在！");
                return false;
            }

            var dstFile = GetNativePath(token, dto.path);
            var dstDir = FileUtils.GetDir(dstFile);
            if (!Directory.Exists(dstDir))
            {
                Directory.CreateDirectory(dstDir);
            }

            if (!FileUtils.MoveDoc(tmpFile, dstFile, true))
            {
                LogUtils.Error(TAG_CHANGE_FILE, "上传文档移动异常！");
                SyncResult.Failure("上传文档移动异常！");
                return false;
            }

            var parentList = new List<SyncResFileDao>();
            var parentDao = CreateRecursiveDirDao(token, NasUtils.GetParentPath(dto.path), parentList);

            var docDao = GetDocDaoByPath(token.user_id, dto.path);
            if (docDao == null)
            {
                docDao = AddCreateFile(token, dto, parentDao.id);
            }
            else
            {
                docDao.hash = dto.hash;
                docDao.size = dto.size;
                docDao.dir_id = parentDao.id;
                UpdateResFileDao(token, docDao);
            }

            AddLogFileByDto(token, dto, docDao.id, parentDao.id, parentList);

            LogUtils.Info(TAG_CHANGE_FILE, "文档更新完成", dto.path);
            result.SetSuccess(docDao.id, docDao.ver);
            return true;
        }

        /// <summary>
        /// 更新目录
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> DealChangeDir(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Info(TAG_CHANGE_FILE, "更新目录", dto.path);

            var tmpFile = GetNativePath(token, dto.path);
            if (!Directory.Exists(tmpFile))
            {
                Directory.CreateDirectory(tmpFile);
            }

            var parentList = new List<SyncResFileDao>();
            var dirDao = CreateRecursiveDirDao(token, dto.path, parentList);

            AddLogFileByDto(token, dto, dirDao.id, dirDao.dir_id, parentList);

            LogUtils.Info(TAG_CHANGE_FILE, "目录更新完成", dto.path);
            result.SetSuccess(dirDao.id, dirDao.ver);
            return true;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 获取所有驱动列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private List<SyncCfgFolderDao> ListFolderDao(long userId)
        {
            return _SqlClient.Queryable<SyncCfgFolderDao>()
                .Where(a => a.user_id == userId && a.row_status == ScmRowStatusEnum.Enabled)
                .ToList();
        }

        private Sync.SyncResFileDao GetResFileDaoByName(long userId, long dirId, string name, ScmFileTypeEnum type)
        {
            return _SqlClient.Queryable<Sync.SyncResFileDao>()
                .Where(a => a.user_id == userId && a.dir_id == dirId && a.name == name && a.type == type)
                .First();
        }

        private Sync.SyncResFileDao GetDirDaoByName(long userId, long dirId, string name)
        {
            return GetResFileDaoByName(userId, dirId, name, ScmFileTypeEnum.Dir);
        }

        /// <summary>
        /// 此方法将会重写，根据路径获取文件对象
        /// </summary>
        /// <param name="path">虚拟绝对路径</param>
        /// <returns></returns>
        private Sync.SyncResFileDao GetResFileDaoByPath(long userId, string path, ScmFileTypeEnum type)
        {
            return _SqlClient.Queryable<Sync.SyncResFileDao>()
                .Where(a => a.user_id == userId && a.path == path && a.type == type)
                .First();
        }

        /// <summary>
        /// 此方法将会重写
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private Sync.SyncResFileDao GetDirDaoByPath(long userId, string path)
        {
            return GetResFileDaoByPath(userId, path, ScmFileTypeEnum.Dir);
        }

        /// <summary>
        /// 此方法将会重写
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private Sync.SyncResFileDao GetDocDaoByPath(long userId, string path)
        {
            return GetResFileDaoByPath(userId, path, ScmFileTypeEnum.Doc);
        }

        private SyncResFileDao UpdateResDirDao(SyncResFileDao dao, ScmUrTerminalDao token, string name, string path, long dirId)
        {
            LogUtils.Debug("更新目录记录" + path);

            dao.name = name;
            dao.path = path;
            dao.kind = ScmFileKindEnum.None;
            dao.dir_id = dirId;
            dao.PrepareUpdate(token.user_id);
            _SqlClient.Updateable(dao).ExecuteCommand();
            return dao;
        }

        private SyncResFileDao UpdateResFileDao(SyncResFileDao dao, ScmUrTerminalDao token, string name, string path, ScmFileKindEnum kind, long dirId)
        {
            LogUtils.Debug("更新文档记录" + path);

            dao.name = name;
            dao.path = path;
            dao.kind = kind;
            dao.dir_id = dirId;
            dao.PrepareUpdate(token.user_id);
            _SqlClient.Updateable(dao).ExecuteCommand();
            return dao;
        }

        private void UpdateResFileDao(ScmUrTerminalDao token, Sync.SyncResFileDao dao)
        {
            dao.PrepareUpdate(token.user_id);
            _SqlClient.Updateable(dao).ExecuteCommand();
        }

        private List<Sync.SyncResFileDao> ListResFileDaoByParent(long dirId, ScmFileTypeEnum type)
        {
            return _SqlClient.Queryable<Sync.SyncResFileDao>()
                .Where(a => a.dir_id == dirId && a.type == type)
                .ToList();
        }

        private List<Sync.SyncResFileDao> ListDirDaoByParent(long dirId)
        {
            return ListResFileDaoByParent(dirId, ScmFileTypeEnum.Dir);
        }

        private List<Sync.SyncResFileDao> ListDocDaoByParent(long dirId)
        {
            return ListResFileDaoByParent(dirId, ScmFileTypeEnum.Doc);
        }

        public long GetParentIdByPath(long userId, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return NasEnv.DEF_DIR_ID;
            }

            path = path.TrimEnd(NasEnv.WebSeparator);
            if (string.IsNullOrEmpty(path))
            {
                return NasEnv.DEF_DIR_ID;
            }

            var index = path.LastIndexOf(NasEnv.WebSeparator);
            if (index > 0)
            {
                path = path.Substring(0, index);
            }

            var dirDao = GetDirDaoByPath(userId, path);
            return dirDao != null ? dirDao.id : NasEnv.DEF_DIR_ID;
        }

        /// <summary>
        /// 获取物理路径
        /// （服务端路径，不可与客户端方法相同）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetNativePath(ScmUrTerminalDao token, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            if (path.StartsWith(NasEnv.VirtualTag, StringComparison.OrdinalIgnoreCase))
            {
                path = path.Substring(NasEnv.VirtualTag.Length);
            }

            if (path[0] != NasEnv.WebSeparator)
            {
                return path;
            }

            var userDao = _ResHolder.GetRes<UserDao>(token.user_id);
            if (userDao == null)
            {
                return path;
            }

            return _EnvConfig.GetDataPath($"/{NasEnv.DEF_NAS_DIR}/{userDao.codes}" + path);
        }

        private string GetClientPath(ScmUrTerminalDao token, string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return path;
            }

            var userDao = _ResHolder.GetRes<UserDao>(token.user_id);
            if (userDao == null)
            {
                return path;
            }

            var key = "/" + userDao.codes;
            if (path.StartsWith(key, StringComparison.OrdinalIgnoreCase))
            {
                path = path.Substring(key.Length);
            }

            return path;
        }

        /// <summary>
        /// 创建特殊目录（如：回收站、收藏夹等）
        /// </summary>
        /// <param name="token"></param>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        private SyncResFileDao CreateSpecialDirDao(ScmUrTerminalDao token, string name, string path, ScmFileKindEnum kind)
        {
            var dao = _SqlClient.Queryable<SyncResFileDao>()
                .Where(a => a.path == path && a.user_id == token.user_id)
                .First();

            if (dao == null)
            {
                dao = new Sync.SyncResFileDao();
                dao.user_id = token.user_id;
                dao.type = ScmFileTypeEnum.Dir;
                dao.kind = kind;
                dao.name = name;
                dao.path = path;
                dao.dir_id = NasEnv.DEF_DIR_ID;
                dao.PrepareCreate(token.user_id);
                _SqlClient.Insertable(dao).ExecuteCommand();
            }

            return dao;
        }

        /// <summary>
        /// 根据路径级联创建目录，
        /// 说明：由于操作系统对路径长度的支持不同，此方法可能会抛出异常
        /// </summary>
        /// <param name="token"></param>
        /// <param name="path"></param>
        /// <param name="dirList">返回的上级列表</param>
        /// <returns></returns>
        private SyncResFileDao CreateRecursiveDirDao(ScmUrTerminalDao token, string path, List<SyncResFileDao> dirList = null)
        {
            LogUtils.Debug("CreateRecursiveDirDao:" + path);

            SyncResFileDao parentDao = new SyncResFileDao { id = NasEnv.DEF_DIR_ID };

            var tmp = "";
            path = path.Trim(NasEnv.WebSeparator);
            foreach (var arr in path.Split(NasEnv.WebSeparator))
            {
                if (string.IsNullOrEmpty(arr))
                {
                    continue;
                }

                tmp += NasEnv.WebSeparator + arr;
                var dao = GetDirDaoByName(token.user_id, parentDao.id, arr);
                if (dao == null)
                {
                    dao = AddResDirDao(token, parentDao.id, arr, tmp);
                }
                else
                {
                    UpdateResDirDao(dao, token, arr, tmp, parentDao.id);
                }

                if (dirList != null)
                {
                    dirList.Add(dao);
                }

                parentDao = dao;
            }

            return parentDao;
        }

        private SyncResFileDao GetDirDaoByPath(ScmUrTerminalDao token, string path, List<SyncResFileDao> daoList)
        {
            LogUtils.Debug("GetDirDaoByPath:" + path);

            SyncResFileDao parentDao = new SyncResFileDao { id = NasEnv.DEF_DIR_ID };

            var tmp = "";
            path = path.Trim(NasEnv.WebSeparator);
            foreach (var arr in path.Split(NasEnv.WebSeparator))
            {
                if (string.IsNullOrEmpty(arr))
                {
                    continue;
                }

                tmp += NasEnv.WebSeparator + arr;
                var dao = GetDirDaoByName(token.user_id, parentDao.id, arr);
                if (dao == null)
                {
                    return null;
                }
                daoList.Add(dao);

                parentDao = dao;
            }

            return parentDao;
        }

        private void DeleteResFileDao(ScmUrTerminalDao token, SyncResFileDao dao)
        {
            _SqlClient.Deleteable(dao).ExecuteCommand();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="token"></param>
        /// <param name="folderId"></param>
        /// <param name="dirId"></param>
        /// <param name="name"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private SyncResFileDao AddResDirDao(ScmUrTerminalDao token, long dirId, string name, string path)
        {
            var dao = new Sync.SyncResFileDao
            {
                user_id = token.user_id,
                type = ScmFileTypeEnum.Dir,
                name = name,
                path = path,
                dir_id = dirId, // 根目录
            };
            dao.PrepareCreate(token.user_id);
            _SqlClient.Insertable(dao).ExecuteCommand();
            return dao;
        }

        /// <summary>
        /// 添加目录记录
        /// </summary>
        /// <param name="token"></param>
        /// <param name="dto"></param>
        /// <param name="dirId"></param>
        /// <returns></returns>
        private Sync.SyncResFileDao AddResDirDao(ScmUrTerminalDao token, NasLogFileDto dto, long dirId)
        {
            var dirDao = new Sync.SyncResFileDao
            {
                user_id = token.user_id,
                type = ScmFileTypeEnum.Dir,
                name = dto.name,
                path = dto.path,
                dir_id = dirId, // 根目录
            };
            dirDao.PrepareCreate(token.user_id);
            _SqlClient.Insertable(dirDao).ExecuteCommand();
            return dirDao;
        }

        /// <summary>
        /// 获取文档分类
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private ScmFileKindEnum GetFileKind(string file)
        {
            if (string.IsNullOrWhiteSpace(file))
            {
                return ScmFileKindEnum.Unknown;
            }

            var ext = FileUtils.GetExtension(file);
            if (string.IsNullOrEmpty(ext))
            {
                return ScmFileKindEnum.Unknown;
            }
            ext = ext.ToLower().TrimStart('.');

            return NasHelper.getKind(ext);
        }

        /// <summary>
        /// 添加文档记录
        /// </summary>
        /// <param name="token"></param>
        /// <param name="dirId"></param>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="hash"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private Sync.SyncResFileDao AddResDocDao(ScmUrTerminalDao token, long dirId, string path, string name, string hash, long size, long time)
        {
            var kind = GetFileKind(name);
            var dirDao = new Sync.SyncResFileDao
            {
                user_id = token.user_id,
                type = ScmFileTypeEnum.Doc,
                kind = kind,
                name = name,
                path = path,
                hash = hash,
                size = size,
                modify_time = time,
                dir_id = dirId, // 根目录
            };
            _SqlClient.Insertable(dirDao).ExecuteCommand();
            return dirDao;
        }

        private SyncResFileDao AddResFileByDto(ScmUrTerminalDao token, NasLogFileDto logDto, long dirId)
        {
            LogUtils.Debug("添加文件记录" + logDto.path);

            var docDao = logDto.Adapt<Sync.SyncResFileDao>();
            docDao.user_id = token.user_id;
            docDao.dir_id = dirId;
            docDao.PrepareCreate(token.user_id);
            _SqlClient.Insertable(docDao).ExecuteCommand();
            return docDao;
        }

        /// <summary>
        /// 记录操作日志
        /// </summary>
        /// <param name="token"></param>
        /// <param name="logDto"></param>
        /// <returns></returns>
        private void AddLogFileByDto(ScmUrTerminalDao token, NasLogFileDto logDto, long resId, long dirId, List<SyncResFileDao> parentList)
        {
            var logDao = logDto.Adapt<Sync.SyncLogFileDao>();
            logDao.user_id = token.user_id;
            logDao.terminal_id = token.id;
            logDao.folder_id = logDto.folder_id;
            logDao.res_id = resId;
            logDao.dir_id = dirId;
            logDao.terminal_id = token.id;
            logDao.PrepareCreate(token.user_id);
            _SqlClient.Insertable(logDao).ExecuteCommand();

            if (parentList == null)
            {
                LogUtils.Error("AddLogFileByDto", "父级列表为空，无法记录文件夹日志");
                return;
            }

            // 按文件夹记录日志
            var folderList = ListFolderDao(token.user_id);
            foreach (var folder in folderList)
            {
                if (folder.id == logDto.folder_id)
                {
                    continue;
                }

                var parent = parentList.Find(a => a.id == folder.res_id);
                if (parent == null)
                {
                    continue;
                }

                var tmpDao = new SyncLogFolderDao();
                tmpDao.user_id = token.user_id;
                tmpDao.folder_id = folder.id;
                tmpDao.log_id = logDao.id;
                tmpDao.PrepareCreate(token.user_id);
                _SqlClient.Insertable(tmpDao).ExecuteCommand();
            }
        }

        private SyncCfgFolderDao GetCfgFolderDaoById(long folderId)
        {
            return _SqlClient.Queryable<SyncCfgFolderDao>()
                 .Where(a => a.id == folderId && a.row_status == ScmRowStatusEnum.Enabled)
                 .First();
        }
        #endregion
    }
}
