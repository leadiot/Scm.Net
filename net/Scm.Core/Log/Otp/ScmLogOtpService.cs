using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Log.Sms.Dvo;
using Com.Scm.Service;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Log.Sms
{
    /// <summary>
    /// 服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "log")]
    public class ScmLogOtpService : ApiService
    {
        private readonly SugarRepository<LogOtpDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        /// <param name="userService"></param>
        /// <returns></returns>
        public ScmLogOtpService(SugarRepository<LogOtpDao> thisRepository, IResHolder userService)
        {
            _thisRepository = thisRepository;
            _ResHolder = userService;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<LogOtpDvo>> GetPagesAsync(ScmSearchPageRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                //.WhereIF(IsValidId(request.option_id), a => a.option_id == request.option_id)
                //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
                .OrderBy(a => a.id)
                .Select<LogOtpDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<LogOtpDvo>> GetListAsync(ScmSearchPageRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
                //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
                .OrderBy(a => a.id)
                .Select<LogOtpDvo>()
                .ToListAsync();

            Prepare(result);
            return result;
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmLogOtpDto> GetAsync(long id)
        {
            var model = await _thisRepository.GetByIdAsync(id);
            return model.Adapt<ScmLogOtpDto>();
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmLogOtpDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ScmLogOtpDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<LogOtpDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<LogOtpDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ScmLogOtpDto model)
        {
            //var dao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec);
            //if (dao != null)
            //{
            //    throw new BusinessException($"已存在编码为{model.codec}的！");
            //}

            //if (string.IsNullOrWhiteSpace(model.names))
            //{
            //    model.names = model.namec;
            //}
            //dao = await _thisRepository.GetFirstAsync(a => a.names == model.names);
            //if (dao != null)
            //{
            //    throw new BusinessException($"已存在简称为{model.names}的！");
            //}

            return await _thisRepository.InsertAsync(model.Adapt<LogOtpDao>());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAsync(ScmLogOtpDto model)
        {
            //var dao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec && a.id != model.id);
            //if (dao != null)
            //{
            //    throw new BusinessException($"已存在编码为{model.codec}的！");
            //}

            //if (string.IsNullOrWhiteSpace(model.names))
            //{
            //    model.names = model.namec;
            //}
            //dao = await _thisRepository.GetFirstAsync(a => a.names == model.names && a.id != model.id);
            //if (dao != null)
            //{
            //    throw new BusinessException($"已存在简称为{model.names}的！");
            //}

            var dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                throw new BusinessException($"无效的数据信息，更新失败！");
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
    }
}