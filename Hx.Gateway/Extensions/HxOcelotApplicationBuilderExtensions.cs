﻿// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot;
using Ocelot.Configuration;
using Ocelot.Configuration.Creator;
using Ocelot.Configuration.Repository;
using Ocelot.Logging;
using Ocelot.Middleware;
using Ocelot.Responses;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder;
/// <summary>
/// 中间件扩展程序
/// </summary>
public static class HxOcelotApplicationBuilderExtensions
{
    public static async Task<IApplicationBuilder> UseHxOcelot(this IApplicationBuilder builder)
    {
        await builder.UseHxOcelot(new OcelotPipelineConfiguration());
        return builder;
    }

    public static async Task<IApplicationBuilder> UseHxOcelot(this IApplicationBuilder builder, Action<OcelotPipelineConfiguration> pipelineConfiguration)
    {
        var config = new OcelotPipelineConfiguration();
        pipelineConfiguration?.Invoke(config);
        return await builder.UseHxOcelot(config);
    }

    public static async Task<IApplicationBuilder> UseHxOcelot(this IApplicationBuilder builder, OcelotPipelineConfiguration pipelineConfiguration)
    {
        //重写创建配置方法
        var configuration = await CreateConfiguration(builder);

        ConfigureDiagnosticListener(builder);

        return CreateOcelotPipeline(builder, pipelineConfiguration);
    }

    private static IApplicationBuilder CreateOcelotPipeline(IApplicationBuilder builder, OcelotPipelineConfiguration pipelineConfiguration)
    {
        builder.BuildOcelotPipeline(pipelineConfiguration);

        /*
        inject first delegate into first piece of asp.net middleware..maybe not like this
        then because we are updating the http context in ocelot it comes out correct for
        rest of asp.net..
        */

        builder.Properties["analysis.NextMiddlewareName"] = "TransitionToOcelotMiddleware";

        return builder;
    }

    private static async Task<IInternalConfiguration> CreateConfiguration(IApplicationBuilder builder)
    {
        //提取文件配置信息
        var fileConfig = await builder.ApplicationServices.GetService<IFileConfigurationRepository>().Get();
        var internalConfigCreator = builder.ApplicationServices.GetService<IInternalConfigurationCreator>();
        var internalConfig = await internalConfigCreator.Create(fileConfig.Data);
        //如果配置文件错误直接抛出异常
        if (internalConfig.IsError)
        {
            ThrowToStopOcelotStarting(internalConfig);
        }
        //配置信息缓存，这块需要注意实现方式，因为后期我们需要改造下满足分布式架构
        var internalConfigRepo = builder.ApplicationServices.GetService<IInternalConfigurationRepository>();
        internalConfigRepo.AddOrReplace(internalConfig.Data);
        //获取中间件配置委托
        var configurations = builder.ApplicationServices.GetServices<OcelotMiddlewareConfigurationDelegate>();
        foreach (var configuration in configurations)
        {
            await configuration(builder);
        }
        return GetOcelotConfigAndReturn(internalConfigRepo);
    }

    private static bool IsError(Response response)
    {
        return response == null || response.IsError;
    }

    private static IInternalConfiguration GetOcelotConfigAndReturn(IInternalConfigurationRepository provider)
    {
        var ocelotConfiguration = provider.Get();

        if (ocelotConfiguration?.Data == null || ocelotConfiguration.IsError)
        {
            ThrowToStopOcelotStarting(ocelotConfiguration);
        }

        return ocelotConfiguration.Data;
    }

    private static void ThrowToStopOcelotStarting(Response config)
    {
        throw new Exception($"Unable to start Ocelot, errors are: {string.Join(",", config.Errors.Select(x => x.ToString()))}");
    }

    private static void ConfigureDiagnosticListener(IApplicationBuilder builder)
    {
        var env = builder.ApplicationServices.GetService<IWebHostEnvironment>();
        var listener = builder.ApplicationServices.GetService<OcelotDiagnosticListener>();
        var diagnosticListener = builder.ApplicationServices.GetService<DiagnosticListener>();
        diagnosticListener.SubscribeWithAdapter(listener);
    }
}
