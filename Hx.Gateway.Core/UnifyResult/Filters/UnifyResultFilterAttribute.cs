// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Hx.Gateway.Core.Extensions;
using Hx.Gateway.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace Microsoft.AspNetCore.Mvc.Filters;
/// <summary>
/// 规范化结构（请求成功）过滤器
/// </summary>
public class UnifyResultFilterAttribute : ResultFilterAttribute
{
    /// <summary>
    /// 执行结果
    /// </summary>
    /// <param name="context"></param>
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        // 排除 Mvc 视图
        bool isHandleResult = false;
        var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
        if (typeof(Controller).IsAssignableFrom(actionDescriptor.ControllerTypeInfo))
        {
            base.OnResultExecuting(context);
            return;
        }
        // 排除 WebSocket 请求处理
        if (context.HttpContext.IsWebSocketRequest())
        {
            base.OnResultExecuting(context);
            return;
        }
        // 判断是否跳过规范化处理
        var method = actionDescriptor.MethodInfo;
        var isSkip =  method.GetRealReturnType().HasImplementedRawGeneric(typeof(RESTfulResult<>))
              || method.CustomAttributes.Any(x => typeof(NonUnifyAttribute).IsAssignableFrom(x.AttributeType) || typeof(ProducesResponseTypeAttribute).IsAssignableFrom(x.AttributeType) || typeof(IApiResponseMetadataProvider).IsAssignableFrom(x.AttributeType))
              || method.ReflectedType.IsDefined(typeof(NonUnifyAttribute), true);
        if (!isSkip)
        {
            // 处理规范化结果
            // 处理 BadRequestObjectResult 验证结果
            if (context.Result is BadRequestObjectResult badResult)
            {
                isHandleResult = true;
                var result = new RESTfulResult<object>
                {
                    StatusCode = badResult.StatusCode.HasValue ? badResult.StatusCode.Value : StatusCodes.Status500InternalServerError,  // 处理没有返回值情况 204
                    Succeeded = false,
                    Message = "Internal Server Error",
                    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
                };

                // 如果是模型验证字典类型
                if (badResult.Value is ModelStateDictionary modelState)
                {
                    // 将验证错误信息转换成字典并序列化成 Json
                    var validationResult = modelState.Where(u => modelState[u.Key].ValidationState == ModelValidationState.Invalid)
                        .Select(u => u.Value)
                        .FirstOrDefault();
                    result.Message = validationResult?.Errors.FirstOrDefault().ErrorMessage;
                }
                // 如果是 ValidationProblemDetails 特殊类型
                else if (badResult.Value is ValidationProblemDetails validation)
                {
                    var error = validation.Errors.FirstOrDefault().Value;
                    result.Message = error.FirstOrDefault();
                }
                // 解析验证消息
                context.Result = new JsonResult(result);
            }
            else
            {
                var result = OnSucceeded(context);
                if (result != null)
                {
                    isHandleResult = true;
                    context.Result = result;
                }
            }
        }
        if (!isHandleResult) base.OnResultExecuting(context);
    }

    /// <summary>
    /// 成功返回值
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private IActionResult OnSucceeded(ResultExecutingContext context)
    {
        object data;
        if (context.Result is EmptyResult) data = null;
        // 处理内容结果
        else if (context.Result is ContentResult contentResult) data = contentResult.Content;
        // 处理对象结果
        else if (context.Result is ObjectResult objectResult)
        {
            data = objectResult.Value;
            if (objectResult.DeclaredType == typeof(RESTfulResult<>))
            {
                return context.Result;
            }
        }
        else return null;

        return new JsonResult(new RESTfulResult<object>
        {
            StatusCode = context.Result is EmptyResult ? StatusCodes.Status204NoContent : StatusCodes.Status200OK,  // 处理没有返回值情况 204
            Succeeded = true,
            Data = data,
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
        });
    }
}
