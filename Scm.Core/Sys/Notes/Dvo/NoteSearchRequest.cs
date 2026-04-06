using Com.Scm.Enums;

namespace Com.Scm.Sys.Notes.Dvo
{
    /// <summary>
    /// 
    /// </summary>
    public class NoteSearchRequest : ScmSearchPageRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public long cat_id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public NoteTypesEnum types { get; set; }
    }
}
