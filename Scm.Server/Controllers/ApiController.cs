using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Controllers;

/// <summary>
/// Web使用的基类Controller
/// </summary>
[ApiController]
//[Authorize("Web")]
[Route("api/[controller]")]
[ApiExplorerSettings(GroupName = "Scm")]
public class ApiController : ControllerBase
{
}