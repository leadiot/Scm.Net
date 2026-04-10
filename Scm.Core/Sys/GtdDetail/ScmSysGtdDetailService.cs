using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Service;
using Com.Scm.Sys.Enums;
using Com.Scm.Sys.Gtd;
using Com.Scm.Sys.GtdDetail.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.GtdDetail
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(GroupName = "gtd")]
    public class ScmSysGtdDetailService : ApiService
    {
        private readonly SugarRepository<GtdDetailDao> _thisRepository;

        public ScmSysGtdDetailService(SugarRepository<GtdDetailDao> thisRepository, IResHolder resHolder)
        {
            _thisRepository = thisRepository;
            _ResHolder = resHolder;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<GtdDetailDvo>> GetPagesAsync(SearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                .WhereIF(IsValidId(request.header_id), a => a.header_id == request.header_id)
                .WhereIF(request.handle != ScmGtdHandleEnum.None, a => a.handle == request.handle)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.title.Contains(request.key))
                .OrderBy(a => a.handle, SqlSugar.OrderByType.Asc)
                .OrderBy(a => a.id)
                .Select<GtdDetailDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<GtdDetailDvo>> GetListAsync(SearchRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
                .WhereIF(IsValidId(request.header_id), a => a.header_id == request.header_id)
                .WhereIF(request.handle != ScmGtdHandleEnum.None, a => a.handle == request.handle)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.title.Contains(request.key))
                .OrderBy(a => a.handle, SqlSugar.OrderByType.Asc)
                .OrderBy(a => a.id)
                .Select<GtdDetailDvo>()
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
        public async Task<GtdDetailDvo> GetEditAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<GtdDetailDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<GtdDetailDvo> GetViewAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<GtdDetailDvo>()
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

            var dao = model.Adapt<GtdDetailDao>();
            dao.handle = ScmGtdHandleEnum.Todo;
            dao.priority = ScmGtdPriorityEnum.Level4;

            return await _thisRepository.InsertAsync(dao);
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAsync(GtdHeaderDto model)
        {
            var dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                return;
            }

            dao = model.Adapt(dao);
            await _thisRepository.UpdateAsync(dao);
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task ChangeHandleAsync(GtdHeaderDto model)
        {
            var dao = await _thisRepository.GetByIdAsync(model.id);
            if (dao == null)
            {
                return;
            }

            dao.handle = model.handle;
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
