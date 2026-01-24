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
        private ISqlSugarClient _SqlClient;

        public DbController(ISqlSugarClient sqlClient)
        {
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
        public IActionResult GetInitAsync()
        {
            ScmDbHelper.InitDb(_SqlClient);
            return Ok();
        }
    }
}
