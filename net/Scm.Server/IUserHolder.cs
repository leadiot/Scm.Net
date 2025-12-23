using Com.Scm.User;

namespace Com.Scm
{
    public interface IUserHolder
    {
        ScmUserInfo GetUser(long id);

        string GetUserNames(long id);

        void Remove(long id);

        void Clear();
    }
}
