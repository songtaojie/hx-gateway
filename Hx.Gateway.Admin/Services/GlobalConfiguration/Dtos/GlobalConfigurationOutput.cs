﻿// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using Hx.Gateway.Core;
using Hx.Gateway.Core.Options.Ocelot;

namespace Hx.Gateway.Application.Services.GlobalConfiguration.Dtos;
/// <summary>
/// 全局配置文件输出
/// </summary>
public class GlobalConfigurationOutput
{
    /// <summary>
    /// 主键id
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// 项目Id 
    ///</summary>
    public Guid? ProjectId { get; set; }

    /// <summary>
    /// 全局配置名称 
    ///</summary>
    public string Name { get; set; }

    /// <summary>
    /// 基础地址 
    ///</summary>
    public string BaseUrl { get; set; }

    /// <summary>
    /// 请求ID 
    ///</summary>
    public string RequestIdKey { get; set; }

    /// <summary>
    /// Http协议（http,https） 
    ///</summary>
    public string DownstreamScheme { get; set; }
    /// <summary>
    /// Http版本（1.0，1.1，2.0） 
    ///</summary>
    public string DownstreamHttpVersion { get; set; }
    /// <summary>
    /// 负载均衡
    ///</summary>
    ///<see cref=" Hx.Gateway.Core.Options.Ocelot.LoadBalancerOptions"/>
    public LoadBalancerOptions LoadBalancerOptions { get; set; } = new LoadBalancerOptions();

    /// <summary>
    /// HttpHandler配置 
    ///</summary>   
    ///<see cref=" Hx.Gateway.Core.Options.Ocelot.HttpHandlerOptions"/>
    public HttpHandlerOptions HttpHandlerOptions { get; set; } = new HttpHandlerOptions();

    /// <summary>
    /// 服务质量控制
    ///</summary>
    ///<see cref=" Hx.Gateway.Core.Options.Ocelot.QoSOptions"/>
    public QoSOptions QoSOptions { get; set; } = new QoSOptions();

    /// <summary>
    /// 全局限流配置 
    ///</summary>
    ///<see cref=" Hx.Gateway.Core.Options.Ocelot.RateLimitOptions"/>
    public RateLimitOptions RateLimitOptions { get; set; } = new RateLimitOptions();

    /// <summary>
    /// 服务发现代理配置
    /// </summary>
    /// <see cref="Hx.Gateway.Core.Options.Ocelot.ServiceDiscoveryProviderOptions"/>
    public ServiceDiscoveryProviderOptions ServiceDiscoveryProviderOptions { get; set; } = new ServiceDiscoveryProviderOptions();

    /// <summary>
    /// 状态
    ///</summary>
    public StatusEnum Status { get; set; }
}
