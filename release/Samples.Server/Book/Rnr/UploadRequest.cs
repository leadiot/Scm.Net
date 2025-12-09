using Microsoft.AspNetCore.Http;

namespace Com.Scm.Samples.Book.Rnr
{
    /// <summary>
    /// 
    /// </summary>
    public class UploadRequest : ScmRequest
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
