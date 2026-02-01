namespace Com.Scm.Terminal.Dvo
{
    public class TokenResult
    {
        public long terminal_id { get; set; }

        public string terminal_codes { get; set; }

        public string terminal_names { get; set; }

        public long user_id { get; set; }

        public string user_codes { get; set; }

        public string user_names { get; set; }

        public string avatar { get; set; }

        public string access_token { get; set; }

        public string refresh_token { get; set; }

        public long expires_in { get; set; }
    }
}
