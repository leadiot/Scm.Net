using Com.Scm.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Controllers;

/// <summary>
/// 
/// </summary>
[ApiController]
//[Authorize("Web")]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "scm")]
public class ApiController : ControllerBase
{
    /// <summary>
    /// 抛出异常
    /// </summary>
    /// <param name="message"></param>
    /// <exception cref="BusinessException"></exception>
    protected void Error(string message)
    {
        throw new BusinessException(message);
    }
}