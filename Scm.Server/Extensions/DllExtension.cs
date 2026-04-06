using Com.Scm.DynamicWebApi;
using Com.Scm.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Com.Scm.Extensions
{
    public static class DllExtension
    {
        public static void RegisterServices(this IServiceCollection services, DllConfig config)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // application services
            if (config.Service != null)
            {
                foreach (var file in config.Service)
                {
                    LoadService(services, file);
                }
            }

            // dynamic webapi
            services.AddDynamicWebApi();
        }

        private static void LoadService(IServiceCollection services, string dll)
        {
            try
            {
                var assemblyService = Assembly.Load(dll);
                var serviceType = assemblyService.GetTypes().Where(u => u.IsClass && !u.IsAbstract && !u.IsGenericType && u.Name.EndsWith("Service")).ToList();
                foreach (var item in serviceType.Where(s => !s.IsInterface))
                {
                    services.AddScoped(item);
                }
            }
            catch (Exception ex)
            {
                LogUtils.Error(ex);
            }
        }
    }
}