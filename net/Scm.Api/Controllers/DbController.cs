using Com.Scm.Config;
using Com.Scm.Controllers;
using Com.Scm.Filters;
using Com.Scm.Nas;
using Com.Scm.Samples;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Com.Scm.Api.Controllers
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
}
