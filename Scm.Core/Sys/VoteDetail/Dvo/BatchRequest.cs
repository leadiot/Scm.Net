using Com.Scm.Sys.Vote;

namespace Com.Scm.Sys.VoteDetail.Dvo
{
    public class BatchRequest : ScmUpdateRequest
    {
        public List<VoteDetailDto> details { get; set; }
    }
}
