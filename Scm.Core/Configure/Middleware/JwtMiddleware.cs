using Com.Scm.Token;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Com.Scm.Api.Configure.Middleware
{
    public class JwtMiddleware
    {
        private readonly List<string> _ignoreUrl = new()
        {
            "swagger",
            "/scmhub",
            "/api-config",
            "/upload/"
        };

        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        private bool IsIgnoreApi(string url)
        {
            if (url == null)
            {
                return false;
            }

            foreach (var item in _ignoreUrl)
            {
                if (url.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }

        public Task Invoke(HttpContext context)
        {
            ScmContextHolder holder = null;

            try
            {
                if (context.Request.Method.ToUpper() == "OPTIONS")
                {
                    return _next(context);
                }

                //过滤，不要验证token的url
                var path = context.Request.Path.Value?.ToLower();
                if (IsIgnoreApi(path))
                {
                    return _next(context);
                }

                holder = AppUtils.GetService<ScmContextHolder>();

                //自动刷新token
                var headers = context.Request.Headers;
                string appToken = headers[ScmToken.AppToken];
                if (!string.IsNullOrEmpty(appToken))
                {
                    return AppToken(context, holder, appToken);
                }

                string apiToken = headers[ScmToken.ApiToken];
                if (!string.IsNullOrEmpty(apiToken))
                {
                    return ApiToken(context, holder, apiToken);
                }

                string scmToken = headers[ScmToken.TokenName];
                if (!string.IsNullOrEmpty(scmToken))
                {
                    if (apiToken.StartsWith(ScmToken.PRE_APP))
                    {
                        return AppToken(context, holder, apiToken);
                    }

                    if (apiToken.StartsWith(ScmToken.PRE_API))
                    {
                        return ApiToken(context, holder, apiToken);
                    }
                }

                holder.SetToken(new ScmToken());
                return _next(context);
            }
            finally
            {
                holder?.Clear();
            }
        }

        /// <summary>
        /// 适用于网页，使用口令登录
        /// </summary>
        /// <param name="context"></param>
        /// <param name="holder"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private Task ApiToken(HttpContext context, ScmContextHolder holder, string token)
        {
            if (token.StartsWith(ScmToken.PRE_API))
            {
                token = token.Substring(ScmToken.PRE_API.Length);
            }

            var jwtToken = JwtUtils.SerializeJwt(token);
            holder.SetToken(jwtToken);

            var now = TimeUtils.GetUnixTime(true);
            // 未超时
            if (now - jwtToken.time <= 60 * 30 * 1000)
            {
                return _next(context);
            }

            // Session过期
            var newToken = JwtUtils.IssueJwt(new ScmToken()
            {
                id = jwtToken.id,
                user_id = jwtToken.user_id,
                user_codes = jwtToken.user_codes,
                user_name = jwtToken.user_name,
                //Role = "Admin",
                //RoleArray = jwtToken.RoleArray,
                time = now,
                data = jwtToken.data,
            });
            context.Response.Headers.Append("X-Refresh-Token", newToken);

            return _next(context);
        }

        /// <summary>
        /// 适用于应用，使用绑定登录
        /// </summary>
        /// <param name="context"></param>
        /// <param name="holder"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private Task AppToken(HttpContext context, ScmContextHolder holder, string token)
        {
            if (token.StartsWith(ScmToken.PRE_APP))
            {
                token = token.Substring(ScmToken.PRE_APP.Length);
            }

            var bytes = Convert.FromBase64String(token);
            token = Encoding.UTF8.GetString(bytes);

            var arr = token.Split(":");
            var scmToken = new ScmToken();
            if (arr.Length == 3)
            {
                var tmp = arr[0];
                if (TextUtils.IsLong(tmp))
                {
                    scmToken.terminal_id = long.Parse(tmp);
                }

                tmp = arr[1];
                if (TextUtils.IsLong(tmp))
                {
                    scmToken.time = long.Parse(tmp);
                }

                scmToken.digest = arr[2];
            }
            holder.SetToken(scmToken);

            return _next(context);
        }
    }
}