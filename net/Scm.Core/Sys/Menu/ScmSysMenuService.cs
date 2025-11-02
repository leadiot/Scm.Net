using Com.Scm.Dsa;
using Com.Scm.Dvo;
using Com.Scm.Enums;
using Com.Scm.Jwt;
using Com.Scm.Sys.Menu.Dvo;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Sys.Menu
{
    /// <summary>
    /// 服务接口
    /// </summary>
    [ApiExplorerSettings(GroupName = "Sys")]
    public class ScmSysMenuService : IApiService
    {
        private readonly SugarRepository<MenuDao> _thisRepository;
        private readonly JwtContextHolder _jwtContextHolder;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public ScmSysMenuService(SugarRepository<MenuDao> thisRepository, JwtContextHolder jwtContextHolder)
        {
            _thisRepository = thisRepository;
            _jwtContextHolder = jwtContextHolder;
        }

        /// <summary>
        /// 查询所有——分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<ScmSearchPageResponse<SysMenuDvo>> GetPagesAsync(ScmSearchPageRequest param)
        {
            var query = await _thisRepository.AsQueryable()
                .WhereIF(!string.IsNullOrEmpty(param.key), m => m.namec.Contains(param.key))
                .Select<SysMenuDvo>()
                .ToPageAsync(param.page, param.limit);
            return query;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public async Task<List<SysMenuDvo>> GetListAsync(ScmSearchPageRequest param)
        {
            var token = _jwtContextHolder.GetToken();

            var list = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
                .WhereIF(!string.IsNullOrEmpty(param.key), m => m.namec.Contains(param.key))
                .OrderBy(a => a.od)
                .Select<SysMenuDvo>()
                .ToListAsync();
            return list;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<List<ResOptionDvo>> OptionAsync()
        {
            return await _thisRepository.AsQueryable()
                .OrderBy(m => m.od)
                .Select(a => new ResOptionDvo { id = a.id, label = a.namec, value = a.id })
                .ToListAsync();
        }
    }
}