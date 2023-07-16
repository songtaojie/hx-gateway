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
        var ocelotSettingsOptions = configuration.GetSection("OcelotSettings").Get<OcelotSettingsOptions>();
        services.AddOcelot(configuration)
            .AddHxOcelot(options => 
            {
                if (ocelotSettingsOptions != null)
                    options = ocelotSettingsOptions;
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
    public static IOcelotBuilder AddHxOcelot(this IOcelotBuilder builder, Action<OcelotSettingsOptions> ocelotOptionAction = default)
    {
        OcelotSettingsOptions ocelotSettingsOptions = new OcelotSettingsOptions();
        ocelotOptionAction?.Invoke(ocelotSettingsOptions);
     
        //添加数据库存储
        var config = SqlSugarConfigProvider.SetDbConfig(new DbConnectionConfig()
        {
            ConfigId = CommonConst.ConfigId,
            DbType = DbType.Sqlite,
            ConnectionString = "DataSource=./Hx.Gateway.db",
            EnableInitDb = true,
            EnableInitSeed = true,
            EnableUnderLine = true,
            EnableSqlLog = true,
        });
        if (ocelotSettingsOptions.DbConnectionConfig != null)
        {
            config = ocelotSettingsOptions.DbConnectionConfig;
        };
        builder.Services.AddSqlSugar(dbOptions => 
        {
            dbOptions.ConnectionConfigs = new DbConnectionConfig[] { config };
        },db =>
        {
            SqlSugarScopeProvider dbProvider = db.GetConnectionScope(config.ConfigId);
            SqlSugarConfigProvider.SetAopLog(dbProvider);
            SqlSugarConfigProvider.InitDatabase(dbProvider, config);
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
