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
/// Ocelot允许您指定一个服务发现提供程序，并使用它来为Ocelot转发请求的下游服务找到主机和端口。目前，
/// 这只在GlobalConfiguration部分中得到支持，
/// 这意味着相同的服务发现提供者将用于您在Route级别指定ServiceName的所有路由
/// Install-Package Ocelot.Provider.Consul
/// </summary>
public class ServiceDiscoveryProviderOptions
{
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool? Enabled { get; set; }

    /// <summary>
    /// Http协议（http,https） 
    /// </summary>
    public string Scheme { get; set; }
    /// <summary>
    /// 
    /// </summary>
    public string Host { get; set; }

    public int? Port { get; set; }

    public string Type { get; set; }

    /// <summary>
    /// 如果您正在使用ACL与Consul Ocelot支持添加X-Consul-Token头。为了使其工作，您必须添加下面的附加属性
    /// Ocelot将把这个令牌添加到Consul客户端，用于发出请求，然后用于每个请求
    /// </summary>
    public string Token { get; set; }

    /// <summary>
    /// 配置key
    /// </summary>
    public string ConfigurationKey { get; set; }

    /// <summary>
    /// 轮询间隔以毫秒为单位，告诉Ocelot多久调用Consul更改一次服务配置
    /// </summary>
    public int? PollingInterval { get; set; }

    /// <summary>
    /// 命名空间
    /// </summary>
    public string Namespace { get; set; }
}
