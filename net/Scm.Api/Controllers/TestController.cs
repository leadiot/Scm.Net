using Com.Scm.Controllers;
using Com.Scm.Token;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Api.Controllers
{
    [ApiExplorerSettings(GroupName = "Scm")]
    public class TestController : ApiController
    {
        private ScmContextHolder _ScmHolder;

        public TestController(ScmContextHolder scmHolder)
        {
            _ScmHolder = scmHolder;
        }

        [HttpPost("Echo")]
        public ScmApiResponse PostEcho(ScmRequest request)
        {
            var token = _ScmHolder.GetToken();
            var response = new ScmApiDataResponse<long>();
            response.Data = token.terminal_id;
            response.SetSuccess();

            return response;
        }
    }
}
