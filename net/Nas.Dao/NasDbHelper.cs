using Com.Scm.Dev;
using Com.Scm.Enums;
using Com.Scm.Ur;
using Com.Scm.Utils;
using System.Reflection;

namespace Com.Scm.Nas
{
    public class NasDbHelper : ScmDbHelper
    {
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

            SaveDbVer(verDao);
            return true;
        }

        private void InitDml()
        {
            var appDao = new ScmDevAppDao();
            appDao.id = 1000000000000002002;
            appDao.types = 10;
            appDao.od = 3;
            appDao.code = "nas.net";
            appDao.name = "私有云盘";
            appDao.content = "<p>Nas.Net是一款针对个人、家庭以及小团队的私有云存储软件，可以直接运行于已有的多种设备上，让您的老旧设备再次焕发新的机会。</p><img src=\"/img/loginbg.svg\" alt=\"logo\"/>";
            SaveDao(appDao);

            // NAS
            var nasDao = CreateMenu(2000099923089035264, "nas", "NAS", ScmEnv.DEFAULT_ID, 1, 4, "/nas", "", "sc-hard-drive-3-line");
            // 配置管理
            var nasCfgDao = CreateMenu(2000099923089234567, "nas-cfg", "配置管理", nasDao.id, 2, 1, "/nas/cfg", "nas/cfg", "");
            var nasCfgFolderDao = CreateMenu(2000099923089526478, "nas-cfg-folder", "驱动管理", nasCfgDao.id, 2, 1, "/nas/cfg/folder", "nas/cfg/folder", "");

            // 日志管理
            var nasLogDao = CreateMenu(2000100080060862464, "nas-log", "日志管理", nasDao.id, 2, 2, "/nas/log", "nas/log", "");
            var nasLogSyncDao = CreateMenu(2000100181730791424, "nas-log-file", "同步日志", nasLogDao.id, 3, 1, "/nas/log/file", "nas/log/file", "");

            // 文件管理
            var nasResDao = CreateMenu(2008530629641244672, "nas-res", "文件管理", nasDao.id, 2, 3, "/nas/res", "nas/res", "");
            var nasResDeviceDao = CreateMenu(2008530926073679872, "nas-res-device", "设备", nasResDao.id, 3, 1, "/nas/res/device", "nas/res/device", "");
            var nasResPublicDao = CreateMenu(2008533412662611968, "nas-res-public", "云盘", nasResDao.id, 3, 2, "/nas/res/public", "nas/res/public", "");
            var nasResSecretDao = CreateMenu(2008533412662619628, "nas-res-secret", "密盘", nasResDao.id, 3, 3, "/nas/res/secret", "nas/res/secret", "");
            var nasResDownDao = CreateMenu(2008531097767514112, "nas-res-down", "下载", nasResDao.id, 3, 4, "/nas/res/down", "nas/res/download", "");
            var nasResAppsDao = CreateMenu(2008531271671746560, "nas-res-apps", "应用", nasResDao.id, 3, 5, "/nas/res/apps", "nas/res/apps", "");

            // 我的文档
            var nasDocDao = CreateMenu(2008531430107385856, "nas-doc", "我的文档", nasDao.id, 2, 4, "/nas/doc", "nas/doc", "");
            var nasDocImageDao = CreateMenu(2008531521073451008, "nas-doc-image", "图片", nasDocDao.id, 3, 1, "/nas/doc/image", "nas/doc/image", "");
            var nasDocAudioDao = CreateMenu(2008531614015033344, "nas-doc-audio", "音频", nasDocDao.id, 3, 2, "/nas/doc/audio", "nas/doc/audio", "");
            var nasDocVedioDao = CreateMenu(2008531700468027392, "nas-doc-vedio", "视频", nasDocDao.id, 3, 3, "/nas/doc/vedio", "nas/doc/vedio", "");
            var nasDocOfficeDao = CreateMenu(2008531798677655552, "nas-doc-office", "办公", nasDocDao.id, 3, 4, "/nas/doc/office", "nas/doc/office", "");
            var nasDocScriptDao = CreateMenu(2008531899831685120, "nas-doc-script", "研发", nasDocDao.id, 3, 5, "/nas/doc/script", "nas/doc/script", "");
            var nasDocArchiveDao = CreateMenu(2008532046519078912, "nas-doc-archive", "归档", nasDocDao.id, 3, 6, "/nas/doc/archive", "nas/doc/archive", "");

            // 快捷访问
            var nasLibDao = CreateMenu(2008532217747345408, "nas-lib", "快捷访问", nasDao.id, 2, 5, "/nas/lib", "nas/lib", "");
            var nasLibRecentDao = CreateMenu(2008532387000094720, "nas-lib-recent", "最近", nasLibDao.id, 3, 1, "/nas/lib/recent", "nas/lib/recent", "");
            var nasLibUsallyDao = CreateMenu(2008532478062628864, "nas-lib-usually", "常用", nasLibDao.id, 3, 2, "/nas/lib/usually", "nas/lib/usually", "");
            var nasLibFavDao = CreateMenu(2008532717624496128, "nas-fav", "收藏", nasLibDao.id, 3, 3, "/nas/lib/fav", "nas/lib/favorite", "");
            var nasLibStarDao = CreateMenu(2008532860977418240, "nas-star", "星标", nasLibDao.id, 3, 4, "/nas/lib/star", "nas/lib/star", "");
            //var nasLibShareDao = CreateMenu(2008532948139249664, "nas-share", "分享", nasLibDao.id, 3, 5, "/nas/lib/share", "nas/lib/share", "");

            // 我的标签
            var nasTagsDao = CreateMenu(2008533524591808512, "nas-tags", "我的标签", nasDao.id, 2, 6, "/nas/tags", "nas/tags", "");

            // 我的分享
            var nasShareDao = CreateMenu(2008533617885712384, "nas-share", "我的分享", nasDao.id, 2, 7, "/nas/share", "nas/share", "");

            var roleId = 1000000000000001030L;
            var roleAdminList = new List<RoleAuthDao>();
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasCfgDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasCfgFolderDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasLogDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasLogSyncDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasResDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasResDeviceDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasResPublicDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasResSecretDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasResDownDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasResAppsDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasDocDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasDocImageDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasDocAudioDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasDocVedioDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasDocOfficeDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasDocScriptDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasDocArchiveDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasLibDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasLibRecentDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasLibUsallyDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasLibFavDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasLibStarDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            //roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasLibShareDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasTagsDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });

            roleAdminList.Add(new RoleAuthDao { role_id = roleId, auth_id = nasShareDao.id, types = ScmRoleAuthTypesEnum.RoleMenu });
            _SqlClient.Insertable(roleAdminList).ExecuteCommand();
        }
    }
}
