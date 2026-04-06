using Com.Scm.Cache;
using Com.Scm.Controllers;
using Com.Scm.Hubs;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Api.Controllers;

/// <summary>
/// 在线用户
/// </summary>
[ApiExplorerSettings(GroupName = "Scm")]
public class OnLineController : ApiController
{
    private readonly ICacheService _cacheService;

    public OnLineController(ICacheService cacheService)
    {
        _cacheService = cacheService;
    }

    /// <summary>
    /// 获取所有在线信息
    /// </summary>
    /// <returns></returns>
    [HttpGet, AllowAnonymous]
    public List<ClientUser> Get()
    {
        var userList = _cacheService.GetCache<List<ClientUser>>(KeyUtils.ONLINEUSERS);

        return userList ?? new List<ClientUser>();
    }
}