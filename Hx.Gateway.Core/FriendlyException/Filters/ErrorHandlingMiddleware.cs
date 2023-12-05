// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hx.Gateway.Core.FriendlyException.Filters;
public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate next;

    private readonly ILogger _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        this.next = next;
        _logger = logger ?? throw new ArgumentNullException("logger");
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context).ConfigureAwait(continueOnCapturedContext: false);
        }
        catch (OperationCanceledException ex)
        {
            context.Request.EnableBuffering();
            HandleException(context.Response, 499, ex.Message);
            LogRequestInfo(context.Request, ex.Message).ConfigureAwait(continueOnCapturedContext: false);
        }
        catch (UserFriendlyException ex2)
        {
            context.Request.EnableBuffering();
            HandleException(context.Response, 200, ex2.Message);
            LogRequestInfo(context.Request, ex2.Message).ConfigureAwait(continueOnCapturedContext: false);
        }
        catch (Exception ex3)
        {
            context.Request.EnableBuffering();
            int statusCode = context.Response.StatusCode;
            if (ex3 is ArgumentException)
            {
                statusCode = 200;
            }

            HandleException(context.Response, statusCode, ex3.Message);
            LogRequestInfo(context.Request, ex3.Message, ex3).ConfigureAwait(continueOnCapturedContext: false);
        }
        finally
        {
            int statusCode2 = context.Response.StatusCode;
            if (statusCode2 >= 400 && statusCode2 != 499)
            {
                context.Request.EnableBuffering();
                string msg = GetMsg(statusCode2);
                if (!string.IsNullOrEmpty(msg))
                {
                    HandleException(context.Response, statusCode2, msg);
                }
            }
        }
    }

    private void HandleException(HttpResponse response, int statusCode, string msg)
    {
        if (!response.HasStarted)
        {
            response.StatusCode = statusCode;
            response.ContentType = "application/json;charset=utf-8";
            response.WriteAsync(JsonSerializer.Serialize(new RESTfulResult<object>
            {
                StatusCode = statusCode,
                Succeeded = false,
                Data = null,
                Message = msg,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            }).ToLower());
        }
    }

    private string GetMsg(int statusCode)
    {
        return statusCode switch
        {
            400 => "请求失败",
            401 => "未授权",
            404 => "未找到服务",
            502 => "无效响应",
            _ => "未知错误",
        };
    }

    private async Task LogRequestInfo(HttpRequest request, string errMsg, Exception ex = null)
    {
        string text = "";
        if (request != null && request.Path != null && !request.Path.Value!.Contains("swagger"))
        {
            string text2 = await ReadRequestBodyAsync(request.BodyReader);
            text = $"[{request.Method}] {request.Path}{request.QueryString} {text2}";
        }

        if (ex == null)
        {
            _logger.LogWarning(text + " \r\n " + errMsg);
        }
        else
        {
            _logger.LogError(ex, text + " \r\n " + errMsg);
        }
    }

    private async Task<string> ReadRequestBodyAsync(PipeReader reader)
    {
        string result = "";
        ReadResult readResult;
        do
        {
            readResult = await reader.ReadAsync();
            ReadOnlySequence<byte> readOnlySequence = readResult.Buffer;
            if (readOnlySequence.IsEmpty)
            {
                break;
            }

            result = AsString(in readOnlySequence);
            reader.AdvanceTo(readOnlySequence.Start, readOnlySequence.End);
        }
        while (!readResult.IsCompleted);
        return result;
    }

    private static string AsString(in ReadOnlySequence<byte> readOnlySequence)
    {
        ReadOnlySpan<byte> bytes = BuffersExtensions.ToArray(in readOnlySequence).AsSpan();
        return Encoding.UTF8.GetString(bytes);
    }
}
