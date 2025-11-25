using Com.Scm.Dev;
using Com.Scm.Dsa;
using Com.Scm.Service;
using Com.Scm.Sys.Ver.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.Ver
{
    /// <summary>
    /// 版本管理
    /// </summary>
    [ApiExplorerSettings(GroupName = "Dev")]
    public class ScmSysVersionService : ApiService
    {
        private readonly SugarRepository<ScmSysVerHeaderDao> _thisRepository;
        private readonly SugarRepository<ScmDevAppDao> _appRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public ScmSysVersionService(SugarRepository<ScmSysVerHeaderDao> thisRepository, SugarRepository<ScmDevAppDao> appRepository)
        {
            _thisRepository = thisRepository;
            _appRepository = appRepository;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<ScmSysVerHeaderDvo>> GetPagesAsync(SearchRequest request)
        {
            var appId = ScmDevAppDto.NET_ID;
            if (!string.IsNullOrWhiteSpace(request.code))
            {
                var appDao = await _appRepository.AsQueryable()
                    .Where(a => a.code == request.code && a.row_status == Enums.ScmRowStatusEnum.Enabled)
                    .FirstAsync();
                if (appDao == null)
                {
                    return null;
                }

                appId = appDao.id;
            }

            var client = request.client;
            if (client == Enums.ScmClientTypeEnum.None)
            {
                client = Enums.ScmClientTypeEnum.Web;
            }

            var result = await _thisRepository.AsQueryable()
                .Where(a => a.app_id == appId && a.client == client && a.row_status == Enums.ScmRowStatusEnum.Enabled)
                .OrderByDescending(m => m.id)
                .Select<ScmSysVerHeaderDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        private void Prepare(List<ScmSysVerHeaderDvo> list)
        {
            foreach (var item in list)
            {
                var dao = _appRepository.GetById(item.app_id);
                item.app_name = dao?.name;
            }
        }
    }
}