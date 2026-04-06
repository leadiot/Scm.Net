namespace Com.Scm.Nas.Log.Dvo
{
    public class SearchRequest : ScmSearchPageRequest
    {
        public long terminal_id { get; set; }
        public long drive_id { get; set; }
        public NasOptEnums opt { get; set; }
    }
}
