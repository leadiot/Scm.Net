using Com.Scm.Request;

namespace Com.Scm
{
    public class ScmApproveRequest : ScmRequest
    {
        public long id { get; set; }

        public string comment { get; set; }
    }
}
