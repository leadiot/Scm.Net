namespace Com.Scm.Config;

public class OidcConfig
{
    public const string NAME = "Oidc";

    public string token_uri { get; set; }

    public string app_key { get; set; }

    public string app_secret { get; set; }

    public string redirect_uri { get; set; }

    public string scope { get; set; }

    public void Prepare(EnvConfig envConfig)
    {
        if (string.IsNullOrEmpty(token_uri))
        {
            token_uri = "http://oidc.org.cn/oauth/token";
        }
    }
}