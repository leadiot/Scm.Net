using Com.Scm.Controllers;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Api.Controllers
{
    [ApiExplorerSettings(GroupName = "Scm")]
    [AllowAnonymous]
    public class TestController : ApiController
    {
        public TestController()
        {
        }

        [HttpGet("Test")]
        public object Test()
        {
            var id = UidUtils.NextId();
            var code = UidUtils.NextCodes("samples_demo");
            return new { id, code };
        }

        [HttpGet("Demo")]
        public object Demo()
        {
            var id = UidUtils.NextId();
            var code = UidUtils.NextCodes("samples_demo");
            return new { id, code };
        }
    }
}
