using Com.Scm.Request;

namespace Com.Scm.Log.Fe.Rnr
{
    public class RecordRequest : ScmRequest
    {
        public List<LogFeDto> logs { get; set; }
    }
}
