namespace Com.Scm.Terminal.Dvo
{
    public class RefreshRequest : ScmRequest
    {
        public long terminal_id { get; set; }

        public string access_token { get; set; }

        public string refresh_token { get; set; }
    }
}
