using Com.Scm.Dsa;
using Com.Scm.Dto;
using Com.Scm.Enums;
using Com.Scm.Filters;
using Com.Scm.Log.Fe.Dvo;
using Com.Scm.Log.Fe.Rnr;
using Com.Scm.Ur;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Log.Fe
{
    /// <summary>
    /// 前端系统日志
    /// </summary>
    [ApiExplorerSettings(GroupName = "log"), NoAuditLog]
    public class ScmLogFeService : IApiService
    {
        private readonly SugarRepository<LogFeDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public ScmLogFeService(SugarRepository<LogFeDao> thisRepository)
        {
            _thisRepository = thisRepository;
        }

        /// <summary>
        /// 查询所有——分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ScmPageResultDto<ScmLogFeDvo>> GetPagesAsync(SearchRequest request)
        {
            long time1 = 0;
            long time2 = 0;
            if (!string.IsNullOrEmpty(request.times))
            {
                var (btime, etime) = TimeUtils.Splitting(request.times);
                time1 = TimeUtils.GetUnixTime(DateTime.Parse(btime));
                time2 = TimeUtils.GetUnixTime(DateTime.Parse(etime));
            }

            var result = await _thisRepository.AsQueryable()
                .WhereIF(!string.IsNullOrEmpty(request.times), m => m.time >= time1 && m.time < time2)
                .WhereIF(request.Level != 0, m => m.level == (ScmLogLevelEnum)request.Level)
                .WhereIF(string.IsNullOrWhiteSpace(request.category), m => m.category == request.category)
                .OrderByDescending(m => m.id)
                .Select<ScmLogFeDvo>()
                .ToPageAsyncV2(request.page, request.limit);
            foreach (var item in result.Items)
            {
                item.LevelName = item.level.ToString();
            }
            return result;
        }

        /// <summary>
        /// 查询根据日志级别查询图表信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<SysLogChartResponse> GetChartAsync()
        {
            var btime = DateTime.Now.AddDays(-14);
            var list = await _thisRepository.Context.Queryable<LogFeDao>()
                .Where(m => SqlFunc.Between(m.date, btime, DateTime.Now))
                .GroupBy(m => new { m.date, m.level })
                .Select(m => new
                {
                    m.date,
                    m.level,
                    Count = SqlFunc.AggregateCount(m.id)
                })
                .ToListAsync();
            var res = new SysLogChartResponse();
            var debug = new List<int>();
            var info = new List<int>();
            var warn = new List<int>();
            var error = new List<int>();
            var fatal = new List<int>();
            for (var i = 0; i < 15; i++)
            {
                var time = DateTime.Now.AddDays(value: -(14 - i));
                var date = time.ToString(ScmEnv.FORMAT_DATE);
                res.Time.Add(time.ToShortDateString());
                debug.Add(list.FirstOrDefault(m => m.level == ScmLogLevelEnum.Debug && m.date == date) == null ? 0 :
                    list.FirstOrDefault(m => m.level == ScmLogLevelEnum.Debug && m.date == date)!.Count);
                info.Add(list.FirstOrDefault(m => m.level == ScmLogLevelEnum.Info && m.date == date) == null ? 0 :
                    list.FirstOrDefault(m => m.level == ScmLogLevelEnum.Info && m.date == date)!.Count);
                warn.Add(list.FirstOrDefault(m => m.level == ScmLogLevelEnum.Warn && m.date == date) == null ? 0 :
                    list.FirstOrDefault(m => m.level == ScmLogLevelEnum.Warn && m.date == date)!.Count);
                error.Add(list.FirstOrDefault(m => m.level == ScmLogLevelEnum.Error && m.date == date) == null ? 0 :
                    list.FirstOrDefault(m => m.level == ScmLogLevelEnum.Error && m.date == date)!.Count);
                fatal.Add(list.FirstOrDefault(m => m.level == ScmLogLevelEnum.Fatal && m.date == date) == null ? 0 :
                    list.FirstOrDefault(m => m.level == ScmLogLevelEnum.Fatal && m.date == date)!.Count);
            }
            res.Count.Add(debug);
            res.Count.Add(info);
            res.Count.Add(error);
            res.Count.Add(warn);
            res.Count.Add(fatal);
            return res;
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<LogFeDto> GetAsync(long id)
        {
            var model = await _thisRepository.GetByIdAsync(id);
            return model.Adapt<LogFeDto>();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(LogFeDto model)
        {
            return await _thisRepository.InsertAsync(model.Adapt<LogFeDao>());
        }

        /// <summary>
        /// 批量上报日志
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> ReportAsync(RecordRequest request)
        {
            if (request == null || request.logs == null || request.logs.Count() < 1)
            {
                return true;
            }

            var daoList = new List<LogFeDao>();
            foreach (var log in request.logs)
            {
                var dao = log.Adapt<LogFeDao>();
                dao.PrepareCreate(UserDto.SYS_ID);
                daoList.Add(dao);
            }
            return await _thisRepository.InsertRangeAsync(daoList);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(LogFeDto model)
        {
            return await _thisRepository.UpdateAsync(model.Adapt<LogFeDao>());
        }

        /// <summary>
        /// 删除,支持多个
        /// </summary>
        /// <param name="ids">逗号分隔</param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> DeleteAsync(string ids)
        {
            return await _thisRepository.DeleteAsync(m => ids.ToListLong().Contains(m.id));
        }

        /// <summary>
        /// 清空日志
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        public async Task<bool> ClearAsync()
        {
            return await _thisRepository.DeleteAsync(m => true);
        }
    }
}
