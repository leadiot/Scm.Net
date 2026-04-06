using Com.Scm.Dsa;
using Com.Scm.Service;
using Com.Scm.Sys.Vote;
using Com.Scm.Sys.VoteResult.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.VoteResult;

/// <summary>
/// 投票日志服务接口
/// </summary>
[ApiExplorerSettings(GroupName = "Sys")]
public class ScmSysVoteResultService : ApiService
{
    private readonly SugarRepository<VoteResultDao> _thisRepository;
    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisRepository"></param>
    public ScmSysVoteResultService(SugarRepository<VoteResultDao> thisRepository)
    {
        _thisRepository = thisRepository;
    }

    /// <summary>
    /// 查询所有——分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<ScmSearchPageResponse<SysVoteResultDvo>> GetPagesAsync(ScmSearchPageRequest param)
    {
        var query = await _thisRepository.AsQueryable()
            .OrderByDescending(m => m.id)
            .Select<SysVoteResultDvo>()
            .ToPageAsync(param.limit, param.page);
        return query;
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<VoteResultDto> GetAsync(long id)
    {
        return await _thisRepository
            .AsQueryable()
            .Where(a => a.id == id)
            .Select<VoteResultDto>()
            .FirstAsync();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> AddAsync(SysVoteResultDvo model) =>
        await _thisRepository.InsertAsync(model.Adapt<VoteResultDao>());

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task<bool> UpdateAsync(SysVoteResultDvo model) =>
        await _thisRepository.UpdateAsync(model.Adapt<VoteResultDao>());

    /// <summary>
    /// 删除,支持批量
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync([FromBody] List<long> ids) =>
        await _thisRepository.DeleteAsync(m => ids.Contains(m.id));
}
