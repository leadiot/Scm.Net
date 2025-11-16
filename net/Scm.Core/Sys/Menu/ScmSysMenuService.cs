using Com.Scm.Dsa;
using Com.Scm.Enums;
using Com.Scm.Sys.Menu.Dvo;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        public ScmSysMenuService(SugarRepository<MenuDao> thisRepository)
        {
            _thisRepository = thisRepository;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        public async Task<List<ScmSysMenuDvo>> GetListAsync(ScmSearchPageRequest param)
        {
            var list = await _thisRepository.AsQueryable()
                .Where(a => a.row_status == ScmRowStatusEnum.Enabled)
                .WhereIF(!string.IsNullOrEmpty(param.key), m => m.namec.Contains(param.key))
                .OrderBy(a => a.od)
                .Select<ScmSysMenuDvo>()
                .ToListAsync();
            return list;
        }
    }
}