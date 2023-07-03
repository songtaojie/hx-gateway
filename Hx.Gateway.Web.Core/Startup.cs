using Furion.SpecificationDocument;
using Hx.Core;
using Hx.Gateway.Application.Services.Consul.Hubs;
using IGeekFan.AspNetCore.Knife4jUI;
using Microsoft.AspNetCore.HttpOverrides;
using NETCore.MailKit.Extensions;
using OnceMi.AspNetCore.OSS;
using System.IO;
using System.Text.Json.Serialization;
using System.Text.Json;
using Yitter.IdGenerator;
using Microsoft.Extensions.Logging;
using Hx.Gateway.Application.Options;
using SixLabors.ImageSharp;

namespace Hx.Gateway.Web.Core
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHxCoreOptions();
            // 缓存注册
            services.AddCache();
            // SqlSugar
            services.AddSqlSugar();

            services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);
            services.AddConfigurableOptions<LoginSettingsOptions>();
            services.AddConfigurableOptions<OcelotSettingsOptions>();
            services.AddRemoteRequest();

            services.AddCorsAccessor();

            services.AddControllers()
                .AddAppLocalization()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.Converters.AddDateTimeTypeConverters("yyyy-MM-dd HH:mm:ss");
                    options.JsonSerializerOptions.Converters.AddLongTypeConverters();
                })
                .AddInjectWithUnifyResult<HxUnifyResultProvider>();

            //定时任务/后台任务 服务注册
            //services.AddTaskScheduler();
            // 控制台格式化
            services.AddConsoleFormatter(options =>
            {
                options.DateFormat = "yyyy-MM-dd HH:mm:ss(zzz) dddd";
            });
            // 日志监听
            services.AddMonitorLogging(options =>
            {
                options.IgnorePropertyNames = new[] { "Byte" };
                options.IgnorePropertyTypes = new[] { typeof(byte[]) };
            });

            services.AddAuthentication();

            // 即时通讯
            services.AddSignalR();
            // logo显示
            services.AddLogoDisplay();
            // 日志记录
            if (App.GetConfig<bool>("Logging:File:Enabled")) // 日志写入文件
            {
                Array.ForEach(new[] { LogLevel.Information, LogLevel.Warning, LogLevel.Error }, logLevel =>
                {
                    services.AddFileLogging(options =>
                    {
                        options.FileNameRule = fileName => string.Format(fileName, DateTime.Now, logLevel.ToString()); // 每天创建一个文件
                        options.WriteFilter = logMsg => logMsg.LogLevel == logLevel; // 日志级别
                        options.HandleWriteError = (writeError) => // 写入失败时启用备用文件
                        {
                            writeError.UseRollbackFileName(Path.GetFileNameWithoutExtension(writeError.CurrentFileName) + "-oops" + Path.GetExtension(writeError.CurrentFileName));
                        };
                    });
                });
            }

            // 配置雪花Id算法机器码
            YitIdHelper.SetIdGenerator(new IdGeneratorOptions
            {
                WorkerId = App.GetOptions<SnowIdOptions>().WorkerId
            });

            services.AddOcelot(Configuration).AddExtOcelot(option =>
            {
                option.DbConnectionStrings = Configuration["OcelotConfig:DbConnectionStrings"];
                option.RedisConnectionString = Configuration["OcelotConfig:RedisConnectionStrings"];
                option.EnableTimer = Convert.ToBoolean(Configuration["OcelotConfig:EnableTimer"]);
                option.TimerDelay = Convert.ToInt32(Configuration["OcelotConfig:TimerDelay"]);
                option.ClientAuthorization = true;
                option.ClientRateLimit = true;
            })
            .UseSqlServer();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseForwardedHeaders();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseForwardedHeaders();
                app.UseHsts();
            }

            // 添加状态码拦截中间件
            app.UseUnifyResultStatusCodes();

            // 配置多语言
            app.UseAppLocalization();

            // 启用HTTPS
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseCorsAccessor();

            app.UseAuthentication();
            app.UseAuthorization();

            // 任务调度看板
            //app.UseScheduleUI();

            // 配置Swagger-Knife4UI（路由前缀一致代表独立，不同则代表共存）
            app.UseKnife4UI(options =>
            {
                options.RoutePrefix = "swagger";
                foreach (var groupInfo in SpecificationDocumentBuilder.GetOpenApiGroups())
                {
                    options.SwaggerEndpoint("/" + groupInfo.RouteTemplate, groupInfo.Title);
                }
            });
            app.UseInject();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapControllers();
                endpoints.MapHub<ConsulHub>("/consulhub");
                endpoints.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}