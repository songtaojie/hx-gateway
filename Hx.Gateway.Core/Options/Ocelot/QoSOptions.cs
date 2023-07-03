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
/// Quality of Service
/// Ocelot当前只支持一个QoS能力。
/// 如果您想在向下游服务发出请求时使用断路器，
/// 则可以在每个路由的基础上进行设置。它使用了一个叫做Polly的很棒的.net库。
/// Install-Package Ocelot.Provider.Polly
/// </summary>
public class QoSOptions
{
    /// <summary>
    /// 流量调控开启
    /// </summary>
    public bool? Enabled { get; set; }
    /// <summary>
    /// 要实现此规则，必须对ExceptionsAllowedBeforeBreaking设置一个大于0的数字
    /// </summary>
    public int? ExceptionsAllowedBeforeBreaking { get; set; }

    /// <summary>
    /// 断路时间是指断路器被跳闸后将保持1秒的开启状态
    /// </summary>
    public int? DurationOfBreak { get; set; }

    /// <summary>
    /// 表示如果一个请求超过5秒，它将自动超时。
    /// </summary>
    public int? TimeoutValue { get; set; }
}
