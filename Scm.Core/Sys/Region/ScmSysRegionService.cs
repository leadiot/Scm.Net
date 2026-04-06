using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Service;
using Com.Scm.Sys.Region.Dvo;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Sys.Region;

/// <summary>
/// 服务接口
/// </summary>
[ApiExplorerSettings(GroupName = "Sys")]
public class ScmSysRegionService : ApiService
{
    private readonly SugarRepository<RegionDao> _thisRepository;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisRepository"></param>
    public ScmSysRegionService(SugarRepository<RegionDao> thisRepository)
    {
        _thisRepository = thisRepository;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="pid"></param>
    /// <returns></returns>
    [HttpGet("{pid}")]
    public async Task<List<ResOptionDvo>> OptionAsync(long pid)
    {
        return await _thisRepository.AsQueryable()
            .Where(a => a.pid == pid)
            .OrderBy(m => m.od, OrderByType.Asc)
            .Select(a => new ResOptionDvo { id = a.id, label = a.namef, value = a.id })
            .ToListAsync();
    }

    /// <summary>
    /// 根据上级查询
    /// </summary>
    /// <param name="pid"></param>
    /// <returns></returns>
    [HttpGet("{pid}")]
    public async Task<List<RegionDvo>> GetListByPidAsync(long pid)
    {
        return await _thisRepository.AsQueryable()
            .Where(a => a.pid == pid)
            .OrderBy(m => m.od, OrderByType.Asc)
            .Select<RegionDvo>()
            .ToListAsync();
    }
}

