using Com.Scm.Dao;
using Com.Scm.Enums;
using Com.Scm.Utils;
using SqlSugar;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Com.Scm
{
    /// <summary>
    /// 模块化数据库管理工具
    /// </summary>
    public interface IModelDbHelper
    {
        void Init(ISqlSugarClient sqlClient, string baseDir);

        /// <summary>
        /// 删除数据库
        /// </summary>
        /// <returns></returns>
        bool DropDb();

        /// <summary>
        /// 初始数据库
        /// </summary>
        /// <returns></returns>
        bool InitDb();
    }

    public abstract class ScmModelDbHelper : IModelDbHelper
    {
        /// <summary>
        /// 
        /// </summary>
        protected ISqlSugarClient _SqlClient;

        /// <summary>
        /// SQL脚本目录
        /// </summary>
        protected string _SqlDir;

        /// <summary>
        /// 默认角色ID
        /// </summary>
        protected const long ROLE_ADMIN_ID = 1000000000000001030L;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="sqlClient"></param>
        /// <param name="sqlDir"></param>
        public void Init(ISqlSugarClient sqlClient, string sqlDir)
        {
            _SqlClient = sqlClient;
            _SqlDir = sqlDir;
        }

        protected abstract string GetKey();

        protected abstract int GetVer();

        protected abstract string GetDate();

        protected abstract void OnCreate(ScmVerDao verDao);

        protected abstract void OnUpgrade(ScmVerDao verDao);

        public abstract bool DropDb();

        public virtual bool InitDb()
        {
            var key = GetKey();

            var verDao = ReadDbVer(key);
            if (verDao == null)
            {
                verDao = new ScmVerDao();
                verDao.key = key;
                verDao.create_time = TimeUtils.GetUnixTime();

                // 表格初始化
                OnCreate(verDao);
            }
            else if (verDao.ver == GetVer())
            {
                // 版本相同，不执行任何操作
                return true;
            }
            else
            {
                // DML处理
                OnUpgrade(verDao);
            }

            verDao.ver = GetVer();
            verDao.date = GetDate();
            verDao.update_time = TimeUtils.GetUnixTime();
            SaveDbVer(verDao);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dao"></param>
        /// <param name="status"></param>
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
        protected bool CreateTable(Assembly assembly)
        {
            var scmDao = typeof(ScmDao);
            var scmTable = typeof(ScmTableAttribute);
            var daoType = assembly.GetTypes().Where(u => u.IsClass && !u.IsAbstract && !u.IsGenericType && u.Name.EndsWith("Dao")).ToList();
            var daoList = new List<Type>();
            foreach (var item in daoType.Where(s => !s.IsInterface))
            {
                // 过滤不需要创建的表格
                var attr = item.GetCustomAttribute<ScmTableAttribute>();
                if (attr != null && attr.IsIgnore)
                {
                    continue;
                }

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
        /// 执行外部脚本
        /// </summary>
        /// <param name="file">目标SQL脚本</param>
        /// <param name="ver">当前数据库版本</param>
        protected void ExecuteSql(string file, int ver)
        {
            if (!File.Exists(file))
            {
                return;
            }

            var lines = File.ReadAllLines(file);
            var inComment = false;
            var needRun = false;

            _SqlClient.Ado.UseTran(() =>
            {
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
                            needRun = ver <= GetSqlVer(sql);
                        }

                        if (sql.EndsWith("*/"))
                        {
                            inComment = false;
                        }

                        continue;
                    }

                    if (!needRun)
                    {
                        continue;
                    }

                    _SqlClient.Ado.ExecuteCommand(_SqlClient.EscapeSql(line));
                }
            });
        }

        /// <summary>
        /// 获取脚本版本
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected static int GetSqlVer(string text)
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
            catch (Exception)
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
    }
}
