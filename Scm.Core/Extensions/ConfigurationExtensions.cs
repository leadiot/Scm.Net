using Com.Scm.Configure.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Com.Scm.Extensions
{
    /// <summary>
    /// 配置扩展方法
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// 配置安全配置服务
        /// </summary>
        public static IServiceCollection AddSecureConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<SecretConfigurationService>();
            return services;
        }

        /// <summary>
        /// 配置应用配置（支持环境变量和 User Secrets）
        /// </summary>
        /// <typeparam name="T">用于确定 User Secrets ID 的类型（通常是 Program）</typeparam>
        public static IConfigurationBuilder AddAppConfiguration<T>(this IConfigurationBuilder builder, IWebHostEnvironment env) where T : class
        {
            // 基础配置文件
            builder.SetBasePath(env.ContentRootPath)
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            // 开发环境添加 User Secrets
            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<T>(optional: true);
            }

            // 环境变量覆盖
            builder.AddEnvironmentVariables("SCM__");

            return builder;
        }

        /// <summary>
        /// 获取敏感配置值（从环境变量或 Secrets）
        /// </summary>
        public static string GetSecret(this IConfiguration configuration, string key)
        {
            // 优先从环境变量读取
            var envKey = "SCM__" + key.Replace(":", "__").Replace(".", "__").ToUpperInvariant();
            var envValue = Environment.GetEnvironmentVariable(envKey);
            
            if (!string.IsNullOrWhiteSpace(envValue))
            {
                return envValue;
            }

            return configuration[key];
        }

        /// <summary>
        /// 获取敏感配置值，带默认值
        /// </summary>
        public static string GetSecret(this IConfiguration configuration, string key, string defaultValue)
        {
            var value = configuration.GetSecret(key);
            return string.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }
    }
}
