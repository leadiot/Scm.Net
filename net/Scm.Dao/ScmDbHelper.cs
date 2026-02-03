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
using System.Reflection;
using System.Text.RegularExpressions;

namespace Com.Scm
{
    public class ScmDbHelper : IModelHelper
    {
        protected ISqlSugarClient _SqlClient;
        protected string _BaseDir;

        public void Init(ISqlSugarClient sqlClient, string baseDir)
        {
            _SqlClient = sqlClient;
            _BaseDir = baseDir;
        }

        /// <summary>
        /// 清空数据库
        /// </summary>
        /// <param name="baseDir"></param>
        /// <returns></returns>
        public virtual bool DropDb()
        {
            return DropTable(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// 重建数据库
        /// </summary>
        /// <param name="baseDir"></param>
        /// <returns></returns>
        public virtual bool InitDb()
        {
            var key = "scm";

            var verDao = ReadDbVer(key);
            if (verDao == null)
            {
                verDao = new ScmVerDao();
                verDao.key = key;
                verDao.create_time = TimeUtils.GetUnixTime();
            }

            InitTable(Assembly.GetExecutingAssembly());

            if (verDao.major == 0)
            {
                InitDml();
            }

            var ddlFile = Path.Combine(_BaseDir, "ddl.sql");
            ExecuteSql(ddlFile, verDao.major);

            var dmlFile = Path.Combine(_BaseDir, "dml.sql");
            ExecuteSql(dmlFile, verDao.major);

            SaveDbVer(verDao);
            return true;
        }

        /// <summary>
        /// 读取数据库版本
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected ScmVerDao ReadDbVer(string key)
        {
            try
            {
                _SqlClient.CodeFirst.InitTables(typeof(ScmVerDao));

                return _SqlClient.Queryable<ScmVerDao>().First(a => a.key == key);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 保存数据库版本
        /// </summary>
        /// <param name="verDao"></param>
        protected void SaveDbVer(ScmVerDao verDao)
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

        /// <summary>
        /// 新增记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dao"></param>
        protected void InsertDao<T>(T dao) where T : ScmDao, new()
        {
            dao.PrepareCreate(ScmEnv.DEFAULT_ID);
            _SqlClient.Insertable(dao).ExecuteCommand();
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dao"></param>
        protected void UpdateDao<T>(T dao) where T : ScmDao, new()
        {
            dao.PrepareUpdate(ScmEnv.DEFAULT_ID);
            _SqlClient.Updateable(dao).ExecuteCommand();
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dao"></param>
        protected void DeleteDao<T>(T dao) where T : ScmDao, new()
        {
            _SqlClient.Deleteable(dao).ExecuteCommand();
        }

        /// <summary>
        /// 保存记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dao"></param>
        protected void SaveDao<T>(T dao) where T : ScmDao, new()
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

        protected void SaveDataDao<T>(T dao, ScmRowStatusEnum status = ScmRowStatusEnum.Enabled) where T : ScmDataDao, new()
        {
            var tmpDao = _SqlClient.Queryable<T>().First(a => a.id == dao.id);
            if (tmpDao != null)
            {
                tmpDao = dao.Adapt(tmpDao);
                tmpDao.PrepareUpdate(ScmEnv.DEFAULT_ID);
                dao.row_status = status;
                _SqlClient.Updateable(tmpDao).ExecuteCommand();
                return;
            }

            dao.PrepareCreate(ScmEnv.DEFAULT_ID);
            dao.row_status = status;
            _SqlClient.Insertable(dao).ExecuteCommand();
        }

        /// <summary>
        /// 清空记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dao"></param>
        protected void TruncateDao<T>(T dao) where T : ScmDao, new()
        {
            _SqlClient.DbMaintenance.TruncateTable(dao.GetType());
        }

        /// <summary>
        /// 清空记录
        /// </summary>
        /// <param name="table"></param>
        protected void TruncateDao(string table)
        {
            _SqlClient.DbMaintenance.TruncateTable(table);
        }

        /// <summary>
        /// 执行外部脚本
        /// </summary>
        /// <param name="file"></param>
        /// <param name="major"></param>
        protected void ExecuteSql(string file, int major)
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

        /// <summary>
        /// 获取脚本版本
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
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
        /// 删除表格
        /// </summary>
        protected bool DropTable(Assembly assembly)
        {
            var scmDao = typeof(ScmDao);
            var daoType = assembly.GetTypes().Where(u => u.IsClass && !u.IsAbstract && !u.IsGenericType && u.Name.EndsWith("Dao")).ToList();
            foreach (var item in daoType.Where(s => !s.IsInterface))
            {
                if (!CommonUtils.HasImplementedRawGeneric(item, scmDao))
                {
                    continue;
                }

                var tableAttr = item.GetCustomAttribute<SugarTable>();
                if (tableAttr == null)
                {
                    continue;
                }

                var infos = _SqlClient.DbMaintenance.GetColumnInfosByTableName(tableAttr.TableName, false);
                if (infos.Count > 0)
                {
                    _SqlClient.DbMaintenance.DropTable(item);
                }
            }

            return true;
        }

        /// <summary>
        /// 数据库定义
        /// </summary>
        /// <param name="sqlClient"></param>
        protected bool InitTable(Assembly assembly)
        {
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
            return true;
        }

        /// <summary>
        /// 清空表格
        /// </summary>
        protected void TruncateTable(Assembly assembly)
        {
            var scmDao = typeof(ScmDao);
            var daoType = assembly.GetTypes().Where(u => u.IsClass && !u.IsAbstract && !u.IsGenericType && u.Name.EndsWith("Dao")).ToList();
            var daoList = new List<Type>();
            foreach (var item in daoType.Where(s => !s.IsInterface))
            {
                if (!CommonUtils.HasImplementedRawGeneric(item, scmDao))
                {
                    continue;
                }

                var tableAttr = item.GetCustomAttribute<SugarTable>();
                if (tableAttr == null)
                {
                    continue;
                }

                var infos = _SqlClient.DbMaintenance.GetColumnInfosByTableName(tableAttr.TableName, false);
                if (infos.Count > 0)
                {
                    daoList.Add(item);
                }
            }

            _SqlClient.DbMaintenance.TruncateTable(daoList.ToArray());
        }


        /// <summary>
        /// 数据库操作
        /// </summary>
        private void InitDml()
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

            var appDao = CreateApp(ScmEnv.DEFAULT_ID, 0, 0, "", "", "");
            appDao.row_status = ScmRowStatusEnum.Normal;
            _SqlClient.Updateable(appDao).ExecuteCommand();

            CreateApp(1000000000000001001, 10, 1, "scm.net", "`Scm.Net", "<p>一款基于Vue3和.Net10.0技术框架、适用于中后台管理系统的快速开发框架。</p><img src=\"/img/loginbg.svg\" alt=\"logo\"/>");
            CreateApp(1000000000000002001, 10, 2, "iam.net", "联合登录", "<p>简单、易用的多平台联合登录系统。</p><img src=\"/img/loginbg.svg\" alt=\"logo\"/>");

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
            positionDao.row_delete = ScmRowDeleteEnum.No;
            SaveDao(positionDao);
            positionDao.row_status = ScmRowStatusEnum.Normal;
            _SqlClient.Updateable(positionDao).ExecuteCommand();

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
            roleAdminDao.id = 1000000000000001030L;
            roleAdminDao.codec = "admin";
            roleAdminDao.names = "系统管理员";
            roleAdminDao.namec = "系统管理员";
            roleAdminDao.row_system = ScmRowSystemEnum.Yes;
            roleAdminDao.row_delete = ScmRowDeleteEnum.No;
            SaveDao(roleAdminDao);

            var roleAdminList = new List<RoleAuthDao>();

            // Root
            var menuRootDao = CreateMenu(ScmEnv.DEFAULT_ID, "", "", 0, 0, 0, "/", "", "");
            menuRootDao.row_status = ScmRowStatusEnum.Normal;
            _SqlClient.Updateable(menuRootDao).ExecuteCommand();
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuRootDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            // 主页
            var menuHomeDao = CreateMenu(1000000000000001000, "home", "主页", menuRootDao.id, 1, 1, "/home", "home", "sc-home-4-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuHomeDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            // 工作台
            var menuDashboardDao = CreateMenu(1000000000000001100, "dashboard", "工作台", menuHomeDao.id, 2, 1, "/dashboard", "home", "sc-menu-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDashboardDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            // 我的收藏
            var menuFavoritesDao = CreateMenu(1000000000000001200, "favorites", "我的收藏", menuHomeDao.id, 2, 2, "/favorites", "", "sc-heart-3-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuFavoritesDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            // 账户信息
            var menuProfilesDao = CreateMenu(1000000000000001300, "profiles", "账户信息", menuHomeDao.id, 2, 3, "/profiles", "", "sc-coffee-cup-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuProfilesDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            // 机构信息
            var menuHomeUnitDao = CreateMenu(1000000000000001310, "profiles-unit", "机构信息", menuProfilesDao.id, 3, 1, "/profiles/unitCenter", "home/unitinfo", "sc-settings-3-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuHomeUnitDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuHomeUserDao = CreateMenu(1000000000000001320, "profiles-user", "个人信息", menuProfilesDao.id, 3, 2, "/profiles/usercenter", "home/userinfo", "sc-user-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuHomeUserDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuHomeOAuthDao = CreateMenu(1000000000000001330, "profiles-oauth", "联合登录", menuProfilesDao.id, 3, 3, "/profiles/oauth", "home/oauth", "sc-bubble-chart-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuHomeOAuthDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuHomeOtpDao = CreateMenu(1000000000000001340, "profiles-otp", "凭证登录", menuProfilesDao.id, 3, 4, "/profiles/otp", "home/otp", "sc-bubble-chart-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuHomeOtpDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            // 我的反馈
            var menuFeedbackDao = CreateMenu(1000000000000001400, "feedback", "我的反馈", menuHomeDao.id, 2, 4, "/feedback", "scm/sys/feedback", "sc-chat-quote-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuFeedbackDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            // 我的终端
            var menuTerminalDao = CreateMenu(1000000000000001500, "terminal", "我的终端", menuHomeDao.id, 2, 5, "/terminal", "scm/ur/terminal", "sc-device-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuTerminalDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            // 下载中心
            var menuDownloadDao = CreateMenu(1000000000000001600, "download", "下载中心", menuHomeDao.id, 2, 6, "/download", "scm/down", "sc-device-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDownloadDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            // 设置
            var menuSettingsDao = CreateMenu(1000000000000003000, "setting", "设置", menuRootDao.id, 1, 3, "/setting", "", "sc-settings-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuSettingsDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            // 研发管理
            var menuDevDao = CreateMenu(1000000000000003100, "setting-dev", "研发管理", menuSettingsDao.id, 2, 1, "/setting/dev", "dev", "sc-bug-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDevDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuDevMenuDao = CreateMenu(1000000000000003110, "setting-dev-menu", "资源管理", menuDevDao.id, 3, 1, "/setting/dev/menu", "scm/dev/menu", "sc-menu-fill");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDevMenuDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuDevAppDao = CreateMenu(1000000000000003120, "setting-dev-app", "应用管理", menuDevDao.id, 3, 2, "/setting/dev/app", "scm/dev/app", "sc-apple");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDevAppDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuDevVerDao = CreateMenu(1000000000000003130, "setting-dev-version", "版本管理", menuDevDao.id, 3, 3, "/setting/dev/version", "scm/dev/version", "sc-file-text-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDevVerDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuDevUidDao = CreateMenu(1000000000000003140, "setting-dev-_uid", "编码管理", menuDevDao.id, 3, 4, "/setting/dev/uid", "scm/dev/uid", "sc-list-ordered");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDevUidDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuDevGenDao = CreateMenu(1000000000000003150, "setting-dev-gen", "代码生成", menuDevDao.id, 3, 5, "/setting/dev/generate", "scm/dev/generate", "sc-code-fill");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDevGenDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //var menuDevDbaDao = CreateMenu(1000000000000003160, "setting-dev-db", "数据库管理", menuDevDao.id, 3, 6, "/setting/dev/db", "scm/dev/db", "sc-coin-line");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDevDbaDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //var menuDevSqlDao = CreateMenu(1000000000000003170, "setting-dev-sql", "数据库脚本", menuDevDao.id, 3, 7, "/setting/dev/sql", "scm/dev/sql", "sc-file-paper-2-line");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDevSqlDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //var menuDevQtzDao = CreateMenu(1000000000000003180, "setting-dev-quartz", "后台任务", menuDevDao.id, 3, 8, "/setting/sys/quartz", "scm/sys/quartz", "sc-drag-move-line");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuDevQtzDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            // 全局配置
            var menuCfgDao = CreateMenu(1000000000000003200, "setting-adm", "全局配置", menuSettingsDao.id, 2, 2, "/setting/adm", "scm/cfg", "sc-settings-3-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuCfgDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuCfgAdmDao = CreateMenu(1000000000000003210, "setting-adm-cfg", "参数配置", menuCfgDao.id, 3, 1, "/setting/adm/cfg", "scm/adm/cfg", "sc-file-text-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuCfgAdmDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuCfgDicDao = CreateMenu(1000000000000003220, "setting-adm-dic", "数据字典", menuCfgDao.id, 3, 2, "/setting/adm/dic", "scm/adm/dic", "sc-file-copy-2-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuCfgDicDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuCfgCatDao = CreateMenu(1000000000000003230, "setting-adm-cat", "分类管理", menuCfgDao.id, 3, 3, "/setting/adm/cat", "scm/res/cat", "sc-layers-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuCfgCatDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuCfgUomDao = CreateMenu(1000000000000003240, "setting-adm-uom", "计量单位", menuCfgDao.id, 3, 4, "/setting/adm/uom", "scm/sys/uom", "sc-signpost-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuCfgUomDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuCfgSecDao = CreateMenu(1000000000000003250, "setting-adm-safety", "安全设置", menuCfgDao.id, 3, 5, "/setting/adm/safety", "scm/adm/safety", "sc-verified-badge-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuCfgSecDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            // 权限管理
            var menuUrDao = CreateMenu(1000000000000003300, "ur", "权限管理", menuSettingsDao.id, 2, 3, "/scm/ur", "scm/ur", "sc-user-settings-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuUrDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuUrOrganizeDao = CreateMenu(1000000000000003310, "scm-ur-organize", "组织管理", menuUrDao.id, 3, 1, "/scm/ur/organize", "scm/ur/organize", "sc-company-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuUrOrganizeDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuUrPositionDao = CreateMenu(1000000000000003320, "scm-ur-position", "岗位管理", menuUrDao.id, 3, 2, "/scm/ur/position", "scm/ur/position", "sc-place");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuUrPositionDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuUrGroupDao = CreateMenu(1000000000000003330, "scm-ur-group", "群组管理", menuUrDao.id, 3, 3, "/scm/ur/group", "scm/ur/group", "sc-user-2-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuUrGroupDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuUrRoleDao = CreateMenu(1000000000000003340, "scm-ur-role", "角色管理", menuUrDao.id, 3, 4, "/scm/ur/role", "scm/ur/role", "sc-contacts-book-upload-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuUrRoleDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuUrAuthDao = CreateMenu(1000000000000003350, "scm-ur-roleauth", "角色权限", menuUrDao.id, 3, 5, "/scm/ur/roleauth", "scm/ur/roleauth", "sc-verified-badge-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuUrAuthDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuUrAuthCDao = CreateMenu(1000000000000003360, "scm-ur-roleconflict", "角色互斥", menuUrDao.id, 3, 6, "/scm/ur/roleconflict", "scm/ur/roleconflict", "sc-cherry");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuUrAuthCDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            var menuUrUserDao = CreateMenu(1000000000000003370, "scm-ur-user", "用户管理", menuUrDao.id, 3, 7, "/scm/ur/user", "scm/ur/user", "sc-user-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuUrUserDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            // 关于
            var menuAboutDao = CreateMenu(1000000000000004000, "about", "关于", menuRootDao.id, 1, 999, "/about", "about", "sc-info");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuAboutDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuAboutSiteDao = CreateMenu(1000000000000004100, "about-site", "关于网站", menuAboutDao.id, 4, 1, "/about/app/site/scm.net", "about/app", "sc-global-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuAboutSiteDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuAboutAuthorDao = CreateMenu(1000000000000004200, "about-author", "关于作者", menuAboutDao.id, 4, 2, "/about/app/author/scm.net", "about/app", "sc-user-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuAboutAuthorDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuAboutContactDao = CreateMenu(1000000000000004300, "about-contact", "联系作者", menuAboutDao.id, 4, 3, "/about/app/contact/scm.net", "about/app", "sc-postcard");
            roleAdminList.Add(new RoleAuthDao { role_id = roleAdminDao.id, auth_id = menuAboutContactDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var menuAboutHistoryDao = CreateMenu(1000000000000004400, "about-history", "更新历史", menuAboutDao.id, 4, 4, "/about/ver/scm.net", "about/ver", "sc-file-text-line");
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
            terminalDao.codes = "";
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
            userRootDao.row_status = ScmRowStatusEnum.Normal;
            SaveDao(userRootDao);
            userRootDao.codes = "X0000001";
            userRootDao.row_status = ScmRowStatusEnum.Normal;
            _SqlClient.Updateable(userRootDao).ExecuteCommand();

            var userAdminDao = new UserDao();
            userAdminDao.id = 1000000000000001030;
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
    }
}
