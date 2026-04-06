using Com.Scm.Dsa;
using Com.Scm.Log;
using Com.Scm.Utils;

namespace Com.Scm.Service
{
    public class ScmLogService : ILogService
    {
        private readonly SugarRepository<LogApiDao> _thisRepository;

        public ScmLogService(SugarRepository<LogApiDao> thisRepository)
        {
            _thisRepository = thisRepository;
        }

        public void Log(LogInfo log)
        {
            _thisRepository.Insert(log.Adapt<LogApiDao>());
        }

        public async Task LogAsync(LogInfo log)
        {
            await _thisRepository.InsertAsync(log.Adapt<LogApiDao>());
        }
    }
}
