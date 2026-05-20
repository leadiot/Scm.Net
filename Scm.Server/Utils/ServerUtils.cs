using Com.Scm.Response;
using Microsoft.AspNetCore.Http;
using System.Net;

namespace Com.Scm.Utils
{
    public class ServerUtils
    {
        private static IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// 初始化（在应用启动时调用，注入 IHttpContextAccessor）
        /// </summary>
        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        #region 获得IP地址
        /// <summary>
        /// 获得IP地址
        /// </summary>
        /// <returns>字符串数组</returns>
        public static string GetIp()
        {
            var context = _httpContextAccessor?.HttpContext;
            if (context == null) return string.Empty;

            var ip = string.Empty;
            if (context.Request.Headers.ContainsKey("X-Real-IP"))
            {
                ip = context.Request.Headers["X-Real-IP"].ToString();
            }
            if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                ip = context.Request.Headers["X-Forwarded-For"].ToString();
            }
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress?.ToString();
            }
            return ip ?? string.Empty;
        }
        #endregion

        #region 获得浏览器信息
        public static string GetUserAgent()
        {
            var context = _httpContextAccessor?.HttpContext;
            if (context == null) return string.Empty;

            return context.Request.Headers["User-Agent"].ToString();
        }
        /// <summary>
        /// 解析浏览器名称
        /// </summary>
        /// <param name="browserAgent">User-Agent 字符串</param>
        /// <returns>浏览器名称</returns>
        public static string GetBrowser(string browserAgent)
        {
            if (string.IsNullOrEmpty(browserAgent))
            {
                return string.Empty;
            }

            if (browserAgent.Contains("Chrome"))
            {
                return "Chrome";
            }
            if (browserAgent.Contains("Firefox"))
            {
                return "Firefox";
            }
            if (browserAgent.Contains("Edg"))
            {
                return "Microsoft Edge";
            }
            if (browserAgent.Contains("Safari"))
            {
                return "Safari";
            }
            if (browserAgent.Contains("Opera"))
            {
                return "Opera";
            }
            if (browserAgent.Contains("MSIE"))
            {
                return "IE";
            }
            return browserAgent;
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
