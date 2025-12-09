using Com.Scm.Samples.Book.Dao;

namespace Com.Scm.Samples.Book
{
    public interface IBookService
    {
        BookDao GetDaoById(long id, bool useCache = true);

        void RemoveCacheById(long id);
    }
}
