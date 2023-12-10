// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Core.Options.Ocelot;
/// <summary>
/// Ocelot全局配置节点
/// </summary>
public class OcelotGlobalConfigurationNode
{
    /// <summary>
    /// 服务发现
    /// </summary>
    public ServiceDiscoveryProviderOptions ServiceDiscoveryProvider { get; set; }

    /// <summary>
    /// 请求id
    /// </summary>
    public string RequestIdKey { get; set; }

    /// <summary>
    /// 限流配置
    /// </summary>
    public RateLimitOptions RateLimitOptions { get; set; }

    /// <summary>
    /// 基础地址 
    ///</summary>
    public string BaseUrl { get; set; }

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
    public LoadBalancerOptions LoadBalancerOptions { get; set; }

    /// <summary>
    /// HttpHandler配置 
    ///</summary>   
    ///<see cref=" Hx.Gateway.Core.Options.Ocelot.HttpHandlerOptions"/>
    public HttpHandlerOptions HttpHandlerOptions { get; set; }

    /// <summary>
    /// 服务质量控制
    ///</summary>
    ///<see cref=" Hx.Gateway.Core.Options.Ocelot.QoSOptions"/>
    public QoSOptions QoSOptions { get; set; }
}
