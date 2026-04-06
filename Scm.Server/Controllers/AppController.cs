using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Controllers;

/// <summary>
/// App使用的基类Controller
/// </summary>
[ApiController]
//[Authorize("App")]
[Route("app/[controller]")]
[ApiExplorerSettings(GroupName = "Scm")]
public class AppController : ControllerBase
{
}