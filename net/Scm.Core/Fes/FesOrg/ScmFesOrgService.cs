using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Exceptions;
using Com.Scm.Nas.FesOrg.Dvo;
using Com.Scm.Service;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Nas.FesOrg
{
    /// <summary>
    /// 服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Scm")]
    public class ScmFesOrgService : ApiService
    {
        private readonly SugarRepository<ScmNasOrgDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public ScmFesOrgService(SugarRepository<ScmNasOrgDao> thisRepository, IResHolder userService)
        {
            _thisRepository = thisRepository;
            _ResHolder = userService;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<ScmFesOrgDvo>> GetPagesAsync(ScmSearchPageRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.namec.Contains(request.key))
                .OrderBy(m => m.id)
                .Select<ScmFesOrgDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<ScmFesOrgDvo>> GetListAsync(ScmSearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.namec.Contains(request.key))
                .OrderBy(m => m.id)
                .Select<ScmFesOrgDvo>()
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
                .Where(a => a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .OrderBy(a => a.id)
                .Select(a => new ResOptionDvo { id = a.id, label = a.names, value = a.id })
                .ToListAsync();

            return result;
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmNasOrgDto> GetAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ScmNasOrgDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmNasOrgDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ScmNasOrgDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmFesOrgDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ScmFesOrgDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ScmNasOrgDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec);
            if (dao != null)
            {
                throw new BusinessException("已存在相同编码的组织！");
            }
            dao = await _thisRepository.GetFirstAsync(a => a.namec == model.namec);
            if (dao != null)
            {
                throw new BusinessException("已存在相同名称的组织！");
            }

            dao = model.Adapt<ScmNasOrgDao>();
            return await _thisRepository.InsertAsync(dao);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ScmNasOrgDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.codec == model.codec && a.id != model.id);
            if (dao != null)
            {
                throw new BusinessException("已存在相同编码的组织！");
            }
            dao = await _thisRepository.GetFirstAsync(a => a.namec == model.namec && a.id != model.id);
            if (dao != null)
            {
                throw new BusinessException("已存在相同名称的组织！");
            }

            dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                throw new BusinessException("无效的组织！");
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