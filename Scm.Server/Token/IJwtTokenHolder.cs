namespace Com.Scm.Token
{
    public interface IJwtTokenHolder
    {
        void SetToken(ScmToken token);

        ScmToken GetToken();

        void Clear();
    }
}
