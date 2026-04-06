using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Res.Tag.Dvo;
using Com.Scm.Service;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Res.Tag
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(GroupName = "Res")]
    public class ScmResTagService : ApiService
    {
        private readonly SugarRepository<ScmResTagDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public ScmResTagService(SugarRepository<ScmResTagDao> thisRepository)
        {
            _thisRepository = thisRepository;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<TagDvo>> GetPagesAsync(SearchRequest request)
        {
            return await _thisRepository.AsQueryable()
                .Where(a => a.app == request.app)
                .WhereIF(!string.IsNullOrEmpty(request.key), m => m.label.Contains(request.key))
                .Select<TagDvo>()
                .ToPageAsync(request.page, request.limit);
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public async Task<List<TagDvo>> GetListAsync(SearchRequest request)
        {
            return await _thisRepository.AsQueryable()
                .Where(a => a.app == request.app)
                .WhereIF(!string.IsNullOrEmpty(request.key), m => m.label.Contains(request.key))
                .Select<TagDvo>()
                .ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        [HttpGet("{app}")]
        public async Task<List<ResOptionDvo>> GetOptionAsync(long app)
        {
            return await _thisRepository.AsQueryable()
                .Where(a => a.app == app)
                .OrderBy(m => m.qty, OrderByType.Desc)
                .Select(a => new ResOptionDvo { id = a.id, label = a.label, value = a.id })
                .ToListAsync();
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmResTagDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ScmResTagDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<TagDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<TagDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ScmResTagDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.app == model.app && a.label == model.label);
            if (dao != null)
            {
                return false;
            }

            dao = model.Adapt<ScmResTagDao>();
            dao.qty = 1;
            await _thisRepository.InsertAsync(dao);
            return true;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ScmResTagDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.app == model.app && a.label == model.label && a.id != model.id);
            if (dao != null)
            {
                return false;
            }

            dao.label = model.label;
            return await _thisRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> SaveAsync(ScmResTagDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.app == model.app && a.label == model.label);
            if (dao != null)
            {
                if (dao.row_status != Enums.ScmRowStatusEnum.Enabled)
                {
                    dao.row_status = Enums.ScmRowStatusEnum.Enabled;
                    dao.qty = 1;
                }
                else
                {
                    dao.qty += 1;
                }

                return await _thisRepository.UpdateAsync(dao);
            }

            dao = model.Adapt<ScmResTagDao>();
            dao.qty = 1;
            await _thisRepository.InsertAsync(dao);
            return true;
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

