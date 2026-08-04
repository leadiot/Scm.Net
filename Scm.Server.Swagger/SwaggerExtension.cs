using Com.Scm.Config;
using Com.Scm.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Com.Scm
{
    public static class SwaggerExtension
    {
        /// <summary>
        /// 兜底文档名：收纳未分组端点与不在 ApiDocs 配置中的孤儿分组，确保所有接口在 Swagger 中可见
        /// </summary>
        private const string OTHER_DOC_NAME = "other";

        private const string OTHER_DOC_TITLE = "其他接口";

        private const string DEFAULT_DOC_TITLE = "Scm.Net API 接口";

        public static void SwaggerSetup(this IServiceCollection services, SwaggerConfig config)
        {
            if (config == null)
            {
                return;
            }

            services.AddTransient<BearerSecuritySchemeTransformer>();

            if (config.HasDocs())
            {
                var configuredGroups = config.ApiDocs
                    .Select(d => d.Group)
                    .Where(g => !string.IsNullOrEmpty(g))
                    .ToList();

                foreach (var apiDoc in config.ApiDocs)
                {
                    var groupName = apiDoc.Group;
                    // 文档名统一以小写注册：AddOpenApi 内部会将文档名规范化为小写注册命名选项/键控服务，
                    // 直接使用小写可保证注册、路由解析、Swagger 下拉框三个环节名称一致
                    services.AddOpenApi(groupName, options =>
                    {
                        options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                        // 用配置的中文标题/描述/版本回填文档 info（否则默认为 "{AppName} | {documentName}"）
                        options.AddDocumentTransformer(new OpenApiInfoTransformer(config, apiDoc));
                        // 只收纳对应分组的端点（忽略大小写比较，避免 GroupName 与文档名大小写不一致）
                        options.ShouldInclude = description => string.Equals(description.GroupName, groupName, StringComparison.OrdinalIgnoreCase);
                        options.ApplyXmlComments(config);
                    });
                }

                // 兜底文档：收纳未分组端点（GroupName == null）与配置之外的孤儿分组，
                // 否则这部分接口不会出现在任何文档中
                services.AddOpenApi(OTHER_DOC_NAME, options =>
                {
                    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                    options.AddDocumentTransformer(new OpenApiInfoTransformer(config, new ApiInfo
                    {
                        Title = OTHER_DOC_TITLE,
                        Description = "未配置分组的接口（含未分组端点与未纳入 ApiDocs 的分组）",
                        Version = config.Version
                    }));
                    options.ShouldInclude = description =>
                        description.GroupName == null ||
                        !configuredGroups.Any(g => string.Equals(description.GroupName, g, StringComparison.OrdinalIgnoreCase));
                    options.ApplyXmlComments(config);
                });
            }
            else
            {
                // 注册默认文档 "v1"
                services.AddOpenApi("v1", options =>
                {
                    options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
                    options.AddDocumentTransformer(new OpenApiInfoTransformer(config, null));
                    options.ApplyXmlComments(config);
                });
            }
        }

        public static void UseSwaggerSetup(this WebApplication app, SwaggerConfig config)
        {
            if (config == null)
            {
                return;
            }

            var openApiRoute = string.IsNullOrWhiteSpace(config.OpenApiRoute) ? "/openapi/{documentName}.json" : config.OpenApiRoute;
            app.MapOpenApi(openApiRoute);

            // SwaggerUI 的文档端点必须与 MapOpenApi 的路由模板匹配，
            // 文档名与 AddOpenApi 注册的名称保持一致（首个文档为默认展示文档）
            app.UseSwaggerUI(options =>
            {
                if (config.HasDocs())
                {
                    foreach (var apiDoc in config.ApiDocs)
                    {
                        options.SwaggerEndpoint($"/openapi/{apiDoc.Group}.json", apiDoc.Title);
                    }
                    // 兜底文档
                    options.SwaggerEndpoint($"/openapi/{OTHER_DOC_NAME}.json", OTHER_DOC_TITLE);
                }
                else
                {
                    options.SwaggerEndpoint($"/openapi/v1.json", config.Title ?? DEFAULT_DOC_TITLE);
                }

                options.EnablePersistAuthorization(); // 持久化授权 Token
                options.DisplayRequestDuration();     // 显示请求耗时
                options.EnableTryItOutByDefault();    // 默认展开“Try it out”
                options.EnableFilter();               // 启用接口过滤
                options.DocExpansion(DocExpansion.List);
                options.DefaultModelsExpandDepth(0);
            });
        }

        /// <summary>
        /// 应用 XML 文档注释（读取程序集生成的 XML 文件填充接口摘要与说明）
        /// </summary>
        private static void ApplyXmlComments(this OpenApiOptions options, SwaggerConfig config)
        {
            if (config?.DllXmls == null || config.DllXmls.Count == 0)
            {
                return;
            }

            var files = config.DllXmls
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => Path.IsPathRooted(x) ? x : Path.Combine(AppContext.BaseDirectory, x))
                .Where(File.Exists)
                .ToList();

            if (files.Count == 0)
            {
                return;
            }

            options.AddOperationTransformer(new XmlCommentsOperationTransformer(files));
        }
    }
}
