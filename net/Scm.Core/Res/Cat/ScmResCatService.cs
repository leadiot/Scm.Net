using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Exceptions;
using Com.Scm.Res.Cat.Dvo;
using Com.Scm.Service;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Res.Cat
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(GroupName = "Res")]
    public class ScmResCatService : ApiService
    {
        private readonly SugarRepository<ScmResCatDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public ScmResCatService(SugarRepository<ScmResCatDao> thisRepository)
        {
            _thisRepository = thisRepository;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<CatDvo>> GetPagesAsync(SearchRequest request)
        {
            return await _thisRepository.AsQueryable()
                .WhereIF(IsValidId(request.id), a => a.pid == request.id)
                .WhereIF(IsValidId(request.app_id), a => a.app == request.app_id)
                .WhereIF(!string.IsNullOrEmpty(request.key), m => m.namec.Contains(request.key))
                .OrderBy(m => m.od, OrderByType.Asc)
                .Select<CatDvo>()
                .ToPageAsync(request.page, request.limit);
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public async Task<List<CatDvo>> GetListAsync(SearchRequest request)
        {
            return await _thisRepository.AsQueryable()
                .WhereIF(IsValidId(request.id), a => a.pid == request.id)
                .WhereIF(IsValidId(request.app_id), a => a.app == request.app_id)
                .WhereIF(!string.IsNullOrEmpty(request.key), m => m.namec.Contains(request.key))
                .OrderBy(m => m.od, OrderByType.Asc)
                .Select<CatDvo>()
                .ToListAsync();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="pid"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ResOptionDvo>> OptionAsync(int app, long pid)
        {
            return await _thisRepository.AsQueryable()
                .Where(a => a.app == app && a.pid == pid)
                .OrderBy(m => m.od, OrderByType.Asc)
                .Select(a => new ResOptionDvo { id = a.id, label = a.namec, value = a.id })
                .ToListAsync();
        }

        /// <summary>
        /// 根据上级查询
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<List<ScmResCatDto>> GetListByPidAsync(long pid)
        {
            var list = await _thisRepository.AsQueryable()
                .Where(a => a.pid == pid)
                .OrderBy(m => m.od, OrderByType.Asc).ToListAsync();
            return list.Adapt<List<ScmResCatDto>>();
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmResCatDto> GetAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<ScmResCatDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmResCatDto> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.id == id)
                .Select<ScmResCatDto>()
                .FirstAsync();
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<CatDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<CatDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(ScmResCatDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.app == model.app && a.pid == model.pid && a.namec == model.namec);
            if (dao != null)
            {
                return false;
            }

            dao = model.Adapt<ScmResCatDao>();
            if (IsValidId(dao.pid))
            {
                var parent = await _thisRepository.GetByIdAsync(dao.pid);
                if (parent == null)
                {
                    throw new BusinessException("无效的上级信息！");
                }

                dao.lv = parent.lv + 1;
                dao.tid = parent.tid;
            }
            else
            {
                dao.pid = 0;
                dao.tid = 0;
            }

            return await _thisRepository.InsertAsync(dao);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(ScmResCatDto model)
        {
            var dao = await _thisRepository.GetFirstAsync(a => a.app == model.app && a.pid == model.pid && a.namec == model.namec && a.id != model.id);
            if (dao != null)
            {
                return false;
            }

            dao = await _thisRepository.GetByIdAsync(model.id);
            dao = model.Adapt(dao);
            if (IsValidId(dao.pid))
            {
                var parent = await _thisRepository.GetByIdAsync(dao.pid);
                if (parent == null)
                {
                    throw new BusinessException("无效的上级信息！");
                }

                dao.lv = parent.lv + 1;
                dao.tid = parent.tid;
            }

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
