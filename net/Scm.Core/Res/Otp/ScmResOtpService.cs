using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Res.Otp.Dvo;
using Com.Scm.Service;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Res.Otp
{
    /// <summary>
    /// Otp管理
    /// </summary>
    [ApiExplorerSettings(GroupName = "res")]
    public class ScmResOtpService : ApiService
    {
        private readonly SugarRepository<ScmResOtpDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        /// <param name="userService"></param>
        /// <returns></returns>
        public ScmResOtpService(SugarRepository<ScmResOtpDao> thisRepository, IResHolder userService)
        {
            _thisRepository = thisRepository;
            _ResHolder = userService;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<OtpDvo>> GetPagesAsync(ScmSearchPageRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                //.WhereIF(IsValidId(request.option_id), a => a.option_id == request.option_id)
                //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
                .OrderBy(a => a.id)
                .Select<OtpDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<OtpDvo>> GetListAsync(ScmSearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
                //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
                .OrderBy(a => a.id)
                .Select<OtpDvo>()
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
        public async Task<ScmResOtpDto> GetAsync(long id)
        {
            var model = await _thisRepository.GetByIdAsync(id);
            return model.Adapt<ScmResOtpDto>();
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmResOtpDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ScmResOtpDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<OtpDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<OtpDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ScmResOtpDto model)
        {
            //var dao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec);
            //if (dao != null)
            //{
            //    throw new BusinessException($"已存在编码为{model.codec}的消息模板！");
            //}

            //if (string.IsNullOrWhiteSpace(model.names))
            //{
            //    model.names = model.namec;
            //}
            //dao = await _thisRepository.GetFirstAsync(a => a.names == model.names);
            //if (dao != null)
            //{
            //    throw new BusinessException($"已存在简称为{model.names}的消息模板！");
            //}

            return await _thisRepository.InsertAsync(model.Adapt<ScmResOtpDao>());
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAsync(ScmResOtpDto model)
        {
            //var dao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec && a.id != model.id);
            //if (dao != null)
            //{
            //    throw new BusinessException($"已存在编码为{model.codec}的消息模板！");
            //}

            //if (string.IsNullOrWhiteSpace(model.names))
            //{
            //    model.names = model.namec;
            //}
            //dao = await _thisRepository.GetFirstAsync(a => a.names == model.names && a.id != model.id);
            //if (dao != null)
            //{
            //    throw new BusinessException($"已存在简称为{model.names}的消息模板！");
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