// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Core;
public static class UnifyResultContext
{
    /// <summary>
    /// 是否启用规范化结果
    /// </summary>
    internal static bool IsEnabledUnifyHandle = false;

    /// <summary>
    /// 规范化结果类型
    /// </summary>
    internal static Type RESTfulResultType = typeof(RESTfulResult<>);

    /// <summary>
    /// 规范化结果额外数据键
    /// </summary>
    internal static string UnifyResultExtrasKey = "UNIFY_RESULT_EXTRAS";

    /// <summary>
    /// 规范化结果状态码
    /// </summary>
    internal static string UnifyResultStatusCodeKey = "UNIFY_RESULT_STATUS_CODE";

    /// <summary>
    /// 获取异常元数据
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public static ExceptionMetadata GetExceptionMetadata(ActionContext context)
    {
        // 获取错误码
        object errorCode = default;
        object errors = default;
        string errorMessage = null;
        object data = default;
        var statusCode = StatusCodes.Status500InternalServerError;
        // 判断是否是 ExceptionContext 或者 ActionExecutedContext
        var exception = context is ExceptionContext exContext
            ? exContext.Exception
            : (
                context is ActionExecutedContext edContext
                ? edContext.Exception
                : default
            );
        if (exception is UserFriendlyException friendlyException)
        {
            errorCode = friendlyException.ErrorCode;
            statusCode = friendlyException.StatusCode;
            errors = friendlyException.ErrorMessage;
            data = friendlyException.Data;
            errorMessage = friendlyException.ErrorMessage;
        }
        var validationFlag = "[Validation]";

        // 处理验证失败异常
        if (!string.IsNullOrEmpty(errorMessage) && errorMessage.StartsWith(validationFlag))
        {
            // 处理结果
            errorMessage = errorMessage[validationFlag.Length..];
            // 设置为400状态码
            statusCode = StatusCodes.Status400BadRequest;
        }
        else
        {
            errorMessage = exception?.InnerException?.Message ?? exception?.Message;
        }

        return new ExceptionMetadata
        {
            Data = data,
            ErrorCode = errorCode,
            Errors = errors,
            StatusCode = statusCode,
            ErrorMessage = errorMessage
        };
    }

    /// <summary>
    /// 获取 Action 特性
    /// </summary>
    /// <typeparam name="TAttribute"></typeparam>
    /// <param name="httpContext"></param>
    /// <returns></returns>
    internal static TAttribute GetMetadata<TAttribute>(this HttpContext httpContext)
        where TAttribute : class
    {
        return httpContext.GetEndpoint()?.Metadata?.GetMetadata<TAttribute>();
    }
}
