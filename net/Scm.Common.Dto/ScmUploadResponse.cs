using Com.Scm.Api;
using System.Collections.Generic;

namespace Com.Scm
{
    public class ScmUploadResponse : ScmApiResponse
    {
        public List<ScmUploadResult> results { get; set; }

        public void AddResult(ScmUploadResult result)
        {
            if (results == null)
            {
                results = new List<ScmUploadResult>();
            }
            results.Add(result);
        }
    }

    public class ScmUploadResult
    {
        public string name { get; set; }
        public string path { get; set; }
        public string size { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
    }
}
