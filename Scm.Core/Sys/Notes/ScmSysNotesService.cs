using Com.Scm.Config;
using Com.Scm.Enums;
using Com.Scm.Filters;
using Com.Scm.Nas.App;
using Com.Scm.Res.Cat;
using Com.Scm.Service;
using Com.Scm.Sys.Notes.Dvo;
using Com.Scm.Ur;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Sys.Notes
{
    /// <summary>
    /// 记事
    /// </summary>
    [ApiExplorerSettings(GroupName = "sys")]
    public class ScmSysNotesService : ApiService
    {
        public const string SOURCE_WEB = "Nas.Web";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        /// <param name="resHolder"></param>
        /// <param name="config"></param>
        public ScmSysNotesService(ISqlSugarClient sqlClient,
            IResHolder resHolder,
            EnvConfig config)
        {
            _SqlClient = sqlClient;
            _ResHolder = resHolder;
            _EnvConfig = config;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<NoteBasicDvo>> GetPagesAsync(NoteSearchRequest request)
        {
            var result = await _SqlClient.Queryable<ScmSysNotesDao>()
                .Where(a => a.row_delete == ScmRowDeleteEnum.No)
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                .WhereIF(IsValidId(request.cat_id), a => a.cat_id == request.cat_id)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.title.Contains(request.key))
                .WhereIF(request.types != ScmNotesTypeEnum.None, a => a.types == request.types)
                .OrderBy(m => m.id)
                .Select<NoteBasicDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<NoteBasicDvo>> GetListAsync(NoteSearchRequest request)
        {
            var result = await _SqlClient.Queryable<ScmSysNotesDao>()
                .Where(a => a.row_delete == ScmRowDeleteEnum.No)
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
                .WhereIF(IsValidId(request.cat_id), a => a.cat_id == request.cat_id)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.title.Contains(request.key))
                .WhereIF(request.types != ScmNotesTypeEnum.None, a => a.types == request.types)
                .OrderBy(m => m.id, SqlSugar.OrderByType.Desc)
                .Select<NoteBasicDvo>()
                .ToListAsync();

            //Prepare(result);
            return result;
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<NotesDvo> GetAsync(long id)
        {
            var dvo = new NotesDvo();

            var dao = await _SqlClient.Queryable<ScmSysNotesDao>()
                .Where(a => a.id == id)
                .FirstAsync();

            if (dao != null)
            {
                dvo.id = dao.id;
                dvo.types = dao.types;
                dvo.title = dao.title;
                dvo.content = dao.summary;

                ReadFile(dao);
            }

            return dvo;
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<NotesDto> GetEditAsync(long id)
        {
            return await _SqlClient.Queryable<ScmSysNotesDao>()
                .Select<NotesDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<NotesDvo> GetViewAsync(long id)
        {
            return await _SqlClient.Queryable<ScmSysNotesDao>()
                .Select<NotesDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<NotesDvo> AddAsync(NotesDto model)
        {
            var dao = model.Adapt<ScmSysNotesDao>();
            if (!IsValidId(dao.cat_id))
            {
                dao.cat_id = ScmResCatDto.SYS_ID;
            }

            dao.client = ScmClientTypeEnum.Web;
            dao.modify_time = TimeUtils.GetUnixTime();
            dao.source = SOURCE_WEB;
            dao.terminal_id = ScmUrTerminalDto.DEFAULT_ID;
            dao.row_delete = ScmRowDeleteEnum.No;

            var qty = await _SqlClient.InsertAsync(dao);

            SaveFile(dao, model);

            return dao.Clone<NotesDvo>();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<NotesDto> SaveAsync(NotesDto model)
        {
            ScmSysNotesDao dao = null;
            var time = TimeUtils.GetUnixTime();

            if (IsNormalId(model.id))
            {
                dao = await _SqlClient.Queryable<ScmSysNotesDao>().FirstAsync(a => a.id == model.id);
            }

            if (dao == null)
            {
                // 此处不能使用Adapt，原有的KEY\Salt可能会丢失
                //dao = model.Adapt<ScmSysNotesDao>();
                dao = new ScmSysNotesDao();
                dao.id = model.id;
                dao.title = model.title;
                dao.sub_title = model.sub_title;
                dao.content = model.content;
                dao.client = ScmClientTypeEnum.Web;
                dao.source = SOURCE_WEB;
                dao.modify_time = time;
                dao.terminal_id = ScmUrTerminalDto.DEFAULT_ID;
                dao.row_delete = ScmRowDeleteEnum.No;
                await _SqlClient.InsertAsync(dao);

                model.id = dao.id;
            }
            else
            {
                // 此处不能使用Adapt，原有的KEY\Salt可能会丢失
                //dao = model.Adapt(dao);
                dao.title = model.title;
                dao.sub_title = model.sub_title;
                dao.content = model.content;
                dao.modify_time = time;
                await _SqlClient.UpdateAsync(dao);

                UpdateSyncTime(dao);
            }

            SaveFile(dao, model);

            model.update_time = dao.update_time;
            model.create_time = dao.create_time;
            return model;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAsync(NotesDto model)
        {
            var dao = await _SqlClient.Queryable<ScmSysNotesDao>().FirstAsync(a => a.id == model.id);
            if (dao == null)
            {
                return;
            }

            // 此处不能使用Adapt，原有的KEY\Salt可能会丢失
            //dao = model.Adapt(dao);
            dao.title = model.title;
            dao.sub_title = model.sub_title;
            dao.content = model.content;
            dao.modify_time = TimeUtils.GetUnixTime();
            if (!IsValidId(dao.cat_id))
            {
                dao.cat_id = ScmResCatDto.SYS_ID;
            }

            await _SqlClient.UpdateAsync(dao);
            UpdateSyncTime(dao);

            SaveFile(dao, model);
        }

        /// <summary>
        /// 批量更新状态
        /// </summary>
        /// <param name="param">逗号分隔</param>
        /// <returns></returns>
        public async Task<int> StatusAsync(ScmChangeStatusRequest param)
        {
            return await UpdateStatusAsync<ScmSysNotesDao>(_SqlClient, param.ids, param.status);
        }

        /// <summary>
        /// 批量删除记录
        /// </summary>
        /// <param name="ids">逗号分隔</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<int> RemoveAsync(string ids)
        {
            var idList = ids.ToListLong();
            var qty = await RemoveRecordAsync<ScmSysNotesDao>(_SqlClient, idList);
            await _SqlClient.Updateable<ScmSysNotesTerminalDao>()
                .SetColumns(a => a.row_delete == ScmRowDeleteEnum.Yes)
                .SetColumns(a => a.sync_time == TimeUtils.GetUnixTime())
                .Where(a => idList.Contains(a.sys_id))
                .ExecuteCommandAsync();
            return qty;
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous, NoJsonResult]
        public async Task<NoteUploadResponse> UploadAsync([FromForm] ScmUploadRequest request)
        {
            var response = new NoteUploadResponse();

            //判断是否上传了文件内容
            if (request.file == null)
            {
                response.SetFailure("上传内容为空！");
                return response;
            }

            #region 保存文件
            var fileName = request.file.FileName;
            var ext = Path.GetExtension(fileName);
            fileName = DateTime.UtcNow.Ticks.ToString() + ext;

            var path = _EnvConfig.GetUploadPath(fileName);
            using (var stream = File.OpenWrite(path))
            {
                //将文件内容复制到流中
                await request.file.CopyToAsync(stream);
            }

            response.SetSuccess(_EnvConfig.ToUri(path));
            #endregion

            return response;
        }

        private void ReadFile(ScmSysNotesDao dao)
        {
            if (dao.files < 1)
            {
                return;
            }

            var content = _EnvConfig.ReadFile(ScmSysNotesDao.FOLDER_NAME, dao.GetFileName());
            if (!string.IsNullOrWhiteSpace(content))
            {
                dao.content = content;
            }
        }

        private void SaveFile(ScmSysNotesDao dao, NotesDto dto)
        {
            if (dao.files < 1)
            {
                return;
            }

            _EnvConfig.SaveFile(ScmSysNotesDao.FOLDER_NAME, dao.GetFileName(), dto.content ?? "");
        }

        private void UpdateSyncTime(ScmSysNotesDao dao)
        {
            _SqlClient.Updateable<ScmSysNotesTerminalDao>()
                .SetColumns(a => a.sync_time == dao.sync_time)
                .SetColumns(a => a.update_time == dao.update_time)
                .Where(a => a.sys_id == dao.id)
                .ExecuteCommand();
        }
    }
}
