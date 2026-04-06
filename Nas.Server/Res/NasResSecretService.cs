using Com.Scm.Dsa;
using Com.Scm.Token;
using SqlSugar;

namespace Com.Scm.Nas.Res
{
    public class NasResSecretService : NasResFileService
    {
        public NasResSecretService(SugarRepository<NasResFileDao> thisRepository, ISqlSugarClient sqlClient, ScmContextHolder scmHolder, IResHolder resHolder)
            : base(thisRepository, sqlClient, scmHolder, resHolder)
        {
        }

        protected override long GetRootDirId()
        {
            var path = NasEnv.PathSecret;
            var dao = _thisRepository.AsQueryable()
                .Where(a => a.path == path)
                .First();

            return dao != null ? dao.id : 0;
        }
    }
}
