// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using Hx.Gateway.Application.Services.Routes.Dtos;
using Hx.Gateway.Core.Entity;
using Hx.Gateway.Core.Options.Ocelot;

namespace Hx.Gateway.Application.AutoMapper;
public class RouteMapper : BaseMapper
{
    public override void Register(TypeAdapterConfig config)
    {
        config.ForType<TgRoute, DetailRouteOutput>()
            .Map(dest => dest.DelegatingHandlers, src => Deserialize<List<string>>(src.DelegatingHandlers))
            .Map(dest => dest.RateLimitOptions, src => Deserialize<RateLimitOptions>(src.RateLimitOptions))
            .Map(dest => dest.AuthenticationOptions, src => Deserialize<AuthenticationOptions>(src.AuthenticationOptions))
            //.Map(dest => dest.DownstreamHostAndPorts, src => src.RouteHostPorts != null
            //    ? src.RouteHostPorts.Select(r=>new DownstreamHostAndPortOptions
            //    { 
            //        Host= r.Host,
            //        Port= r.Port,
            //    })
            //    : null)
            .Map(dest => dest.FileCacheOptions, src => Deserialize<FileCacheOptions>(src.FileCacheOptions))
            .Map(dest => dest.QoSOptions, src => Deserialize<QoSOptions>(src.QoSOptions))
            .Map(dest => dest.LoadBalancerOptions, src => Deserialize<LoadBalancerOptions>(src.LoadBalancerOptions))
            .Map(dest => dest.HttpHandlerOptions, src => Deserialize<HttpHandlerOptions>(src.HttpHandlerOptions))
            .Map(dest => dest.UpstreamHttpMethod, src => Deserialize<List<string>>(src.UpstreamHttpMethod));

        config.ForType<AddRouteInput, TgRoute>()
          .Map(dest => dest.DelegatingHandlers, src => Serialize(src.DelegatingHandlers))
          .Map(dest => dest.RateLimitOptions, src => Serialize(src.RateLimitOptions))
          .Map(dest => dest.AuthenticationOptions, src => Serialize(src.AuthenticationOptions))
          .Map(dest => dest.UpstreamHttpMethod, src => Serialize(src.UpstreamHttpMethod))
          .Map(dest => dest.QoSOptions, src => Serialize(src.QoSOptions))
          .Map(dest => dest.LoadBalancerOptions, src => Serialize(src.LoadBalancerOptions))
          .Map(dest => dest.FileCacheOptions, src => Serialize(src.FileCacheOptions))
          .Map(dest => dest.HttpHandlerOptions, src => Serialize(src.HttpHandlerOptions));

    }
}
