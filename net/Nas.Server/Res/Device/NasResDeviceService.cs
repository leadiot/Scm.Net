using Com.Scm.Dsa;
using Com.Scm.Nas.Res.Files;
using Com.Scm.Token;

namespace Com.Scm.Nas.Res.Device
{
    public class NasResDeviceService : NasResFileService
    {
        public NasResDeviceService(SugarRepository<NasResFileDao> thisRepository, ScmContextHolder scmHolder, IResHolder resHolder)
            : base(thisRepository, scmHolder, resHolder)
        {
        }

        protected override long GetRootDirId()
        {
            var path = NasEnv.PathDevices;
            var dao = _thisRepository.AsQueryable()
                .Where(a => a.path == path)
                .First();

            return dao != null ? dao.id : 0;
        }
    }
}
