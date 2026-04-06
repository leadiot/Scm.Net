using Com.Scm.Response;

namespace Com.Scm.Hubs
{
    public class ScmResultResponse<T> : ScmApiResponse
    {
        public T Data { get; set; }

        public void SetSuccess(T Data)
        {
            this.Code = 200;
            this.Data = Data;
        }
    }
}
