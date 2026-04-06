using Com.Scm.Dsa;
using Com.Scm.Exceptions;
using Com.Scm.Service;
using Com.Scm.Sys.Vote;
using Com.Scm.Sys.VoteHeader.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.VoteHeader;

/// <summary>
/// 投票表服务接口
/// </summary>
[ApiExplorerSettings(GroupName = "Sys")]
public class ScmSysVoteHeaderService : ApiService
{
    private readonly SugarRepository<VoteHeaderDao> _thisRepository;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sqlClient"></param>
    public ScmSysVoteHeaderService(SugarRepository<VoteHeaderDao> thisRepository)
    {
        _thisRepository = thisRepository;
    }

    /// <summary>
    /// 查询所有——分页
    /// </summary>
    /// <param name="param"></param>
    /// <returns></returns>
    public async Task<ScmSearchPageResponse<SysVoteHeaderDvo>> GetPagesAsync(ScmSearchPageRequest param)
    {
        var query = await _thisRepository
            .AsQueryable()
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
        return await _thisRepository
            .AsQueryable()
            .Where(m => m.id == id)
            .Select<VoteHeaderDto>()
            .FirstAsync();
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task AddAsync(VoteHeaderDto model)
    {
        var dao = await _thisRepository.GetFirstAsync(a => a.title == model.title);
        if (dao != null)
        {
            throw new BusinessException("已存在相同名称的投票规则！");
        }

        dao = model.Adapt<VoteHeaderDao>();
        await _thisRepository.InsertAsync(dao);
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public async Task UpdateAsync(VoteHeaderDto model)
    {
        var dao = await _thisRepository
            .AsQueryable()
            .Where(a => a.title == model.title && a.id != model.id)
            .FirstAsync();
        if (dao != null)
        {
            throw new BusinessException("已存在相同名称的投票规则！");
        }

        dao = await _thisRepository.GetFirstAsync(a => a.id == model.id);
        if (dao == null)
        {
            return;
        }

        dao = model.Adapt(dao);
        await _thisRepository.UpdateAsync(dao);
    }

    /// <summary>
    /// 删除,支持批量
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task DeleteAsync([FromBody] List<long> ids)
    {
        await _thisRepository.DeleteAsync(m => ids.Contains(m.id));
    }
}
