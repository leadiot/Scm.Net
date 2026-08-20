using Com.Scm.Config;
using Com.Scm.Enums;
using Com.Scm.Filters;
using Com.Scm.Service;
using Com.Scm.Sys.Sms.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Sys.Sms
{
    /// <summary>
    /// 短信
    /// </summary>
    [ApiExplorerSettings(GroupName = "sys")]
    public class ScmSysSmsService : ApiService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlClient"></param>
        /// <param name="resHolder"></param>
        /// <param name="config"></param>
        public ScmSysSmsService(ISqlSugarClient sqlClient,
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
        public async Task<ScmSearchPageResponse<ScmSysSmsDvo>> GetPagesAsync(SmsSearchRequest request)
        {
            var result = await _SqlClient.Queryable<ScmSysSmsDao>()
                .Where(a => a.row_delete == ScmRowDeleteEnum.No)
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.address.Contains(request.key))
                .OrderBy(m => m.id)
                .Select<ScmSysSmsDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<ScmSysSmsThreadDvo>> GetConversationsAsync(SmsSearchRequest request)
        {
            var result = await _SqlClient.Queryable<ScmSysSmsThreadDao>()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
                .OrderBy(m => m.update_time, SqlSugar.OrderByType.Desc)
                .Select<ScmSysSmsThreadDvo>()
                .ToListAsync();

            //Prepare(result);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<ScmSysSmsDvo>> GetListAsync(SmsSearchRequest request)
        {
            var result = await _SqlClient.Queryable<ScmSysSmsDao>()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled && a.row_delete == ScmRowDeleteEnum.No)
                .WhereIF(IsValidId(request.id), a => a.thread_id == request.id)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.address.Contains(request.key))
                .OrderBy(m => m.id, SqlSugar.OrderByType.Asc)
                .Select<ScmSysSmsDvo>()
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
        public async Task<ScmSysSmsDvo> GetAsync(long id)
        {
            var dvo = new ScmSysSmsDvo();

            var dao = await _SqlClient.Queryable<ScmSysSmsDao>()
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
        public async Task<ScmSysSmsDto> GetEditAsync(long id)
        {
            var dao = await _SqlClient.Queryable<ScmSysSmsDao>()
                .FirstAsync(m => m.id == id);

            ReadFile(dao);

            return dao.Clone<ScmSysSmsDto>();
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmSysSmsDvo> GetViewAsync(long id)
        {
            var dao = await _SqlClient.Queryable<ScmSysSmsDao>()
                .FirstAsync(m => m.id == id);

            ReadFile(dao);

            return dao.Clone<ScmSysSmsDvo>();
        }

        private void ReadFile(ScmSysSmsDao dao)
        {
            if (dao.files < 1)
            {
                return;
            }

            dao.body = _EnvConfig.ReadFile(ScmSysSmsDao.FILE_DIR, dao.GetFileName());
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ScmSysSmsDvo> AddAsync(ScmSysSmsDto dto)
        {
            var address = dto.address;

            var threadDao = await _SqlClient.Queryable<ScmSysSmsThreadDao>()
                .Where(a => a.address == address)
                .FirstAsync();

            var time = TimeUtils.GetUnixTime();

            if (threadDao == null)
            {
                threadDao = new ScmSysSmsThreadDao();
                threadDao.address = address;
                threadDao.name = dto.name ?? dto.address;
                threadDao.body = dto.body;
                threadDao.time = time;
                await _SqlClient.InsertAsync(threadDao);
            }
            else
            {
                threadDao.body = dto.body;
                threadDao.time = time;
                await _SqlClient.UpdateAsync(threadDao);
            }

            var dao = dto.Adapt<ScmSysSmsDao>();
            dao.thread_id = threadDao.id;
            dao.modify_time = time;
            dao.date = time;
            dao.delivery_date = time;
            dao.type = ScmSmsTypeEnum.SENT;
            dao.row_delete = ScmRowDeleteEnum.No;

            dao.source = "web";
            dao.res_id = "";
            dao.os_params = new Dictionary<string, string>();

            var qty = await _SqlClient.InsertAsync(dao);

            SaveFile(dao, dto);

            return dao.Clone<ScmSysSmsDvo>();
        }

        /// <summary>
        /// 保存，暂时使用不到
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ScmSysSmsDto> SaveAsync(ScmSysSmsDto dto)
        {
            ScmSysSmsDao dao = null;
            var time = TimeUtils.GetUnixTime();

            if (IsNormalId(dto.id))
            {
                dao = await _SqlClient.GetByIdAsync<ScmSysSmsDao>(dto.id);
            }

            if (dao == null)
            {
                dao = dto.Adapt<ScmSysSmsDao>();
                dao.modify_time = time;
                await _SqlClient.InsertAsync(dao);

                dto.id = dao.id;
            }
            else
            {
                dao = dto.Adapt(dao);
                dao.modify_time = time;
                await _SqlClient.UpdateAsync(dao);
            }

            SaveFile(dao, dto);

            dto.update_time = dao.update_time;
            dto.create_time = dao.create_time;
            return dto;
        }

        /// <summary>
        /// 更新，暂时使用不到
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task UpdateAsync(ScmSysSmsDto dto)
        {
            var dao = await _SqlClient.GetByIdAsync<ScmSysSmsDao>(dto.id);
            if (dao == null)
            {
                return;
            }

            dao = dto.Adapt(dao);
            dao.modify_time = TimeUtils.GetUnixTime();

            await _SqlClient.UpdateAsync(dao);

            SaveFile(dao, dto);
        }

        private void SaveFile(ScmSysSmsDao dao, ScmSysSmsDto dto)
        {
            if (dao.files < 1)
            {
                return;
            }

            // 写入文件
            _EnvConfig.SaveFile(ScmSysSmsDao.FILE_DIR, dao.GetFileName(), dto.body ?? "");
        }

        /// <summary>
        /// 批量更新状态
        /// </summary>
        /// <param name="param">逗号分隔</param>
        /// <returns></returns>
        public async Task<int> StatusAsync(ScmChangeStatusRequest param)
        {
            return await UpdateStatusAsync<ScmSysSmsDao>(_SqlClient, param.ids, param.status);
        }

        /// <summary>
        /// 批量删除记录
        /// </summary>
        /// <param name="ids">逗号分隔</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<int> RemoveAsync(string ids)
        {
            return await RemoveRecordAsync<ScmSysSmsDao>(_SqlClient, ids.ToListLong());
        }

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost, AllowAnonymous, NoJsonResult]
        public async Task<SmsUploadResponse> UploadAsync([FromForm] ScmUploadRequest request)
        {
            var response = new SmsUploadResponse();

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
