// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com


using Hx.Gateway.Admin.Services;
using Hx.Gateway.Application.Services.GlobalConfiguration;
using Ocelot.Admin.Api.Application;

namespace Microsoft.Extensions.DependencyInjection;

public static class GatewayServiceCollectionExtensions
{
    public static IServiceCollection AddGatewayServices(this IServiceCollection services)
    {
        services.AddTransient<ProjectService>();
        services.AddTransient<GlobalConfigService>();
        services.AddTransient<SystemService>();
        return services;
    }
}

