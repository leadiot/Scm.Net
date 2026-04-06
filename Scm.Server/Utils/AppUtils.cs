using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Com.Scm.Utils;

public static class AppUtils
{
    /// <summary>
    /// 注入对象服务提供类
    /// </summary>
    public static IServiceProvider ServiceProvider { get; set; }

    public static IConfiguration Configuration { get; private set; }

    public static void Init(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public static IConfigurationSection GetConfig(string path)
    {
        return Configuration.GetSection(path);
    }

    public static T GetConfig<T>(string path)
    {
        return Configuration.GetSection(path).Get<T>();
    }

    public static T GetConfig<T>(string path, T def)
    {
        return Configuration.GetSection(path).Get<T>() ?? def;
    }

    /// <summary>
    /// 手动获取注入的对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetService<T>() where T : class
    {
        return ServiceProvider?.GetService<IHttpContextAccessor>()?.HttpContext.RequestServices.GetService<T>();
    }
}
