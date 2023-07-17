// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Admin.Options.Ocelot;
/// <summary>
/// 使用路由配置中的HttpHandlerOptions设置HttpHandler行为
/// </summary>
public class HttpHandlerOptions
{
    /// <summary>
    /// AllowAutoRedirect是一个指示请求是否应该遵循重定向响应的值。
    /// 如果请求应该自动跟随来自下游资源的重定向响应，
    /// 则将其设置为true;否则错误。默认值为false
    /// </summary>
    public bool? AllowAutoRedirect { get; set; }

    /// <summary>
    /// UseCookieContainer是一个值，指示处理程序是否使用cookie econtainer属性存储服务器cookie，并在发送请求时使用这些cookie。默认值为false。请注意，如果您正在使用CookieContainer, 
    /// Ocelot会为每个下游服务缓存HttpClient。
    /// 这意味着对DownstreamService的所有请求都将共享相同的cookie
    /// </summary>
    public bool? UseCookieContainer { get; set; }

    /// <summary>
    /// 是否使用跟踪
    /// </summary>
    public bool? UseTracing { get; set; }

    /// <summary>
    /// 是否使用代理
    /// </summary>
    public bool? UseProxy { get; set; }

    /// <summary>
    /// 这将控制内部HttpClient将打开多少个连接。这可以在Route或全局级别设置。
    /// </summary>
    public int? MaxConnectionsPerServer { get; set; }
}
