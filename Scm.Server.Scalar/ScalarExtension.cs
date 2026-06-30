using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Scalar.AspNetCore;
using Scm.Server.Scalar.Config;

namespace Scm.Server.Scalar
{
    public static class ScalarExtension
    {
        public static void ScalarSetup(this IServiceCollection services, ScalarConfig config)
        {
            services.AddOpenApi();
        }


        public static void UseScalarSetup(this WebApplication app, ScalarConfig config)
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }
    }
}
