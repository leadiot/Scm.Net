using Com.Scm.Service;
using Com.Scm.Sys.VoteHeader.Dvo;
using Com.Scm.Token;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Sys.Vote;

/// <summary>
/// 投票表服务接口
/// </summary>
[ApiExplorerSettings(GroupName = "Sys")]
public class ScmSysVoteService : ApiService
{
    private readonly ScmContextHolder _JwtHolder;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sqlClient"></param>
    public ScmSysVoteService(ISqlSugarClient sqlClient, ScmContextHolder jwtHolder)
    {
        _SqlClient = sqlClient;
        _JwtHolder = jwtHolder;
    }

    /// <summary>
    /// 查询所有——分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<ScmSearchPageResponse<SysVoteHeaderDvo>> GetPagesAsync(ScmSearchPageRequest param)
    {
        var query = await _SqlClient.Queryable<VoteHeaderDao>()
            .WhereIF(!string.IsNullOrEmpty(param.key), m => m.title.Contains(param.key))
            .Select<SysVoteHeaderDvo>()
            .ToPageAsync(param.page, param.limit);
        return query;
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<VoteHeaderDto> GetAsync(long id)
    {
        var headerDto = await _SqlClient
            .Queryable<VoteHeaderDao>()
            .Where(m => m.id == id)
            .Select<VoteHeaderDto>()
            .FirstAsync();

        if (headerDto != null)
        {
            headerDto.details = await _SqlClient.Queryable<VoteDetailDao>()
                .Where(a => a.header_id == id && a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .Select<VoteDetailDto>()
                .ToListAsync();
        }

        return headerDto;
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task AddAsync(VoteHeaderDto model)
    {
        var dao = model.Adapt<VoteHeaderDao>();

        await _SqlClient.InsertAsync(dao);

        if (model.details == null || model.details.Count < 1)
        {
            return;
        }

        var detailDaoList = new List<VoteDetailDao>();
        foreach (var item in model.details)
        {
            var detailDao = item.Adapt<VoteDetailDao>();
            detailDao.header_id = dao.id;
            detailDaoList.Add(detailDao);
        }

        await _SqlClient.InsertAsync(detailDaoList);
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task UpdateAsync(VoteHeaderDto model)
    {
        var headerDao = await _SqlClient.GetFirstAsync<VoteHeaderDao>(a => a.id == model.id);
        if (headerDao == null)
        {
            return;
        }

        headerDao = model.Adapt(headerDao);
        await _SqlClient.UpdateAsync(headerDao);

        await _SqlClient.Updateable<VoteDetailDao>()
            .SetColumns(a => a.row_status == Enums.ScmRowStatusEnum.Disabled)
            .Where(a => a.header_id == headerDao.id)
            .ExecuteCommandAsync();

        if (model.details == null || model.details.Count < 1)
        {
            return;
        }

        var detailDaoList = await _SqlClient.GetListAsync<VoteDetailDao>(a => a.header_id == headerDao.id);
        foreach (var detail in model.details)
        {
            var detailDao = detailDaoList.Find(a => a.id == detail.id);
            if (detailDao != null)
            {
                detailDao = detail.Adapt(detailDao);
                await _SqlClient.UpdateAsync(detailDao);
                continue;
            }

            detailDao = detail.Adapt<VoteDetailDao>();
            await _SqlClient.InsertAsync(detailDao);
        }
    }

    /// <summary>
    /// 删除,支持批量
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task DeleteAsync([FromBody] List<long> ids)
    {
        await _SqlClient.DeleteAsync<VoteHeaderDao>(m => ids.Contains(m.id));
        await _SqlClient.DeleteAsync<VoteDetailDao>(m => ids.Contains(m.header_id));
        await _SqlClient.DeleteAsync<VoteResultDao>(m => ids.Contains(m.header_id));
    }
}
