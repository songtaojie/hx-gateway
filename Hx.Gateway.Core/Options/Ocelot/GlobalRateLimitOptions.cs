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
/// 全局限流配置
/// </summary>
public class GlobalRateLimitOptions
{

    /// <summary>
    /// 该值指定是否禁用X-Rate-Limit和Retry-After报头
    /// </summary>
    public bool? DisableRateLimitHeaders { get; set; }

    /// <summary>
    /// 此值指定超过的消息
    /// </summary>
    public string QuotaExceededMessage { get; set; }

    /// <summary>
    /// 此值指定限速发生时返回的HTTP状态码
    /// </summary>
    public int? HttpStatusCode { get; set; }

    /// <summary>
    /// 允许您指定应用于标识客户端的标头。默认情况下是" ClientId "
    /// </summary>
    public string ClientIdHeader { get; set; }
}
