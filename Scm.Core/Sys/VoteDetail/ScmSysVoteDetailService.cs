using Com.Scm.Dsa;
using Com.Scm.Service;
using Com.Scm.Sys.Vote;
using Com.Scm.Sys.VoteDetail.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.VoteDetail;

/// <summary>
/// 投票项服务接口
/// </summary>
[ApiExplorerSettings(GroupName = "Sys")]
public class ScmSysVoteDetailService : ApiService
{
    private readonly SugarRepository<VoteDetailDao> _thisRepository;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisRepository"></param>
    public ScmSysVoteDetailService(SugarRepository<VoteDetailDao> thisRepository)
    {
        _thisRepository = thisRepository;
    }

    /// <summary>
    /// 查询所有——分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<ScmSearchPageResponse<SysVoteDetailDvo>> GetPagesAsync(ScmSearchPageRequest param)
    {
        var query = await _thisRepository.AsQueryable()
            .OrderByDescending(m => m.id)
            .Select<SysVoteDetailDvo>()
            .ToPageAsync(param.limit, param.page);
        return query;
    }

    public async Task<List<SysVoteDetailDvo>> GetListAsync(ScmSearchRequest param)
    {
        var query = await _thisRepository
            .AsQueryable()
            .Where(a => a.header_id == param.id && a.row_status == Enums.ScmRowStatusEnum.Enabled)
            .Select<SysVoteDetailDvo>()
            .ToListAsync();
        return query;
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<VoteDetailDto> GetAsync(long id)
    {
        return await _thisRepository
            .AsQueryable()
            .Where(a => a.id == id)
            .Select<VoteDetailDto>()
            .FirstAsync();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> AddAsync(SysVoteDetailDvo model)
    {
        return await _thisRepository.InsertAsync(model.Adapt<VoteDetailDao>());
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(SysVoteDetailDvo model)
    {
        return await _thisRepository.UpdateAsync(model.Adapt<VoteDetailDao>());
    }

    [HttpPost]
    public async Task<bool> UpdateBatchAsync(BatchRequest request)
    {
        await _thisRepository.AsUpdateable()
            .SetColumns(a => a.row_status == Enums.ScmRowStatusEnum.Disabled)
            .Where(a => a.header_id == request.id)
            .ExecuteCommandAsync();

        if (request.details == null)
        {
            return true;
        }

        var detailDaoList = await _thisRepository.GetListAsync(a => a.header_id == request.id);
        foreach (var detail in request.details)
        {
            var detailDao = detailDaoList.Find(a => a.id == detail.id);
            if (detailDao != null)
            {
                detailDao = detail.Adapt(detailDao);
                detailDao.row_status = Enums.ScmRowStatusEnum.Enabled;
                await _thisRepository.UpdateAsync(detailDao);
                continue;
            }

            detailDao = detail.Adapt<VoteDetailDao>();
            detailDao.header_id = request.id;
            await _thisRepository.InsertAsync(detailDao);
        }
        return true;
    }

    /// <summary>
    /// 删除,支持批量
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync([FromBody] List<long> ids)
    {
        return await _thisRepository.DeleteAsync(m => ids.Contains(m.id));
    }
}
