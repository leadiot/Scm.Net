using Com.Scm.Request;
using Microsoft.AspNetCore.Http;

namespace Com.Scm
{
    public class ScmImportRequest : ScmRequest
    {
        public string filename { get; set; }
        public IFormFile file { get; set; }
    }
}
