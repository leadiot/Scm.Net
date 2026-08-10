using Com.Scm.Enums;

namespace Com.Scm.Log.Fe.Dvo
{
    public class SearchRequest : ScmSearchPageRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public ScmLogLevelEnum Level { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string category { get; set; }
    }
}
