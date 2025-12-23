using Com.Scm.Dsa;
using Com.Scm.Filters;
using Com.Scm.Service;
using Com.Scm.Sys.Pv.Dvo;
using Com.Scm.Token;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.Pv
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(GroupName = "Sys")]
    public class ScmSysPvService : ApiService
    {
        private readonly SugarRepository<PvHeaderDao> _headerRepository;
        private readonly SugarRepository<PvDetailDao> _detailRepository;
        private readonly ScmContextHolder _jwtContextHolder;

        /// <summary>
        /// 
        /// </summary>
        public ScmSysPvService(SugarRepository<PvHeaderDao> headerRepository,
            SugarRepository<PvDetailDao> detailRepository,
            ScmContextHolder contextHolder)
        {
            _headerRepository = headerRepository;
            _detailRepository = detailRepository;
            _jwtContextHolder = contextHolder;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [NoAuditLog]
        public async Task AddAsync(PvRequest request)
        {
            var token = _jwtContextHolder.GetToken();

            var now = DateTime.Now;
            var date = now.ToString(ScmEnv.FORMAT_DATE);

            try
            {
                var detailDao = new PvDetailDao();
                detailDao.date = date;
                detailDao.user_id = token.user_id;
                detailDao.url = request.url;
                //detailDao.title = request.title;
                detailDao.time = TimeUtils.GetUnixTime(now);

                await _detailRepository.InsertAsync(detailDao);

                var qty = await _headerRepository.AsUpdateable()
                     .SetColumns(a => a.qty == a.qty + 1)
                     .Where(a => a.date == date && a.user_id == token.user_id && a.url == request.url)
                     .ExecuteCommandAsync();
                if (qty < 1)
                {
                    var headerDao = new PvHeaderDao();
                    headerDao.date = date;
                    headerDao.user_id = token.user_id;
                    headerDao.url = request.url;
                    headerDao.qty = 1;
                    await _headerRepository.InsertAsync(headerDao);
                }
            }
            catch (Exception ex)
            {
                LogUtils.Error(ex.ToString());
            }
        }
    }
}
