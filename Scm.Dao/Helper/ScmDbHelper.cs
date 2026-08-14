using Com.Scm.Dev;
using Com.Scm.Enums;
using Com.Scm.Sys;
using Com.Scm.Sys.Lang;
using Com.Scm.Sys.Menu;
using Com.Scm.Sys.Theme;
using Com.Scm.Ur;
using System.Reflection;

namespace Com.Scm.Helper
{
    public class ScmDbHelper : ScmModelDbHelper
    {
        private const string KEY = "Scm.Net";
        /// <summary>
        /// 数据版本
        /// </summary>
        private const int VER = 7;
        /// <summary>
        /// 发行日期
        /// </summary>
        private const string DATE = "2026-07-17";

        #region 版本信息
        protected override string GetKey()
        {
            return KEY;
        }

        protected override int GetVer()
        {
            return VER;
        }

        protected override string GetDate()
        {
            return DATE;
        }
        #endregion

        #region 数据库创建
        /// <summary>
        /// 数据库初始化
        /// </summary>
        /// <param name="verDao"></param>
        protected override void OnCreate(ScmVerDao verDao)
        {
            // 表格处理
            CreateTable(Assembly.GetExecutingAssembly());

            InitUid();

            InitLang();

            InitUom();

            InitApp();

            InitGroup();

            InitOrganize();

            InitPosition();

            InitTerminal();

            var userRootDao = InitUserRoot();

            var userAdminDao = InitUserAdmin();

            InitRoleAuth(userRootDao, userAdminDao);

            CreateTheme(1, "Default", "{\"page\":{\"backgroundImage\":\"url('/images/login/bg01.jpg')\",\"backgroundColor\":\"\",\"backgroundSize\":\"cover\",\"backgroundPosition\":\"center center\",\"backgroundRepeat\":\"no-repeat\"},\"mask\":{\"backgroundColor\":\"rgba(0,0,0,0.5)\"}}");

            var dmlFile = Path.Combine(_SqlDir, "scm-init.sql");
            ExecuteSql(dmlFile, verDao.ver);
        }

        private void InitUid()
        {
            CreateUid(ScmEnv.DEFAULT_ID, "scm", 0, "", "");
            CreateUid(1000000000000000002, "test", 0, "", "");
            CreateUid(1000000000000000011, "scm_sys_uom", 5, "UOM", "");
            CreateUid(1000000000000000012, "scm_sys_task", 12, "TASK", "");
            //CreateUid(1000000000000001011, "scm_ur_unit", 7, "U", "");
            CreateUid(1000000000000001012, "scm_ur_organize", 7, "O", "");
            CreateUid(1000000000000001013, "scm_ur_position", 7, "P", "");
            CreateUid(1000000000000001014, "scm_ur_group", 7, "G", "");
            CreateUid(1000000000000001015, "scm_ur_role", 7, "R", "");
            CreateUid(1000000000000001016, "scm_ur_user", 7, "X", "");
            CreateUid(1000000000000001017, "scm_ur_terminal", 9, "T", "");
            CreateUid(1000000000000001018, "scm_sys_table_header", 1, "", "");
        }

        private void InitLang()
        {
            var langDao = new LangDao();
            langDao.id = 1895368041135476736;
            langDao.code = "zh-cn";
            langDao.text = "简体中文";
            langDao.od = 1;
            SaveDao(langDao);

            langDao = new LangDao();
            langDao.id = 1895370805823541248;
            langDao.code = "en-us";
            langDao.text = "English(US)";
            langDao.od = 1;
            SaveDao(langDao);
        }

        private void InitUom()
        {
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
        }

        private void InitApp()
        {
            var appDao = CreateApp(ScmEnv.DEFAULT_ID, 0, 0, "", "", "");
            appDao.row_status = ScmRowStatusEnum.Normal;
            _SqlClient.Updateable(appDao).ExecuteCommand();

            CreateApp(1000000000000001001, 10, 1, "scm.net", "Scm.Net", "<p>一款基于 .NET 10.0 及 Vue 3.5 的企业级快速开发框架。</p><img src=\"/images/loginbg.svg\" alt=\"logo\"/>");
            CreateApp(1000000000000002001, 10, 2, "iam.net", "联合登录", "<p>简单、易用的多平台联合登录系统。</p><img src=\"/images/loginbg.svg\" alt=\"logo\"/>");
        }

