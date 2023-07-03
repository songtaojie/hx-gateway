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
/// Ocelot支持上游请求的速率限制，这样下游服务就不会过载
/// </summary>
public class RateLimitOptions
{
    /// <summary>
    /// 这是一个包含客户端白名单的数组。这意味着该数组中的客户端将不受速率限制的影响
    /// </summary>
    public List<string> ClientWhitelist { get; set; }

    /// <summary>
    /// 此值指定启用端点速率限制
    /// </summary>
    public bool? EnableRateLimiting { get; set; }

    /// <summary>
    /// 该值指定限制适用的时间段，如1s、5m、1h、1d等。
    /// 如果在此时间段内发出的请求超过限制，则需要等待PeriodTimespan结束后再发出另一个请求
    /// </summary>
    public int? Period { get; set; }

    /// <summary>
    /// 这个值指定我们可以在特定的秒数后重试
    /// </summary>
    public int? PeriodTimespan { get; set; }

    /// <summary>
    /// 此值指定客户端在指定时间段内可以发出的最大请求数
    /// </summary>
    public int? Limit { get; set; }

}
