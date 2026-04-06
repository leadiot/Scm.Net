using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Filters;
using Com.Scm.Log.Api.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Log.Api
{
    /// <summary>
    /// 操作日志
    /// </summary>
    [ApiExplorerSettings(GroupName = "Log"), NoAuditLog]
    public class ScmLogApiService : IApiService
    {
        private readonly SugarRepository<LogApiDao> _thisRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public ScmLogApiService(SugarRepository<LogApiDao> thisRepository)
        {
            _thisRepository = thisRepository;
        }

        /// <summary>
        /// 查询所有——分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ScmSearchPageResponse<ScmLogApiDvo>> GetPagesAsync(SearchRequest param)
        {
            long time1 = 0;
            long time2 = 0;
            if (!string.IsNullOrEmpty(param.times))
            {
                var (btime, etime) = TimeUtils.Splitting(param.times);
                time1 = TimeUtils.GetUnixTime(DateTime.Parse(btime));
                time2 = TimeUtils.GetUnixTime(DateTime.Parse(etime));
            }

            var result = await _thisRepository.AsQueryable()
                .WhereIF(!string.IsNullOrEmpty(param.times), m => m.operate_time >= time1 && m.operate_time < time2)
                .WhereIF(param.Level != 0, m => m.level == (ScmLogLevelEnum)param.Level)
                .WhereIF(param.Type != 0, m => m.types == (ScmLogTypesEnum)param.Type)
                .OrderByDescending(m => m.id)
                .Select<ScmLogApiDvo>()
                .ToPageAsync(param.page, param.limit);
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
            var list = await _thisRepository.Context.Queryable<LogApiDao>()
                .Where(m => SqlFunc.Between(m.operate_date, btime, DateTime.Now))
                .GroupBy(m => new { m.operate_date, m.level })
                .Select(m => new
                {
                    m.operate_date,
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
                debug.Add(list.FirstOrDefault(m => m.level == ScmLogLevelEnum.Debug && m.operate_date == date) == null ? 0 :
                    list.FirstOrDefault(m => m.level == ScmLogLevelEnum.Debug && m.operate_date == date)!.Count);
                info.Add(list.FirstOrDefault(m => m.level == ScmLogLevelEnum.Info && m.operate_date == date) == null ? 0 :
                    list.FirstOrDefault(m => m.level == ScmLogLevelEnum.Info && m.operate_date == date)!.Count);
                warn.Add(list.FirstOrDefault(m => m.level == ScmLogLevelEnum.Warn && m.operate_date == date) == null ? 0 :
                    list.FirstOrDefault(m => m.level == ScmLogLevelEnum.Warn && m.operate_date == date)!.Count);
                error.Add(list.FirstOrDefault(m => m.level == ScmLogLevelEnum.Error && m.operate_date == date) == null ? 0 :
                    list.FirstOrDefault(m => m.level == ScmLogLevelEnum.Error && m.operate_date == date)!.Count);
                fatal.Add(list.FirstOrDefault(m => m.level == ScmLogLevelEnum.Fatal && m.operate_date == date) == null ? 0 :
                    list.FirstOrDefault(m => m.level == ScmLogLevelEnum.Fatal && m.operate_date == date)!.Count);
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
        public async Task<LogApiDto> GetAsync(long id)
        {
            var model = await _thisRepository.GetByIdAsync(id);
            return model.Adapt<LogApiDto>();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(LogApiDto model)
        {
            return await _thisRepository.InsertAsync(model.Adapt<LogApiDao>());
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(LogApiDto model)
        {
            return await _thisRepository.UpdateAsync(model.Adapt<LogApiDao>());
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