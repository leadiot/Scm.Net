using Com.Scm.Enums;

namespace Com.Scm.Sys.Tasks.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchRequest : ScmSearchPageRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public TaskTypesEnum types { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ScmHandleEnum handle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ScmResultEnum result { get; set; }
    }
}
