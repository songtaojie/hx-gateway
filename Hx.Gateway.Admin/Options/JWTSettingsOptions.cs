// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Microsoft.IdentityModel.Tokens;

namespace Hx.Gateway.Admin.Options;

/// <summary>
/// Jwt 配置
/// </summary>
public sealed class JWTSettingsOptions
{
    /// <summary>
    /// 验证签发方密钥
    /// </summary>
    public bool? ValidateIssuerSigningKey { get; set; }

    /// <summary>
    /// 签发方密钥
    /// </summary>
    public string IssuerSigningKey { get; set; }

    /// <summary>
    /// 验证签发方
    /// </summary>
    public bool? ValidateIssuer { get; set; }

    /// <summary>
    /// 签发方
    /// </summary>
    public string ValidIssuer { get; set; }

    /// <summary>
    /// 验证签收方
    /// </summary>
    public bool? ValidateAudience { get; set; }

    /// <summary>
    /// 签收方
    /// </summary>
    public string ValidAudience { get; set; }

    /// <summary>
    /// 验证生存期
    /// </summary>
    public bool? ValidateLifetime { get; set; }

    /// <summary>
    /// 过期时间容错值，解决服务器端时间不同步问题（秒）
    /// </summary>
    public long? ClockSkew { get; set; }

    /// <summary>
    /// 过期时间（分钟）
    /// </summary>
    public long? ExpiredTime { get; set; }

    /// <summary>
    /// 加密算法
    /// </summary>
    public string Algorithm { get; set; }


    /// <summary>
    /// 设置默认 Jwt 配置
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    internal static JWTSettingsOptions SetDefaultJwtSettings(JWTSettingsOptions options)
    {
        options.ValidateIssuerSigningKey ??= true;
        if (options.ValidateIssuerSigningKey == true)
        {
            options.IssuerSigningKey ??= "U2FsdGVkX1+6H3D8Q//yQMhInzTdRZI9DbUGetbyaag=";
        }
        options.ValidateIssuer ??= true;
        if (options.ValidateIssuer == true)
        {
            options.ValidIssuer ??= "hx.gateway";
        }
        options.ValidateAudience ??= true;
        if (options.ValidateAudience == true)
        {
            options.ValidAudience ??= "powerby hx.gateway";
        }
        options.ValidateLifetime ??= true;
        if (options.ValidateLifetime == true)
        {
            options.ClockSkew ??= 10;
        }
        options.ExpiredTime ??= 20;
        options.Algorithm ??= SecurityAlgorithms.HmacSha256;

        return options;
    }
}
