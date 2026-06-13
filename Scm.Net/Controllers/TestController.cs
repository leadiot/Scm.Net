using Com.Scm.Request;
using Com.Scm.Response;
using Com.Scm.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Controllers
{
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "Scm")]
    public class TestController : ApiController
    {
        private IScmTokenHolder _ScmHolder;

        public TestController(IScmTokenHolder scmHolder)
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

        [HttpPost("Mime")]
        public ScmApiResponse MimeAsync()
        {
            var token = _ScmHolder.GetToken();
            var response = new ScmApiDataResponse<long>();
            response.Data = token.terminal_id;
            response.SetSuccess();

            return response;
        }
    }
}
