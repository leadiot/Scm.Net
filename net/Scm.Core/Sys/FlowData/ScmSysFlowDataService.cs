using Com.Scm.Service;
using Com.Scm.Sys.FlowData.Dvo;
using Com.Scm.Sys.FlowData.Rnr;
using Com.Scm.Token;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Sys.FlowData
{
    [ApiExplorerSettings(GroupName = "Sys")]
    public class ScmSysFlowDataService : ApiService
    {
        private readonly ScmContextHolder _jwtHolder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlClient"></param>
        public ScmSysFlowDataService(ISqlSugarClient sqlClient, IResHolder userService, ScmContextHolder jwtHolder)
        {
            _SqlClient = sqlClient;
            _ResHolder = userService;
            _jwtHolder = jwtHolder;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<ScmFlowDataDvo>> GetPageAsync(SearchRequest request)
        {
            var token = _jwtHolder.GetToken();
            var userId = token.user_id;

            var result = await _SqlClient.Queryable<ScmFlowDataHeaderDao>()
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                .Where(a => SqlFunc.Subqueryable<ScmFlowDataDetailDao>()
                    .WhereIF(request.filter == SearchFilter.ApproveByMe, b => b.user_id == userId)
                    .WhereIF(request.filter == SearchFilter.CreatedByMe, b => b.create_user == userId)
                    .Any())
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.order_codes.Contains(request.key))
                .OrderBy(m => m.id)
                .Select<ScmFlowDataDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<ScmFlowDataDvo>> GetListAsync(ScmSearchRequest request)
        {
            var result = await _SqlClient.Queryable<ScmFlowDataHeaderDao>()
                .Where(a => a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .WhereIF(!string.IsNullOrEmpty(request.key), a => a.order_codes.Contains(request.key))
                .OrderBy(m => m.id)
                .Select<ScmFlowDataDvo>()
                .ToListAsync();

            Prepare(result);
            return result;
        }

        /// <summary>
        /// 获取审批日志
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<List<ScmFlowDataResultDvo>> GetResultAsync(long id)
        {
            var result = await _SqlClient.Queryable<ScmFlowDataResultDao>()
                .Where(a => a.data_id == id && a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .OrderBy(m => m.id)
                .Select<ScmFlowDataResultDvo>()
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
        public async Task<ScmFlowDataHeaderDto> GetAsync(long id)
        {
            return await _SqlClient.Queryable<ScmFlowDataHeaderDao>()
                .Select<ScmFlowDataHeaderDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 编辑读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmFlowDataHeaderDto> GetEditAsync(long id)
        {
            return await _SqlClient.Queryable<ScmFlowDataHeaderDao>()
                .Select<ScmFlowDataHeaderDto>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 查看读取
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ScmFlowDataDvo> GetViewAsync(long id)
        {
            return await _SqlClient.Queryable<ScmFlowDataHeaderDao>()
                .Select<ScmFlowDataDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 批量更新状态
        /// </summary>
        /// <param name="param">逗号分隔</param>
        /// <returns></returns>
        public async Task<int> StatusAsync(ScmChangeStatusRequest param)
        {
            return await UpdateStatus<ScmFlowDataHeaderDao>(_SqlClient, param.ids, param.status);
        }

        /// <summary>
        /// 批量删除记录
        /// </summary>
        /// <param name="ids">逗号分隔</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<int> DeleteAsync(string ids)
        {
            return await DeleteRecord<ScmFlowDataHeaderDao>(_SqlClient, ids.ToListLong(), false);
        }
    }
}
