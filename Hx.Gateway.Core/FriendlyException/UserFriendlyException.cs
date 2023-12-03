﻿// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Core;
/// <summary>
/// 用户友好的异常提示
/// </summary>
public class UserFriendlyException : Exception
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public UserFriendlyException() : base()
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message"></param>
    public UserFriendlyException(string message) : base(message)
    {
        ErrorCode = StatusCodes.Status500InternalServerError;
        ErrorMessage = message;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message"></param>
    /// <param name="errorCode"></param>
    public UserFriendlyException(string message, object errorCode) : base(message)
    {
        ErrorCode = errorCode;
        ErrorMessage = message;
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="message"></param>
    /// <param name="errorCode"></param>
    /// <param name="innerException"></param>
    public UserFriendlyException(string message, object errorCode, Exception innerException) : base(message, innerException)
    {
        ErrorCode = errorCode;
    }

    /// <summary>
    /// 错误码
    /// </summary>
    public object ErrorCode { get; set; }

    /// <summary>
    /// 错误消息（支持 Object 对象）
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// 状态码
    /// </summary>
    public int StatusCode { get; set; } = StatusCodes.Status500InternalServerError;

    /// <summary>
    /// 结果数据
    /// </summary>
    public new object Data { get; set; }
}
