using Com.Scm.Config;
using Com.Scm.Configure.Filters;
using Com.Scm.Configure.Middleware;
using Com.Scm.Dsa;
using Com.Scm.Dsa.Dba.Sugar.UnitOfWork.Filters;
using Com.Scm.Email.Config;
using Com.Scm.Extensions;
using Com.Scm.Generator.Config;
using Com.Scm.Holder;
using Com.Scm.Hubs;
using Com.Scm.Image.ImageSharp;
using Com.Scm.Login.Otp;
using Com.Scm.Mapper;
using Com.Scm.Nas;
using Com.Scm.Phone.Config;
using Com.Scm.Quartz;
using Com.Scm.Quartz.Config;
using Com.Scm.Samples;
using Com.Scm.Server;
using Com.Scm.Service;
using Com.Scm.Uid.Config;
using Com.Scm.Utils;
using Microsoft.Extensions.FileProviders;
using Serilog;
using SqlSugar;

namespace Com.Scm
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            AppUtils.Init(builder.Configuration);

            // LOG配置
            Serilog.Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration)
                .CreateLogger();

            LogUtils.Info("正在启动系统...");

            var services = builder.Services;

            // 环境变量
            LogUtils.Info("正在进行环境配置...");
            var envConfig = AppUtils.GetConfig<EnvConfig>(EnvConfig.NAME) ?? new EnvConfig();
            envConfig.Prepare(builder);
            services.AddSingleton(envConfig);

            // 单位配置
            RenameFile(envConfig, "unit-origin.json", "unit.json");

            // Uid配置
            LogUtils.Info("正在进行Uid配置...");
            RenameFile(envConfig, "uid-origin.db", "uid.db");
            var uidConfig = AppUtils.GetConfig<UidConfig>(UidConfig.NAME);
            UidUtils.InitConfig(uidConfig);

            // Sql配置
            LogUtils.Info("正在进行Sql配置...");
            RenameFile(envConfig, "scm-origin.db", "scm.db");
            var sqlConfig = AppUtils.GetConfig<SqlConfig>(SqlConfig.NAME) ?? new SqlConfig();
            sqlConfig.Prepare(envConfig);
            SqlSetup(services, envConfig, sqlConfig);

            // 字体配置
            LogUtils.Info("正在进行字体配置...");
            FontSetup(services, envConfig);

            // 缓存配置
            services.CacheSetup(envConfig);

            // Swagger配置
            var swaggerConfig = AppUtils.GetConfig<SwaggerConfig>(SwaggerConfig.NAME);
            services.SwaggerSetup(swaggerConfig);

            // 数据配置
            //var dataConfig = AppUtils.GetConfig<DataConfig>(DataConfig.NAME) ?? new DataConfig();
            //dataConfig.Prepare(builder.Environment);
            //services.AddSingleton(dataConfig);

            // 安全配置
            LogUtils.Info("正在进行安全配置...");
            var secConfig = AppUtils.GetConfig<SecurityConfig>(SecurityConfig.NAME);
            secConfig.Prepare(builder.Environment);
            services.AddSingleton(secConfig);

            // 代码生成
            var genConfig = AppUtils.GetConfig<GeneratorConfig>(GeneratorConfig.NAME);
            genConfig.Prepare(envConfig);
            services.GeneratorSetup(genConfig);

            // Quartz
            var quartzConfig = AppUtils.GetConfig<QuartzConfig>(QuartzConfig.NAME) ?? new QuartzConfig();
            quartzConfig.Prepare(envConfig);
            services.QuartzSetup(quartzConfig);
            services.AddQuartzClassJobs();

            // EMail
            var emailConfig = AppUtils.GetConfig<EmailConfig>(EmailConfig.NAME) ?? new EmailConfig();
            emailConfig.Prepare(envConfig);
            services.AddSingleton(emailConfig);

            // Phone
            var phoneConfig = AppUtils.GetConfig<PhoneConfig>(PhoneConfig.NAME) ?? new PhoneConfig();
            phoneConfig.Prepare(envConfig);
            services.AddSingleton(phoneConfig);

            // Aiml
            var aimlConfig = AppUtils.GetConfig<AimlConfig>(AimlConfig.NAME) ?? new AimlConfig();
            aimlConfig.Prepare(envConfig);
            services.AddSingleton(aimlConfig);

            // Oidc
            var oidcConfig = AppUtils.GetConfig<OidcConfig>(OidcConfig.NAME) ?? new OidcConfig();
            oidcConfig.Prepare(envConfig);
            services.AddSingleton(oidcConfig);

            // Otp
            var otpConfig = AppUtils.GetConfig<OtpConfig>(OtpConfig.NAME) ?? new OtpConfig();
            otpConfig.Prepare(envConfig);
            services.AddSingleton(otpConfig);

            // Cors
            var corsConfig = AppUtils.GetConfig<CorsConfig>(CorsConfig.NAME);
            if (corsConfig != null)
            {
                corsConfig.Prepare(envConfig);
            }

            LogUtils.Info("正在进行服务配置...");
            services.AddScoped<IResHolder, ScmResHolder>();
            services.AddScoped<ILogService, ScmLogService>();
            services.AddScoped<IDicService, ScmDicService>();
            services.AddScoped<ICfgService, ScmCfgService>();
            services.AddScoped<ISecService, ScmSecService>();
            services.AddScoped<ICatService, ScmCatService>();
            services.AddScoped<ITagService, ScmTagService>();
            services.AddScoped<IFlowService, ScmFlowService>();

            // 自定义服务
            SamplesServerUtils.Setup(services);

            // 全局过滤
            services.AddControllers(options =>
            {
                options.Filters.Add<AopActionFilter>();
                options.Filters.Add<GlobalExceptionFilter>();
                options.Filters.Add<UnitOfWorkFilter>();
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            }).NewtonJsonSetup();

            // 接口配置
            var apiConfig = AppUtils.GetConfig<DllConfig>(DllConfig.NAME);
            apiConfig.Prepare(builder.Environment);
            services.RegisterServices(apiConfig);

            // Jwt Config
            services.SetupJwt(envConfig);

            // 跨域访问
            services.CorsSetup(corsConfig);

            // SignalR
            services.AddSignalR();

            // Mapper
            services.AddMapperProfile();

            var app = builder.Build();

            AppUtils.ServiceProvider = app.Services;

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerSetup(swaggerConfig);
            }

            app.UseHttpsRedirection();
            //app.UseDefaultFiles(new DefaultFilesOptions
            //{
            //    DefaultFileNames = new List<string>
            //    {
            //        "index.html"
            //    }
            //});
            //app.UseStaticFiles();
            app.UseFileServer();

            if (!string.IsNullOrEmpty(envConfig.DataUri))
            {
                app.UseFileServer(new FileServerOptions
                {
                    FileProvider = new PhysicalFileProvider(envConfig.DataDir),
                    RequestPath = envConfig.DataUri,
                });
            }

            app.UseRouting();

            // 跨域设置
            LogUtils.Info("正在进行跨域设置...");
            if (corsConfig != null)
            {
                if (corsConfig.GlobalCors)
                {
                    app.UseCors(ScmEnv.SCM_CORS);
                }
                else
                {
                    app.UseCors();
                }
            }

            app.Use(next => async context =>
            {
                context.Request.EnableBuffering();
                await next(context);
            });

            // 认证
            app.UseAuthentication();
            // 授权
            app.UseAuthorization();

            // 中间件异常处理
            app.UseMiddleware<ExceptionMiddleware>();
            // Jwt中间件处理
            app.UseMiddleware<JwtMiddleware>();

            app.UseQuartz();

            app.MapControllers().RequireAuthorization();
            app.MapHub<ScmHub>("/scmhub");

            var kestrelConfig = AppUtils.GetConfig<KestrelConfig>(KestrelConfig.NAME);
            if (kestrelConfig != null)
            {
                var url = kestrelConfig?.Endpoints?.Http?.Url;
                if (url == null)
                {
                    url = "http://*:9999";
                }
                url = url.Replace("*", "localhost");
                LogUtils.Info("系统启动完成，您可以通过以下地址访问系统：" + url);
            }
            else
            {
                LogUtils.Info("系统启动完成！");
            }

            LogUtils.Info("===========");

            app.Run();
        }

        private static void RenameFile(EnvConfig envConfig, string src, string dst)
        {
            var dstFile = envConfig.GetDataPath(dst);
            if (File.Exists(dstFile))
            {
                return;
            }

            var srcFile = envConfig.GetDataPath(src);
            if (!File.Exists(srcFile))
            {
                return;
            }

            File.Move(srcFile, dstFile);
        }

        /// <summary>
        /// 注册单例服务
        /// </summary>
        /// <param name="services"></param>
        public static void SqlSetup(IServiceCollection services, EnvConfig envConfig, SqlConfig sqlConfig)
        {
            //LogUtils.Info("正在初始化数据库...");

            var dbType = SqlSugarUtils.GetDbType(sqlConfig.Type);
            SqlSugarScope sugarScope = new SqlSugarScope(new ConnectionConfig()
            {
                DbType = dbType,
                ConnectionString = sqlConfig.Text,
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

                        if (dbType == DbType.Sqlite)
                        {
                            if (c.PropertyType.IsEnum)
                            {
                                p.DataType = "INTEGER";
                            }
                            else if (c.PropertyType == typeof(long))
                            {
                                p.DataType = "INTEGER";
                            }
                        }
                        else
                        {
                            if (c.PropertyType.IsEnum)
                            {
                                p.DataType = "TINYINT";
                            }
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

            var sqlDir = envConfig.GetDataPath("sql");
            IModelHelper dbHelper = new ScmDbHelper();
            dbHelper.Init(sugarScope, sqlDir);
            dbHelper.InitDb();

            dbHelper = new SamplesDbHelper();
            dbHelper.Init(sugarScope, sqlDir);
            dbHelper.InitDb();

            dbHelper = new NasDbHelper();
            dbHelper.Init(sugarScope, sqlDir);
            dbHelper.InitDb();
            //LogUtils.Info("数据库初始化完成！");

            // 单例注册SqlSugar
            services.AddSingleton<ISqlSugarClient>(sugarScope);
            //注册仓储
            services.AddScoped(typeof(SugarRepository<>));
        }

        public static void FontSetup(IServiceCollection services, EnvConfig envConfig)
        {
            LogUtils.Info("字体目录：" + envConfig.Fonts);
            ImageEngine.LoadFont(envConfig.Fonts);
            LogUtils.Info("默认字体：" + envConfig.DefaultFont);
            ImageEngine.SetDefaultFontName(envConfig.DefaultFont);
        }
    }
}