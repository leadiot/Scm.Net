using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Res.Icon.Dvo;
using Com.Scm.Service;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Res.Icon
{
    [ApiExplorerSettings(GroupName = "Res")]
    public class ScmResIconService : ApiService
    {
        private readonly SugarRepository<ScmResIconDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public ScmResIconService(SugarRepository<ScmResIconDao> thisRepository)
        {
            _thisRepository = thisRepository;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<IconDvo>> GetPagesAsync(SearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .Where(m => m.cat_id == request.cat_id)
                .WhereIF(!string.IsNullOrEmpty(request.key), m => m.key.Contains(request.key))
                .OrderBy(m => m.od)
                .Select<IconDvo>()
                .ToPageAsync(request.page, request.limit);

            await Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public async Task<List<IconDvo>> GetListAsync(SearchRequest request)
        {
            var list = await _thisRepository.AsQueryable()
                .Where(m => m.cat_id == request.cat_id)
                .WhereIF(!string.IsNullOrEmpty(request.key), m => m.key.Contains(request.key))
                .OrderBy(m => m.od)
                .Select<IconDvo>()
                .ToListAsync();

            await Prepare(list);
            return list;
        }

        private async Task Prepare(List<IconDvo> list)
        {
            var catDao = await _thisRepository.Change<ScmResIconCatDao>().GetByIdAsync(0);
            foreach (var cat in list)
            {
                if (catDao == null)
                {
                    catDao = await _thisRepository.Change<ScmResIconCatDao>().GetByIdAsync(cat.set_id);
                }
                cat.set = catDao.code;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ResOptionDvo>> OptionAsync(long catId)
        {
            return await _thisRepository.AsQueryable()
                .Where(a => a.cat_id == catId)
                .OrderBy(m => m.od, OrderByType.Asc)
                .Select(a => new ResOptionDvo { id = a.id, label = a.key, value = a.id })
                .ToListAsync();
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmResIconDto> GetAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<ScmResIconDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ScmResIconDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.cat_id == model.cat_id && a.key == model.key);
            if (dao != null)
            {
                return false;
            }

            dao = model.Adapt<ScmResIconDao>();
            return await _thisRepository.InsertAsync(dao);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ScmResIconDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.cat_id == model.cat_id && a.key == model.key && a.id != model.id);
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