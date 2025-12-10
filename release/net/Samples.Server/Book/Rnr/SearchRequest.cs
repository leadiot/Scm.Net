using Com.Scm.Samples.Book.Enums;

namespace Com.Scm.Samples.Book.Rnr
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchRequest : ScmSearchPageRequest
    {
        public BookTypesEnum types { get; set; }
    }
}
