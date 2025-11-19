using Com.Scm.Dev.App.Dvo;
using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Enums;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Dev.App
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(GroupName = "Dev")]
    public class ScmDevAppService : ApiService
    {
        private readonly SugarRepository<ScmDevAppDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        /// <param name="userService"></param>
        public ScmDevAppService(SugarRepository<ScmDevAppDao> thisRepository, IUserService userService)
        {
            _thisRepository = thisRepository;
            _UserService = userService;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<AppDvo>> GetPagesAsync(SearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                .WhereIF(IsValidInt(request.types), a => a.types == request.types)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.name.Contains(request.key))
                .OrderBy(a => a.id)
                .Select<AppDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<AppDvo>> GetListAsync(SearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.name.Contains(request.key))
                .OrderBy(a => a.id)
                .Select<AppDvo>()
                .ToListAsync();

            Prepare(result);
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        [HttpGet("{types}")]
        public async Task<List<ResOptionDvo>> OptionAsync(int types)
        {
            return await _thisRepository.AsQueryable()
                .WhereIF(IsValidInt(types), a => a.types == types)
                .OrderBy(m => m.od, OrderByType.Asc)
                .Select(a => new ResOptionDvo { id = a.id, label = a.name, value = a.id })
                .ToListAsync();
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmDevAppDto> GetAsync(long id)
        {
            var model = await _thisRepository.GetByIdAsync(id);
            return model.Adapt<ScmDevAppDto>();
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmDevAppDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ScmDevAppDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<AppDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<AppDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ScmDevAppDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.code == model.code);
            if (dao != null)
            {
                throw new BusinessException($"已存在编码为{model.code}的应用！");
            }

            dao = await _thisRepository.GetFirstAsync(a => a.name == model.name);
            if (dao != null)
            {
                throw new BusinessException($"已存在简称为{model.name}的应用！");
            }

            dao = model.Adapt<ScmDevAppDao>();
            return await _thisRepository.InsertAsync(dao);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAsync(ScmDevAppDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.code == model.code && a.id != model.id);
            if (dao != null)
            {
                throw new BusinessException($"已存在编码为{model.code}的应用！");
            }

            dao = await _thisRepository.GetFirstAsync(a => a.name == model.name && a.id != model.id);
            if (dao != null)
            {
                throw new BusinessException($"已存在简称为{model.name}的应用！");
            }

            dao = await _thisRepository.GetByIdAsync(model.id);
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
