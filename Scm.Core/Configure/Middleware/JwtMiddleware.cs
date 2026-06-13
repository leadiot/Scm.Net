using Com.Scm.Config;
using Com.Scm.Token;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Http;

namespace Com.Scm.Configure.Middleware
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

        public async Task Invoke(HttpContext context)
        {
            ScmContextHolder holder = null;

            try
            {
                if (context.Request.Method.ToUpper() == "OPTIONS")
                {
                    await _next(context);
                    return;
                }

                //过滤，不要验证token的url
                var path = context.Request.Path.Value?.ToLower();
                if (IsIgnoreApi(path))
                {
                    await _next(context);
                    return;
                }

                holder = AppUtils.GetService<ScmContextHolder>();

                var headers = context.Request.Headers;

                // 优先级1：AppToken 请求头（适用于设备/应用绑定登录）
                string appToken = headers[ScmToken.AppToken];
                if (!string.IsNullOrEmpty(appToken))
                {
                    await AppToken(context, holder, appToken);
                    return;
                }

                // 优先级2：ApiToken 请求头（适用于网页/Api，JWT 口令登录）
                string apiToken = headers[ScmToken.ApiToken];
                if (!string.IsNullOrEmpty(apiToken))
                {
                    await ApiToken(context, holder, apiToken);
                    return;
                }

                // 优先级3：ScmToken 请求头（兼容旧格式，自动识别类型）
                string scmToken = headers[ScmToken.TokenName];
                if (!string.IsNullOrEmpty(scmToken))
                {
                    if (scmToken.StartsWith(ScmToken.PRE_APP))
                    {
                        await AppToken(context, holder, scmToken);
                        return;
                    }

                    if (scmToken.StartsWith(ScmToken.PRE_API))
                    {
                        await ApiToken(context, holder, scmToken);
                        return;
                    }
                }

                holder.SetToken(new ScmToken());
                await _next(context);
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
        private async Task ApiToken(HttpContext context, ScmContextHolder holder, string token)
        {
            if (token.StartsWith(ScmToken.PRE_API))
            {
                token = token.Substring(ScmToken.PRE_API.Length);
            }

            var jwtToken = JwtUtils.SerializeJwt(token);
            holder.SetToken(jwtToken);

            var now = TimeUtils.GetUnixTime(true);

            // 从配置读取刷新阈值（单位：毫秒）
            var jwtConfig = AppUtils.GetConfig<JwtConfig>(JwtConfig.Name);
            var refreshThreshold = (jwtConfig?.Expires ?? 60) * 60 * 1000L;

            // 未超过刷新阈值，直接放行
            if (now - jwtToken.time <= refreshThreshold)
            {
                await _next(context);
                return;
            }

            // 超过刷新阈值，签发新 Token 通过响应头返回（无感续期）
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

            await _next(context);
        }

        /// <summary>
        /// 适用于应用，使用绑定登录（统一调用 ScmToken.FromAppToken 解析）
        /// </summary>
        /// <param name="context"></param>
        /// <param name="holder"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private async Task AppToken(HttpContext context, ScmContextHolder holder, string token)
        {
            var scmToken = ScmToken.FromAppToken(token);
            holder.SetToken(scmToken);

            await _next(context);
        }
    }
}
