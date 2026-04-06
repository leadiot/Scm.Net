using Com.Scm.Cache;
using Com.Scm.Service;
using Com.Scm.Sys.SysSafety.Dto;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.SysSafety;

/// <summary>
/// 
/// </summary>
[ApiExplorerSettings(GroupName = "Sys")]
public class ScmSysSafetyService : ApiService
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="cacheService"></param>
    public ScmSysSafetyService(ICacheService cacheService)
    {
        _CacheService = cacheService;
    }

    /// <summary>
    /// 查询
    /// </summary>
    /// <returns></returns>
    public SafetySetting Get()
    {
        var model = _CacheService.GetCache<SafetySetting>(KeyUtils.SYSTEMSAFETY);
        return model ?? new SafetySetting();
    }

    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    public void Add(SafetySetting model)
    {
        _CacheService.SetCache(KeyUtils.SYSTEMSAFETY, model);
    }
}