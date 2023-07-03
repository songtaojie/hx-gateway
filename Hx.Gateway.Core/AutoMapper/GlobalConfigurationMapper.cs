// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using Hx.Gateway.Application.Options.Ocelot;
using Hx.Gateway.Application.Services.GlobalConfiguration.Dtos;
using Hx.Gateway.Application.Services.Routes.Dtos;
using Ocelot.Configuration.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Application.AutoMapper;
/// <summary>
/// 全局配置映射
/// </summary>
public class GlobalConfigurationMapper : BaseMapper
{
    public override void Register(TypeAdapterConfig config)
    {
        config.ForType<TgGlobalConfiguration, GlobalConfigurationOutput>()
            .Map(dest => dest.LoadBalancerOptions, src => Deserialize<LoadBalancerOptions>(src.LoadBalancerOptions) ?? new LoadBalancerOptions())
            .Map(dest => dest.HttpHandlerOptions, src => Deserialize<HttpHandlerOptions>(src.HttpHandlerOptions) ?? new HttpHandlerOptions())
            .Map(dest => dest.QoSOptions, src => Deserialize<QoSOptions>(src.QoSOptions) ?? new QoSOptions())
            .Map(dest => dest.RateLimitOptions, src => Deserialize<RateLimitOptions>(src.RateLimitOptions) ?? new RateLimitOptions())
            .Map(dest => dest.ServiceDiscoveryProviderOptions, src => Deserialize<ServiceDiscoveryProviderOptions>(src.ServiceDiscoveryProviderOptions) ?? new ServiceDiscoveryProviderOptions());

        config.ForType<TgGlobalConfiguration, FileGlobalConfiguration>()
           .Map(dest => dest.LoadBalancerOptions, src => Deserialize<FileLoadBalancerOptions>(src.LoadBalancerOptions))
           .Map(dest => dest.HttpHandlerOptions, src => Deserialize<FileHttpHandlerOptions>(src.HttpHandlerOptions))
           .Map(dest => dest.QoSOptions, src => Deserialize<FileQoSOptions>(src.QoSOptions) ?? new FileQoSOptions())
           .Map(dest => dest.RateLimitOptions, src => Deserialize<FileRateLimitOptions>(src.RateLimitOptions))
           .Map(dest => dest.ServiceDiscoveryProvider, src => Deserialize<FileServiceDiscoveryProvider>(src.ServiceDiscoveryProviderOptions));


        config.ForType<AddGlobalConfigurationInput, TgGlobalConfiguration>()
          .Map(dest => dest.RateLimitOptions, src => Serialize(src.RateLimitOptions))
          .Map(dest => dest.QoSOptions, src => Serialize(src.QoSOptions))
          .Map(dest => dest.LoadBalancerOptions, src => Serialize(src.LoadBalancerOptions))
          .Map(dest => dest.HttpHandlerOptions, src => Serialize(src.HttpHandlerOptions))
          .Map(dest => dest.ServiceDiscoveryProviderOptions, src => Serialize(src.ServiceDiscoveryProviderOptions));

        config.ForType<UpdateGlobalConfigurationInput, TgGlobalConfiguration>()
          .Map(dest => dest.RateLimitOptions, src => Serialize(src.RateLimitOptions))
          .Map(dest => dest.QoSOptions, src => Serialize(src.QoSOptions))
          .Map(dest => dest.LoadBalancerOptions, src => Serialize(src.LoadBalancerOptions))
          .Map(dest => dest.HttpHandlerOptions, src => Serialize(src.HttpHandlerOptions))
          .Map(dest => dest.ServiceDiscoveryProviderOptions, src => Serialize(src.ServiceDiscoveryProviderOptions));

    }
}
