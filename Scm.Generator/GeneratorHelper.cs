using Com.Scm.Generator;
using Com.Scm.Generator.Config;
using Microsoft.Extensions.DependencyInjection;

namespace Com.Scm
{
    public static class GeneratorHelper
    {
        public static void GeneratorSetup(this IServiceCollection services, GeneratorConfig config)
        {
            // code generator
            services.AddSingleton(config);
            services.AddScoped<IGeneratorService, GeneratorService>();
        }
    }
}
