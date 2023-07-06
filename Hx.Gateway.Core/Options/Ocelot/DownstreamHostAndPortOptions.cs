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
/// 它定义希望向其转发请求的任何下游服务的主机和端口。
/// 通常这将只包含一个条目，但有时你可能想要负载平衡请求到你的下游服务，
/// Ocelot允许你添加多个条目，然后选择负载均衡器。
/// </summary>
public class DownstreamHostAndPortOptions
{
    /// <summary>
    /// 主机
    /// </summary>
    public string Host { get; set; }

    /// <summary>
    /// 端口
    /// </summary>
    public int? Port { get; set; }
}
