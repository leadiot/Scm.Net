namespace Com.Scm.Operator.Oidc
{
    public class OidcAccessTokenResponse : OidcResponse
    {
        public string access_token { get; set; }

        public string refresh_token { get; set; }

        public long expires_in { get; set; }

        public OidcUserInfo User { get; set; }
    }
}
