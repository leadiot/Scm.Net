using Com.Scm.Config;
using Com.Scm.Enums;
using Com.Scm.Filters;
using Com.Scm.Nas.App;
using Com.Scm.Service;
using Com.Scm.Sys.Contacts.Dvo;
using Com.Scm.Ur;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Sys.Contacts
{
    /// <summary>
    /// 联系人
    /// </summary>
    [ApiExplorerSettings(GroupName = "sys")]
    public class ScmSysContactsService : ApiService
    {
        public const string SOURCE_WEB = "Nas.Web";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        /// <param name="resHolder"></param>
        /// <param name="config"></param>
        public ScmSysContactsService(ISqlSugarClient sqlClient,
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
        public async Task<ScmSearchPageResponse<ScmSysContactsDvo>> GetPagesAsync(ContactSearchRequest request)
        {
            var result = await _SqlClient.Queryable<ScmSysContactsDao>()
                .Where(a => a.row_delete == ScmRowDeleteEnum.No)
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.title.Contains(request.key))
                .OrderBy(m => m.id)
                .Select<ScmSysContactsDvo>()
                .ToPageAsync(request.page, request.limit);

            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<ScmSysContactsDvo>> GetListAsync(ContactSearchRequest request)
        {
            var daoList = await _SqlClient.Queryable<ScmSysContactsDao>()
                .Where(a => a.row_delete == ScmRowDeleteEnum.No)
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.title.Contains(request.key))
                .OrderBy(m => m.id, SqlSugar.OrderByType.Desc)
                .ToListAsync();

            var dvoList = new List<ScmSysContactsDvo>();
            foreach (var dao in daoList)
            {
                dvoList.Add(dao.Clone<ScmSysContactsDvo>());
            }

            return dvoList;
        }

        private void Prepare(List<ScmSysContactsDvo> list)
        {
            foreach (var item in list)
            {
                var terminalDao = _ResHolder.GetRes<ScmUrTerminalDao>(item.terminal_id);
                item.terminal_name = terminalDao?.names;
            }
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmSysContactsDvo> GetAsync(long id)
        {
            var dvo = new ScmSysContactsDvo();

            var dao = await _SqlClient.Queryable<ScmSysContactsDao>()
                .Where(a => a.id == id)
                .FirstAsync();

            return dvo;
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmSysContactsDto> GetEditAsync(long id)
        {
            return await _SqlClient.Queryable<ScmSysContactsDao>()
                .Select<ScmSysContactsDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmSysContactsDvo> GetViewAsync(long id)
        {
            return await _SqlClient.Queryable<ScmSysContactsDao>()
                .Select<ScmSysContactsDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ScmSysContactsDvo> AddAsync(ScmSysContactsDto model)
        {
            var dao = model.Adapt<ScmSysContactsDao>();
            dao.modify_time = TimeUtils.GetUnixTime();
            dao.source = SOURCE_WEB;
            dao.terminal_id = ScmUrTerminalDto.DEFAULT_ID;
            dao.row_delete = ScmRowDeleteEnum.No;

            var qty = await _SqlClient.InsertAsync(dao);

            return dao.Clone<ScmSysContactsDvo>();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ScmSysContactsDto> SaveAsync(ScmSysContactsDto dto)
        {
            ScmSysContactsDao dao = null;
            var time = TimeUtils.GetUnixTime();

            if (IsNormalId(dto.id))
            {
                dao = await _SqlClient.Queryable<ScmSysContactsDao>().FirstAsync(a => a.id == dto.id);
            }

            if (dao == null)
            {
                dao = dto.Adapt<ScmSysContactsDao>();
                dao.modify_time = time;
                dao.source = SOURCE_WEB;
                dao.terminal_id = ScmUrTerminalDto.DEFAULT_ID;
                dao.row_delete = ScmRowDeleteEnum.No;
                await _SqlClient.InsertAsync(dao);

                dto.id = dao.id;
            }
            else
            {
                dao = dto.Adapt(dao);
                dao.modify_time = time;
                await _SqlClient.UpdateAsync(dao);

                UpdateSyncTime(dao);
            }

            dto.update_time = dao.update_time;
            dto.create_time = dao.create_time;
            return dto;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task UpdateAsync(ScmSysContactsDto dto)
        {
            var dao = await _SqlClient.Queryable<ScmSysContactsDao>().FirstAsync(a => a.id == dto.id);
            if (dao == null)
            {
                return;
            }

            dao = dto.Adapt(dao);
            dao.modify_time = TimeUtils.GetUnixTime();

            await _SqlClient.UpdateAsync(dao);

            UpdateSyncTime(dao);
        }

        private void UpdateSyncTime(ScmSysContactsDao dao)
        {
            _SqlClient.Updateable<ScmSysContactsTerminalDao>()
                .SetColumns(a => a.sync_time == dao.sync_time)
                .SetColumns(a => a.update_time == dao.update_time)
                .Where(a => a.sys_id == dao.id)
                .ExecuteCommand();
        }

        /// <summary>
        /// 批量更新状态
        /// </summary>
        /// <param name="param">逗号分隔</param>
        /// <returns></returns>
        public async Task<int> StatusAsync(ScmChangeStatusRequest param)
        {
            return await UpdateStatusAsync<ScmSysContactsDao>(_SqlClient, param.ids, param.status);
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
            var qty = await RemoveRecordAsync<ScmSysContactsDao>(_SqlClient, idList);
            await _SqlClient.Updateable<ScmSysContactsTerminalDao>()
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
        [HttpPost, NoJsonResult]
        public async Task<ContactUploadResponse> UploadAsync([FromForm] ScmUploadRequest request)
        {
            var response = new ContactUploadResponse();

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
    }
}
