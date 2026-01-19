namespace Com.Scm.Nas.Sync.Dvo
{
    public class GetDocRequest : ScmSearchPageRequest
    {
        public long folder_id { get; set; }

        public long dir_id { get; set; }

        public bool by_path { get; set; }

        public string path { get; set; }
    }
}
