using Com.Scm.Dsa;
using Com.Scm.Log.User.Dvo;
using Com.Scm.Service;
using Com.Scm.Token;
using Com.Scm.Ur;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Log.User
{
    /// <summary>
    /// 登录日志
    /// </summary>
    [ApiExplorerSettings(GroupName = "log")]
    public class ScmLogUserService : ApiService
    {
        private readonly SugarRepository<LogUserDao> _thisRepository;
        private readonly ScmContextHolder _Holder;
        private readonly IDicService _DicService;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        /// <param name="userService"></param>
        /// <returns></returns>
        public ScmLogUserService(SugarRepository<LogUserDao> thisRepository, ScmContextHolder holder, IDicService dicService)
        {
            _thisRepository = thisRepository;
            _Holder = holder;
            _DicService = dicService;
        }

        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<LogUserDvo>> GetPagesAsync(SearchRequest request)
        {
            var userId = _Holder.GetToken().user_id;

            var date = DateTime.Now;
            if (!TextUtils.IsDate(request.datef ?? ""))
            {
                date = date.AddDays(-7);
            }
            else
            {
                date = DateTime.Parse(request.datef);
            }
            var datef = TimeUtils.GetUnixTime(date);

            var datet = 0L;
            if (TextUtils.IsDate(request.datet ?? ""))
            {
                date = DateTime.Parse(request.datet);
                datet = TimeUtils.GetUnixTime(date);
            }

            var result = await _thisRepository.AsQueryable()
                .Where(a => a.user_id == userId)
                .Where(a => a.time >= datef)
                .WhereIF(datet > 0, a => a.time < datet)
                .OrderBy(a => a.id, SqlSugar.OrderByType.Desc)
                .Select<LogUserDvo>()
                .ToPageAsync(request.page, request.limit);

            await Prepare(result.Items);

            return result;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<List<LogUserDvo>> GetListAsync(SearchRequest request)
        {
            var userId = _Holder.GetToken().user_id;

            var date = DateTime.Now;
            if (!TextUtils.IsDate(request.datef))
            {
                date = date.AddDays(-7);
            }
            else
            {
                date = DateTime.Parse(request.datef);
            }
            var datef = TimeUtils.GetUnixTime(date);

            var datet = 0L;
            if (TextUtils.IsDate(request.datet))
            {
                date = DateTime.Parse(request.datet);
                datet = TimeUtils.GetUnixTime(date);
            }

            var result = await _thisRepository.AsQueryable()
                .Where(a => a.user_id == userId)
                .Where(a => a.time >= datef)
                .WhereIF(datet > 0, a => a.time < datet)
                .OrderBy(a => a.id, SqlSugar.OrderByType.Desc)
                .Select<LogUserDvo>()
                .ToListAsync();

            await Prepare(result);

            return result;
        }

        private async Task Prepare(List<LogUserDvo> list)
        {
            var clientDic = await _DicService.GetDicAsync("client_type");
            var modeDic = await _DicService.GetDicAsync("login_mode");
            foreach (var item in list)
            {
                item.client_names = clientDic.GetDetail((int)item.client)?.namec;
                item.mode_names = modeDic.GetDetail((int)item.mode)?.namec;
            }
        }

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<UserOAuthDto> GetAsync(long id)
        {
            var model = await _thisRepository.GetByIdAsync(id);
            return model.Adapt<UserOAuthDto>();
        }
    }
}