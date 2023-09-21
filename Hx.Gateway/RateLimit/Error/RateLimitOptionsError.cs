// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using Ocelot.Errors;

namespace Hx.Gateway.Core.RateLimit;
/// <summary>
/// 限流错误信息
/// </summary>
public class RateLimitOptionsError : Error
{
    public RateLimitOptionsError(string message) : base(message, OcelotErrorCode.RateLimitOptionsError, 429)
    {

    }
}
