using Com.Scm.Jwt;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Http;

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
            var jwtContextHolder = AppUtils.GetService<JwtContextHolder>();
            try
            {
                if (context.Request.Method == "OPTIONS")
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
                var token = headers[JwtToken.TokenName];
                if (string.IsNullOrEmpty(token))
                {
                    jwtContextHolder?.SetToken(new JwtToken());
                }
                else
                {
                    var jwtToken = JwtUtils.SerializeJwt(token);
                    jwtContextHolder?.SetToken(jwtToken);

                    var ts = DateTime.Now.Subtract(jwtToken.time);
                    // 未超时
                    if (ts.Minutes is <= 30 or >= 60)
                    {
                        return _next(context);
                    }

                    // Session过期
                    var newToken = JwtUtils.IssueJwt(new JwtToken()
                    {
                        id = jwtToken.id,
                        user_id = jwtToken.user_id,
                        user_codes = jwtToken.user_codes,
                        user_name = jwtToken.user_name,
                        //Role = "Admin",
                        //RoleArray = jwtToken.RoleArray,
                        time = DateTime.Now,
                        data = jwtToken.data,
                    });
                    context.Response.Headers.Append("X-Refresh-Token", newToken);
                }
                return _next(context);
            }
            finally
            {
                jwtContextHolder?.Clear();
            }
        }
    }
}