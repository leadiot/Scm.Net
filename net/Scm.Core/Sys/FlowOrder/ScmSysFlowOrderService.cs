using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Flow
{
    /// <summary>
    /// 服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Sys")]
    public class ScmSysFlowOrderService : ApiService
    {
        private readonly SugarRepository<ScmFlowOrderDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public ScmSysFlowOrderService(SugarRepository<ScmFlowOrderDao> thisRepository, IUserHolder userService)
        {
            _thisRepository = thisRepository;
            _UserService = userService;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<ScmFlowOrderDvo>> GetPageAsync(ScmSearchPageRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                //.WhereIF(IsValidId(request.option_id), a => a.option_id == request.option_id)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.namec.Contains(request.key))
                .OrderBy(m => m.id)
                .Select<ScmFlowOrderDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<ScmFlowOrderDvo>> GetListAsync(ScmSearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == Com.Scm.Enums.ScmRowStatusEnum.Enabled)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.namec.Contains(request.key))
                .OrderBy(m => m.id)
                .Select<ScmFlowOrderDvo>()
                .ToListAsync();

            Prepare(result);
            return result;
        }

        /// <summary>
        /// 下拉列表
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<ResOptionDvo>> GetOptionAsync(ScmSearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
                .OrderBy(a => a.id)
                .Select(a => new ResOptionDvo { id = a.id, label = a.namec, value = a.id })
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmFlowOrderDto> GetAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ScmFlowOrderDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmFlowOrderDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ScmFlowOrderDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmFlowOrderDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ScmFlowOrderDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ScmFlowOrderDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec);
            if (dao != null)
            {
                throw new BusinessException("已存在相同编码的单据！");
            }
            dao = await _thisRepository.GetFirstAsync(a => a.namec == model.namec);
            if (dao != null)
            {
                throw new BusinessException("已存在相同名称的单据！");
            }

            dao = model.Adapt<ScmFlowOrderDao>();
            return await _thisRepository.InsertAsync(dao);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ScmFlowOrderDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec && a.id != model.id);
            if (dao != null)
            {
                throw new BusinessException("已存在相同编码的单据！");
            }
            dao = await _thisRepository.GetFirstAsync(a => a.namec == model.namec && a.id != model.id);
            if (dao != null)
            {
                throw new BusinessException("已存在相同名称的单据！");
            }

            dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                throw new BusinessException("无效的单据！");
            }

            dao = model.Adapt(dao);
            return await _thisRepository.UpdateAsync(dao);
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