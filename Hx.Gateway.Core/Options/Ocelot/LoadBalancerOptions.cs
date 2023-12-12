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
/// Ocelot可以为每条路由在可用的下游服务之间进行负载平衡。这意味着您可以扩展您的下游服务，Ocelot可以有效地使用它们
/// </summary>
public class LoadBalancerOptions
{
    /// <summary>
    /// 可用的负载均衡器类型
    /// LeastConnection:跟踪哪些服务正在处理请求，并将新请求发送给现有请求最少的服务。算法状态并不分布在Ocelot的集群中
    /// RoundRobin:循环可用的服务并发送请求。算法状态并不分布在Ocelot的集群中
    /// NoLoadBalancer:从配置或服务发现中获取第一个可用服务
    /// CookieStickySessions :使用cookie将所有请求粘贴到特定的服务器
    /// Custom Load Balancers:自定义类型，需要实现ILoadBalancer
    /// </summary>
    public string Type { get; set; }

    /// <summary>
    /// Key:这是您希望用于粘滞会话的cookie的密钥
    /// </summary>
    public string Key { get; set; }

    /// <summary>
    /// 这是您希望会话被卡住的毫秒数
    /// </summary>
    public int Expiry { get; set; }
}
