using Com.Scm.Response;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Com.Scm.Utils
{
    public class ServerUtils
    {
        #region 获得IP地址
        /// <summary>
        /// 获得IP地址
        /// </summary>
        /// <returns>字符串数组</returns>
        public static string GetIp()
        {
            HttpContextAccessor _context = new HttpContextAccessor();
            var ip = string.Empty;
            if (_context.HttpContext.Request.Headers.ContainsKey("X-Real-IP"))
            {
                ip = _context.HttpContext.Request.Headers["X-Real-IP"].ToString();
            }
            if (_context.HttpContext.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ip = _context.HttpContext.Request.Headers["X-Forwarded-For"].ToString();
            }
            if (string.IsNullOrEmpty(ip))
            {
                ip = _context.HttpContext.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
        #endregion

        #region 获得浏览器信息
        public static string GetUserAgent()
        {
            var context = new HttpContextAccessor();
            return context.HttpContext.Request.Headers["User-Agent"].ToString();
        }
        /// <summary>
        /// 获得IP地址
        /// </summary>
        /// <returns>字符串数组</returns>
        public static string GetBrowser(string browserAgent)
        {
            string res;
            if (browserAgent.Contains("Chrome"))
            {
                res = "Chrome";
            }
            else if (browserAgent.Contains("Safari"))
            {
                res = "Safari";
            }
            else if (browserAgent.Contains("Firefox"))
            {
                res = "Firefox";
            }
            else if (browserAgent.Contains("Firefox"))
            {
                res = "Firefox";
            }
            else if (browserAgent.Contains("Edg"))
            {
                res = "Microsoft Edge";
            }
            else if (browserAgent.Contains("Opera"))
            {
                res = "Opera";
            }
            else if (browserAgent.Contains("MSIE"))
            {
                res = "IE";
            }
            else
            {
                res = browserAgent;
            }
            return res;
        }
        #endregion

        public static ScmApiResponse Error()
        {
            return new ScmApiResponse
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Message = "服务端发生错误"
            };
        }

        public static ScmApiResponse Error(string message)
        {
            return new ScmApiResponse
            {
                Code = -1,
                Message = message
            };
        }

        public static ScmApiResponse Success()
        {
            return new ScmApiResponse
            {
                Success = true,
                Code = (int)HttpStatusCode.OK
            };
        }

        public static ScmApiResponse Success<T>(T resultData)
        {
            return new ScmApiDataResponse<T>
            {
                Success = true,
                Code = (int)HttpStatusCode.OK,
                Data = resultData
            };
        }
    }
}
