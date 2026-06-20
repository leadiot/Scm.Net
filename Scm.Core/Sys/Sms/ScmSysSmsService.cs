using Com.Scm.Config;
using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Filters;
using Com.Scm.Service;
using Com.Scm.Sys.Sms.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.Sms
{
    /// <summary>
    /// 短信
    /// </summary>
    [ApiExplorerSettings(GroupName = "Sys")]
    public class ScmSysSmsService : ApiService
    {
        private readonly SugarRepository<ScmSysSmsDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        /// <param name="resHolder"></param>
        /// <param name="config"></param>
        public ScmSysSmsService(SugarRepository<ScmSysSmsDao> thisRepository,
            IResHolder resHolder,
            EnvConfig config)
        {
            _thisRepository = thisRepository;
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
            var result = await _thisRepository.AsQueryable()
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
        public async Task<List<ScmSysSmsDvo>> GetListAsync(SmsSearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.address.Contains(request.key))
                .OrderBy(m => m.id, SqlSugar.OrderByType.Desc)
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

            var dao = await _thisRepository
                .AsQueryable()
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
            return await _thisRepository
                .AsQueryable()
                .Select<ScmSysSmsDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmSysSmsDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ScmSysSmsDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ScmSysSmsDvo> AddAsync(ScmSysSmsDto model)
        {
            var dao = model.Adapt<ScmSysSmsDao>();

            var qty = await _thisRepository.InsertAsync(dao);

            return dao.Clone<ScmSysSmsDvo>();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ScmSysSmsDto> SaveAsync(ScmSysSmsDto model)
        {
            ScmSysSmsDao dao = null;

            if (IsNormalId(model.id))
            {
                dao = await _thisRepository.GetByIdAsync(model.id);
            }

            if (dao == null)
            {
                dao = model.Adapt<ScmSysSmsDao>();
                await _thisRepository.InsertAsync(dao);

                model.id = dao.id;
            }
            else
            {
                dao = model.Adapt(dao);
                await _thisRepository.UpdateAsync(dao);
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
        public async Task UpdateAsync(ScmSysSmsDto model)
        {
            var dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                return;
            }

            dao = model.Adapt(dao);

            await _thisRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 批量更新状态
        /// </summary>
        /// <param name="param">逗号分隔</param>
        /// <returns></returns>
        public async Task<int> StatusAsync(ScmChangeStatusRequest param)
        {
            return await UpdateStatus(_thisRepository, param.ids, param.status);
        }

        /// <summary>
        /// 批量删除记录
        /// </summary>
        /// <param name="ids">逗号分隔</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<int> DeleteAsync(string ids)
        {
            return await DeleteRecord(_thisRepository, ids.ToListLong());
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
