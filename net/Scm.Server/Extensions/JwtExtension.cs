using Com.Scm.Config;
using Com.Scm.Token;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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

            services.AddAuthentication(x =>
            {
                // 指定默认认证方案（必须配置，与后续中间件联动）
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // 认证失败时的默认挑战方案
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                // 验证令牌的有效性参数
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    // 验证签发者
                    ValidateIssuer = true,
                    // 签发者（与令牌签发时一致）
                    ValidIssuer = jwtConfig.Issuer,
                    // 验证受众
                    ValidateAudience = true,
                    // 受众（与令牌签发时一致）
                    ValidAudience = jwtConfig.Audience,
                    // 验证签名密钥
                    ValidateIssuerSigningKey = true,
                    // 签名密钥（与令牌签发时一致，必须保密）
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Security)),
                    // 验证令牌过期时间
                    ValidateLifetime = true,
                    RequireExpirationTime = true,
                    /*AudienceValidator = (m, n, z) => 
                        m != null && m.FirstOrDefault()!.Equals(JwtConst.ValidAudience),*/
                };
                x.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        // 令牌验证失败时的自定义逻辑（如打印日志）
                        var values = context.Request.Headers[ScmToken.ApiToken];
                        context.Token = values.FirstOrDefault();
                        return Task.CompletedTask;
                    },
                };
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("App", policy => policy.RequireRole("App").Build());
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin").Build());
            });
        }
    }
}