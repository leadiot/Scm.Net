using Com.Scm.Dao;

namespace Com.Scm
{
    public interface IResHolder
    {
        T GetRes<T>(long id, bool cached = false) where T : IResDao;

        string GetResNames<T>(long id, bool cached = false) where T : IResDao;

        string GetResNamec<T>(long id, bool cached = false) where T : IResDao;

        void Remove(long id);

        void Clear();
    }
}
