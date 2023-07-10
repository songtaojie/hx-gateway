// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using FreeRedis;
using Hx.Gateway.Core;
using Hx.Gateway.Core.Cache;
using Hx.Gateway.Core.Configuration;
using Hx.Gateway.Core.Configuration.Repository;
using Hx.Gateway.Core.Const;
using Hx.Gateway.Core.Options;
using Hx.Gateway.Core.RateLimit;
using Hx.Sdk.SqlSugar;
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
    /// <returns></returns>
    public static IServiceCollection AddHxOcelot(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOcelot(configuration)
            .AddHxOcelot();
        return services;
    }

    /// <summary>
    /// 添加默认的注入方式，所有需要传入的参数都是用默认值
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration">配置</param>
    /// <param name="option">配置信息</param>
    /// <returns></returns>
    public static IServiceCollection AddHxOcelot(this IServiceCollection services, IConfiguration configuration, Action<OcelotSettingsOptions> option)
    {
        services.AddOcelot(configuration)
            .AddHxOcelot(option);
        return services;
    }


    /// <summary>
    /// 添加默认的注入方式，所有需要传入的参数都是用默认值
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IOcelotBuilder AddHxOcelot(this IOcelotBuilder builder)
    {
        var ocelotSettingsOptions = builder.Configuration.GetValue("OcelotSettings", new OcelotSettingsOptions());
        //配置信息
        builder.Services.Configure<OcelotSettingsOptions>(option =>
        {
            option = ocelotSettingsOptions;
        });
        //重写缓存
        //builder.Services.AddSingleton<IOcelotCache<ClientRoleModel>, OcelotCache<ClientRoleModel>>();
        //builder.Services.AddSingleton<IOcelotCache<RateLimitRuleModel>, OcelotCache<RateLimitRuleModel>>();
        //builder.Services.AddSingleton<IOcelotCache<DiffClientRateLimitCounter?>, OcelotCache<DiffClientRateLimitCounter?>>();
        ////注入授权
        //builder.Services.AddSingleton<ICusAuthenticationProcessor, CusAuthenticationProcessor>();
        ////注入限流实现
        //builder.Services.AddSingleton<IClientRateLimitProcessor, DiffClientRateLimitProcessor>();

        ////http输出转换类
        //builder.Services.AddSingleton<IHttpResponder, HttpContextResponder>();
        //添加数据库存储
        var config = new DbConnectionConfig()
        {
            ConfigId = CommonConst.ConfigId,
            DbType = DbType.Sqlite,
            ConnectionString = "./Hx.Gateway.db",
            EnableInitDb = true,
            EnableInitSeed = true,
            EnableUnderLine = true,
            EnableSqlLog = true,
        } ;
        if (ocelotSettingsOptions.DbConnectionConfig != null)
        {
            config = ocelotSettingsOptions.DbConnectionConfig;
        }
        builder.Services.Configure<DbSettingsOptions>((option) =>
        {
            option.ConnectionConfigs = new[] { SqlSugarConfigProvider.SetDbConfig(config) };
        });
        builder.Services.AddSqlSugar((db, provider) =>
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

    /// <summary>
    /// 添加默认的注入方式，所有需要传入的参数都是用默认值
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="option">配置信息</param>
    /// <returns></returns>
    public static IOcelotBuilder AddHxOcelot(this IOcelotBuilder builder, Action<OcelotSettingsOptions> option)
    {
        OcelotSettingsOptions ocelotSettingsOptions = new OcelotSettingsOptions();
        option?.Invoke(ocelotSettingsOptions);
        //配置信息
        builder.Services.Configure<OcelotSettingsOptions>(opt =>
        {
            opt = ocelotSettingsOptions;
        });
        //重写缓存
        //builder.Services.AddSingleton<IOcelotCache<ClientRoleModel>, OcelotCache<ClientRoleModel>>();
        //builder.Services.AddSingleton<IOcelotCache<RateLimitRuleModel>, OcelotCache<RateLimitRuleModel>>();
        //builder.Services.AddSingleton<IOcelotCache<DiffClientRateLimitCounter?>, OcelotCache<DiffClientRateLimitCounter?>>();
        ////注入授权
        //builder.Services.AddSingleton<ICusAuthenticationProcessor, CusAuthenticationProcessor>();
        ////注入限流实现
        //builder.Services.AddSingleton<IClientRateLimitProcessor, DiffClientRateLimitProcessor>();

        ////http输出转换类
        //builder.Services.AddSingleton<IHttpResponder, HttpContextResponder>();
        //添加数据库存储
        var config = SqlSugarConfigProvider.SetDbConfig(new DbConnectionConfig()
        {
            DbType = DbType.Sqlite,
            ConnectionString = "./Hx.Gateway.db",
            EnableInitDb = true,
            EnableInitSeed = true,
            EnableUnderLine = true,
            EnableSqlLog = true,
        });
        if (ocelotSettingsOptions.DbConnectionConfig != null)
        {
            config = ocelotSettingsOptions.DbConnectionConfig;
        };
        builder.Services.Configure<DbSettingsOptions>((option) => 
        {
            option.ConnectionConfigs = new[] { config };
        });
        builder.Services.AddSqlSugar((db, provider) =>
        {
            var ocelotOptions = provider.GetService<IOptions<OcelotSettingsOptions>>();
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
