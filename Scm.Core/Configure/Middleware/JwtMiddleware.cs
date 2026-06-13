using Com.Scm.Config;
using Com.Scm.Token;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Http;

namespace Com.Scm.Configure.Middleware
{
    /// <summary>
    /// JWT 中间件 — 从 Authorization 请求头按 scheme 前缀分发，
    /// 解析令牌并注入 ScmContextHolder 供业务层使用。
    /// 同时负责 Api 方案的无感续期逻辑。
    /// </summary>
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

                // 统一从 Authorization 请求头读取令牌
                var authHeader = context.Request.Headers.Authorization.FirstOrDefault();

                if (!string.IsNullOrEmpty(authHeader))
                {
                    var scheme = ScmToken.GetScheme(authHeader);

                    if (string.Equals(scheme, ScmToken.SCHEME_API, StringComparison.OrdinalIgnoreCase))
                    {
                        // Api 方案：Authorization: Api <jwt>
                        var credentials = ScmToken.GetCredentials(authHeader, ScmToken.SCHEME_API);
                        await HandleApiToken(context, holder, credentials);
                        return;
                    }

                    if (string.Equals(scheme, ScmToken.SCHEME_APP, StringComparison.OrdinalIgnoreCase))
                    {
                        // App 方案：Authorization: App <base64>
                        var credentials = ScmToken.GetCredentials(authHeader, ScmToken.SCHEME_APP);
                        await HandleAppToken(context, holder, credentials);
                        return;
                    }

                    // Bearer 方案：Authorization: Bearer <jwt>
                    // JwtBearerHandler 已完成签名验证，此处仅需解析 Claims 注入上下文
                    if (string.Equals(scheme, ScmToken.SCHEME_BEARER, StringComparison.OrdinalIgnoreCase))
                    {
                        var credentials = ScmToken.GetCredentials(authHeader, ScmToken.SCHEME_BEARER);
                        await HandleBearerToken(context, holder, credentials);
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
        /// 处理 Bearer 方案令牌（标准 JWT，解析 Claims 注入上下文）
        /// </summary>
        private async Task HandleBearerToken(HttpContext context, ScmContextHolder holder, string token)
        {
            var jwtToken = JwtUtils.SerializeJwt(token);
            holder.SetToken(jwtToken);
            await _next(context);
        }

        /// <summary>
        /// 处理 Api 方案令牌（自定义 JWT，含无感续期逻辑）
        /// </summary>
        private async Task HandleApiToken(HttpContext context, ScmContextHolder holder, string token)
        {
            var jwtToken = JwtUtils.SerializeJwt(token);
            holder.SetToken(jwtToken);

            var now = TimeUtils.GetUnixTime(true);

            // 从配置读取刷新阈值（单位：毫秒）
            var jwtConfig = AppUtils.GetConfig<JwtConfig>(JwtConfig.Name);
            var refreshThreshold = (jwtConfig?.Expires ?? 30) * 60 * 1000L;

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
                time = now,
                data = jwtToken.data,
            });
            context.Response.Headers.Append("X-Refresh-Token", newToken);

            await _next(context);
        }

        /// <summary>
        /// 处理 App 方案令牌（设备绑定令牌，解析终端信息注入上下文）
        /// </summary>
        private async Task HandleAppToken(HttpContext context, ScmContextHolder holder, string token)
        {
            var scmToken = ScmToken.FromAppToken(token);
            holder.SetToken(scmToken);

            await _next(context);
        }
    }
}
