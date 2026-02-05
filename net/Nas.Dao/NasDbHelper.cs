using Com.Scm.Enums;
using Com.Scm.Ur;
using Com.Scm.Utils;
using System.Reflection;

namespace Com.Scm.Nas
{
    public class NasDbHelper : ScmDbHelper
    {
        private const int MAJOR = 1;
        private const int MINOR = 0;
        private const int PATCH = 0;
        private const string BUILD = "2026020601";
        private const string RELEASE_DATE = "2026-02-06";

        public NasDbHelper()
        {
            //ScmServerHelper.Register(new NasDbHelper());
        }

        public override bool DropDb()
        {
            return DropTable(Assembly.GetExecutingAssembly());
        }

        public override bool InitDb()
        {
            var key = "scm.nas";

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

            var ddlFile = Path.Combine(_BaseDir, "ddl-nas.sql");
            ExecuteSql(ddlFile, verDao.major);

            var dmlFile = Path.Combine(_BaseDir, "dml-nas.sql");
            ExecuteSql(dmlFile, verDao.major);

            verDao.major = MAJOR;
            verDao.minor = MINOR;
            verDao.patch = PATCH;
            verDao.build = BUILD;
            verDao.release_date = RELEASE_DATE;
            verDao.update_time = TimeUtils.GetUnixTime();
            SaveDbVer(verDao);
            return true;
        }

