using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Res.Icon;
using Com.Scm.Res.IconCat.Dvo;
using Com.Scm.Service;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Res.IconCat
{
    /// <summary>
    /// 图标分类
    /// </summary>
    [ApiExplorerSettings(GroupName = "Res")]
    public class ScmResIconCatService : ApiService
    {
        private readonly SugarRepository<ScmResIconCatDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public ScmResIconCatService(SugarRepository<ScmResIconCatDao> thisRepository)
        {
            _thisRepository = thisRepository;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<IconCatDvo>> GetPagesAsync(SearchRequest request)
        {
            return await _thisRepository.AsQueryable()
                .Where(m => m.pid == request.id)
                .WhereIF(!string.IsNullOrEmpty(request.key), m => m.name.Contains(request.key))
                .OrderBy(m => m.od)
                .Select<IconCatDvo>()
                .ToPageAsync(request.page, request.limit);
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public async Task<List<IconCatDvo>> GetListAsync(SearchRequest request)
        {
            return await _thisRepository.AsQueryable()
                .Where(m => m.pid == request.pid)
                .WhereIF(!string.IsNullOrEmpty(request.key), m => m.name.Contains(request.key))
                .OrderBy(m => m.od)
                .Select<IconCatDvo>()
                .ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ResOptionDvo>> OptionAsync(long pid)
        {
            return await _thisRepository.AsQueryable()
                .Where(a => a.pid == pid)
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
        public async Task<ScmResIconCatDto> GetAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<ScmResIconCatDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ScmResIconCatDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.pid == model.pid && a.code == model.code);
            if (dao != null)
            {
                return false;
            }

            dao = model.Adapt<ScmResIconCatDao>();
            return await _thisRepository.InsertAsync(dao);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ScmResIconCatDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.pid == model.pid && a.code == model.code && a.id != model.id);
            if (dao != null)
            {
                return false;
            }

            dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                return false;
            }

            dao = model.Adapt(dao);
            return await _thisRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 更新记录状态
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> StatusAsync(ScmChangeStatusRequest param)
        {
            return await UpdateStatus(_thisRepository, param.ids, param.status);
        }

        /// <summary>
        /// 删除记录,支持多个
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
