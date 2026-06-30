using Com.Scm.Config;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Com.Scm
{
    public static class SwaggerExtension
    {
        public static void SwaggerSetup(this IServiceCollection services, SwaggerConfig config)
        {
            if (config == null)
            {
                return;
            }

            services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>();
            });
        }

        public static void UseSwaggerSetup(this WebApplication app, SwaggerConfig config)
        {
            if (config == null)
            {
                return;
            }

            app.MapOpenApi();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/openapi/v1.json", "v1");
                options.EnablePersistAuthorization(); // 持久化授权 Token
                options.DisplayRequestDuration();     // 显示请求耗时
                options.EnableTryItOutByDefault();    // 默认展开“Try it out”
                options.EnableFilter();               // 启用接口过滤
                options.DocExpansion(DocExpansion.List);
                options.DefaultModelsExpandDepth(0);
            });
        }
    }
}