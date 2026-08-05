using Com.Scm.Request;

namespace Com.Scm.Sys.Computer.Dvo
{
    public class FileCreateRequest : ScmRequest
    {
        public string path { get; set; }

        public string name { get; set; }
    }
}
