using Com.Scm.Config;
using Com.Scm.Controllers;
using Com.Scm.Filters;
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

        [HttpGet("clear")]
        public IActionResult GetClearAsync()
        {
            return Ok();
        }

        /// <summary>
        /// 数据库初始化
        /// </summary>
        [HttpGet("init")]
        public ScmResponse GetInitAsync()
        {
            var response = new ScmResponse();
            try
            {
                new ScmDbHelper(_SqlClient).InitDb(_EnvConfig.GetDataPath("sql"));
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
