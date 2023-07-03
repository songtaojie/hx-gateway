// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hx.Gateway.Application.Options.Ocelot;

namespace Hx.Gateway.Application.Options.Ocelot;
/// <summary>
/// Ocelot路由节点配置
/// </summary>
public class OcelotRouteNode
{
    /// <summary>
    /// 下游的路由模板，即真实处理请求的路径模板 
    ///</summary>
    public string DownstreamPathTemplate { get; set; }

    /// <summary>
    /// 下游的路由模板，即真实处理请求的路径模板 
    ///</summary>
    public string UpstreamPathTemplate { get; set; }

    /// <summary>
    /// 下游请求的http方法
    /// </summary>
    public string DownstreamHttpMethod { get; set; }

    /// <summary>
    /// 上游请求的http方法（数组：GET、POST、PUT） 
    ///</summary>
    public List<string> UpstreamHttpMethod { get; set; }

    /// <summary>
    /// 请求Id 
    ///</summary>
    public string RequestIdKey { get; set; }

    /// <summary>
    /// 开启上下游路由模板大小写匹配 
    ///</summary>
    public bool? RouteIsCaseSensitive { get; set; }

    /// <summary>
    ///  服务名 
    ///</summary>
    public string ServiceName { get; set; }

    /// <summary>
    /// 请求的方式，如：http,htttps 
    ///</summary>
    public string DownstreamScheme { get; set; }

    /// <summary>
    /// 下游请求地址和端口
    /// </summary>
    public List<DownstreamHostAndPortOptions> DownstreamHostAndPorts { get; set; }

    public QoSOptions QoSOptions { get; set; }

    /// <summary>
    /// 负载均衡
    /// </summary>
    public LoadBalancerOptions LoadBalancerOptions { get; set; }

    public RateLimitOptions RateLimitOptions { get; set; }

    public AuthenticationOptions AuthenticationOptions { get; set; }

    public bool? UseServiceDiscovery { get; set; }

    public bool? DangerousAcceptAnyServerCertificateValidator { get; set; }

    public HttpHandlerOptions HttpHandlerOptions { get; set; }

    public FileCacheOptions FileCacheOptions { get; set; }

    /// <summary>
    /// 委托
    /// </summary>
    public List<string> DelegatingHandlers { get; set; }
}
