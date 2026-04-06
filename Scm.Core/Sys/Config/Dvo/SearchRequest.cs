using Com.Scm.Enums;

namespace Com.Scm.Sys.Config.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchRequest : ScmSearchPageRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public ScmClientTypeEnum client { get; set; }
    }
}
