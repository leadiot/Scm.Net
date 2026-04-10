using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Service;
using Com.Scm.Sys.Enums;
using Com.Scm.Sys.Gtd;
using Com.Scm.Sys.GtdHeader.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.GtdHeader
{
    /// <summary>
    /// 待办服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "gtd")]
    public class ScmSysGtdHeaderService : ApiService
    {
        public const long SYS_ID = 1738391109933076480;

        private readonly SugarRepository<GtdHeaderDao> _thisRepository;

        public ScmSysGtdHeaderService(SugarRepository<GtdHeaderDao> thisRepository, IResHolder resHolder)
        {
            _thisRepository = thisRepository;
            _ResHolder = resHolder;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<GtdHeaderDvo>> GetPagesAsync(SearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                .WhereIF(IsValidId(request.cat_id), a => a.cat_id == request.cat_id)
                .WhereIF(request.handle != ScmGtdHandleEnum.None, a => a.handle == request.handle)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.title.Contains(request.key))
                .OrderBy(a => a.handle, SqlSugar.OrderByType.Asc)
                .OrderBy(a => a.id)
                .Select<GtdHeaderDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<GtdHeaderDvo>> GetListAsync(SearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
                .WhereIF(IsValidId(request.cat_id), a => a.cat_id == request.cat_id)
                .WhereIF(request.handle != ScmGtdHandleEnum.None, a => a.handle == request.handle)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.title.Contains(request.key))
                .OrderBy(a => a.handle, SqlSugar.OrderByType.Asc)
                .OrderBy(a => a.id)
                .Select<GtdHeaderDvo>()
                .ToListAsync();

            Prepare(result);
            return result;
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<GtdHeaderDvo> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<GtdHeaderDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<GtdHeaderDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<GtdHeaderDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(GtdHeaderDto model)
        {
            if (string.IsNullOrEmpty(model.title))
            {
                return false;
            }

            var dao = model.Adapt<GtdHeaderDao>();
            dao.handle = ScmGtdHandleEnum.Todo;
            dao.priority = ScmGtdPriorityEnum.Level4;

            return await _thisRepository.InsertAsync(dao);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(GtdHeaderDto model)
        {
            var dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                return false;
            }

            dao = model.Adapt(dao);
            return await _thisRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> HandleAsync(GtdHeaderDto model)
        {
            var dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                return false;
            }

            dao.handle = model.handle;
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