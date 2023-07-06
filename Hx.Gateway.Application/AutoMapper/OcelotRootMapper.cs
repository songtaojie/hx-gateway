﻿// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using Hx.Gateway.Core.Entity;
using Hx.Gateway.Core.Options.Ocelot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Application.AutoMapper;
public class OcelotRootMapper : BaseMapper
{
    public override void Register(TypeAdapterConfig config)
    {
        config.ForType<TgGlobalConfiguration, OcelotGlobalConfigurationNode>()
            .Map(dest => dest.RateLimitOptions, src => Deserialize<GlobalRateLimitOptions>(src.RateLimitOptions))
            .Map(dest => dest.ServiceDiscoveryProvider, src => Deserialize<ServiceDiscoveryProviderOptions>(src.ServiceDiscoveryProviderOptions));

        config.ForType<TgRoute, OcelotRouteNode>()
            .Map(dest => dest.RateLimitOptions, src => Deserialize<RateLimitOptions>(src.RateLimitOptions))
             .Map(dest => dest.HttpHandlerOptions, src => Deserialize<HttpHandlerOptions>(src.HttpHandlerOptions))
             .Map(dest => dest.AuthenticationOptions, src => Deserialize<AuthenticationOptions>(src.AuthenticationOptions))
             .Map(dest => dest.DownstreamHostAndPorts,
                 src => src.DownstreamHostAndPorts.Count > 0 ? src.DownstreamHostAndPorts.Select(o => new DownstreamHostAndPortOptions()
                 {
                     Host = o.Host,
                     Port = o.Port
                 }) : null)
             .Map(dest => dest.UpstreamHttpMethod, src => Deserialize<List<string>>(src.UpstreamHttpMethod))
             .Map(dest => dest.QoSOptions, src => Deserialize<QoSOptions>(src.QoSOptions))
             //设置负载均衡
             .Map(dest => dest.LoadBalancerOptions, src => Deserialize<LoadBalancerOptions>(src.LoadBalancerOptions))
             //设置缓存
             .Map(dest => dest.FileCacheOptions, src => Deserialize<FileCacheOptions>(src.FileCacheOptions))
             .Map(dest => dest.DelegatingHandlers, src => Deserialize<List<string>>(src.DelegatingHandlers));

        config.ForType<TgRoute, Ocelot.Configuration.File.FileRoute>()
            .Map(dest => dest.RateLimitOptions, src => Deserialize<Ocelot.Configuration.File.FileRateLimitRule>(src.RateLimitOptions))
             .Map(dest => dest.HttpHandlerOptions, src => Deserialize<Ocelot.Configuration.File.FileHttpHandlerOptions>(src.HttpHandlerOptions))
             .Map(dest => dest.AuthenticationOptions, src => Deserialize<Ocelot.Configuration.File.FileAuthenticationOptions>(src.AuthenticationOptions))
             .Map(dest => dest.DownstreamHostAndPorts,
                 src => src.DownstreamHostAndPorts.Count > 0 ? src.DownstreamHostAndPorts.Select(o => new Ocelot.Configuration.File.FileHostAndPort()
                 {
                     Host = o.Host,
                     Port = o.Port
                 }) : null)
             .Map(dest => dest.UpstreamHttpMethod, src => Deserialize<List<string>>(src.UpstreamHttpMethod))
             .Map(dest => dest.QoSOptions, src => Deserialize<Ocelot.Configuration.File.FileQoSOptions> (src.QoSOptions))
             //设置负载均衡
             .Map(dest => dest.LoadBalancerOptions, src => Deserialize<Ocelot.Configuration.File.FileLoadBalancerOptions> (src.LoadBalancerOptions))
             //设置缓存
             .Map(dest => dest.FileCacheOptions, src => Deserialize<Ocelot.Configuration.File.FileCacheOptions>(src.FileCacheOptions))
             .Map(dest => dest.DelegatingHandlers, src => Deserialize<List<string>>(src.DelegatingHandlers));

        config.ForType<Ocelot.Configuration.File.FileRoute, TgRoute>()
          .Map(dest => dest.RateLimitOptions, src => Serialize(src.RateLimitOptions))
           .Map(dest => dest.HttpHandlerOptions, src => Serialize(src.HttpHandlerOptions))
           .Map(dest => dest.AuthenticationOptions, src => Serialize(src.AuthenticationOptions))
           .Map(dest => dest.DownstreamHostAndPorts,
               src => src.DownstreamHostAndPorts.Count > 0 ? src.DownstreamHostAndPorts.Select(o => new TgRouteHostPort
               {
                   Host = o.Host,
                   Port = o.Port
               }) : null)
           .Map(dest => dest.UpstreamHttpMethod, src => Serialize(src.UpstreamHttpMethod))
           .Map(dest => dest.QoSOptions, src => Serialize(src.QoSOptions))
           //设置负载均衡
           .Map(dest => dest.LoadBalancerOptions, src => Serialize(src.LoadBalancerOptions))
           //设置缓存
           .Map(dest => dest.FileCacheOptions, src => Serialize(src.FileCacheOptions))
           .Map(dest => dest.DelegatingHandlers, src => Serialize(src.DelegatingHandlers));
    }

}
