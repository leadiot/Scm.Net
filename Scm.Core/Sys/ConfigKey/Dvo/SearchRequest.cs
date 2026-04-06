using Com.Scm.Enums;

namespace Com.Scm.Sys.ConfigKey.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchRequest : ScmSearchPageRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public long cat_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ScmClientTypeEnum client { get; set; }
    }
}
