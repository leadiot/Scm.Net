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
    [ApiExplorerSettings(GroupName = "Sys")]
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
        public async Task<ScmSearchPageResponse<ScmSysSmsDetailDvo>> GetPagesAsync(SmsSearchRequest request)
        {
            var result = await _SqlClient.Queryable<ScmSysSmsDetailDao>()
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.address.Contains(request.key))
                .OrderBy(m => m.id)
                .Select<ScmSysSmsDetailDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<ScmSysSmsHeaderDvo>> GetConversationsAsync(SmsSearchRequest request)
        {
            var result = await _SqlClient.Queryable<ScmSysSmsHeaderDao>()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
                .OrderBy(m => m.id, SqlSugar.OrderByType.Desc)
                .Select<ScmSysSmsHeaderDvo>()
                .ToListAsync();

            //Prepare(result);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<ScmSysSmsDetailDvo>> GetListAsync(SmsSearchRequest request)
        {
            var result = await _SqlClient.Queryable<ScmSysSmsDetailDao>()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
                .WhereIF(IsValidId(request.id), a => a.header_id == request.id)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.address.Contains(request.key))
                .OrderBy(m => m.id, SqlSugar.OrderByType.Asc)
                .Select<ScmSysSmsDetailDvo>()
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
        public async Task<ScmSysSmsDetailDvo> GetAsync(long id)
        {
            var dvo = new ScmSysSmsDetailDvo();

            var dao = await _SqlClient.Queryable<ScmSysSmsDetailDao>()
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
        public async Task<ScmSysSmsDetailDto> GetEditAsync(long id)
        {
            return await _SqlClient.Queryable<ScmSysSmsDetailDao>()
                .Select<ScmSysSmsDetailDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmSysSmsDetailDvo> GetViewAsync(long id)
        {
            return await _SqlClient.Queryable<ScmSysSmsDetailDao>()
                .Select<ScmSysSmsDetailDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ScmSysSmsDetailDvo> AddAsync(ScmSysSmsDetailDto model)
        {
            var phone = model.phone;
            var headerDao = await _SqlClient.Queryable<ScmSysSmsHeaderDao>()
                .Where(a => a.phone == phone)
                .FirstAsync();
            if (headerDao == null)
            {
                headerDao = new ScmSysSmsHeaderDao();
                headerDao.phone = phone;
                headerDao.body = model.body;
                headerDao.name = model.name;
                await _SqlClient.InsertAsync(headerDao);
            }
            else
            {
                headerDao.body = model.body;
                await _SqlClient.UpdateAsync(headerDao);
            }

            var dao = model.Adapt<ScmSysSmsDetailDao>();
            dao.header_id = headerDao.id;
            dao.type = ScmSmsTypeEnum.SENT;

            var qty = await _SqlClient.InsertAsync(dao);

            return dao.Clone<ScmSysSmsDetailDvo>();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ScmSysSmsDetailDto> SaveAsync(ScmSysSmsDetailDto model)
        {
            ScmSysSmsDetailDao dao = null;

            if (IsNormalId(model.id))
            {
                dao = await _SqlClient.GetByIdAsync<ScmSysSmsDetailDao>(model.id);
            }

            if (dao == null)
            {
                dao = model.Adapt<ScmSysSmsDetailDao>();
                await _SqlClient.InsertAsync(dao);

                model.id = dao.id;
            }
            else
            {
                dao = model.Adapt(dao);
                await _SqlClient.UpdateAsync(dao);
            }

            model.update_time = dao.update_time;
            model.create_time = dao.create_time;
            return model;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAsync(ScmSysSmsDetailDto model)
        {
            var dao = await _SqlClient.GetByIdAsync<ScmSysSmsDetailDao>(model.id);
            if (dao == null)
            {
                return;
            }

            dao = model.Adapt(dao);

            await _SqlClient.UpdateAsync(dao);
        }

        /// <summary>
        /// 批量更新状态
        /// </summary>
        /// <param name="param">逗号分隔</param>
        /// <returns></returns>
        public async Task<int> StatusAsync(ScmChangeStatusRequest param)
        {
            return await UpdateStatus<ScmSysSmsDetailDao>(_SqlClient, param.ids, param.status);
        }

        /// <summary>
        /// 批量删除记录
        /// </summary>
        /// <param name="ids">逗号分隔</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<int> DeleteAsync(string ids)
        {
            return await DeleteRecord<ScmSysSmsDetailDao>(_SqlClient, ids.ToListLong());
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
