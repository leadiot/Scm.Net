using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Com.Scm
{
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
