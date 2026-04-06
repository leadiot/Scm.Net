namespace Com.Scm.Sys.FlowData.Rnr
{
    public class SearchRequest : ScmSearchPageRequest
    {
        public SearchFilter filter { get; set; }
    }

    public enum SearchFilter
    {
        None,
        /// <summary>
        /// 我提交的
        /// </summary>
        CreatedByMe,
        /// <summary>
        /// 我审批的
        /// </summary>
        ApproveByMe,
    }
}
