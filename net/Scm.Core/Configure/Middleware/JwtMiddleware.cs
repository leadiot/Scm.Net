using Com.Scm.Token;
using Com.Scm.Token.Utils;
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

                //自动刷新token
                var headers = context.Request.Headers;
                string token = headers[ScmToken.TokenName];
                holder = AppUtils.GetService<ScmContextHolder>();
                if (string.IsNullOrEmpty(token))
                {
                    holder.SetToken(new ScmToken());
                }
                else
                {
                    if (token.StartsWith(ScmToken.KEY_BASIC))
                    {
                        BasicToken(context, holder, token);
                    }
                    else if (token.StartsWith(ScmToken.KEY_BEARER))
                    {
                        BearerToken(context, holder, token);
                    }
                    else
                    {
                        holder.SetToken(new ScmToken());
                    }
                }

                return _next(context);
            }
            finally
            {
                holder?.Clear();
            }
        }

        private Task BasicToken(HttpContext context, ScmContextHolder holder, string token)
        {
            token = token.Substring(ScmToken.KEY_BASIC.Length + 2);
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

        private Task BearerToken(HttpContext context, ScmContextHolder holder, string token)
        {
            token = token.Substring(ScmToken.KEY_BEARER.Length + 2);

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
    }
}