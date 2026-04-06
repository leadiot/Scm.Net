using Com.Scm.Dsa;
using Com.Scm.Res.Tag;

namespace Com.Scm.Service
{
    public class ScmTagService : ApiService, ITagService
    {
        private readonly SugarRepository<ScmResTagDao> _thisRepository;

        public ScmTagService(SugarRepository<ScmResTagDao> thisRepository)
        {
            _thisRepository = thisRepository;
        }

        public ScmResTagDao GetById(long id)
        {
            return _thisRepository.GetFirst(a => a.id == id);
        }

        public async Task<ScmResTagDao> GetByIdAsync(long id)
        {
            return await _thisRepository.GetFirstAsync(a => a.id == id);
        }

        public ScmResTagDao GetByName(long appId, string name)
        {
            return _thisRepository.GetFirst(a => a.app == appId && a.label == name);
        }

        public async Task<ScmResTagDao> GetByNameAsync(long appId, string name)
        {
            return await _thisRepository.GetFirstAsync(a => a.app == appId && a.label == name);
        }
    }
}
