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
    [ApiExplorerSettings(GroupName = "Scm")]
    [AllowAnonymous]
    public class NasSyncService : AppService
    {
        private ScmContextHolder _ScmHolder;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sqlClient"></param>
        /// <param name="nasConfig"></param>
        /// <param name="resHolder"></param>
        public NasSyncService(ISqlSugarClient sqlClient,
            EnvConfig envConfig,
            ScmContextHolder scmHolder,
            IResHolder resHolder)
        {
            _SqlClient = sqlClient;
            _EnvConfig = envConfig;
            _ScmHolder = scmHolder;
            _ResHolder = resHolder;
        }

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
                model.folder_id = CreateResDirDao(model.path, ScmEnv.DEFAULT_ID).id;
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
                await DeleteFile(terminalDao, dto, result);
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

        #region 删除文件
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> DeleteFile(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Debug("删除文件：" + dto.path);

            if (dto == null)
            {
                return false;
            }

            if (dto.type == NasTypeEnums.Dir)
            {
                return await DeleteDir(token, dto, result);
            }

            if (dto.type == NasTypeEnums.Doc)
            {
                return await DeleteDoc(token, dto, result);
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
        private async Task<bool> DeleteDoc(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Debug("删除文档：" + dto.path);

            var dstFile = GetPhysicalPath(dto.path);
            FileUtils.DeleteDoc(dstFile);

            var docDao = await _SqlClient.Queryable<Sync.SyncResFileDao>()
                .Where(a => a.type == NasTypeEnums.Doc && a.path == dto.path && a.hash == dto.hash)
                .FirstAsync();
            var dirId = 0L;
            if (docDao != null)
            {
                await DeleteDocDao(docDao);
                dirId = docDao.dir_id;
            }

            AddLogFileByDto(token, dto, dirId);

            result.SetSuccess();
            return true;
        }

        private async Task DeleteDocDao(Sync.SyncResFileDao dao)
        {
            await _SqlClient.Deleteable(dao).ExecuteCommandAsync();
        }

        /// <summary>
        /// 删除目录
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<bool> DeleteDir(ScmUrTerminalDao token, NasLogFileDto dto, SyncResult result)
        {
            LogUtils.Debug("删除目录：" + dto.path);

            var dstFile = GetPhysicalPath(dto.path);
            FileUtils.DeleteDir(dstFile);

            var dirDao = await _SqlClient.Queryable<Sync.SyncResFileDao>()
                .Where(a => a.type == NasTypeEnums.Dir && a.path == dto.path)
                .FirstAsync();

            var dirId = 0L;
            if (dirDao != null)
            {
                await DeleteDirDao(dirDao);

                await _SqlClient.Deleteable(dirDao).ExecuteCommandAsync();
                dirId = dirDao.dir_id;
            }

            AddLogFileByDto(token, dto, dirId);

            result.SetSuccess();
            return true;
        }

        private async Task DeleteDirDao(Sync.SyncResFileDao dao)
        {
            var dirList = await _SqlClient.Queryable<Sync.SyncResFileDao>()
                .Where(a => a.type == NasTypeEnums.Dir && a.dir_id == dao.id)
                .ToListAsync();
            foreach (var dir in dirList)
            {
                await DeleteDirDao(dir);
            }

            await _SqlClient.Deleteable<Sync.SyncResFileDao>()
                .Where(a => a.dir_id == dao.dir_id)
                .ExecuteCommandAsync();
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

            var dirDao = CreateResDirDao(dto.path, token.user_id);

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

            var dirDao = CreateResDirDao(GetParentPath(dto.path), token.user_id);

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

            var srcFile = GetPhysicalPath(dto.src);
            if (!FileUtils.ExistsDir(srcFile))
            {
                result.SetFailure($"来源目录 {dto.src} 不存在！");
                return false;
            }

            var dstFile = GetPhysicalPath(dto.path);
            FileUtils.MoveDir(srcFile, dstFile, true);

            var dstDao = GetDirDaoByPath(dto.path);
            if (dstDao != null)
            {
                DeleteResFileDao(token, dstDao);
            }

            // 创建父级目录
            var dirDao = CreateResDirDao(GetParentPath(dto.path), token.user_id);

            var srcDao = GetDirDaoByPath(dto.src);
            if (srcDao != null)
            {
                srcDao.name = dto.name;
                srcDao.path = dto.path;
                srcDao.dir_id = dirDao.id;
                UpdateResFileDao(token, srcDao);

                RenameDirDao(token, srcDao);
            }
            else
            {
                srcDao = CreateResDirDao(dto.path, token.user_id);
            }

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

            var dstDao = GetDirDaoByPath(dto.path);
            if (dstDao != null)
            {
                DeleteResFileDao(token, dstDao);
            }

            // 创建父级目录
            var dirDao = CreateResDirDao(GetParentPath(dto.path), token.user_id);

            var srcDao = GetDocDaoByPath(dto.src);
            if (srcDao != null)
            {
                srcDao.name = dto.name;
                srcDao.path = dto.path;
                srcDao.dir_id = dirDao.id;
                UpdateResFileDao(token, srcDao);

                RenameDirDao(token, srcDao);
            }
            else
            {
                srcDao = AddResFileByDto(token, dto);
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

            var srcFile = GetPhysicalPath(dto.src);
            if (!FileUtils.ExistsDir(srcFile))
            {
                LogUtils.Debug($"来源目录 {dto.src} 不存在！");
                result.SetFailure($"来源目录 {dto.src} 不存在！");
                return false;
            }

            var dstFile = GetPhysicalPath(dto.path);
            FileUtils.CopyDir(srcFile, dstFile, true);

            var dirDao = CreateResDirDao(dto.path, token.user_id);

            AddLogFileByDto(token, dto, dirDao.dir_id);

            result.SetSuccess(dirDao.id);
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

            var dirId = GetParentIdByPath(dto.path);

            var dstFile = GetPhysicalPath(dto.path);
            FileUtils.CopyDoc(srcFile, dstFile, true);

            var docDao = GetDocDaoByPath(dto.path);
            if (docDao != null)
            {
                docDao.dir_id = dirId;
                docDao.name = dto.name;
                docDao.path = dto.path;
                docDao.hash = dto.hash;
                docDao.size = dto.size;
                UpdateResFileDao(token, docDao);
            }
            else
            {
                dto.dir_id = dirId;
                // 追加文档记录
                AddResFileByDto(token, dto);
            }

            AddLogFileByDto(token, dto, dirId);

            result.SetSuccess(docDao.id);
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

            var srcFile = GetPhysicalPath(dto.src);
            if (!FileUtils.ExistsDir(srcFile))
            {
                result.SetFailure($"来源目录 {dto.src} 不存在！");
                return false;
            }

            var dstFile = GetPhysicalPath(dto.path);
            FileUtils.MoveDir(srcFile, dstFile, true);

            var dstDao = GetDirDaoByPath(dto.path);
            if (dstDao != null)
            {
                DeleteResFileDao(token, dstDao);
            }

            var dirDao = CreateResDirDao(GetParentPath(dto.path), token.user_id);

            var srcDao = GetDirDaoByPath(dto.src);
            if (srcDao != null)
            {
                srcDao.name = dto.name;
                srcDao.path = dto.path;
                srcDao.dir_id = dirDao.id;
                UpdateResFileDao(token, srcDao);

                RenameDirDao(token, srcDao);
            }
            else
            {
                srcDao = CreateResDirDao(dto.path, token.user_id);
                // 追加文档记录
                AddResFileByDto(token, dto);
            }

            AddLogFileByDto(token, dto, srcDao.dir_id);

            result.SetSuccess();
            return true;
        }

        /// <summary>
        /// 级联更新子级文件
        /// </summary>
        /// <param name="token"></param>
        /// <param name="dao"></param>
        private void RenameDirDao(ScmUrTerminalDao token, Sync.SyncResFileDao dao)
        {
            // 子级目录路径更新
            var dirListDao = ListDirDaoByParent(dao.id);
            foreach (var dir in dirListDao)
            {
                dir.path = dao.path + NasEnv.WebSeparator + dir.name;
                RenameDirDao(token, dir);
            }
            _SqlClient.Updateable(dirListDao).ExecuteCommand();

            // 子级文档路径更新
            var docListDao = ListDocDaoByParent(dao.id);
            foreach (var doc in docListDao)
            {
                doc.path = dao.path + NasEnv.WebSeparator + doc.name;
            }
            _SqlClient.Updateable(docListDao).ExecuteCommand();
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

            var docDao = GetDocDaoByPath(dto.src);
            var dirId = 0L;
            if (docDao != null)
            {
                docDao.name = dto.name;
                docDao.path = dto.path;
                dirId = docDao.dir_id;
                UpdateResFileDao(token, docDao);
            }
            else
            {
                dirId = GetParentIdByPath(dto.path);
                dto.dir_id = dirId;
                // 追加文档记录
                AddResFileByDto(token, dto);
            }

            AddLogFileByDto(token, dto, dirId);

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

            var dirDao = CreateResDirDao(dto.path, token.user_id);

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
        private SyncResFileDao CreateResDirDao(string path, long userId)
        {
            var tmp = "";
            SyncResFileDao parent = new SyncResFileDao() { id = NasEnv.DEF_DIR_ID };
            path = path.Trim(NasEnv.WebSeparator);
            foreach (var arr in path.Split(NasEnv.WebSeparator))
            {
                if (string.IsNullOrEmpty(arr))
                {
                    continue;
                }

                tmp += NasEnv.WebSeparator + arr;
                var dao = _SqlClient.Queryable<Sync.SyncResFileDao>().Where(a => a.path == tmp).First();
                if (dao == null)
                {
                    dao = new Sync.SyncResFileDao
                    {
                        user_id = userId,
                        type = NasTypeEnums.Dir,
                        name = arr,
                        path = tmp,
                        dir_id = parent.id, // 根目录
                    };
                    dao.PrepareCreate(userId);
                    _SqlClient.Insertable(dao).ExecuteCommand();
                }
                parent = dao;
            }

            return parent;
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

        private SyncResFileDao AddResFileByDto(ScmUrTerminalDao token, NasLogFileDto dto)
        {
            var docDao = dto.Adapt<Sync.SyncResFileDao>();
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
