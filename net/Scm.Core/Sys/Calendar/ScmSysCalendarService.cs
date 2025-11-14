using Com.Scm.Dsa;
using Com.Scm.Jwt;
using Com.Scm.Sys.Calendar.Dvo;
using Com.Scm.Ur;
using Com.Scm.Ur.User.Dvo;
using Com.Scm.Utils;

using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Sys.Calendar;

/// <summary>
/// 日程表服务接口
/// </summary>
[ApiExplorerSettings(GroupName = "Sys")]
public class ScmSysCalendarService : IApiService
{
    private readonly SugarRepository<CalendarDao> _thisRepository;
    private readonly JwtContextHolder _jwtContextHolder;
    private readonly ISqlSugarClient _Client;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="thisRepository"></param>
    /// <param name="jwtContextHolder"></param>
    /// <param name="client"></param>
    public ScmSysCalendarService(SugarRepository<CalendarDao> thisRepository, JwtContextHolder jwtContextHolder, ISqlSugarClient client)
    {
        _thisRepository = thisRepository;
        _jwtContextHolder = jwtContextHolder;
        _Client = client;
    }

    /// <summary>
    /// 查询所有——分页
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<List<ScmSysCalendarDvo>> GetListAsync(SearchRequest request)
    {
        var userId = request.user_id;
        if (!ScmUtils.IsValidId(userId))
        {
            userId = _jwtContextHolder.GetToken().user_id;
        }

        var timef = 0L;
        var timet = 0L;
        if (request.date != null)
        {
            var date = request.date.Value;
            timef = TimeUtils.GetUnixTime(date);
            timet = TimeUtils.GetUnixTime(date.AddDays(1));
        }
        var query = await _thisRepository.AsQueryable()
            .Where(a => SqlFunc.Subqueryable<CalendarUserDao>().Where(b => a.id == b.calendar_id && b.user_id == userId && b.row_status == Enums.ScmRowStatusEnum.Enabled).Any())
            .WhereIF(request.types != 0, a => a.types == request.types)
            .WhereIF(request.level != 0, a => a.level == request.level)
            .WhereIF(request.date != null, a => a.start_time >= timef && a.start_time < timet)
            .OrderBy(a => a.id, OrderByType.Asc)
            .Select<ScmSysCalendarDvo>()
            .ToListAsync();

        return query;
    }

    /// <summary>
    /// 根据主键查询
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ScmSysCalendarDvo> GetAsync(long id)
    {
        var model = await _thisRepository
            .AsQueryable()
            .FirstAsync(a => a.id == id);

        var dvo = model.Adapt<ScmSysCalendarDvo>();
        if (dvo != null)
        {
            dvo.users = await _Client.Queryable<CalendarUserDao, UserDao>((a, b) => a.user_id == b.id)
                .Where(a => a.calendar_id == id)
                .Select((a, b) => new SimpleUserDvo { id = a.user_id, codes = b.codes, codec = b.codec, names = b.names, namec = b.namec, phone = b.cellphone, email = b.email, avatar = b.avatar })
                .ToListAsync();
        }

        return dvo;
    }

    /// <summary>
    /// 添加
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<bool> AddAsync(CalendarDto request)
    {
        var dao = new CalendarDao();
        dao = CommonUtils.Adapt(request, dao);
        var result = await _thisRepository.InsertAsync(dao);

        if (request.users != null)
        {
            var list = new List<CalendarUserDao>();
            foreach (var user in request.users)
            {
                list.Add(new CalendarUserDao { calendar_id = dao.id, user_id = user });
            }
            await _thisRepository.Change<CalendarUserDao>().InsertRangeAsync(list);
        }
        return result;
    }

    /// <summary>
    /// 修改
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<bool> UpdateAsync(CalendarDto model)
    {
        var dao = await _thisRepository.GetByIdAsync(model.id);
        if (dao == null)
        {
            return false;
        }

        dao = CommonUtils.Adapt(model, dao);
        var calendarUserClient = _thisRepository.Change<CalendarUserDao>();
        await calendarUserClient.DeleteAsync(a => a.calendar_id == dao.id);
        if (model.users != null)
        {
            var list = new List<CalendarUserDao>();
            foreach (var user in model.users)
            {
                list.Add(new CalendarUserDao { calendar_id = dao.id, user_id = user });
            }
            await _thisRepository.Change<CalendarUserDao>().InsertRangeAsync(list);
        }

        return await _thisRepository.UpdateAsync(dao);
    }

    /// <summary>
    /// 删除,支持批量
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync([FromBody] List<long> ids)
    {
        return await _thisRepository.DeleteAsync(m => ids.Contains(m.id));
    }
}
