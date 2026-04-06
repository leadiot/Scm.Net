using Com.Scm.Cfg.Export;
using Com.Scm.Config;
using Com.Scm.Enums;
using Com.Scm.Sys.Tasks;
using Com.Scm.Ur.User.Dvo;
using Com.Scm.Utils;
using CsvHelper;
using SqlSugar;
using System.Globalization;

namespace Com.Scm.Ur.User
{
    /// <summary>
    /// 
    /// </summary>
    public class UserExportHandler : ScmTaskHandler
    {
        private SearchUserRequest _Request;

        /// <summary>
        /// 
        /// </summary>
        public UserExportHandler()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        public UserExportHandler(SearchUserRequest request)
        {
            _Request = request;
        }

        /// <summary>
        /// 
        /// </summary>
        public override int Types
        {
            get { return 0; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override string Name { get { return "用户数据导出"; } }

        /// <summary>
        /// 
        /// </summary>
        public override string Json { get { return _Request.ToJsonString(); } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        /// <param name="client"></param>
        /// <param name="dao"></param>
        /// <returns></returns>
        public override void Execute(EnvConfig config, ISqlSugarClient client, TaskDao dao)
        {
            if (string.IsNullOrWhiteSpace(dao.json))
            {
                dao.handle = ScmHandleEnum.Done;
                dao.message = "查询条件为空！";
                client.Updateable(dao).ExecuteCommand();
                return;
            }

            _Request = dao.json.AsJsonObject<SearchUserRequest>();
            if (_Request == null)
            {
                dao.handle = ScmHandleEnum.Done;
                dao.message = "查询对象为空！";
                client.Updateable(dao).ExecuteCommand();
                return;
            }

            dao.handle = ScmHandleEnum.Doing;
            client.Updateable(dao).ExecuteCommand();

            var file = Export(config, client, dao, _Request);

            dao.file = config.ToUri(file);
            dao.handle = ScmHandleEnum.Done;
            client.Updateable(dao).ExecuteCommand();
        }

        /// <summary>
        /// 
        /// </summary>
        public string Export(EnvConfig env, ISqlSugarClient client, TaskDao dao, SearchUserRequest request)
        {
            var list = client.Queryable<UserDao>()
                .WhereIF(!request.IsAllStatus(), m => m.row_status == request.row_status)
                .WhereIF(IsNormalId(request.organize_id), m => SqlFunc.Subqueryable<UserOrganizeDao>().Where(r => r.user_id == m.id && r.organize_id == request.organize_id).Any())
                .WhereIF(IsNormalId(request.position_id), m => SqlFunc.Subqueryable<UserPositionDao>().Where(r => r.user_id == m.id && r.position_id == request.position_id).Any())
                .WhereIF(IsNormalId(request.group_id), m => SqlFunc.Subqueryable<UserGroupDao>().Where(r => r.user_id == m.id && r.group_id == request.group_id).Any())
                .WhereIF(IsNormalId(request.role_id), m => SqlFunc.Subqueryable<UserRoleDao>().Where(r => r.user_id == m.id && r.role_id == request.role_id).Any())
                .WhereIF(!string.IsNullOrEmpty(request.key), m => m.namec.Contains(request.key) || m.cellphone.Contains(request.key) || m.email.Contains(request.key) || m.names.Contains(request.key))
                .OrderBy(m => m.id, OrderByType.Asc)
                .ToList();

            var name = TimeUtils.GetUnixTime().ToString() + ".csv";
            var file = "";

            var headerDao = client.Queryable<ExportHeaderDao>()
                 .Where(a => a.codec == dao.clazz && a.row_status == ScmRowStatusEnum.Enabled)
                 .First();
            if (headerDao == null)
            {
                file = env.GetTempPath(name);
                SaveByRaw(file, list);
                return file;
            }

            var detailListDao = client.Queryable<ExportDetailDao>()
                .Where(a => a.export_id == headerDao.id)
                .OrderBy(a => a.od)
                .ToList();
            if (detailListDao == null || detailListDao.Count() < 1)
            {
                file = env.GetTempPath(name);
                SaveByRaw(file, list);
                return file;
            }

            name = headerDao.file + "_" + name;
            file = env.GetTempPath(name);
            using (var writer = new StreamWriter(file))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    WriteHead(csv, detailListDao);
                    csv.NextRecord();

                    foreach (var item in list)
                    {
                        WriteData(csv, detailListDao, item);
                        csv.NextRecord();
                    }
                }
            }

            return file;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ExportHeaderDao GenDao()
        {
            var dao = new ExportHeaderDao();
            dao.codec = "scm_ur_user";
            dao.names = "用户数据导出模板";
            dao.file = "user";

            var details = new ExportDetailDao[] {
                new ExportDetailDao{ col="", namec=""},
                new ExportDetailDao{ col="", namec=""},
                new ExportDetailDao{ col="", namec=""},
                new ExportDetailDao{ col="", namec=""},
                new ExportDetailDao{ col="", namec=""},
                new ExportDetailDao{ col="", namec=""},
                new ExportDetailDao{ col="", namec=""},
                new ExportDetailDao{ col="", namec=""},
            };
            dao.Add(details);

            return dao;
        }
    }
}
