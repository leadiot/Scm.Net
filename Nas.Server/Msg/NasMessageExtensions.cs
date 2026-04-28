using Microsoft.Extensions.DependencyInjection;

namespace Com.Scm.Nas.Msg
{
    public static class NasMessageExtensions
    {
        /// <summary>
        /// 注册 NAS 消息服务
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <returns></returns>
        public static IServiceCollection AddNasMessageService(this IServiceCollection services)
        {
            services.AddScoped<NasMessageService>();
            return services;
        }
    }
}
