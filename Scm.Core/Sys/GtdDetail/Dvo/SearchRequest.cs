using Com.Scm.Sys.Enums;

namespace Com.Scm.Sys.GtdDetail.Dvo
{
    public class SearchRequest : ScmSearchPageRequest
    {
        public long header_id { get; set; }

        public ScmGtdHandleEnum handle { get; set; }
    }
}
