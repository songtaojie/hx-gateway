// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using Serilog;
using Serilog.Events;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Serilog日志扩展
/// </summary>
public static class SerilogServiceCollectionExtensions
{
    /// <summary>
    /// 添加Serilog日志
    /// </summary>
    /// <param name="builder"></param>
    public static void UseDefaultSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((builderContext,loggerConfig) =>
        {
            LoggerConfiguration loggerConfiguration = loggerConfig.ReadFrom
                .Configuration(builderContext.Configuration)
                .Enrich.WithProperty("AppName", builder.Configuration["Serilog:AppName"] ?? "Gateway Api")
                .Enrich.FromLogContext();

            if (builderContext.Configuration["Serilog:WriteTo:0:Name"] == null)
            {
                loggerConfiguration.WriteTo.Console(LogEventLevel.Verbose, "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}").WriteTo.File(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs", "application.log"), LogEventLevel.Information, "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}", null, retainedFileCountLimit: null, encoding: Encoding.UTF8, fileSizeLimitBytes: 1073741824L, levelSwitch: null, buffered: false, shared: false, flushToDiskInterval: null, rollingInterval: RollingInterval.Day);
            }
        });
    }
}
