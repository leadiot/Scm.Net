using Com.Scm.Config;
using Com.Scm.Dsa;
using Com.Scm.Utils;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Com.Scm.Server
{
    public static class SugarExtension
    {
        /// <summary>
        /// 注册单例服务
        /// </summary>
        /// <param name="services"></param>
        public static void SqlSugarSetup(this IServiceCollection services, SqlConfig config)
        {
            if (config == null)
            {
                return;
            }

            // 单例注册SqlSugar
            services.AddSingleton<ISqlSugarClient>(provider =>
            {
                SqlSugarScope sugarScope = new SqlSugarScope(new ConnectionConfig()
                {
                    DbType = SqlSugarUtils.GetDbType(config.Type),
                    ConnectionString = config.Text,
                    InitKeyType = InitKeyType.Attribute,
                    IsAutoCloseConnection = true,
                    ConfigureExternalServices = new ConfigureExternalServices
                    {
                        EntityService = (c, p) =>
                        {
                            if (c.PropertyType.IsGenericType && c.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                            {
                                p.IsNullable = true;
                            }
                        }
                    }
                },
                db =>
                {
                    //每次Sql执行前事件
                    db.Aop.OnLogExecuting = (s, p) =>
                    {
                        //var sqlValue = string.Empty;
                        var sql = s;
                        foreach (var item in p)
                        {
                            sql = sql.Replace(item.ParameterName, "'" + item.Value + "'");
                        }
                        //LogUtils.Debug("Sql脚本：" + sql, "db");
                    };
                });
                return sugarScope;
            });
            //注册仓储
            services.AddScoped(typeof(SugarRepository<>));
        }
    }
}