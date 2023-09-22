// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using Hx.Gateway.Core;
using Hx.Gateway.Core.Entity;
using Hx.Gateway.Core.Options.Ocelot;

namespace Hx.Gateway.Application.Services.Routes.Dtos;
/// <summary>
/// 路由详情信息
/// </summary>
public class DetailRouteOutput
{
    /// <summary>
    /// 主键id
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 项目Id 
    ///</summary>
    public long ProjectId { get; set; }

    /// <summary>
    /// 请求Id 
    ///</summary>
    public string RequestIdKey { get; set; }

    /// <summary>
    /// 上游主机
    /// </summary>
    public string UpstreamHost { get; set; }

    /// <summary>
    /// 上游请求的模板，即用户真实请求的链接 
    ///</summary>
    public string UpstreamPathTemplate { get; set; }

    /// <summary>
    /// 上游请求的http方法（数组：GET、POST、PUT） 
    ///</summary>
    public List<string> UpstreamHttpMethod { get; set; }

    /// <summary>
    /// 下游请求的协议，如：http,htttps 
    ///</summary>
    public string DownstreamScheme { get; set; }

    /// <summary>
    /// 下游请求的http方法
    /// </summary>
    public string DownstreamHttpMethod { get; set; }

    /// <summary>
    /// 下游的路由模板，即真实处理请求的路径模板 
    ///</summary>
    public string DownstreamPathTemplate { get; set; }

    /// <summary>
    ///  下游Http版本 
    ///</summary>
    public string DownstreamHttpVersion { get; set; }

    /// <summary>
    /// 开启上下游路由模板大小写匹配 
    ///</summary>
    public bool? RouteIsCaseSensitive { get; set; }

    /// <summary>
    /// 是否使用服务发现
    /// </summary>
    public bool? UseServiceDiscovery { get; set; }
    /// <summary>
    ///  服务名 
    ///</summary>
    public string ServiceName { get; set; }

    /// <summary>
    ///  服务命名空间 
    ///</summary>
    public string ServiceNamespace { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public QoSOptions QoSOptions { get; set; }

    /// <summary>
    /// 负载均衡
    /// </summary>
    public LoadBalancerOptions LoadBalancerOptions { get; set; }

    public RateLimitOptions RateLimitOptions { get; set; }

    public AuthenticationOptions AuthenticationOptions { get; set; }

    /// <summary>
    /// 如果要忽略SSL警告/错误，请在路由配置中设置以下参数
    /// </summary>
    public bool? DangerousAcceptAnyServerCertificateValidator { get; set; }

    public HttpHandlerOptions HttpHandlerOptions { get; set; }

    public FileCacheOptions FileCacheOptions { get; set; }

    /// <summary>
    /// 委托
    /// </summary>
    public List<string> DelegatingHandlers { get; set; }

    /// <summary>
    ///  你可以通过在ocelot.json中包含“Priority”属性来定义你想要的路由匹配Upstream HttpRequest的顺序,0是最低优先级
    ///</summary>
    public int? Priority { get; set; }

    /// <summary>
    /// 路由地址配置
    /// </summary>
    public List<TgRouteHostPort> DownstreamHostAndPorts { get; set; }

    /// <summary>
    /// 路由属性
    /// </summary>
    public List<TgRouteProperty> RouteProperties { get; set; }

    /// <summary>
    ///  状态
    ///</summary>
    public StatusEnum? Status { get; set; }
}
