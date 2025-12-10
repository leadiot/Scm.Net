using Com.Scm.Dvo;
using Microsoft.AspNetCore.Http;

namespace Com.Scm.Samples.Demo.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class UploadRequest : ScmRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string path { get; set; }
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