        private void InitDml()
        {
            CreateApp(1000000000000002002, 10, 3, "nas.net", "私有云盘", "<p>Nas.Net是一款针对个人、家庭以及小团队的私有云存储软件，可以直接运行于已有的多种设备上，让您的老旧设备再次焕发新的机会。</p><img src=\"/img/loginbg.svg\" alt=\"logo\"/>");

            var roleId = 1000000000000001030L;
            var roleAdminList = new List<RoleAuthDao>();

            // 设置
            var admId = 1000000000000003000;
            // 配置管理
            var admFesDao = CreateMenu(1984069861600530432, "adm-fes", "文件类型", admId, 2, 4, "/adm/fes", "adm/fes", "sc-bill-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = admFesDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var admFesOrgDao = CreateMenu(1984070313574535168, "adm-fes-org", "组织管理", admFesDao.id, 3, 1, "/adm/res/org", "scm/res/org", "");
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = admFesOrgDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var admFesAppDao = CreateMenu(1984070535126061056, "adm-fes-app", "应用管理", admFesDao.id, 3, 2, "/adm/res/app", "scm/res/app", "");
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = admFesAppDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var admFesExtDao = CreateMenu(1984070855642189824, "adm-fes-ext", "后缀管理", admFesDao.id, 3, 3, "/adm/res/ext", "scm/res/ext", "");
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = admFesExtDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            // NAS
            var nasDao = CreateMenu(2000099923089035264, "nas", "NAS", ScmEnv.DEFAULT_ID, 1, 4, "/nas", "", "sc-hard-drive-3-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            // 配置管理
            var nasCfgDao = CreateMenu(2000099923089234567, "nas-cfg", "配置管理", nasDao.id, 2, 1, "/nas/cfg", "nas/cfg", "sc-settings-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasCfgDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var nasCfgFolderDao = CreateMenu(2000099923089526478, "nas-cfg-folder", "驱动管理", nasCfgDao.id, 2, 1, "/nas/cfg/folder", "nas/cfg/folder", "");
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasCfgFolderDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            // 日志管理
            var nasLogDao = CreateMenu(2000100080060862464, "nas-log", "日志管理", nasDao.id, 2, 2, "/nas/log", "nas/log", "sc-file-copy-2-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasLogDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var nasLogSyncDao = CreateMenu(2000100181730791424, "nas-log-file", "同步日志", nasLogDao.id, 3, 1, "/nas/log/file", "nas/log/file", "");
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasLogSyncDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            // 文件管理
            var nasResDao = CreateMenu(2008530629641244672, "nas-res", "文件管理", nasDao.id, 2, 3, "/nas/res", "nas/res", "sc-folder-open-line");
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasResDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var nasResDeviceDao = CreateMenu(2008530926073679872, "nas-res-device", "设备", nasResDao.id, 3, 1, "/nas/res/device", "nas/res/device", "");
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasResDeviceDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            var nasResPublicDao = CreateMenu(2008533412662611968, "nas-res-public", "云盘", nasResDao.id, 3, 2, "/nas/res/public", "nas/res/public", "");
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasResPublicDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //var nasResSecretDao = CreateMenu(2008533412662619628, "nas-res-secret", "密盘", nasResDao.id, 3, 3, "/nas/res/secret", "nas/res/secret", "");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasResSecretDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //var nasResDownDao = CreateMenu(2008531097767514112, "nas-res-down", "下载", nasResDao.id, 3, 4, "/nas/res/down", "nas/res/download", "");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasResDownDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //var nasResAppsDao = CreateMenu(2008531271671746560, "nas-res-apps", "应用", nasResDao.id, 3, 5, "/nas/res/apps", "nas/res/apps", "");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasResAppsDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            // 我的文档
            //var nasDocDao = CreateMenu(2008531430107385856, "nas-doc", "我的文档", nasDao.id, 2, 4, "/nas/doc", "nas/doc", "sc-book-line");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasDocDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //var nasDocImageDao = CreateMenu(2008531521073451008, "nas-doc-image", "图片", nasDocDao.id, 3, 1, "/nas/doc/image", "nas/doc/image", "");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasDocImageDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //var nasDocAudioDao = CreateMenu(2008531614015033344, "nas-doc-audio", "音频", nasDocDao.id, 3, 2, "/nas/doc/audio", "nas/doc/audio", "");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasDocAudioDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //var nasDocVedioDao = CreateMenu(2008531700468027392, "nas-doc-vedio", "视频", nasDocDao.id, 3, 3, "/nas/doc/vedio", "nas/doc/vedio", "");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasDocVedioDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //var nasDocOfficeDao = CreateMenu(2008531798677655552, "nas-doc-office", "办公", nasDocDao.id, 3, 4, "/nas/doc/office", "nas/doc/office", "");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasDocOfficeDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //var nasDocScriptDao = CreateMenu(2008531899831685120, "nas-doc-script", "研发", nasDocDao.id, 3, 5, "/nas/doc/script", "nas/doc/script", "");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasDocScriptDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //var nasDocArchiveDao = CreateMenu(2008532046519078912, "nas-doc-archive", "归档", nasDocDao.id, 3, 6, "/nas/doc/archive", "nas/doc/archive", "");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasDocArchiveDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            // 快捷访问
            //var nasLibDao = CreateMenu(2008532217747345408, "nas-lib", "快捷访问", nasDao.id, 2, 5, "/nas/lib", "nas/lib", "sc-command-line");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasLibDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //var nasLibRecentDao = CreateMenu(2008532387000094720, "nas-lib-recent", "最近", nasLibDao.id, 3, 1, "/nas/lib/recent", "nas/lib/recent", "");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasLibRecentDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //var nasLibUsallyDao = CreateMenu(2008532478062628864, "nas-lib-usually", "常用", nasLibDao.id, 3, 2, "/nas/lib/usually", "nas/lib/usually", "");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasLibUsallyDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //var nasLibFavDao = CreateMenu(2008532717624496128, "nas-fav", "收藏", nasLibDao.id, 3, 3, "/nas/lib/fav", "nas/lib/favorite", "");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasLibFavDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //var nasLibStarDao = CreateMenu(2008532860977418240, "nas-star", "星标", nasLibDao.id, 3, 4, "/nas/lib/star", "nas/lib/star", "");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasLibStarDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            ////var nasLibShareDao = CreateMenu(2008532948139249664, "nas-share", "分享", nasLibDao.id, 3, 5, "/nas/lib/share", "nas/lib/share", "");
            ////roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasLibShareDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            // 我的标签
            //var nasTagsDao = CreateMenu(2008533524591808512, "nas-tags", "我的标签", nasDao.id, 2, 6, "/nas/tags", "nas/tags", "sc-price-tag-line");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasTagsDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            // 我的分享
            //var nasShareDao = CreateMenu(2008533617885712384, "nas-share", "我的分享", nasDao.id, 2, 7, "/nas/share", "nas/share", "sc-stackshare-line");
            //roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasShareDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            _SqlClient.Insertable(roleAdminList).ExecuteCommand();
        }
    }
}
