using Com.Scm.Api;
using Com.Scm.Config;
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

        #region 对外接口
        /// <summary>
        /// 获取驱动列表
        /// </summary>
        /// <returns></returns>
        public async Task<ScmApiListResponse<NasCfgDriveDto>> GetDriveAsync([FromHeader] string appToken)
        {
            var token = ScmToken.FromAppToken(appToken);
            var terminalDao = _ResHolder.GetRes<ScmUrTerminalDao>(token.terminal_id);
            if (terminalDao == null || terminalDao.IsExpired())
            {
                return null;
            }

            var list = await _SqlClient.Queryable<NasCfgDriveDao>()
                .Where(a => a.user_id == terminalDao.user_id && a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .Select<NasCfgDriveDto>()
                .ToListAsync();

            var response = new ScmApiListResponse<NasCfgDriveDto>();
            response.SetSuccess(list);

            return response;
        }

        /// <summary>
        /// 更新驱动
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<NasCfgDriveDto> PostDriveAsync(NasCfgDriveDto model, [FromHeader] string appToken)
        {
            var token = ScmToken.FromAppToken(appToken);
            var terminalDao = _ResHolder.GetRes<ScmUrTerminalDao>(token.terminal_id);
            if (terminalDao == null || terminalDao.IsExpired())
            {
                return null;
            }

            if (!ScmUtils.IsValidId(model.folder_id))
            {
                model.folder_id = CreateRecursiveDirDao(model.path, ScmEnv.DEFAULT_ID).id;
            }

            var dao = await _SqlClient.Queryable<SyncCfgDriveDao>()
                .Where(a => a.terminal_id == token.terminal_id && a.folder_id == model.folder_id)
                .FirstAsync();

            if (dao == null)
            {
                dao = model.Adapt<SyncCfgDriveDao>();
                dao.terminal_id = token.terminal_id;
                dao.PrepareCreate(terminalDao.user_id);
                await _SqlClient.Insertable(dao).ExecuteCommandAsync();
            }
            else
            {
                dao.row_status = Enums.ScmRowStatusEnum.Enabled;
                dao.PrepareUpdate(terminalDao.user_id);
                await _SqlClient.Updateable(dao).ExecuteCommandAsync();
            }

            model.id = dao.id;

            return model;
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

            var driveId = request.drive_id;
            //var driveDao = GetDriveDao(driveId);

            return await _SqlClient.Queryable<Sync.SyncLogFileDao>()
                .Where(a => a.user_id == terminalDao.user_id &&
                    a.drive_id != driveId &&
                    a.row_status == Enums.ScmRowStatusEnum.Enabled &&
                    //a.path.StartsWith(driveDao.path) &&
                    a.id > request.id)
                .OrderBy(a => a.id, OrderByType.Asc)
                .Select<NasLogFileDto>()
                .ToPageAsync(request.page, request.limit);
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

            return await _SqlClient.Queryable<Sync.SyncResFileDao>()
                .Where(a => a.user_id == terminalDao.user_id && a.type == NasTypeEnums.Dir && a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .WhereIF(byPath, a => a.path == request.path)
                .WhereIF(!byPath, a => a.dir_id == request.dir_id)
                .OrderBy(a => a.name, OrderByType.Asc)
                .Select<NasResFileDto>()
                .ToPageAsync(request.page, request.limit);
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

            return await _SqlClient.Queryable<Sync.SyncResFileDao>()
                .Where(a => a.user_id == terminalDao.user_id && a.type == NasTypeEnums.Doc && a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .WhereIF(byPath, a => a.path == request.path)
                .WhereIF(!byPath, a => a.dir_id == request.dir_id)
                .OrderBy(a => a.name, OrderByType.Asc)
                .Select<NasResFileDto>()
                .ToPageAsync(request.page, request.limit);
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

            var items = await _SqlClient.Queryable<Sync.SyncResFileDao>()
                .Where(a => a.user_id == terminalDao.user_id && a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .WhereIF(byPath, a => a.path == request.path)
                .WhereIF(!byPath, a => a.dir_id == request.dir_id)
                .OrderBy(a => a.type, OrderByType.Asc)
                .OrderBy(a => a.name, OrderByType.Asc)
                .Select<NasResFileDto>()
                .ToPageAsync(request.page, request.limit);

            return items;
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
                LogUtils.Debug("上传对象为空！");
                return SyncResult.Failure("上传对象为空！");
            }

            var token = ScmToken.FromAppToken(appToken);
            var terminalDao = _ResHolder.GetRes<ScmUrTerminalDao>(token.terminal_id);
            if (terminalDao == null || terminalDao.IsExpired())
            {
                return null;
            }

            var result = new SyncResult();
            if (dto.opt == NasOptEnums.Delete)
            {
                DeleteFile(terminalDao, dto, result);
                return result;
            }

            if (dto.opt == NasOptEnums.Create)
            {
                await CreateFile(terminalDao, dto, result);
                return result;
            }

            if (dto.opt == NasOptEnums.Rename)
            {
                await RenameFile(terminalDao, dto, result);
                return result;
            }

            if (dto.opt == NasOptEnums.Move)
            {
                await MoveFile(terminalDao, dto, result);
                return result;
            }

            if (dto.opt == NasOptEnums.Copy)
            {
                await CopyFile(terminalDao, dto, result);
                return result;
            }

            if (dto.opt == NasOptEnums.Change)
            {
                await ChangeFile(terminalDao, dto, result);
                return result;
            }

            LogUtils.Debug("不支持的操作：" + dto.opt);
            result.SetFailure("不支持的操作：" + dto.opt);
            return result;
        }
        #endregion

        #region 删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool DeleteFile(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Debug("删除文件：" + dto.path);

            if (dto == null)
            {
                return false;
            }

            if (dto.type == NasTypeEnums.Dir)
            {
                return DeleteDir(token, dto, result);
            }

            if (dto.type == NasTypeEnums.Doc)
            {
                return DeleteDoc(token, dto, result);
            }

            LogUtils.Debug("未知的文件类型：" + dto.type);
            result.SetFailure("未知的文件类型：" + dto.type);
            return false;
        }

        /// <summary>
        /// 删除文档
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool DeleteDoc(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Debug("删除文档：" + dto.path);

            var dstFile = GetPhysicalPath(dto.path);
            FileUtils.DeleteDoc(dstFile);

            var docDao = GetDocDaoByPath(dto.path);
            var dirId = 0L;
            if (docDao != null)
            {
                DeleteDocDao(docDao);
                dirId = docDao.dir_id;
            }

            AddLogFileByDto(token, dto, dirId);

            result.SetSuccess();
            return true;
        }

        private void DeleteDocDao(Sync.SyncResFileDao dao)
        {
            _SqlClient.Deleteable(dao).ExecuteCommand();
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private bool DeleteDir(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Debug("删除目录：" + dto.path);

            var dstFile = GetPhysicalPath(dto.path);
            FileUtils.DeleteDir(dstFile);

            var dirDao = GetDirDaoByPath(dto.path);

            var dirId = 0L;
            if (dirDao != null)
            {
                DeleteDirDao(dirDao);

                DeleteResFileDao(token, dirDao);
                dirId = dirDao.dir_id;
            }

            AddLogFileByDto(token, dto, dirId);

            result.SetSuccess();
            return true;
        }

        private void DeleteDirDao(Sync.SyncResFileDao dao)
        {
            var dirList = _SqlClient.Queryable<Sync.SyncResFileDao>()
                .Where(a => a.type == NasTypeEnums.Dir && a.dir_id == dao.id)
                .ToList();
            foreach (var dir in dirList)
            {
                DeleteDirDao(dir);
            }

            _SqlClient.Deleteable<Sync.SyncResFileDao>()
                .Where(a => a.dir_id == dao.id)
                .ExecuteCommand();
        }
        #endregion

        #region 创建文件
        /// <summary>
        /// 创建文件
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> CreateFile(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Debug("创建文件：" + dto.path);

            if (dto.type == NasTypeEnums.Dir)
            {
                return await CreateDir(token, dto, result);
            }

            if (dto.type == NasTypeEnums.Doc)
            {
                return await CreateDoc(token, dto, result);
            }

            result.SetFailure("未知的文件类型：" + dto.type);
            return false;
        }

        /// <summary>
        /// 创建目录
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> CreateDir(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Debug("创建目录：" + dto.path);

            var path = GetPhysicalPath(dto.path);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var dirDao = CreateRecursiveDirDao(dto.path, token.user_id);

            AddLogFileByDto(token, dto, dirDao.dir_id);

            result.SetSuccess(dirDao.id);
            return true;
        }

        /// <summary>
        /// 创建文档
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> CreateDoc(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Debug("创建文档：" + dto.path);

            if (string.IsNullOrEmpty(dto.src))
            {
                return false;
            }

            var tmpFile = _EnvConfig.GetTempPath(dto.src);
            if (!FileUtils.ExistsDoc(tmpFile))
            {
                result.SetFailure($"上传文档不存在！");
                return false;
            }

            var dstFile = GetPhysicalPath(dto.path);
            if (!FileUtils.MoveDoc(tmpFile, dstFile, true))
            {
                SyncResult.Failure("上传文档移动异常！");
                return false;
            }

            var dirDao = CreateRecursiveDirDao(GetParentPath(dto.path), token.user_id);

            var docDao = AddCreateFile(token, dto, dirDao.id);

            AddLogFileByDto(token, dto, dirDao.id);

            result.SetSuccess(docDao.id);
            return true;
        }

        private Sync.SyncResFileDao AddCreateFile(ScmUrTerminalDao token, NasLogFileDto dto, long dirId)
        {
            var docDao = GetResFileDaoByPath(dto.path, dto.type);
            if (docDao == null)
            {
                docDao = new Sync.SyncResFileDao();
                docDao.user_id = token.user_id;
                docDao.type = dto.type;
                docDao.name = dto.name;
                docDao.path = dto.path;
                docDao.hash = dto.hash;
                docDao.size = dto.size;
                //docDao.modify_time = dto.modify_time;
                docDao.dir_id = dirId;
                docDao.PrepareCreate(token.user_id);

                _SqlClient.Insertable(docDao).ExecuteCommand();
            }
            return docDao;
        }

        /// <summary>
        /// 添加目录记录
        /// </summary>
        /// <param name="token"></param>
        /// <param name="dirId"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private Sync.SyncResFileDao AddCreateResDirDao(ScmUrTerminalDao token, long dirId, string name, string path)
        {
            var dirDao = new Sync.SyncResFileDao
            {
                user_id = token.user_id,
                type = NasTypeEnums.Dir,
                name = name,
                path = path,
                dir_id = dirId, // 根目录
            };
            _SqlClient.Insertable(dirDao).ExecuteCommand();
            return dirDao;
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
        private Sync.SyncResFileDao AddCreateResDocDao(ScmUrTerminalDao token, long dirId, string path, string name, string hash, long size, long time)
        {
            var dirDao = new Sync.SyncResFileDao
            {
                user_id = token.user_id,
                type = NasTypeEnums.Doc,
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
        #endregion

        #region 移动文件
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> MoveFile(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Debug("移动文件：" + dto.path);

            if (dto == null)
            {
                return false;
            }

            if (dto.type == NasTypeEnums.Dir)
            {
                return await MoveDir(token, dto, result);
            }

            if (dto.type == NasTypeEnums.Doc)
            {
                return await MoveDoc(token, dto, result);
            }

            result.SetFailure("未知的文件类型：" + dto.type);
            return false;
        }

        /// <summary>
        /// 移动目录
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> MoveDir(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Debug("移动目录：" + dto.path);

            var srcDir = GetPhysicalPath(dto.src);
            if (!FileUtils.ExistsDir(srcDir))
            {
                result.SetFailure($"来源目录 {dto.src} 不存在！");
                return false;
            }

            var parentDao = CreateRecursiveDirDao(GetParentPath(dto.path), token.user_id);
            var dstDao = GetDirDaoByPath(dto.path);
            if (dstDao != null)
            {
                DeleteResFileDao(token, dstDao);
            }

            var srcDao = GetDirDaoByPath(dto.src);
            if (srcDao != null)
            {
                UpdateResFileDao(srcDao, dto.name, dto.path, parentDao.id, token.user_id);
            }
            else
            {
                srcDao = AddCreateResDirDao(token, parentDao.id, dto.name, dto.path);
            }

            var dstDir = GetPhysicalPath(dto.path);
            MoveDirCasced(token, srcDao, srcDir, dto.src, dstDir, dto.path);

            AddLogFileByDto(token, dto, srcDao.dir_id);

            result.SetSuccess(srcDao.id);
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
        private bool MoveDirCasced(ScmUrTerminalDao token, SyncResFileDao parentDao, string srcDir, string srcPath, string dstDir, string dstPath)
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
                var dstDao = GetDirDaoByPath(dstUri);
                if (dstDao != null)
                {
                    DeleteResFileDao(token, dstDao);
                }

                var srcUri = srcPath + NasEnv.WebSeparator + name;
                var dirDao = GetDirDaoByPath(srcUri);
                if (dirDao != null)
                {
                    UpdateResFileDao(dirDao, name, dstUri, parentDao.id, token.user_id);
                }
                else
                {
                    dirDao = AddCreateResDirDao(token, parentDao.id, name, dstUri);
                }

                var dstFile = Path.Combine(dstDir, name);
                if (!Directory.Exists(dstFile))
                {
                    Directory.CreateDirectory(dstFile);
                }

                MoveDirCasced(token, dirDao, dir, srcUri, dstFile, dstUri);
            }

            var docList = Directory.GetFiles(srcDir);
            foreach (var doc in docList)
            {
                var name = FileUtils.GetFileName(doc);
                var dstUri = dstPath + NasEnv.WebSeparator + name;

                // 若目标存在，则删除
                var dstDao = GetDocDaoByPath(dstUri);
                if (dstDao != null)
                {
                    DeleteResFileDao(token, dstDao);
                }

                var srcUri = srcPath + NasEnv.WebSeparator + name;
                var docDao = GetDocDaoByPath(srcUri);
                if (docDao != null)
                {
                    UpdateResFileDao(docDao, name, dstUri, parentDao.id, token.user_id);
                }
                else
                {
                    var hash = FileUtils.Sha(doc);
                    var info = new FileInfo(doc);
                    var time = TimeUtils.GetUnixTime(info.LastAccessTimeUtc);
                    docDao = AddCreateResDocDao(token, parentDao.id, dstUri, name, hash, info.Length, time);
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
        private async Task<bool> MoveDoc(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Debug("移动文档：" + dto.path);

            var srcFile = GetPhysicalPath(dto.src);
            if (!FileUtils.ExistsDoc(srcFile))
            {
                result.SetFailure($"来源文档 {dto.src} 不存在！");
                return false;
            }

            var dstFile = GetPhysicalPath(dto.path);
            FileUtils.MoveDoc(srcFile, dstFile, true);

            var dstDao = GetDocDaoByPath(dto.path);
            if (dstDao != null)
            {
                DeleteResFileDao(token, dstDao);
            }

            // 创建父级目录
            var parentDao = CreateRecursiveDirDao(GetParentPath(dto.path), token.user_id);

            var srcDao = GetDocDaoByPath(dto.src);
            if (srcDao != null)
            {
                UpdateResFileDao(srcDao, dto.name, dto.path, parentDao.id, token.user_id);
            }
            else
            {
                srcDao = AddResFileByDto(token, dto, parentDao.id);
            }

            AddLogFileByDto(token, dto, srcDao.dir_id);

            result.SetSuccess(srcDao.id);
            return true;
        }
        #endregion

        #region 复件文件
        /// <summary>
        /// 移动文件
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> CopyFile(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Debug("复制文件：" + dto.path);

            if (dto == null)
            {
                return false;
            }

            if (dto.type == NasTypeEnums.Dir)
            {
                return await CopyDir(token, dto, result);
            }

            if (dto.type == NasTypeEnums.Doc)
            {
                return await CopyDoc(token, dto, result);
            }

            result.SetFailure("未知的文件类型：" + dto.type);
            return false;
        }

        /// <summary>
        /// 移动目录
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> CopyDir(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Debug("复制目录：" + dto.path);

            var srcPath = GetPhysicalPath(dto.src);
            if (!FileUtils.ExistsDir(srcPath))
            {
                LogUtils.Debug($"来源目录 {dto.src} 不存在！");
                result.SetFailure($"来源目录 {dto.src} 不存在！");
                return false;
            }

            var parentDao = CreateRecursiveDirDao(GetParentPath(dto.path), token.user_id);

            var dstDao = GetDirDaoByPath(dto.path);
            if (dstDao != null)
            {
                UpdateResFileDao(dstDao, dto.name, dto.path, parentDao.id, token.user_id);
            }
            else
            {
                dstDao = AddCreateResDirDao(token, parentDao.id, dto.name, dto.path);
            }

            var dstDir = GetPhysicalPath(dto.path);
            CopyDirCasced(token, dstDao, srcPath, dto.src, dstDir, dto.path);

            AddLogFileByDto(token, dto, dstDao.dir_id);

            result.SetSuccess(dstDao.id);
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
        private bool CopyDirCasced(ScmUrTerminalDao token, SyncResFileDao parentDao, string srcDir, string srcPath, string dstDir, string dstPath)
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
                var dstDao = GetDirDaoByPath(dstUri);
                if (dstDao != null)
                {
                    UpdateResFileDao(dstDao, name, dstUri, parentDao.id, token.user_id);
                }
                else
                {
                    dstDao = AddCreateResDirDao(token, parentDao.id, name, dstUri);
                }

                var dstFile = Path.Combine(dstDir, name);
                if (!Directory.Exists(dstFile))
                {
                    Directory.CreateDirectory(dstFile);
                }

                var srcUri = srcPath + NasEnv.WebSeparator + name;
                CopyDirCasced(token, dstDao, dir, srcUri, dstFile, dstUri);
            }

            var docList = Directory.GetFiles(srcDir);
            foreach (var doc in docList)
            {
                var name = FileUtils.GetFileName(doc);
                var dstUri = dstPath + NasEnv.WebSeparator + name;

                var dstDao = GetDocDaoByPath(dstUri);
                if (dstDao != null)
                {
                    UpdateResFileDao(dstDao, name, dstUri, parentDao.id, token.user_id);
                }
                else
                {
                    var hash = FileUtils.Sha(doc);
                    var info = new FileInfo(doc);
                    var time = TimeUtils.GetUnixTime(info.LastAccessTimeUtc);
                    dstDao = AddCreateResDocDao(token, parentDao.id, name, dstUri, hash, info.Length, time);
                }

                var dstFile = Path.Combine(dstDir, name);
                File.Copy(doc, dstFile, true);
            }

            return true;
        }

        /// <summary>
        /// 移动文档
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> CopyDoc(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Debug("复制文档：" + dto.path);

            var srcFile = GetPhysicalPath(dto.src);
            if (!FileUtils.ExistsDoc(srcFile))
            {
                LogUtils.Debug($"来源文档 {dto.src} 不存在！");
                result.SetFailure($"来源文档 {dto.src} 不存在！");
                return false;
            }

            var dstFile = GetPhysicalPath(dto.path);
            FileUtils.CopyDoc(srcFile, dstFile, true);

            var parentDao = CreateRecursiveDirDao(GetParentPath(dto.path), token.user_id);

            var dstDao = GetDocDaoByPath(dto.path);
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
                AddResFileByDto(token, dto, parentDao.id);
            }

            AddLogFileByDto(token, dto, parentDao.id);

            result.SetSuccess(dstDao.id);
            return true;
        }
        #endregion

        #region 更名文件
        /// <summary>
        /// 更名文件
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> RenameFile(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Debug("更名文件：" + dto.path);

            if (dto == null)
            {
                return false;
            }

            if (dto.type == NasTypeEnums.Dir)
            {
                return await RenameDir(token, dto, result);
            }

            if (dto.type == NasTypeEnums.Doc)
            {
                return await RenameDoc(token, dto, result);
            }

            result.SetFailure("未知的文件类型：" + dto.type);
            return false;
        }

        /// <summary>
        /// 更名目录
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> RenameDir(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Debug("更名目录：" + dto.path);

            var srcDir = GetPhysicalPath(dto.src);
            if (!FileUtils.ExistsDir(srcDir))
            {
                result.SetFailure($"来源目录 {dto.src} 不存在！");
                return false;
            }

            var parentDao = CreateRecursiveDirDao(GetParentPath(dto.path), token.user_id);

            var dstDao = GetDirDaoByPath(dto.path);
            if (dstDao != null)
            {
                DeleteResFileDao(token, dstDao);
            }

            var srcDao = GetDirDaoByPath(dto.src);
            if (srcDao != null)
            {
                UpdateResFileDao(srcDao, dto.name, dto.path, parentDao.id, token.user_id);
            }
            else
            {
                srcDao = AddCreateResDirDao(token, parentDao.id, dto.name, dto.path);
            }

            var dstDir = GetPhysicalPath(dto.path);
            MoveDirCasced(token, srcDao, srcDir, dto.src, dstDir, dto.path);

            AddLogFileByDto(token, dto, srcDao.dir_id);

            result.SetSuccess(srcDao.id);
            return true;
        }

        /// <summary>
        /// 移动文档
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> RenameDoc(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Debug("更名文档：" + dto.path);

            var srcFile = GetPhysicalPath(dto.src);
            if (!FileUtils.ExistsDoc(srcFile))
            {
                result.SetFailure($"来源文档 {dto.src} 不存在！");
                return false;
            }

            var dstFile = GetPhysicalPath(dto.path);
            FileUtils.MoveDoc(srcFile, dstFile, true);

            var paretnDao = CreateRecursiveDirDao(GetParentPath(dto.path), token.user_id);

            var dstDao = GetDocDaoByPath(dto.path);
            if (dstDao != null)
            {
                DeleteResFileDao(token, dstDao);
            }

            var srcDao = GetDocDaoByPath(dto.src);
            if (srcDao != null)
            {
                UpdateResFileDao(srcDao, dto.name, dto.path, paretnDao.id, token.user_id);
            }
            else
            {
                // 追加文档记录
                AddResFileByDto(token, dto, paretnDao.id);
            }

            AddLogFileByDto(token, dto, paretnDao.id);

            result.SetSuccess();
            return true;
        }
        #endregion

        #region 更新文件
        /// <summary>
        /// 更新文件
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> ChangeFile(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Debug("更新文件：" + dto.path);

            if (dto.type == NasTypeEnums.Dir)
            {
                return await ChangeDir(token, dto, result);
            }

            if (dto.type == NasTypeEnums.Doc)
            {
                return await ChangeDoc(token, dto, result);
            }

            result.SetFailure("未知的文件类型：" + dto.type);
            return false;
        }

        /// <summary>
        /// 更新文档
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> ChangeDoc(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Debug("更新文档：" + dto.path);

            if (string.IsNullOrEmpty(dto.src))
            {
                return false;
            }

            var tmpFile = _EnvConfig.GetTempPath(dto.src);
            if (!FileUtils.ExistsDoc(tmpFile))
            {
                result.SetFailure($"上传文档不存在！");
                return false;
            }

            var dstFile = GetPhysicalPath(dto.path);
            if (!FileUtils.MoveDoc(tmpFile, dstFile, true))
            {
                SyncResult.Failure("上传文档移动异常！");
                return false;
            }

            var docDao = GetDocDaoByPath(dto.path);
            if (docDao == null)
            {
                var dirId = GetParentIdByPath(dto.path);
                AddCreateFile(token, dto, dirId);

                AddLogFileByDto(token, dto, dirId);
            }
            else
            {
                docDao.hash = dto.hash;
                docDao.size = dto.size;
                UpdateResFileDao(token, docDao);

                AddLogFileByDto(token, dto, docDao.dir_id);
            }

            result.SetSuccess();
            return true;
        }

        /// <summary>
        /// 更新目录
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> ChangeDir(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Debug("更新目录：" + dto.path);

            var tmpFile = GetPhysicalPath(dto.path);
            if (!Directory.Exists(tmpFile))
            {
                Directory.CreateDirectory(tmpFile);
            }

            var dirDao = CreateRecursiveDirDao(dto.path, token.user_id);

            AddLogFileByDto(token, dto, dirDao.dir_id);

            result.SetSuccess(dirDao.id);
            return true;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 根据路径获取文件对象
        /// </summary>
        /// <param name="path">虚拟绝对路径</param>
        /// <returns></returns>
        private Sync.SyncResFileDao GetResFileDaoByPath(string path, NasTypeEnums type)
        {
            return _SqlClient.Queryable<Sync.SyncResFileDao>()
                .Where(a => a.path == path && a.type == type)
                .First();
        }

        private Sync.SyncResFileDao GetDirDaoByPath(string path)
        {
            return GetResFileDaoByPath(path, NasTypeEnums.Dir);
        }

        private Sync.SyncResFileDao GetDocDaoByPath(string path)
        {
            return GetResFileDaoByPath(path, NasTypeEnums.Doc);
        }

        private SyncResFileDao AddDirDao(string name, string path, long dirId, long userId)
        {
            var dao = new Sync.SyncResFileDao
            {
                user_id = userId,
                type = NasTypeEnums.Dir,
                name = name,
                path = path,
                dir_id = dirId, // 根目录
            };
            dao.PrepareCreate(userId);
            _SqlClient.Insertable(dao).ExecuteCommand();
            return dao;
        }

        private SyncResFileDao UpdateResFileDao(SyncResFileDao dao, string name, string path, long dirId, long userId)
        {
            dao.name = name;
            dao.path = path;
            dao.dir_id = dirId;
            dao.PrepareUpdate(userId);
            _SqlClient.Updateable(dao).ExecuteCommand();
            return dao;
        }

        private List<Sync.SyncResFileDao> ListResFileDaoByParent(long dirId, NasTypeEnums type)
        {
            return _SqlClient.Queryable<Sync.SyncResFileDao>()
                .Where(a => a.dir_id == dirId && a.type == type)
                .ToList();
        }

        private List<Sync.SyncResFileDao> ListDirDaoByParent(long dirId)
        {
            return ListResFileDaoByParent(dirId, NasTypeEnums.Dir);
        }

        private List<Sync.SyncResFileDao> ListDocDaoByParent(long dirId)
        {
            return ListResFileDaoByParent(dirId, NasTypeEnums.Doc);
        }

        public long GetParentIdByPath(string path)
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

            var dirDao = GetDirDaoByPath(path);
            return dirDao != null ? dirDao.id : NasEnv.DEF_DIR_ID;
        }

        /// <summary>
        /// 获取物理路径
        /// （服务端路径，不可与客户端方法相同）
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private string GetPhysicalPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }

            if (path.StartsWith(NasEnv.VirtualTag, StringComparison.OrdinalIgnoreCase))
            {
                path = path.Substring(NasEnv.VirtualTag.Length);
            }

            return _EnvConfig.GetDataPath("Nas" + path);
        }

        private string GetVirtualPath(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return "" + NasEnv.WebSeparator;
            }

            var nasPath = _EnvConfig.GetDataPath("Nas");
            return path.Substring(nasPath.Length);
        }

        /// <summary>
        /// 获取上级目录
        /// </summary>
        /// <param name="file">虚拟绝对路径</param>
        /// <returns></returns>
        private static string GetParentPath(string file)
        {
            file = file.TrimEnd(NasEnv.WebSeparator);
            var idx = file.LastIndexOf(NasEnv.WebSeparator);
            if (idx > 0)
            {
                return file.Substring(0, idx);
            }
            return "" + NasEnv.WebSeparator;
        }

        /// <summary>
        /// 根据路径级联创建目录
        /// </summary>
        /// <param name="path"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        private SyncResFileDao CreateRecursiveDirDao(string path, long userId)
        {
            var tmp = "";
            SyncResFileDao parentDao = new SyncResFileDao() { id = NasEnv.DEF_DIR_ID };
            path = path.Trim(NasEnv.WebSeparator);
            foreach (var arr in path.Split(NasEnv.WebSeparator))
            {
                if (string.IsNullOrEmpty(arr))
                {
                    continue;
                }

                tmp += NasEnv.WebSeparator + arr;
                var dao = GetDirDaoByPath(tmp);
                if (dao == null)
                {
                    AddDirDao(arr, tmp, parentDao.id, userId);
                }
                else
                {
                    UpdateResFileDao(dao, arr, tmp, parentDao.id, userId);
                }

                parentDao = dao;
            }

            return parentDao;
        }

        private void DeleteResFileDao(ScmUrTerminalDao token, SyncResFileDao dao)
        {
            _SqlClient.Deleteable(dao).ExecuteCommand();
        }

        private void UpdateResFileDao(ScmUrTerminalDao token, Sync.SyncResFileDao dao)
        {
            dao.PrepareUpdate(token.user_id);
            _SqlClient.Updateable(dao).ExecuteCommand();
        }

        private SyncResFileDao AddResFileByDto(ScmUrTerminalDao token, NasLogFileDto dto, long dirId)
        {
            var docDao = dto.Adapt<Sync.SyncResFileDao>();
            docDao.dir_id = dirId;
            docDao.user_id = token.user_id;
            docDao.PrepareCreate(token.user_id);
            _SqlClient.Insertable(docDao).ExecuteCommand();
            return docDao;
        }

        /// <summary>
        /// 记录操作日志
        /// </summary>
        /// <param name="token"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        private void AddLogFileByDto(ScmUrTerminalDao token, NasLogFileDto dto, long dirId)
        {
            var dao = dto.Adapt<Sync.SyncLogFileDao>();
            dao.user_id = token.user_id;
            dao.dir_id = dirId;
            dao.terminal_id = token.id;
            dao.PrepareCreate(token.user_id);
            _SqlClient.Insertable(dao).ExecuteCommand();
        }
        #endregion
    }
}
