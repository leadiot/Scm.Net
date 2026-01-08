using Com.Scm.Dr.Web.Dto;
using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Service;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Dr.Web
{
    /// <summary>
    /// 服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Dr")]
    public class ScmDrWebDailyService : ApiService
    {
        private readonly SugarRepository<ScmDrWebDailyDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public ScmDrWebDailyService(SugarRepository<ScmDrWebDailyDao> thisRepository, IUserHolder userService)
        {
            _thisRepository = thisRepository;
            _ResHolder = userService;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<ScmDrWebDailyDvo>> GetPagesAsync(ScmSearchPageRequest request)
        {
            var result = await _thisRepository.AsQueryable()
                .WhereIF(!request.IsAllStatus(), a => a.row_status == request.row_status)
                //.WhereIF(IsValidId(request.option_id), a => a.option_id == request.option_id)
                //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
                .OrderBy(m => m.id)
                .Select<ScmDrWebDailyDvo>()
                .ToPageAsync(request.page, request.limit);

            Prepare(result.Items);
            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<ScmDrWebDailyDvo>> GetListAsync(SearchRequest request)
        {
            var now = DateTime.Now;
            var start = now.AddMonths(-1);
            var days = (int)(now - start).TotalDays;
            var day = TimeUtils.FormatDate(start);

            var daoList = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled && day.CompareTo(a.day) >= 0)
                //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
                //.OrderBy(m => m.day)
                .ToListAsync();

            if (daoList.Count < days)
            {
                var random = new Random();
                var appendList = new List<ScmDrWebDailyDao>();
                while (start <= now)
                {
                    var tmp = TimeUtils.FormatDate(start);
                    var dao = daoList.FirstOrDefault(a => a.day == tmp);
                    if (dao == null)
                    {
                        var pv = random.Next(10, 10000);
                        var uv = random.Next(1, pv);
                        dao = new ScmDrWebDailyDao
                        {
                            day = day,
                            pv = pv,
                            uv = uv,
                        };
                        appendList.Add(dao);
                    }
                    start = start.AddDays(1);
                }
                await _thisRepository.InsertRangeAsync(appendList);
            }

            var result = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled && day.CompareTo(a.day) >= 0)
                //.WhereIF(!string.IsNullOrEmpty(request.key), a => a.text.Contains(request.key))
                .OrderBy(m => m.day)
                .Select<ScmDrWebDailyDvo>()
                .ToListAsync();

            return result;
        }
    }
}