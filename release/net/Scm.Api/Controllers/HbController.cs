using Com.Scm.Controllers;
using Com.Scm.Dsa;
using Com.Scm.Filters;
using Com.Scm.Log;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Com.Scm.Api.Controllers
{
    /// <summary>
    /// 心跳服务
    /// </summary>
    [ApiExplorerSettings(GroupName = "Scm")]
    public class HbController : ApiController
    {
        private SugarRepository<LogHbDao> _ThisRepository;

        public HbController(SugarRepository<LogHbDao> thisRepository)
        {
            _ThisRepository = thisRepository;
        }

        /// <summary>
        /// 终端设备心跳
        /// </summary>
        /// <param name="ip">主机地址</param>
        /// <param name="ma">网卡地址(Mac Address)</param>
        /// <param name="hn">主机名称(Host Name)</param>
        /// <param name="os">操作系统</param>
        /// <param name="rv">运行环境(Runtime Version)</param>
        /// <param name="un">登录用户(User Name)</param>
        /// <param name="dt">心跳时间(Datetime)</param>
        /// <returns></returns>
        [HttpPost("hd"), AllowAnonymous, NoAuditLog]
        public async Task<bool> HdAsync([FromForm] string ip,
            [FromForm] string ma,
            [FromForm] string hn,
            [FromForm] string os,
            [FromForm] string rv,
            [FromForm] string un,
            [FromForm] long dt)
        {
            var dao = new LogHbDao();
            dao.type = LogHbDto.TYPE_1;
            dao.iip = ip;
            dao.oip = GetClientIP(Request);
            dao.mac = ma;
            dao.host = hn;
            dao.os = os;
            dao.clr = rv;
            dao.user = un;
            dao.time = dt;
            dao.create_time = TimeUtils.GetUnixTime();

            await _ThisRepository.InsertAsync(dao);

            //var response = new ScmResponse();
            //response.SetSuccess();
            //return response;
            return true;
        }

        /// <summary>
        /// 三方服务心跳
        /// </summary>
        /// <param name="ip">主机地址</param>
        /// <param name="ma">网卡地址(Mac Address)</param>
        /// <param name="hn">主机名称(Host Name)</param>
        /// <param name="os">操作系统</param>
        /// <param name="rv">运行环境(Runtime Version)</param>
        /// <param name="un">登录用户(User Name)</param>
        /// <param name="dt">执行时间(Datetime)</param>
        /// <returns></returns>
        [HttpPost("ts"), AllowAnonymous, NoAuditLog]
        public async Task<bool> TsAsync([FromForm] string ip,
            [FromForm] string ma,
            [FromForm] string hn,
            [FromForm] string os,
            [FromForm] string rv,
            [FromForm] string un,
            [FromForm] long dt)
        {
            var dao = new LogHbDao();
            dao.type = LogHbDto.TYPE_2;
            dao.iip = ip;
            dao.oip = GetClientIP(Request);
            dao.mac = ma;
            dao.host = hn;
            dao.os = os;
            dao.clr = rv;
            dao.user = un;
            dao.time = dt;
            dao.create_time = TimeUtils.GetUnixTime();

            await _ThisRepository.InsertAsync(dao);

            //var response = new ScmResponse();
            //response.SetSuccess();
            //return response;
            return true;
        }

        private string GetClientIP(HttpRequest request)
        {
            var headersToCheck = new[] { "X-Forwarded-For", "X-Real-IP", "Proxy-Client-IP", "WL-Proxy-Client-IP" };

            foreach (var header in headersToCheck)
            {
                var ip = request.Headers[header].FirstOrDefault();
                if (!string.IsNullOrEmpty(ip) && !ip.Equals("unknown", StringComparison.OrdinalIgnoreCase))
                    return ip.Split(',').First().Trim();
            }
            return request.HttpContext.Connection.RemoteIpAddress?.ToString();
        }
    }
}
