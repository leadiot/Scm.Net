using Com.Scm.Request;

namespace Com.Scm
{
    public class ScmUpdateRequest : ScmRequest
    {
        public long id { get; set; }
    }

    public class ScmUpdateRequest<T> : ScmRequest
    {
        public T data { get; set; }
    }
}
