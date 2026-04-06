namespace Com.Scm.Token
{
    public interface IScmHolder
    {
        void SetToken(ScmToken token);

        ScmToken GetToken();

        void Clear();
    }
}
