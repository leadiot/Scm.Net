namespace Com.Scm.Response
{
    public class ScmApiDataResponse<T> : ScmApiResponse
    {
        /// <summary>
        /// 返回数据
        /// </summary>
        public T Data { get; set; }

        public void SetSuccess(T data)
        {
            _rr = true;
            _rc = 0;
            _rm = "";
            Data = data;
        }
    }
}
