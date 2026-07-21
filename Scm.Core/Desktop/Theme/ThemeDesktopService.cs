using Com.Scm.Dsa;
using Com.Scm.Service;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Desktop.Theme
{
    [AllowAnonymous]
    [ApiExplorerSettings(GroupName = "v1")]
    public class ThemeDesktopService : ApiService
    {
        private readonly SugarRepository<ThemeDesktopDao> _thisRepository;
        private readonly ICfgService _CfgServive;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="thisRepository"></param>
        /// <param name="resHolder"></param>
        /// <returns></returns>
        public ThemeDesktopService(SugarRepository<ThemeDesktopDao> thisRepository, ICfgService cfgService)
        {
            _thisRepository = thisRepository;
            _CfgServive = cfgService;
        }

        [HttpGet("{id}")]
        public async Task<ThemeDesktopDvo> GetAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Select<ThemeDesktopDvo>()
                .FirstAsync(m => m.id == id);
        }


        [HttpGet("{id}")]
        public async Task<ThemeDesktopDvo> GetUserAsync()
        {
            var cfg = await _CfgServive.GetConfigAsync("desktop_theme");
            if (cfg == null)
            {
                return null;
            }

            var val = cfg.value;
            if (!ScmUtils.IsValidId(val))
            {
                return null;
            }

            var id = long.Parse(val);
            return await _thisRepository
                .AsQueryable()
                .Select<ThemeDesktopDvo>()
                .FirstAsync(m => m.id == id);
        }

        /// <summary>
        /// 读取预览列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<List<ThemeDesktopDvo>> GetListAsync(long id)
        {
            return await _thisRepository
                .AsQueryable()
                .Where(a => a.style_id == id)
                .OrderBy(a => a.od)
                .Select<ThemeDesktopDvo>()
                .ToListAsync();
        }
    }
}
