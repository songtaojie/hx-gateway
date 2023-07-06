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
    public GlobalRateLimitOptions RateLimitOptions { get; set; }
}
