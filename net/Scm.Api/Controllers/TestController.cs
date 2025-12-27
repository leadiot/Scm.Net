using Com.Scm.Controllers;
using Com.Scm.Token;
using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("Echo")]
        public ScmApiResponse GetEcho(string id)
        {
            var token = _ScmHolder.GetToken();
            var response = new ScmApiResponse();
            response.SetSuccess();

            return response;
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
