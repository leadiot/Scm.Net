using Com.Scm.Api.Service;
using Com.Scm.Jwt;
using Com.Scm.Jwt.Model;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Http;

namespace Com.Scm.Api.Middleware;

public class JwtMiddleware
{
    private readonly List<string> _ignoreUrl = new()
    {
        "swagger",
        "/api/exammaterial/upload",
        "/fytapiui",
        "/chathub",
        "/api-config",
        "/fyt",
        "/upload/"
    };
    private readonly RequestDelegate _next;

    public JwtMiddleware(RequestDelegate next)
    {
        _next = next;
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
            var headers = context.Request.Headers;
            //过滤，不要验证token的url
            var path = context.Request.Path.Value?.ToLower();
            var isIgnore = false;
            foreach (var item in _ignoreUrl.Where(item => path != null && path.Contains(item)))
            {
                isIgnore = true;
            }
            if (isIgnore)
            {
                return _next(context);
            }
            //自动刷新token
            var token = headers[JwtConstant.TokenName];
            if (string.IsNullOrEmpty(token))
            {
                jwtContextHolder?.SetToken(new JwtToken());
            }
            else
            {
                var jwtToken = JwtAuthService.SerializeJwt(token);
                jwtContextHolder?.SetToken(jwtToken);
                var ts = DateTime.Now.Subtract(jwtToken.time);
                if (ts.Minutes is <= 30 or >= 60) return _next(context);
                var newToken = JwtAuthService.IssueJwt(new JwtToken()
                {
                    id = jwtToken.id,
                    user_name = jwtToken.user_name,
                    Role = "Admin",
                    RoleArray = jwtToken.RoleArray,
                    unit_id = jwtToken.unit_id,
                    time = DateTime.Now
                });
                context.Response.Headers.Add("X-Refresh-Token", newToken);
            }
            return _next(context);
        }
        finally
        {
            jwtContextHolder?.Clear();
        }
    }
}