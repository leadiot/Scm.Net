using Com.Scm.Config;
using Com.Scm.Token;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Com.Scm
{
    public static class JwtExtension
    {
        public static void SetupJwt(this IServiceCollection services, EnvConfig envConfig)
        {
            services.AddScoped(typeof(ScmContextHolder));

            var section = AppUtils.GetConfig(JwtConfig.Name);
            services.Configure<JwtConfig>(section);
            var jwtConfig = section.Get<JwtConfig>();
            jwtConfig.Prepare(envConfig);

            // 构建统一的令牌验证参数（三个 JWT 类方案共用）
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwtConfig.Issuer,
                ValidateAudience = true,
                ValidAudience = jwtConfig.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Security)),
                ValidateLifetime = true,
                RequireExpirationTime = true,
            };

            // 三种方案统一使用 Authorization 请求头，通过 scheme 前缀区分（RFC 7235）
            // Authorization: Bearer <jwt>  → 方案1（标准 JWT，第三方集成 / OpenAPI）
            // Authorization: Api <jwt>     → 方案2（自定义 JWT，Web 前端）
            // Authorization: App <base64>  → 方案3（设备绑定令牌，IoT / 移动端）
            // 认证链按 Bearer → Api → App 顺序依次尝试，任一成功即放行
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // ── 方案 1: Bearer — ASP.NET Core 内置 JwtBearerHandler，天然识别 "Bearer" scheme ──
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParameters;
            })
            // ── 方案 2: Api — 自定义 Handler，识别 "Api" scheme ──
            .AddScheme<AuthenticationSchemeOptions, ApiTokenHandler>(ApiTokenHandler.SchemeName, null)
            // ── 方案 3: App — 自定义 Handler，识别 "App" scheme ──
            .AddScheme<AuthenticationSchemeOptions, AppTokenHandler>(AppTokenHandler.SchemeName, null);

            services.AddAuthorization(options =>
            {
                // 默认策略接受三种认证方案，依次尝试：Bearer → ApiToken → AppToken
                // RequireAuthorization() 全局生效时，任一方案通过即可访问
                options.DefaultPolicy = new AuthorizationPolicyBuilder()
                    .AddAuthenticationSchemes(
                        JwtBearerDefaults.AuthenticationScheme,
                        ApiTokenHandler.SchemeName,
                        AppTokenHandler.SchemeName)
                    .RequireAuthenticatedUser()
                    .Build();

                options.AddPolicy("App", policy => policy.RequireRole("App").Build());
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
            });
        }
    }
}
