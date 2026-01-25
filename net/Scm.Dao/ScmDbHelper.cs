using Com.Scm.Dao;
using Com.Scm.Dev;
using Com.Scm.Enums;
using Com.Scm.Sys;
using Com.Scm.Sys.Lang;
using Com.Scm.Sys.Menu;
using Com.Scm.Sys.Theme;
using Com.Scm.Ur;
using Com.Scm.Utils;
using SqlSugar;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Com.Scm
{
    public class ScmDbHelper
    {
        private ISqlSugarClient _SqlClient;

        public ScmDbHelper(ISqlSugarClient sqlClient)
        {
            _SqlClient = sqlClient;
        }

        public bool InitDb(string baseDir)
        {
            var key = "scmdb";

            var verDao = ReadVer(key);
            if (verDao == null)
            {
                verDao = new ScmVerDao();
                verDao.key = key;
                verDao.create_time = TimeUtils.GetUnixTime();
            }

            InitDdl();

            if (verDao.major == 0)
            {
                InitDml();
            }

            var ddlFile = Path.Combine(baseDir, "ddl.sql");
            ExecuteSql(ddlFile, verDao.major);

            var dmlFile = Path.Combine(baseDir, "dml.sql");
            ExecuteSql(dmlFile, verDao.major);

            SaveVer(verDao);
            return true;
        }

        private void ExecuteSql(string file, int major)
        {
            if (!File.Exists(file))
            {
                return;
            }

            var lines = File.ReadAllLines(file);
            var inComment = false;
            var needRun = false;
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                var sql = line.Trim();
                if (sql.StartsWith("/*"))
                {
                    inComment = true;
                }

                if (inComment)
                {
                    if (!needRun)
                    {
                        var ver = GetSqlVer(sql);
                        if (ver > major)
                        {
                            needRun = true;
                        }
                    }

                    if (sql.EndsWith("*/"))
                    {
                        inComment = false;
                    }

                    continue;
                }

                if (!needRun)
                {
                    return;
                }

                _SqlClient.Ado.ExecuteCommand(line);
            }
        }

        private static int GetSqlVer(string text)
        {
            var match = Regex.Match(text, @"[Vv]er[:]\s*(\d+)");
            if (!match.Success)
            {
                return 0;
            }
            if (match.Groups.Count < 2)
            {
                return 0;
            }
            var ver = match.Groups[1].Value;
            if (TextUtils.IsInteger(ver))
            {
                return int.Parse(ver);
            }

            return 0;
        }

        /// <summary>
        /// 数据库定义
        /// </summary>
        /// <param name="sqlClient"></param>
        private void InitDdl()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var scmDao = typeof(ScmDao);
            var daoType = assembly.GetTypes().Where(u => u.IsClass && !u.IsAbstract && !u.IsGenericType && u.Name.EndsWith("Dao")).ToList();
            var daoList = new List<Type>();
            foreach (var item in daoType.Where(s => !s.IsInterface))
            {
                if (CommonUtils.HasImplementedRawGeneric(item, scmDao))
                {
                    daoList.Add(item);
                }
            }
            _SqlClient.CodeFirst.InitTables(daoList.ToArray());
        }

        private ScmVerDao ReadVer(string key)
        {
            try
            {
                return _SqlClient.Queryable<ScmVerDao>().First(a => a.key == key);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void SaveVer(ScmVerDao verDao)
        {
            verDao.update_time = TimeUtils.GetUnixTime();
            verDao.major = ScmVerDao.VER_MAJOR;
            verDao.minor = ScmVerDao.VER_MINOR;
            verDao.patch = ScmVerDao.VER_PATCH;
            verDao.build = ScmVerDao.VER_BUILD;

            if (verDao.id == 0)
            {
                _SqlClient.Insertable(verDao).ExecuteCommand();
            }
            else
            {
                _SqlClient.Updateable(verDao).ExecuteCommand();
            }
        }

        private void InitDml()
        {
            var appDao = new ScmDevAppDao();
            appDao.id = ScmEnv.DEFAULT_ID;
            appDao.types = 0;
            appDao.od = 0;
            appDao.code = "scm.net";
            appDao.name = "Scm.Net";
            appDao.content = "<p>一款基于Vue3和.Net10.0技术框架、适用于中后台管理系统的快速开发框架。</p><img src=\"/img/loginbg.svg\" alt=\"logo\"/>";
            SaveDao(appDao);

            var langDao = new LangDao();
            langDao.code = "zh-cn";
            langDao.text = "简体中文";
            langDao.od = 1;
            SaveDao(langDao);

            langDao = new LangDao();
            langDao.code = "en-us";
            langDao.text = "English(US)";
            langDao.od = 1;
            SaveDao(langDao);

            // Root
            var menuRootDao = CreateMenu(ScmEnv.DEFAULT_ID, "", "", 0, 0, 0, "/", "", "");
            menuRootDao.row_status = ScmRowStatusEnum.Normal;
            _SqlClient.Updateable(menuRootDao).ExecuteCommand();

            // 主页
            var menuHomeDao = CreateMenu(1000000000000001000, "home", "主页", menuRootDao.id, 1, 1, "/home", "home", "sc-home-4-line");
            // 工作台
            var menuDashboardDao = CreateMenu(1000000000000001100, "dashboard", "工作台", menuHomeDao.id, 2, 1, "/dashboard", "home", "sc-menu-line");
            // 我的收藏
            var menuFavoritesDao = CreateMenu(1000000000000001200, "favorites", "我的收藏", menuHomeDao.id, 2, 2, "/favorites", "", "sc-heart-3-line");
            // 账户信息
            var menuProfilesDao = CreateMenu(1000000000000001300, "profiles", "账户信息", menuHomeDao.id, 2, 3, "/profiles", "", "sc-coffee-cup-line");
            // 机构信息
            var menuHomeUnitDao = CreateMenu(1000000000000001310, "home_unit", "机构信息", menuProfilesDao.id, 3, 1, "/home/unitCenter", "home/unitinfo", "sc-settings-3-line");
            var menuHomeUserDao = CreateMenu(1000000000000001320, "home_user", "个人信息", menuProfilesDao.id, 3, 2, "/home/usercenter", "home/userinfo", "sc-user-line");
            var menuHomeOAuthDao = CreateMenu(1000000000000001330, "scm-oauth", "联合登录", menuProfilesDao.id, 3, 3, "/home/oauth", "home/oauth", "sc-bubble-chart-line");
            var menuHomeOtpDao = CreateMenu(1000000000000001340, "scm-otp", "凭证登录", menuProfilesDao.id, 3, 4, "/scm/otp", "home/otp", "sc-bubble-chart-line");
            // 我的反馈
            var menuFeedbackDao = CreateMenu(1000000000000001400, "feedback", "我的反馈", menuHomeDao.id, 2, 4, "/scm/feedback", "scm/sys/feedback", "sc-chat-quote-line");
            // 我的终端
            var menuTerminalDao = CreateMenu(1000000000000001500, "terminal", "我的终端", menuHomeDao.id, 2, 5, "/scm/terminal", "scm/ur/terminal", "sc-device-line");
            // 下载中心
            var menuDownloadDao = CreateMenu(1000000000000001600, "download", "下载中心", menuHomeDao.id, 2, 6, "/scm/download", "scm/down", "sc-device-line");

            // 设置
            var menuSettingsDao = CreateMenu(1000000000000003000, "setting", "设置", menuRootDao.id, 1, 3, "/setting", "", "mi-settings_applications");
            // 研发管理
            var menuDevDao = CreateMenu(1000000000000003100, "setting", "研发管理", menuSettingsDao.id, 2, 1, "/dev", "dev", "sc-bug-line");
            var menuDevMenuDao = CreateMenu(1000000000000003110, "dev_menu", "资源管理", menuDevDao.id, 3, 1, "/scm/dev/menu", "scm/dev/menu", "sc-menu-fill");
            var menuDevAppDao = CreateMenu(1000000000000003120, "dev_app", "应用管理", menuDevDao.id, 3, 2, "/scm/dev/app", "scm/dev/app", "sc-apple");
            var menuDevVerDao = CreateMenu(1000000000000003130, "dev_version", "版本管理", menuDevDao.id, 3, 3, "/scm/dev/version", "scm/dev/version", "sc-file-text-line");
            var menuDevUidDao = CreateMenu(1000000000000003140, "dev_uid", "编码管理", menuDevDao.id, 3, 4, "/scm/dev/uid", "scm/dev/uid", "sc-list-ordered");
            var menuDevGenDao = CreateMenu(1000000000000003150, "dev_gen", "代码生成", menuDevDao.id, 3, 5, "/scm/dev/generate", "scm/dev/generate", "sc-code-fill");
            //var menuDevDbaDao = CreateMenu(1000000000000003160, "dev_db", "数据库管理", menuDevDao.id, 3, 6, "/scm/dev/db", "scm/dev/db", "sc-coin-line");
            //var menuDevSqlDao = CreateMenu(1000000000000003170, "dev_sql", "数据库脚本", menuDevDao.id, 3, 7, "/scm/dev/sql", "scm/dev/sql", "sc-file-paper-2-line");
            //var menuDevQtzDao = CreateMenu(1000000000000003180, "dev_quartz", "后台任务", menuDevDao.id, 3, 8, "/scm/sys/quartz", "scm/sys/quartz", "sc-drag-move-line");

            // 全局配置
            var menuCfgDao = CreateMenu(1000000000000003200, "scm_cfg", "全局配置", menuSettingsDao.id, 2, 2, "/scm/cfg", "scm/cfg", "sc-settings-line");
            var menuCfgAdmDao = CreateMenu(1000000000000003210, "scm-adm-cfg", "参数配置", menuCfgDao.id, 3, 1, "/scm/adm/cfg", "scm/adm/cfg", "sc-file-text-line");
            var menuCfgDicDao = CreateMenu(1000000000000003220, "scm-adm-dic", "数据字典", menuCfgDao.id, 3, 2, "/scm/adm/dic", "scm/adm/dic", "sc-file-copy-2-line");
            var menuCfgCatDao = CreateMenu(1000000000000003230, "scm-res-cat", "分类管理", menuCfgDao.id, 3, 3, "/scm/res/cat", "scm/res/cat", "sc-layers-line");
            var menuCfgUomDao = CreateMenu(1000000000000003240, "scm-sys-uom", "计量单位", menuCfgDao.id, 3, 4, "/scm/sys/uom", "scm/sys/uom", "sc-signpost-line");
            var menuCfgSecDao = CreateMenu(1000000000000003250, "scm-adm-safety", "安全设置", menuCfgDao.id, 3, 5, "/scm/adm/safety", "scm/adm/safety", "sc-verified-badge-line");

            // 权限管理
            var menuUrDao = CreateMenu(1000000000000003300, "scm-adm-safety", "权限管理", menuSettingsDao.id, 2, 3, "/scm/ur", "scm/ur", "sc-user-settings-line");
            var menuUrOrganizeDao = CreateMenu(1000000000000003310, "scm_ur_organize", "组织管理", menuUrDao.id, 3, 1, "/scm/ur/organize", "scm/ur/organize", "sc-company-line");
            var menuUrPositionDao = CreateMenu(1000000000000003320, "scm_ur_position", "岗位管理", menuUrDao.id, 3, 2, "/scm/ur/position", "scm/ur/position", "sc-place");
            var menuUrRoleDao = CreateMenu(1000000000000003330, "scm_ur_role", "角色管理", menuUrDao.id, 3, 3, "/scm/ur/role", "scm/ur/role", "sc-contacts-book-upload-line");
            var menuUrUserDao = CreateMenu(1000000000000003340, "scm_ur_user", "用户管理", menuUrDao.id, 3, 4, "/scm/ur/user", "scm/ur/user", "sc-user-line");
            var menuUrAuthDao = CreateMenu(1000000000000003350, "scm_ur_permission", "权限管理", menuUrDao.id, 3, 5, "/scm/ur/permission", "scm/ur/roleauth", "sc-verified-badge-line");
            var menuUrAuthCDao = CreateMenu(1000000000000003360, "scm_ur_roleconflict", "角色互斥", menuUrDao.id, 3, 6, "/scm/ur/roleconflict", "scm/ur/roleconflict", "sc-cherry");
            var menuUrGroupDao = CreateMenu(1000000000000003370, "scm_ur_group", "群组管理", menuUrDao.id, 3, 7, "/scm/ur/group", "scm/ur/group", "sc-user-2-line");

            // 关于
            var menuAboutDao = CreateMenu(1000000000000004000, "about", "关于", menuRootDao.id, 1, 999, "/about", "about", "sc-info");
            var menuAboutSiteDao = CreateMenu(1000000000000004100, "about_site", "关于网站", menuAboutDao.id, 4, 1, "/about/app/site/scm.net", "about/app", "sc-global-line");
            var menuAboutAuthorDao = CreateMenu(1000000000000004200, "about_author", "关于作者", menuAboutDao.id, 4, 2, "/about/app/author/scm.net", "about/app", "sc-user-line");
            var menuAboutContactDao = CreateMenu(1000000000000004300, "about_contact", "联系作者", menuAboutDao.id, 4, 3, "/about/app/contact/scm.net", "about/app", "sc-postcard");
            var menuAboutHistoryDao = CreateMenu(1000000000000004400, "about_history", "更新历史", menuAboutDao.id, 4, 4, "/about/ver/scm.net", "about/ver", "sc-file-text-line");

            CreateUid(ScmEnv.DEFAULT_ID, "", 0, "", "");
            CreateUid(1000000000000000011, "scm_sys_uom", 0, "", "");
            CreateUid(1000000000000000012, "scm_sys_task", 12, "TASK", "");
            CreateUid(1000000000000001001, "scm_ur_unit", 7, "U", "");
            CreateUid(1000000000000001002, "scm_ur_user", 7, "X", "");
            CreateUid(1000000000000001011, "scm_ur_group", 7, "G", "");
            CreateUid(1000000000000001012, "scm_ur_organize", 7, "O", "");
            CreateUid(1000000000000001013, "scm_ur_position", 7, "P", "");
            CreateUid(1000000000000001014, "scm_ur_role", 1, "", "");
            CreateUid(1000000000000001016, "scm_ur_terminal", 9, "T", "");
            CreateUid(1000000000000001015, "scm_sys_table_header", 1, "", "");

            var uomDao = new ScmSysUomDao();
            uomDao.id = ScmEnv.DEFAULT_ID;
            uomDao.types = ScmUomTypesEnum.None;
            uomDao.lang = "";
            uomDao.codes = "";
            uomDao.codec = "";
            uomDao.names = "";
            uomDao.namec = "";
            uomDao.basic_id = ScmEnv.DEFAULT_ID;
            uomDao.basic_qty = 1;
            uomDao.symbol = "";
            uomDao.refer_qty = 0;
            uomDao.basic_qty = 0;
            SaveDao(uomDao);
            uomDao.row_status = ScmRowStatusEnum.Normal;
            _SqlClient.Updateable(uomDao).ExecuteCommand();

            var groupDao = new GroupDao();
            groupDao.id = ScmEnv.DEFAULT_ID;
            groupDao.codes = "";
            groupDao.codec = "";
            groupDao.names = "";
            groupDao.namec = "";
            groupDao.pid = ScmEnv.DEFAULT_ID;
            SaveDao(groupDao);
            groupDao.row_status = ScmRowStatusEnum.Normal;
            _SqlClient.Updateable(groupDao).ExecuteCommand();

            var organizeDao = new OrganizeDao();
            organizeDao.id = ScmEnv.DEFAULT_ID;
            organizeDao.codes = "";
            organizeDao.codec = "";
            organizeDao.names = "";
            organizeDao.namec = "";
            organizeDao.pid = ScmEnv.DEFAULT_ID;
            organizeDao.row_delete = Enums.ScmRowDeleteEnum.No;
            SaveDao(organizeDao);
            organizeDao.row_status = ScmRowStatusEnum.Normal;
            _SqlClient.Updateable(organizeDao).ExecuteCommand();

            var positionDao = new PositionDao();
            positionDao.id = ScmEnv.DEFAULT_ID;
            positionDao.codes = "";
            positionDao.codec = "";
            positionDao.names = "";
            positionDao.namec = "";
            SaveDao(positionDao);
            positionDao.row_status = ScmRowStatusEnum.Normal;
            _SqlClient.Updateable(positionDao).ExecuteCommand();

            var roleRootDao = new RoleDao();
            roleRootDao.id = ScmEnv.DEFAULT_ID;
            roleRootDao.codec = "admin";
            roleRootDao.namec = "系统管理员";
            roleRootDao.row_system = ScmRowSystemEnum.Yes;
            roleRootDao.row_delete = ScmRowDeleteEnum.No;
            SaveDao(roleRootDao);
            roleRootDao.row_status = ScmRowStatusEnum.Normal;
            _SqlClient.Updateable(roleRootDao).ExecuteCommand();

            var roleAdminDao = new RoleDao();
            roleAdminDao.id = 1000000000000001030L;
            roleAdminDao.codec = "admin";
            roleAdminDao.namec = "系统管理员";
            roleAdminDao.row_system = ScmRowSystemEnum.Yes;
            roleAdminDao.row_delete = ScmRowDeleteEnum.No;
            SaveDao(roleAdminDao);

            var roleAdminList = new List<RoleAuthDao>();
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuRootDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuHomeDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDashboardDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuFavoritesDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuProfilesDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuHomeUnitDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuHomeUserDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuHomeOAuthDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuHomeOtpDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuFeedbackDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuTerminalDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDownloadDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuSettingsDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDevDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDevMenuDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDevAppDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDevVerDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDevUidDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDevGenDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDevDbaDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDevSqlDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDevQtzDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuCfgDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuCfgAdmDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuCfgDicDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuCfgCatDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuCfgUomDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuCfgSecDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuUrDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuUrOrganizeDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuUrPositionDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuUrRoleDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuUrUserDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuUrAuthDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuUrAuthCDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuUrGroupDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuAboutDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuAboutSiteDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuAboutAuthorDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuAboutContactDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuAboutHistoryDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            _SqlClient.Insertable(roleAdminList).ExecuteCommand();

            var terminalDao = new ScmUrTerminalDao();
            terminalDao.id = ScmEnv.DEFAULT_ID;
            terminalDao.types = ScmClientTypeEnum.None;
            terminalDao.codes = "";
            terminalDao.codec = "";
            terminalDao.names = "";
            terminalDao.namec = "";
            terminalDao.pass = "";
            terminalDao.access_token = "";
            terminalDao.refresh_token = "";
            SaveDao(terminalDao);
            terminalDao.row_status = ScmRowStatusEnum.Normal;
            _SqlClient.Updateable(terminalDao).ExecuteCommand();

            var userRootDao = new UserDao();
            userRootDao.id = ScmEnv.DEFAULT_ID;
            userRootDao.codes = "X0000001";
            userRootDao.codec = "(SYS)";
            userRootDao.names = "系统";
            userRootDao.namec = "系统";
            //userRootDao.user = "";
            userRootDao.pass = "02118dx6yf969eef6ecadfqf63c29a3a629280e686cf0c3f5d5a86aff3ca12020c923adc6c928nf2";
            userRootDao.avatar = "0.png";
            userRootDao.cellphone = "";
            userRootDao.telephone = "";
            userRootDao.email = "";
            userRootDao.sex = ScmSexEnum.None;
            userRootDao.data = ScmUserDataEnum.None;
            userRootDao.row_system = ScmRowSystemEnum.Yes;
            userRootDao.row_delete = ScmRowDeleteEnum.No;
            SaveDao(userRootDao);
            userRootDao.row_status = ScmRowStatusEnum.Normal;
            _SqlClient.Updateable(userRootDao).ExecuteCommand();

            var userAdminDao = new UserDao();
            userAdminDao.id = 1000000000000001030;
            userAdminDao.codes = "X0000010";
            userAdminDao.codec = "admin";
            userAdminDao.names = "系统管理员";
            userAdminDao.namec = "系统管理员";
            //userAdminDao.user = "";
            userAdminDao.pass = "17298d969eef6ecad3c29a3a6297bnl280e686cf0c3f5njs5d5a86aff3ca12020c923adc6c92j6km";
            userAdminDao.avatar = "0.png";
            userAdminDao.cellphone = "";
            userAdminDao.telephone = "";
            userAdminDao.email = "";
            userAdminDao.sex = ScmSexEnum.None;
            userAdminDao.data = ScmUserDataEnum.None;
            userAdminDao.row_system = ScmRowSystemEnum.Yes;
            userAdminDao.row_delete = ScmRowDeleteEnum.No;
            SaveDao(userAdminDao);

            // System
            //var userRoleDao = new UserRoleDao();
            //userRoleDao.user_id = userRootDao.id;
            //userRoleDao.role_id = roleRootDao.id;
            //SaveDao(userRoleDao);

            // Admin
            var userRoleDao = new UserRoleDao();
            userRoleDao.user_id = userAdminDao.id;
            userRoleDao.role_id = roleAdminDao.id;
            SaveDao(userRoleDao);

            CreateTheme(1, "Default", "{\"page\":{\"backgroundImage\":\"url('http://api.c-scm.net/data/bg/bg01.jpg')\",\"backgroundColor\":\"\",\"backgroundSize\":\"cover\",\"backgroundPosition\":\"center center\",\"backgroundRepeat\":\"no-repeat\"},\"mask\":{\"backgroundColor\":\"rgba(0,0,0,0.5)\"}}");
        }

        private MenuDao CreateMenu(
            long id,
            string codec,
            string namec,
            long pid,
            int layer,
            int od,
            string uri,
            string view,
            string icon,
            ScmRowStatusEnum status = ScmRowStatusEnum.Enabled)
        {
            var menuDao = new MenuDao();
            menuDao.id = id;
            menuDao.types = ScmMenuTypesEnum.Menu;
            menuDao.client = ScmClientTypeEnum.Web;
            menuDao.lang = "zh-cn";
            menuDao.codec = codec;
            menuDao.namec = namec;
            menuDao.pid = pid;
            menuDao.layer = layer;
            menuDao.od = od;
            menuDao.uri = uri;
            menuDao.view = view;
            menuDao.icon = icon;
            menuDao.visible = true;
            menuDao.enabled = true;
            menuDao.keepAlive = true;
            menuDao.row_delete = ScmRowDeleteEnum.No;
            SaveDao(menuDao);
            return menuDao;
        }

        private ScmDevUidDao CreateUid(long id, string k, int l, string m, string p)
        {
            var uidDao = new ScmDevUidDao();
            uidDao.id = id;
            uidDao.k = k;
            uidDao.v = 1;
            uidDao.c = 1;
            uidDao.b = 1;
            uidDao.l = l;
            uidDao.m = m;
            uidDao.p = p;
            SaveDao(uidDao);
            return uidDao;
        }

        private ThemeDao CreateTheme(long id, string name, string theme)
        {
            var themeDao = new ThemeDao();
            themeDao.names = name;
            themeDao.theme = theme;
            SaveDao(themeDao);
            return themeDao;
        }

        private void SaveDao<T>(T dao) where T : ScmDao, new()
        {
            var tmpDao = _SqlClient.Queryable<T>().First(a => a.id == dao.id);
            if (tmpDao != null)
            {
                tmpDao = dao.Adapt(tmpDao);
                tmpDao.PrepareUpdate(ScmEnv.DEFAULT_ID);
                _SqlClient.Updateable(tmpDao).ExecuteCommand();
                return;
            }

            dao.PrepareCreate(ScmEnv.DEFAULT_ID);
            _SqlClient.Insertable(dao).ExecuteCommand();
        }
    }
}
