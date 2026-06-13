namespace Com.Scm.Token
{
    public interface IScmTokenHolder
    {
        void SetToken(ScmToken token);

        ScmToken GetToken();

        void Clear();
    }
}
