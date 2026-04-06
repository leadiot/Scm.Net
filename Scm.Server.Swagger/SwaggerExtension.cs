using Com.Scm.Config;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Com.Scm;

public static class SwaggerExtension
{
    public static void SwaggerSetup(this IServiceCollection services, SwaggerConfig config)
    {
        if (config == null)
        {
            return;
        }

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(s =>
        {
            // 基本信息与多文档支持
            if (config.ApiDocs != null)
            {
                foreach (var doc in config.ApiDocs)
                {
                    s.SwaggerDoc(doc.Group, new OpenApiInfo
                    {
                        Version = doc.Version,
                        Title = doc.Title,
                        Description = doc.Description,
                    });
                }
            }

            s.OrderActionsBy(o => o.RelativePath);

            // XML 注释
            if (config.DllXmls != null)
            {
                foreach (var file in config.DllXmls)
                {
                    var path = Path.Combine(AppContext.BaseDirectory, file);
                    if (File.Exists(path))
                    {
                        s.IncludeXmlComments(path, true);
                    }
                }
            }

            // 解决模型名称冲突
            s.CustomSchemaIds(type => type.FullName);

            // JWT Bearer 定义（更标准的 Bearer 形式）
            var schemeId = "ScmBearer";
            s.AddSecurityDefinition(schemeId, new OpenApiSecurityScheme
            {
                Name = "Authorization",
                //Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                Description = "请输入认证令牌（格式：Bearer {Token} 或直接输入Token）",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT"
            });

            // 3. 配置全局安全要求（让所有接口默认携带该 Authorization 头部，也可单独给接口配置）
            s.AddSecurityRequirement(document => new()
            {
                [new OpenApiSecuritySchemeReference(schemeId, document)] = []
            });

            // 对有 [Authorize] 的接口，自动添加 401/403 响应和 Security 要求
            s.OperationFilter<AuthResponsesOperationFilter>();
        });
    }

    public static void UseSwaggerSetup(this IApplicationBuilder app, SwaggerConfig config)
    {
        if (app == null)
        {
            throw new ArgumentNullException(nameof(app));
        }
        if (config == null)
        {
            return;
        }

        app.UseSwagger();
        // Swagger UI
        app.UseSwaggerUI(c =>
        {
            // 可选：设置 Swagger UI 为应用首页（访问根路径 "/" 直接进入 Swagger）
            // 可选：自定义路由前缀（若 config 中提供）
            var prefix = GetRoutePrefix(config);
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                c.RoutePrefix = prefix;
            }

            foreach (var doc in config.ApiDocs)
            {
                c.SwaggerEndpoint($"/swagger/{doc.Group}/swagger.json", $"{doc.Title} {doc.Version}");
            }

            // UI 友好设置
            c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
            c.EnableDeepLinking();
        });
    }

    // 从配置中获取 RoutePrefix（如果没有则返回 null）
    private static string GetRoutePrefix(SwaggerConfig config)
    {
        // 如果将来需要在 SwaggerConfig 添加 RoutePrefix 字段，可在此读取并返回
        return string.Empty;
    }

    // 为带有 [Authorize] 的操作自动添加 401/403 响应和 security requirement
    internal class AuthResponsesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation == null || context == null)
            {
                return;
            }

            var hasAuthorize = (context.MethodInfo?.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ?? false)
                               || (context.MethodInfo?.DeclaringType?.GetCustomAttributes(true).OfType<AuthorizeAttribute>().Any() ?? false);

            if (!hasAuthorize)
            {
                return;
            }

            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

            operation.Security ??= new List<OpenApiSecurityRequirement>();

            //var scheme = new OpenApiSecurityScheme
            //{
            //    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            //};

            //operation.Security.Add(new OpenApiSecurityRequirement
            //{
            //    [scheme] = new List<string>()
            //});
        }
    }
}