// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using Hx.Gateway.Web.Core.RateLimit.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder;
/// <summary>
/// 自定义客户端限流中间件扩展
/// </summary>
public static class DiffClientRateLimitApplicationBuilderExtensions
{
    /// <summary>
    /// 使用自定义限流
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseDiffClientRateLimit(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<DiffClientRateLimitMiddleware>();
    }
}
