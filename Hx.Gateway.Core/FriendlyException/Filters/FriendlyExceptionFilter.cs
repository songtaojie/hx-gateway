// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Hx.Gateway.Core;
using Hx.Gateway.Core.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Mvc.Filters;

/// <summary>
/// 友好异常拦截器
/// </summary>
public sealed class FriendlyExceptionFilter : IAsyncExceptionFilter
{
    private readonly ILogger _logger;
    /// <summary>
    /// 服务提供器
    /// </summary>
    private readonly IServiceProvider _serviceProvider;
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="serviceProvider">服务提供器</param>
    public FriendlyExceptionFilter(IServiceProvider serviceProvider,
        ILogger<FriendlyExceptionFilter> logger)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    /// <summary>
    /// 异步异常的处理
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task OnExceptionAsync(ExceptionContext context)
    {
        // 如果异常在其他地方被标记了处理，那么这里不再处理
        if (context.ExceptionHandled) return;
        // 排除 WebSocket 请求处理
        if (context.HttpContext.IsWebSocketRequest()) return;

        // 解析异常信息
        var exceptionMetadata = UnifyResultContext.GetExceptionMetadata(context);
        context.Result = new JsonResult(new RESTfulResult<object>
        {
            StatusCode = exceptionMetadata.StatusCode,
            Succeeded = false,
            Data = exceptionMetadata.Data,
            Message = exceptionMetadata.ErrorMessage,
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        });
        context.ExceptionHandled = true;
        await Task.CompletedTask;
    }
}
