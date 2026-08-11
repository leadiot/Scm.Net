using Com.Scm.Cfg;
using Com.Scm.Config;
using Com.Scm.Dev;
using Com.Scm.Enums;
using Com.Scm.Operator.Dvo;
using Com.Scm.Service;
using Com.Scm.Sys.Menu;
using Com.Scm.Token;
using Com.Scm.Ur;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Menu
{
    /// <summary>
    /// 
    /// </summary>
    [ApiExplorerSettings(GroupName = "scm")]
    public class MenuService : ApiService
    {
        private IJwtTokenHolder _jwtHolder;
        private I18nHolder _i18NHolder;

        public MenuService(ISqlSugarClient sqlClient, EnvConfig envConfig,
            Cache.ICacheService cacheService,
            IJwtTokenHolder jwtHolder, I18nHolder i18nHolder)
        {
            _SqlClient = sqlClient;
            _EnvConfig = envConfig;
            _CacheService = cacheService;
            _jwtHolder = jwtHolder;
            _i18NHolder = i18nHolder;
        }

        #region 读取菜单
        /// <summary>
        /// 根据登录人ID查询权限菜单[SCUI]
        /// </summary>
        /// <param name="client">客户端</param>
        /// <param name="lang">语言：形如zh-CN、en等。</param>
        /// <returns></returns>
        public async Task<List<MenuDto>> GetAuthorityMenuAsync(ScmClientTypeEnum client, ScmLayoutEnum layout, string lang)
        {
            var user = _jwtHolder.GetToken();

            if (string.IsNullOrWhiteSpace(lang))
            {
                lang = _EnvConfig.DefaultCulture;
            }

            //根据用户查询角色ID
            var userDao = await _SqlClient.Queryable<UserDao>()
                .FirstAsync(m => m.id == user.user_id && m.row_status == ScmRowStatusEnum.Enabled);

            var userRoleDao = await _SqlClient.Queryable<UserRoleDao>()
                .Where(a => a.user_id == user.user_id && a.row_status == ScmRowStatusEnum.Enabled)
                .ToListAsync();
            var userRoleIds = userRoleDao.Select(m => m.role_id).ToList();

            //根据角色查询授权的菜单Id集合
            var roleAuthList = await _SqlClient.Queryable<RoleAuthDao>()
                .Where(a => userRoleIds.Contains(a.role_id) && a.row_status == ScmRowStatusEnum.Enabled)
                .ToListAsync();
            roleAuthList = roleAuthList.DistinctBy(m => m.auth_id).ToList();

            #region 保存授权api
            //var apiList = new List<SysMenuApiUrl>();
            //foreach (var item in roleAuthList)
            //{
            //    apiList.AddRange(item.api);
            //}
            _CacheService.SetCache(KeyUtils.AUTHORIZZATIONAPI + ":" + userDao.id, "");
            #endregion

            //查询菜单集合
            var menuIds = roleAuthList.Select(m => m.auth_id).ToList();

            //根据菜单ID查询菜单详细
            var menuList = await _SqlClient.Queryable<ScmDevMenuDao>()
                .Where(a => menuIds.Contains(a.id) && a.row_status == ScmRowStatusEnum.Enabled)
                .WhereIF(client != ScmClientTypeEnum.None, a => a.client == client)
                .WhereIF(layout != ScmLayoutEnum.None, a => a.layout == layout)
                .OrderBy(a => a.od)
                .Select<MenuDto>()
                .ToListAsync();

            _i18NHolder.Translate(menuList, lang);

            // 查询用户常用菜单
            await ListFavMenu(menuList, user.user_id);

            //return RecursiveModuleSc(menuList, 0);
            return menuList;
        }

        private async Task ListFavMenu(List<MenuDto> menuList, long userId)
        {
            var favMenuDao = menuList.Find(a => a.id == MenuDto.FAV_ID);
            if (favMenuDao == null)
            {
                return;
            }

            var favMenuList = await _SqlClient.Queryable<CfgMenuDao>()
                .Where(a => a.user_id == userId && a.row_status == ScmRowStatusEnum.Enabled)
                .ToListAsync();

            foreach (var favMenu in favMenuList)
            {
                var menuDto = menuList.Find(a => a.id == favMenu.menu_id);
                if (menuDto == null)
                {
                    continue;
                }

                var favDao = menuDto.Adapt<MenuDto>();
                favDao.id = favMenu.id;
                favDao.pid = favMenuDao.id;
                menuList.Add(favDao);
            }
        }

        /// <summary>
        /// 递归模块列表
        /// </summary>
        /// <param name="menuList"></param>
        /// <param name="pId"></param>
        /// <returns></returns>
        private List<AuthorityDvo> RecursiveModuleSc(List<ScmDevMenuDao> menuList, long pId)
        {
            var result = new List<AuthorityDvo>();
            foreach (var item in menuList.Where(m => m.pid == pId).OrderBy(m => m.od))
            {
                var recursiveList = RecursiveModuleSc(menuList, item.id);
                result.Add(new AuthorityDvo()
                {
                    id = item.id,
                    path = item.uri,
                    name = item.codec,
                    component = item.view,
                    meta = new AuthorityMeta()
                    {
                        //id = item.id,
                        title = item.namec,
                        icon = item.icon,
                        type = item.types.ToKey().ToLower(),
                        hidden = !item.visible,
                        fullpage = item.fullpage ? true : null,
                        keep_alive = item.keep_alive,
                        affix = item.codec == "dashboard"
                    },
                    children = recursiveList
                });
            }

            return result;
        }
        #endregion
    }
}
