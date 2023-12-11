// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using Hx.Gateway.Core.Cache;
using Hx.Gateway.Core.Configuration;
using Hx.Gateway.Core.Configuration.Repository;
using Hx.Gateway.Core.Const;
using Hx.Gateway.Core.Options;
using Hx.Sdk.Sqlsugar;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Ocelot.Cache;
using Ocelot.Configuration;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.DependencyInjection;
using Ocelot.Values;
using System.Configuration;

namespace Microsoft.Extensions.DependencyInjection;
public static class HxOcelotServiceCollectionExtensions
{
    /// <summary>
    /// 添加默认的注入方式，所有需要传入的参数都是用默认值
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration">配置</param>
    /// <param name="ocelotOptionAction">配置信息</param>
    /// <returns></returns>
    public static IServiceCollection AddHxOcelot(this IServiceCollection services, IConfiguration configuration, Action<OcelotSettingsOptions> ocelotOptionAction = default)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));
        if (configuration == null) throw new ArgumentNullException(nameof(configuration));
        var ocelotSettingsOptions = configuration.GetSection("OcelotSettings").Get<OcelotSettingsOptions>();
        services.AddOcelot(configuration)
            .AddHxOcelotCore(options => 
            {
                if (ocelotSettingsOptions != null)
                {
                    options.ProjectCode = ocelotSettingsOptions.ProjectCode;
                    options.ClusterEnvironment = ocelotSettingsOptions.ClusterEnvironment;
                    options.CacheTime = ocelotSettingsOptions.CacheTime;
                    options.ClientAuthorization = ocelotSettingsOptions.ClientAuthorization;
                    options.ClientKey = ocelotSettingsOptions.ClientKey;
                    options.ClientRateLimit = ocelotSettingsOptions.ClientRateLimit;
                    options.ClusterEnvironment = ocelotSettingsOptions.ClusterEnvironment;
                    options.DbConnectionConfig = ocelotSettingsOptions.DbConnectionConfig;
                    options.EnableTimer = ocelotSettingsOptions.EnableTimer;
                    options.TimerDelay = ocelotSettingsOptions.TimerDelay;
                }
                ocelotOptionAction?.Invoke(options);
            });
        return services;
    }

    /// <summary>
    /// 添加默认的注入方式，所有需要传入的参数都是用默认值
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="ocelotOptionAction">配置信息</param>
    /// <returns></returns>
    public static IOcelotBuilder AddHxOcelotCore(this IOcelotBuilder builder, Action<OcelotSettingsOptions> ocelotOptionAction = default)
    {
        if (builder == null) throw new ArgumentNullException(nameof(builder));
        var ocelotSettingsOptions = new OcelotSettingsOptions();
        builder.Configuration.GetSection("OcelotSettings").Bind(ocelotSettingsOptions);
        builder.Services.Configure<OcelotSettingsOptions>(builder.Configuration.GetSection("OcelotSettings"))
            .PostConfigure<OcelotSettingsOptions>(options => 
            {
                ocelotOptionAction?.Invoke(options);
            });
        ocelotOptionAction?.Invoke(ocelotSettingsOptions);
        ocelotSettingsOptions.DbConnectionConfig ??= new DbConnectionConfig()
        {
            ConfigId = CommonConst.ConfigId,
            DbType = DbType.Sqlite,
            ConnectionString = "DataSource=./Hx-Gateway.db",
            EnableInitDb = false,
            EnableInitSeed = false,
            EnableUnderLine = true,
            EnableSqlLog = false,
        };
        builder.Services.AddSqlSugar(config=> 
        {
            if (ocelotSettingsOptions.DbConnectionConfig != null)
            {
                config.ConfigId = ocelotSettingsOptions.DbConnectionConfig.ConfigId;
                config.DbType = ocelotSettingsOptions.DbConnectionConfig.DbType;
                config.DbLinkName = ocelotSettingsOptions.DbConnectionConfig.DbLinkName;
                config.SlaveConnectionConfigs = ocelotSettingsOptions.DbConnectionConfig.SlaveConnectionConfigs;
                config.ConnectionString = ocelotSettingsOptions.DbConnectionConfig.ConnectionString;
                config.IsAutoCloseConnection = ocelotSettingsOptions.DbConnectionConfig.IsAutoCloseConnection;
                config.AopEvents = ocelotSettingsOptions.DbConnectionConfig.AopEvents;
                config.EnableUnderLine = ocelotSettingsOptions.DbConnectionConfig.EnableUnderLine;
                config.EnableInitDb = ocelotSettingsOptions.DbConnectionConfig.EnableInitDb;
                config.EnableInitSeed = ocelotSettingsOptions.DbConnectionConfig.EnableInitSeed;
                config.EnableSqlLog = ocelotSettingsOptions.DbConnectionConfig.EnableSqlLog;
            }
        });

        //配置文件仓储注入
        builder.Services.AddSingleton<IFileConfigurationRepository, DbFileConfigurationRepository>();
        //使用Redis重写缓存
        builder.Services.AddSingleton<IOcelotCache<FileConfiguration>, RedisOcelotCache<FileConfiguration>>();
        builder.Services.AddSingleton<IOcelotCache<CachedResponse>, RedisOcelotCache<CachedResponse>>();
        builder.Services.AddSingleton<IOcelotCache<InternalConfiguration>, RedisOcelotCache<InternalConfiguration>>();
        builder.Services.AddSingleton<IInternalConfigurationRepository, RedisInternalConfigurationRepository>();
        //注册后端服务
        builder.Services.AddHostedService<DbConfigurationPoller>();
        return builder;
    }

}
