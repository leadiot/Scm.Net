using Com.Scm.Dao;
using Com.Scm.Utils;
using SqlSugar;
using System.Data;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Com.Scm
{
    public class ScmDbHelper
    {
        public static void InitDb(ISqlSugarClient sqlClient)
        {
            try
            {
                InitDdl(sqlClient);

                var verDao = ReadVer(sqlClient);
                if (verDao == null)
                {
                    verDao = new ScmVerDao();
                    verDao.key = "scm";
                    verDao.create_time = TimeUtils.GetUnixTime();
                }

                var dir = AppDomain.CurrentDomain.BaseDirectory;
                var ddlFile = Path.Combine(dir, "data", "ddl.sql");
                ExecuteSql(sqlClient, ddlFile, verDao.major);

                var dmlFile = Path.Combine(dir, "data", "dml.sql");
                ExecuteSql(sqlClient, dmlFile, verDao.major);

                SaveVer(sqlClient, verDao);
            }
            catch (Exception ex)
            {
            }
        }

        private static void ExecuteSql(ISqlSugarClient sqlClient, string file, int major)
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
                if (inComment && !needRun)
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

                if (!needRun)
                {
                    return;
                }

                sqlClient.Ado.ExecuteCommand(line);
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

        private static void InitDdl(ISqlSugarClient sqlClient)
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
            sqlClient.CodeFirst.InitTables(daoList.ToArray());
        }

        private static ScmVerDao ReadVer(ISqlSugarClient sqlClient)
        {
            try
            {
                return sqlClient.Queryable<ScmVerDao>().First();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static void SaveVer(ISqlSugarClient sqlClient, ScmVerDao verDao)
        {
            verDao.update_time = TimeUtils.GetUnixTime();
            verDao.major = ScmVerDao.VER_MAJOR;
            verDao.minor = ScmVerDao.VER_MINOR;
            verDao.patch = ScmVerDao.VER_PATCH;
            verDao.build = ScmVerDao.VER_BUILD;

            if (verDao.id == 0)
            {
                sqlClient.Insertable(verDao).ExecuteCommand();
            }
            else
            {
                sqlClient.Updateable(verDao).ExecuteCommand();
            }
        }
    }
}
