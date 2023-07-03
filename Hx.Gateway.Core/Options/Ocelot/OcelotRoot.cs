// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Application.Options.Ocelot;
/// <summary>
/// Ocelot配置
/// </summary>
public class OcelotRoot
{
    /// <summary>
    /// 路由配置
    /// </summary>
    public List<OcelotRouteNode> Routes { get; set; }

    /// <summary>
    /// 全局配置
    /// </summary>
    public OcelotGlobalConfigurationNode GlobalConfiguration { get; set; }
}