        private void InitGroup()
        {
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
        }

        private void InitOrganize()
        {
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
        }

        private void InitPosition()
        {
            var positionDao = new PositionDao();
            positionDao.id = ScmEnv.DEFAULT_ID;
            positionDao.codes = "";
            positionDao.codec = "";
            positionDao.names = "";
            positionDao.namec = "";
            positionDao.row_delete = ScmRowDeleteEnum.No;
            SaveDao(positionDao);
            positionDao.row_status = ScmRowStatusEnum.Normal;
            _SqlClient.Updateable(positionDao).ExecuteCommand();
        }

        private void InitRoleAuth(UserDao userRootDao, UserDao userAdminDao)
        {
            var roleRootDao = new RoleDao();
            roleRootDao.id = ScmEnv.DEFAULT_ID;
            roleRootDao.codec = "admin";
            roleRootDao.names = "系统管理员";
            roleRootDao.namec = "系统管理员";
            roleRootDao.row_system = ScmRowSystemEnum.Yes;
            roleRootDao.row_delete = ScmRowDeleteEnum.No;
            SaveDao(roleRootDao);
            roleRootDao.row_status = ScmRowStatusEnum.Normal;
            _SqlClient.Updateable(roleRootDao).ExecuteCommand();

            var roleAdminDao = new RoleDao();
            roleAdminDao.id = ROLE_ADMIN_ID;
            roleAdminDao.codec = "admin";
            roleAdminDao.names = "系统管理员";
            roleAdminDao.namec = "系统管理员";
            roleAdminDao.row_system = ScmRowSystemEnum.Yes;
            roleAdminDao.row_delete = ScmRowDeleteEnum.No;
            SaveDao(roleAdminDao);

            var roleAdminList = new List<RoleAuthDao>();

            // Root
            var rootDao = CreateMenu(ScmEnv.DEFAULT_ID, "", "", 0, 0, 0, "/", "", "");
            rootDao.row_status = ScmRowStatusEnum.Normal;
            _SqlClient.Updateable(rootDao).ExecuteCommand();
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = rootDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            // 主页
            var homeDao = CreateMenu(1000000000000001000, "home", "主页", rootDao.id, 1, 1, "/home", "home", "sc-home-4-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = homeDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            // 工作台
            var homeConsoleDao = CreateMenu(1000000000000001101, "console", "工作台", homeDao.id, 2, 1, "/console", "home/console", "ms-dashboard", ScmLayoutEnum.Console);
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = homeConsoleDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var homeDesktopDao = CreateMenu(1000000000000001102, "desktop", "云桌面", homeDao.id, 2, 2, "/desktop", "home/desktop", "ms-computer", ScmLayoutEnum.Console);
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = homeDesktopDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var homeMonitorDao = CreateMenu(1000000000000001103, "monitor", "大屏幕", homeDao.id, 2, 3, "/monitor", "home/monitor", "ms-monitor", ScmLayoutEnum.Console);
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = homeMonitorDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            // 我的收藏
            var homeFavoritesDao = CreateMenu(1000000000000001200, "favorites", "我的收藏", homeDao.id, 2, 4, "/favorites", "home/console/favorites", "sc-heart-3-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = homeFavoritesDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            // 账户信息
            var homeProfilesDao = CreateMenu(1000000000000001300, "profiles", "账户信息", homeDao.id, 2, 5, "/profiles", "home/console/profiles", "sc-coffee-cup-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = homeProfilesDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            // 机构信息
            var homeUnitDao = CreateMenu(1000000000000001310, "profiles-unit", "机构信息", homeProfilesDao.id, 3, 1, "/profiles/unitCenter", "home/console/unitinfo", "sc-settings-3-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = homeUnitDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var homeUserDao = CreateMenu(1000000000000001320, "profiles-user", "个人信息", homeProfilesDao.id, 3, 2, "/profiles/usercenter", "home/console/userinfo", "sc-user-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = homeUserDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var homeOAuthDao = CreateMenu(1000000000000001330, "profiles-oauth", "联合登录", homeProfilesDao.id, 3, 3, "/profiles/oauth", "home/console/oauth", "sc-bubble-chart-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = homeOAuthDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var homeOtpDao = CreateMenu(1000000000000001340, "profiles-otp", "凭证登录", homeProfilesDao.id, 3, 4, "/profiles/otp", "home/console/otp", "sc-bubble-chart-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = homeOtpDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            // 我的终端
            var homeTerminalDao = CreateMenu(1000000000000001400, "terminal", "我的终端", homeDao.id, 2, 6, "/terminal", "scm/ur/terminal", "sc-device-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = homeTerminalDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            // 下载中心
            var homeDownloadDao = CreateMenu(1000000000000001500, "download", "下载中心", homeDao.id, 2, 7, "/download", "scm/down", "sc-device-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = homeDownloadDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            // 我的反馈
            var homeFeedbackDao = CreateMenu(1000000000000001600, "feedback", "我的反馈", homeDao.id, 2, 8, "/feedback", "scm/sys/feedback", "sc-chat-quote-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = homeFeedbackDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            // 设置
            var admDao = CreateMenu(1000000000000003000, "setting", "设置", rootDao.id, 1, 3, "/setting", "", "sc-settings-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = admDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            // 研发管理
            var admDevDao = CreateMenu(1000000000000003100, "setting-dev", "研发管理", admDao.id, 2, 1, "/setting/dev", "dev", "sc-bug-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = admDevDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var admDevMenuDao = CreateMenu(1000000000000003110, "setting-dev-menu", "资源管理", admDevDao.id, 3, 1, "/setting/dev/menu", "scm/dev/menu", "sc-menu-fill");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = admDevMenuDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var admDevAppDao = CreateMenu(1000000000000003120, "setting-dev-app", "应用管理", admDevDao.id, 3, 2, "/setting/dev/app", "scm/dev/app", "sc-apple");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = admDevAppDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var admDevVerDao = CreateMenu(1000000000000003130, "setting-dev-version", "版本管理", admDevDao.id, 3, 3, "/setting/dev/version", "scm/dev/version", "sc-file-text-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = admDevVerDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var admDevUidDao = CreateMenu(1000000000000003140, "setting-dev-_uid", "编码管理", admDevDao.id, 3, 4, "/setting/dev/uid", "scm/dev/uid", "sc-list-ordered");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = admDevUidDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var admDevGenDao = CreateMenu(1000000000000003150, "setting-dev-gen", "代码生成", admDevDao.id, 3, 5, "/setting/dev/generate", "scm/dev/generate", "sc-code-fill");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = admDevGenDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //var menuDevDbaDao = CreateMenu(1000000000000003160, "setting-dev-db", "数据库管理", menuDevDao.id, 3, 6, "/setting/dev/db", "scm/dev/db", "sc-coin-line");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDevDbaDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //var menuDevSqlDao = CreateMenu(1000000000000003170, "setting-dev-sql", "数据库脚本", menuDevDao.id, 3, 7, "/setting/dev/sql", "scm/dev/sql", "sc-file-paper-2-line");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDevSqlDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //var menuDevQtzDao = CreateMenu(1000000000000003180, "setting-dev-quartz", "后台任务", menuDevDao.id, 3, 8, "/setting/sys/quartz", "scm/sys/quartz", "sc-drag-move-line");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDevQtzDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            // 全局配置
            var admCfgDao = CreateMenu(1000000000000003200, "setting-adm", "全局配置", admDao.id, 2, 2, "/setting/adm", "scm/cfg", "sc-settings-3-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = admCfgDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var admCfgAdmDao = CreateMenu(1000000000000003210, "setting-adm-cfg", "参数配置", admCfgDao.id, 3, 1, "/setting/adm/cfg", "scm/adm/cfg", "sc-file-text-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = admCfgAdmDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var admCfgDicDao = CreateMenu(1000000000000003220, "setting-adm-dic", "数据字典", admCfgDao.id, 3, 2, "/setting/adm/dic", "scm/adm/dic", "sc-file-copy-2-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = admCfgDicDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var admCfgCatDao = CreateMenu(1000000000000003230, "setting-adm-cat", "分类管理", admCfgDao.id, 3, 3, "/setting/adm/cat", "scm/res/cat", "sc-layers-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = admCfgCatDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var admCfgUomDao = CreateMenu(1000000000000003240, "setting-adm-uom", "计量单位", admCfgDao.id, 3, 4, "/setting/adm/uom", "scm/sys/uom", "sc-signpost-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = admCfgUomDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var admCfgSecDao = CreateMenu(1000000000000003250, "setting-adm-safety", "安全设置", admCfgDao.id, 3, 5, "/setting/adm/safety", "scm/adm/safety", "sc-verified-badge-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = admCfgSecDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            // 权限管理
            var admUrDao = CreateMenu(1000000000000003300, "ur", "权限管理", admDao.id, 2, 3, "/scm/ur", "scm/ur", "sc-user-settings-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = admUrDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var admUrOrganizeDao = CreateMenu(1000000000000003310, "scm-ur-organize", "组织管理", admUrDao.id, 3, 1, "/scm/ur/organize", "scm/ur/organize", "sc-company-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = admUrOrganizeDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var admUrPositionDao = CreateMenu(1000000000000003320, "scm-ur-position", "岗位管理", admUrDao.id, 3, 2, "/scm/ur/position", "scm/ur/position", "sc-place");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = admUrPositionDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var admUrGroupDao = CreateMenu(1000000000000003330, "scm-ur-group", "群组管理", admUrDao.id, 3, 3, "/scm/ur/group", "scm/ur/group", "sc-user-2-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = admUrGroupDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var admUrRoleDao = CreateMenu(1000000000000003340, "scm-ur-role", "角色管理", admUrDao.id, 3, 4, "/scm/ur/role", "scm/ur/role", "sc-contacts-book-upload-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = admUrRoleDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var admUrAuthDao = CreateMenu(1000000000000003350, "scm-ur-roleauth", "角色权限", admUrDao.id, 3, 5, "/scm/ur/roleauth", "scm/ur/roleauth", "sc-verified-badge-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = admUrAuthDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var admUrAuthCDao = CreateMenu(1000000000000003360, "scm-ur-roleconflict", "角色互斥", admUrDao.id, 3, 6, "/scm/ur/roleconflict", "scm/ur/roleconflict", "sc-cherry");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = admUrAuthCDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var admUrUserDao = CreateMenu(1000000000000003370, "scm-ur-user", "用户管理", admUrDao.id, 3, 7, "/scm/ur/user", "scm/ur/user", "sc-user-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = admUrUserDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            // 关于
            var aboutDao = CreateMenu(1000000000000004000, "about", "关于", rootDao.id, 1, 999, "/about", "about", "sc-info");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = aboutDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var aboutSiteDao = CreateMenu(1000000000000004100, "about-site", "关于网站", aboutDao.id, 4, 1, "/about/app/site/scm.net", "about/app", "sc-global-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = aboutSiteDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var aboutAuthorDao = CreateMenu(1000000000000004200, "about-author", "关于作者", aboutDao.id, 4, 2, "/about/app/author/scm.net", "about/app", "sc-user-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = aboutAuthorDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var aboutContactDao = CreateMenu(1000000000000004300, "about-contact", "联系作者", aboutDao.id, 4, 3, "/about/app/contact/scm.net", "about/app", "sc-postcard");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = aboutContactDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var aboutHistoryDao = CreateMenu(1000000000000004400, "about-history", "更新历史", aboutDao.id, 4, 4, "/about/ver/scm.net", "about/ver", "sc-file-text-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = aboutHistoryDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            _SqlClient.Insertable(roleAdminList).ExecuteCommand();

            // Root
            var userRoleDao = new UserRoleDao();
            userRoleDao.user_id = userRootDao.id;
            userRoleDao.role_id = roleRootDao.id;
            SaveDao(userRoleDao);

            // Admin
            userRoleDao = new UserRoleDao();
            userRoleDao.user_id = userAdminDao.id;
            userRoleDao.role_id = roleAdminDao.id;
            SaveDao(userRoleDao);
        }

        private void InitTerminal()
        {
            var terminalDao = new ScmUrTerminalDao
            {
                id = ScmEnv.DEFAULT_ID,
                types = ScmClientTypeEnum.None,
                codes = "",
                codec = "",
                names = "",
                namec = "",
                icon = "",
                pass = "",
                access_token = "",
                refresh_token = "",
                remark = ""
            };
            SaveDao(terminalDao);

            terminalDao.codes = "";
            terminalDao.row_status = ScmRowStatusEnum.Normal;
            _SqlClient.Updateable(terminalDao).ExecuteCommand();
        }

        private UserDao InitUserRoot()
        {
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
            userRootDao.row_status = ScmRowStatusEnum.Normal;
            SaveDao(userRootDao);
            userRootDao.codes = "X0000001";
            userRootDao.row_status = ScmRowStatusEnum.Normal;
            _SqlClient.Updateable(userRootDao).ExecuteCommand();

            return userRootDao;
        }

        private UserDao InitUserAdmin()
        {
            var userAdminDao = new UserDao();
            userAdminDao.id = ROLE_ADMIN_ID;
            userAdminDao.codes = "X0001030";
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
            userAdminDao.codes = "X0001030";
            _SqlClient.Updateable(userAdminDao).ExecuteCommand();

            return userAdminDao;
        }
        #endregion

        #region 数据库升级
        /// <summary>
        /// 数据库升级
        /// </summary>
        /// <returns></returns>
        protected override void OnUpgrade(ScmVerDao verDao)
        {
            // 版本较新，不执行DML
            if (verDao.ver >= VER)
            {
                return;
            }

            var dmlFile = Path.Combine(_SqlDir, "scm-upgrade.sql");
            ExecuteSql(dmlFile, verDao.ver);
        }
        #endregion

        #region 数据库清空
        /// <summary>
        /// 清空数据库
        /// </summary>
        /// <param name="baseDir"></param>
        /// <returns></returns>
        public override bool DropDb()
        {
            return DropTable(Assembly.GetExecutingAssembly());
        }
        #endregion

        #region 具体数据操作
        /// <summary>
        /// 保存应用
        /// </summary>
        /// <param name="id"></param>
        /// <param name="types"></param>
        /// <param name="od"></param>
        /// <param name="code"></param>
        /// <param name="name"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        protected ScmDevAppDao CreateApp(long id, int types, int od, string code, string name, string content)
        {
            var appDao = new ScmDevAppDao();
            appDao.id = id;
            appDao.types = types;
            appDao.od = od;
            appDao.code = code;
            appDao.name = name;
            appDao.content = content;
            SaveDao(appDao);

            return appDao;
        }

        /// <summary>
        /// 添加主题
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="theme"></param>
        /// <returns></returns>
        private ThemeDao CreateTheme(long id, string name, string theme)
        {
            var themeDao = new ThemeDao();
            themeDao.names = name;
            themeDao.theme = theme;
            SaveDao(themeDao);
            return themeDao;
        }

        /// <summary>
        /// 添加UID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="k"></param>
        /// <param name="l"></param>
        /// <param name="m"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        protected ScmDevUidDao CreateUid(long id, string k, int l, string m, string p)
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

        /// <summary>
        /// 添加菜单
        /// </summary>
        /// <param name="id"></param>
        /// <param name="codec"></param>
        /// <param name="namec"></param>
        /// <param name="pid"></param>
        /// <param name="layer"></param>
        /// <param name="od"></param>
        /// <param name="uri"></param>
        /// <param name="view"></param>
        /// <param name="icon"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        protected MenuDao CreateMenu(
            long id,
            string codec,
            string namec,
            long pid,
            int layer,
            int od,
            string uri,
            string view,
            string icon,
            ScmLayoutEnum layout = ScmLayoutEnum.Console,
            ScmRowStatusEnum status = ScmRowStatusEnum.Enabled)
        {
            var menuDao = new MenuDao();
            menuDao.id = id;
            menuDao.types = ScmMenuTypesEnum.Menu;
            menuDao.client = ScmClientTypeEnum.Web;
            menuDao.lang = "zh-cn";
            menuDao.i18n = "";
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
            menuDao.keep_alive = true;
            menuDao.layout = layout;
            menuDao.row_delete = ScmRowDeleteEnum.No;
            SaveDao(menuDao);
            return menuDao;
        }
        #endregion
    }
}
