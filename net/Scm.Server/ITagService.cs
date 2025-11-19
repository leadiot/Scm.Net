using Com.Scm.Res.Tag;

namespace Com.Scm
{
    public interface ITagService
    {
        ScmResTagDao GetById(long id);

        Task<ScmResTagDao> GetByIdAsync(long id);

        ScmResTagDao GetByName(long appId, string name);

        Task<ScmResTagDao> GetByNameAsync(long appId, string name);
    }
}
