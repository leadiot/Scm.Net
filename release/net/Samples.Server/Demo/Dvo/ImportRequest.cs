using Com.Scm.Dvo;
using Microsoft.AspNetCore.Http;

namespace Com.Scm.Samples.Demo.Dvo
{
    public class ImportRequest : ScmRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public IFormFile file { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string filename { get; set; }
    }
}
