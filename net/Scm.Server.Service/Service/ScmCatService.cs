using Com.Scm.Dsa;
using Com.Scm.Res.Cat;

namespace Com.Scm.Service
{
    public class ScmCatService : ApiService, ICatService
    {
        private readonly SugarRepository<ScmResCatDao> _thisRepository;

        public ScmCatService(SugarRepository<ScmResCatDao> thisRepository)
        {
            _thisRepository = thisRepository;
        }

    }
}
