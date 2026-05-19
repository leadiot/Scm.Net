using Com.Scm.Request;

namespace Com.Scm.Sys.Files.Dvo
{
    public class AddFileRequest : ScmRequest
    {
        public string path { get; set; }

        public string name { get; set; }
    }
}
