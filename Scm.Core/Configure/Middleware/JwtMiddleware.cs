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

        public Task Invoke(HttpContext context)
        {
            IJwtTokenHolder holder = AppUtils.GetService<IJwtTokenHolder>();

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

                // 统一从 Authorization 请求头读取令牌
                var authHeader = context.Request.Headers.Authorization.FirstOrDefault();

                if (!string.IsNullOrEmpty(authHeader))
                {
                    var scheme = ScmToken.GetScheme(authHeader);

                    if (string.Equals(scheme, ScmToken.SCHEME_TERMINAL, StringComparison.OrdinalIgnoreCase))
                    {
                        // App 方案：Authorization: Terminal <base64>
                        var credentials = ScmToken.GetCredentials(authHeader, ScmToken.SCHEME_TERMINAL);
                        return HandleTerminalToken(context, holder, credentials);
                    }

                    if (string.Equals(scheme, ScmToken.SCHEME_OPERATOR, StringComparison.OrdinalIgnoreCase))
                    {
                        // Api 方案：Authorization: Operator <jwt>
                        var credentials = ScmToken.GetCredentials(authHeader, ScmToken.SCHEME_OPERATOR);
                        return HandleOperatorToken(context, holder, credentials);
                    }

                    // Bearer 方案：Authorization: Bearer <jwt>
                    // JwtBearerHandler 已完成签名验证，此处仅需解析 Claims 注入上下文
                    if (string.Equals(scheme, ScmToken.SCHEME_BEARER, StringComparison.OrdinalIgnoreCase))
                    {
                        var credentials = ScmToken.GetCredentials(authHeader, ScmToken.SCHEME_BEARER);
                        return HandleBearerToken(context, holder, credentials);
                    }
                }

                LogUtils.Debug("No valid Authorization header found, proceeding without token.");
                holder.SetToken(new ScmToken());
                return _next(context);
            }
            finally
            {
                holder?.Clear();
            }
        }

        /// <summary>
        /// 处理 Bearer 方案令牌（标准 JWT，解析 Claims 注入上下文）
        /// </summary>
        private Task HandleBearerToken(HttpContext context, IJwtTokenHolder holder, string token)
        {
            LogUtils.Debug("Handling Bearer token: " + token);

            var jwtToken = JwtUtils.SerializeJwt(token);
            holder.SetToken(jwtToken);
            return _next(context);
        }

        /// <summary>
        /// 处理 Api 方案令牌（自定义 JWT，含无感续期逻辑）
        /// </summary>
        private Task HandleOperatorToken(HttpContext context, IJwtTokenHolder holder, string token)
        {
            LogUtils.Debug("Handling Operator token: " + token);

            var jwtToken = JwtUtils.SerializeJwt(token);
            holder.SetToken(jwtToken);

            var now = TimeUtils.GetUnixTime(true);

            // 从配置读取刷新阈值（单位：毫秒）
            var jwtConfig = AppUtils.GetConfig<JwtConfig>(JwtConfig.Name);
            var refreshThreshold = (jwtConfig?.Expires ?? 30) * 60 * 1000L;

            if (now - jwtToken.time > refreshThreshold)
            {
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
            }

            return _next(context);
        }

        /// <summary>
        /// 处理 App 方案令牌（设备绑定令牌，解析终端信息注入上下文）
        /// </summary>
        private Task HandleTerminalToken(HttpContext context, IJwtTokenHolder holder, string token)
        {
            LogUtils.Debug("Handling Terminal token: " + token);

            var scmToken = ScmToken.FromTerminalToken(token);
            holder.SetToken(scmToken);

            return _next(context);
        }
    }
}
