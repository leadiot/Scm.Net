using Com.Scm.Config;
using Com.Scm.Controllers;
using Com.Scm.Filters;
using Com.Scm.Nas;
using Com.Scm.Response;
using Com.Scm.Samples;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Controllers
{
    /// <summary>
    /// 数据库服务
    /// </summary>
    [NoAuditLog]
    [AllowAnonymous]
    public class DbController : ApiController
    {
        private EnvConfig _EnvConfig;
        private ISqlSugarClient _SqlClient;

        public DbController(EnvConfig envConfig, ISqlSugarClient sqlClient)
        {
            _EnvConfig = envConfig;
            _SqlClient = sqlClient;
        }

        [HttpGet("test")]
        public ScmApiResponse TestAsync(DbConfig config)
        {
            var response = new ScmApiResponse();
            if (config == null)
            {
                response.SetFailure("参数不能为空！");
                return response;
            }

            DbType dbType = 0;
            string dbString = "";
            switch (config.DbType.ToLower())
            {
                case "mariadb":
                case "mysql":
                    dbType = DbType.MySql;
                    if (string.IsNullOrWhiteSpace(config.Host))
                    {
                        response.SetFailure("数据库主机地址不能为空！");
                        return response;
                    }
                    if (!TextUtils.IsInteger(config.Port))
                    {
                        response.SetFailure("无效的数据库主机端口！");
                        return response;
                    }
                    if (string.IsNullOrEmpty(config.Username))
                    {
                        response.SetFailure("数据库登录用户不能为空！");
                        return response;
                    }
                    if (string.IsNullOrEmpty(config.Database))
                    {
                        response.SetFailure("数据库名称不能为空！");
                        return response;
                    }
                    //dbString = $"server={config.Host};port={config.Port};database={config.Database};uid={config.Username};pwd={config.Password};charset=utf8mb4;sslmode=None;allow user variables=true;max pool size=100;min pool size=10;connection timeout=30;";
                    dbString = $"server={config.Host};port={config.Port};database={config.Database};uid={config.Username};pwd={config.Password};charset=utf8mb4;sslmode=None;";
                    break;
                case "sqlserver":
                    dbType = DbType.SqlServer;
                    if (string.IsNullOrWhiteSpace(config.Host))
                    {
                        response.SetFailure("数据库主机地址不能为空！");
                        return response;
                    }
                    if (!TextUtils.IsInteger(config.Port))
                    {
                        response.SetFailure("无效的数据库主机端口！");
                        return response;
                    }
                    if (string.IsNullOrEmpty(config.Username))
                    {
                        response.SetFailure("数据库登录用户不能为空！");
                        return response;
                    }
                    if (string.IsNullOrEmpty(config.Database))
                    {
                        response.SetFailure("数据库名称不能为空！");
                        return response;
                    }
                    //dbString = $"Server={config.Host};Database={config.Database};Trusted_Connection=True;";
                    //dbString = $"Data Source={config.Host};Initial Catalog={config.Database};User ID={config.Username};Password={config.Password};TrustServerCertificate=True;Connect Timeout=30;Max Pool Size=100;Min Pool Size=10;";
                    dbString = $"Data Source={config.Host},{config.Port};Initial Catalog={config.Database};User ID={config.Username};Password={config.Password};TrustServerCertificate=True;";
                    break;
                case "sqlite":
                    dbType = DbType.Sqlite;
                    if (string.IsNullOrEmpty(config.Database))
                    {
                        response.SetFailure("数据库名称不能为空！");
                        return response;
                    }
                    //dbString = $"Data Source={config.Database};Version=3;Pooling=true;Max Pool Size=100;Journal Mode=WAL;";
                    dbString = $"Data Source={config.Database};";
                    break;
                case "oracle":
                    dbType = DbType.Oracle;
                    if (string.IsNullOrWhiteSpace(config.Host))
                    {
                        response.SetFailure("数据库主机地址不能为空！");
                        return response;
                    }
                    if (!TextUtils.IsInteger(config.Port))
                    {
                        response.SetFailure("无效的数据库主机端口！");
                        return response;
                    }
                    if (string.IsNullOrEmpty(config.Username))
                    {
                        response.SetFailure("数据库登录用户不能为空！");
                        return response;
                    }
                    if (string.IsNullOrEmpty(config.Database))
                    {
                        response.SetFailure("数据库名称不能为空！");
                        return response;
                    }
                    //dbString = $"Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={config.Host})(PORT={config.Port})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={config.Database})));User Id={config.Username};Password={config.Password};";
                    //dbString = $"User Id={config.Username};Password={config.Password};Data Source={config.Host}:{config.Port}/{config.Database};Connect Timeout=30;Pooling=true;";
                    dbString = $"User Id={config.Username};Password={config.Password};Data Source={config.Host}:{config.Port}/{config.Database};Pooling=true;";
                    break;
                case "postgresql":
                    dbType = DbType.PostgreSQL;
                    if (string.IsNullOrWhiteSpace(config.Host))
                    {
                        response.SetFailure("数据库主机地址不能为空！");
                        return response;
                    }
                    if (!TextUtils.IsInteger(config.Port))
                    {
                        response.SetFailure("无效的数据库主机端口！");
                        return response;
                    }
                    if (string.IsNullOrEmpty(config.Username))
                    {
                        response.SetFailure("数据库登录用户不能为空！");
                        return response;
                    }
                    if (string.IsNullOrEmpty(config.Database))
                    {
                        response.SetFailure("数据库名称不能为空！");
                        return response;
                    }
                    //dbString = $"Host={config.Host};Port={config.Port};Database={config.Database};Username={config.Username};Password={config.Password};Timeout=30;Pooling=true;MaxPoolSize=100;MinPoolSize=10;SSL Mode=Require;Trust Server Certificate=true;";
                    dbString = $"Host={config.Host};Port={config.Port};Database={config.Database};Username={config.Username};Password={config.Password};";
                    break;
                case "db2":
                    dbType = DbType.DB2;
                    //dbString = $"Server={config.Host}:{config.Port};Database={config.Database};Uid={config.Username};Pwd={config.Password};Connect Timeout=30;Pooling=true;Max Pool Size=100;Min Pool Size=10;CharSet=UTF-8;";
                    dbString = $"Server={config.Host}:{config.Port};Database={config.Database};UID={config.Username};PWD={config.Password};CharSet=UTF-8;";
                    break;
                case "dm":
                    dbType = DbType.Dm;
                    if (string.IsNullOrWhiteSpace(config.Host))
                    {
                        response.SetFailure("数据库主机地址不能为空！");
                        return response;
                    }
                    if (!TextUtils.IsInteger(config.Port))
                    {
                        response.SetFailure("无效的数据库主机端口！");
                        return response;
                    }
                    if (string.IsNullOrEmpty(config.Username))
                    {
                        response.SetFailure("数据库登录用户不能为空！");
                        return response;
                    }
                    if (string.IsNullOrEmpty(config.Database))
                    {
                        response.SetFailure("数据库名称不能为空！");
                        return response;
                    }
                    //dbString = $"Server={config.Host};Port={config.Port};Database={config.Database};Uid={config.Username};Pwd={config.Password};Connect Timeout=30;Pooling=true;";
                    dbString = $"Server={config.Host};Port={config.Port};Database={config.Database};Uid={config.Username};Pwd={config.Password};Pooling=true;";
                    break;
                default:
                    response.SetFailure("不支持的数据库类型！");
                    return response;
            }

            var sqlConfig = new ConnectionConfig()
            {
                DbType = dbType,
                ConnectionString = dbString,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            };

            try
            {
                using (var sqlClient = new SqlSugarClient(sqlConfig))
                {
                    sqlClient.Open();
                    sqlClient.CodeFirst.InitTables(typeof(ScmVerDao));
                }
                response.SetSuccess("数据库连接成功！");
            }
            catch (Exception exp)
            {
                response.SetFailure(exp.Message);
            }
            return response;
        }

        [HttpGet("drop")]
        public ScmApiResponse GetDropAsync()
        {
            var response = new ScmApiResponse();
            try
            {
                var baseDir = _EnvConfig.GetDataPath("sql");

                IModelHelper helper = new ScmDbHelper();
                helper.Init(_SqlClient, baseDir);
                helper.DropDb();

                helper = new SamplesDbHelper();
                helper.Init(_SqlClient, baseDir);
                helper.DropDb();

                helper = new NasDbHelper();
                helper.Init(_SqlClient, baseDir);
                helper.DropDb();

                response.SetSuccess();
            }
            catch (Exception ex)
            {
                response.SetFailure(ex.Message);
            }
            return response;
        }

        /// <summary>
        /// 数据库初始化
        /// </summary>
        [HttpGet("init")]
        public ScmApiResponse GetInitAsync()
        {
            var response = new ScmApiResponse();
            try
            {
                var baseDir = _EnvConfig.GetDataPath("sql");

                IModelHelper helper = new ScmDbHelper();
                helper.Init(_SqlClient, baseDir);
                helper.InitDb();

                helper = new SamplesDbHelper();
                helper.Init(_SqlClient, baseDir);
                helper.InitDb();

                helper = new NasDbHelper();
                helper.Init(_SqlClient, baseDir);
                helper.InitDb();

                response.SetSuccess();
            }
            catch (Exception ex)
            {
                response.SetFailure(ex.Message);
            }
            return response;
        }
    }

    public class DbConfig
    {
        public string DbType { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
    }
}
