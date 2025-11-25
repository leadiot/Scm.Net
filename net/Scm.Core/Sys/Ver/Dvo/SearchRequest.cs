using Com.Scm.Enums;

namespace Com.Scm.Sys.Ver.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchRequest : ScmSearchPageRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ScmClientTypeEnum client { get; set; }
    }
}
