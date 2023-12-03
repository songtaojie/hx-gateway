// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com


using Hx.Gateway.Admin.Authentication;
using Hx.Gateway.Admin.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// JWT 授权服务拓展类
/// </summary>
public static class JWTAuthorizationServiceCollectionExtensions
{
    /// <summary>
    /// 添加 JWT 授权
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration">授权配置</param>
    /// <returns></returns>
    public static AuthenticationBuilder AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        // 配置 JWT 选项
        ConfigureJWTOptions(services);
        // 添加默认授权
        JWTEncryption.BindJWTSetting(configuration);
        var authenticationBuilder = services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            // 配置 JWT 验证信息
            options.TokenValidationParameters = JWTEncryption.CreateTokenValidationParameters(JWTEncryption.GetJWTSettings());
        });
        return authenticationBuilder;
    }

    /// <summary>
    /// 添加 JWT 授权
    /// </summary>
    /// <param name="services"></param>
    private static void ConfigureJWTOptions(IServiceCollection services)
    {
        // 配置验证
        services.AddOptions<JWTSettingsOptions>()
                .BindConfiguration("JWTSettings")
                .ValidateDataAnnotations()
                .PostConfigure(options =>
                {
                    _ = JWTSettingsOptions.SetDefaultJwtSettings(options);
                });
    }
}
