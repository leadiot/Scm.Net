namespace Com.Scm.Config
{
    public class KestrelConfig
    {
        public const string NAME = "Kestrel";

        public EndpointsConfig Endpoints { get; set; }
    }

    public class EndpointsConfig
    {
        public HttpConfig Http { get; set; }
    }

    public class HttpConfig
    {
        public string Url { get; set; }
    }

    public class LimitsConfig
    {
    }
}
