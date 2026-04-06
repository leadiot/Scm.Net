namespace Com.Scm
{
    /// <summary>
    /// 分页参数
    /// </summary>
    public class ScmSearchPageRequest : ScmSearchRequest
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int page { get; set; } = 1;

        /// <summary>
        /// 每页多少条
        /// </summary>
        public int limit { get; set; } = 20;
    }

    /// <summary>
    /// 分页参数
    /// </summary>
    public class ScmSearchPageRequest<T> : ScmSearchRequest
    {
        /// <summary>
        /// 当前页
        /// </summary>
        public int page { get; set; } = 1;

        /// <summary>
        /// 每页多少条
        /// </summary>
        public int limit { get; set; } = 20;

        /// <summary>
        /// 查询条件
        /// </summary>
        public T filter { get; set; }
    }
}