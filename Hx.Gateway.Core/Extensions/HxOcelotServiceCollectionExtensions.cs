// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using Hx.Gateway.Core;
using Hx.Gateway.Core.Configuration.Repository;
using Hx.Gateway.Core.Options;
using Hx.Gateway.Core.RateLimit;
using Microsoft.Extensions.Options;
using Ocelot.Configuration.Repository;
using Ocelot.DependencyInjection;

namespace Microsoft.Extensions.DependencyInjection;
public static class HxOcelotServiceCollectionExtensions
{
    /// <summary>
    /// 添加默认的注入方式，所有需要传入的参数都是用默认值
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="option">配置信息</param>
    /// <returns></returns>
    public static IOcelotBuilder AddHxOcelot(this IOcelotBuilder builder, Action<OcelotSettingsOptions> option)
    {
        //重写缓存
        //builder.Services.AddSingleton<IOcelotCache<FileConfiguration>, OcelotCache<FileConfiguration>>();
        //builder.Services.AddSingleton<IOcelotCache<InternalConfiguration>, OcelotCache<InternalConfiguration>>();
        //builder.Services.AddSingleton<IOcelotCache<CachedResponse>, OcelotCache<CachedResponse>>();
        //builder.Services.AddSingleton<IOcelotCache<ClientRoleModel>, OcelotCache<ClientRoleModel>>();
        //builder.Services.AddSingleton<IOcelotCache<RateLimitRuleModel>, OcelotCache<RateLimitRuleModel>>();
        //builder.Services.AddSingleton<IOcelotCache<DiffClientRateLimitCounter?>, OcelotCache<DiffClientRateLimitCounter?>>();
        //builder.Services.AddSingleton<IInternalConfigurationRepository, RedisInternalConfigurationRepository>();
        ////注入授权
        //builder.Services.AddSingleton<ICusAuthenticationProcessor, CusAuthenticationProcessor>();
        ////注入限流实现
        //builder.Services.AddSingleton<IClientRateLimitProcessor, DiffClientRateLimitProcessor>();

        ////http输出转换类
        //builder.Services.AddSingleton<IHttpResponder, HttpContextResponder>();
        //添加数据库存储
        builder.Services.AddSqlSugar();
        //配置信息
        builder.Services.Configure(option);
        builder.Services.AddSingleton(
            resolver => resolver.GetRequiredService<IOptions<OcelotSettingsOptions>>().Value);
        //配置文件仓储注入
        builder.Services.AddSingleton<IFileConfigurationRepository, DbFileConfigurationRepository>();
        return builder;
    }

}
