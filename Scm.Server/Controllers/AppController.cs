using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Controllers;

/// <summary>
/// Appʹ�õĻ���Controller
/// </summary>
[ApiController]
//[Authorize("App")]
[Route("app/[controller]")]
[ApiExplorerSettings(GroupName = "scm")]
public class AppController : ControllerBase
{
}