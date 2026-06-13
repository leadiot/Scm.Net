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

            services.AddAuthentication(x =>
            {
                // 默认认证方案：标准 JwtBearer（Authorization: Bearer xxx）
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            // ── 方案 1: Bearer（标准 JWT，适用于第三方集成 / OpenAPI / 标准客户端）──
            // 从 Authorization: Bearer xxx 请求头读取，ASP.NET Core 默认行为
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = tokenValidationParameters;
            })
            // ── 方案 2: ApiToken（自定义 JWT，适用于 Web 前端）──
            // 从 ApiToken: Api xxx 请求头读取，自动剥离前缀后验证签名
            .AddScheme<AuthenticationSchemeOptions, ApiTokenHandler>(ApiTokenHandler.SchemeName, null)
            // ── 方案 3: AppToken（设备绑定令牌，适用于 IoT / 移动端）──
            // 从 AppToken: App base64(...) 请求头读取，解析终端绑定信息
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
