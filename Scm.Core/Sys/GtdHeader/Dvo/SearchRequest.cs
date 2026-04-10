using Com.Scm.Sys.Enums;

namespace Com.Scm.Sys.GtdHeader.Dvo
{
    public class SearchRequest : ScmSearchPageRequest
    {
        public long cat_id { get; set; }

        public ScmGtdHandleEnum handle { get; set; }
    }
}
