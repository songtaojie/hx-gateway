// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 跨域扩展服务
/// </summary>
public static class CorsAccessorServiceCollectionExtensions
{
    private static string DefaultCorsPolicyName = "allow_all";
    /// <summary>
    /// 配置跨域
    /// </summary>
    /// <param name="services">服务集合</param>
    /// <param name="configuration">配置</param>
    /// <returns>服务集合</returns>
    public static IServiceCollection AddCorsAccessor(this IServiceCollection services, IConfiguration configuration)
    {
        // 添加跨域服务
        services.AddCors(options =>
        {
            options.AddPolicy(DefaultCorsPolicyName, builder =>
            {
                builder.SetIsOriginAllowedToAllowWildcardSubdomains()
                //.AllowAnyOrigin() // 允许任何来源的主机访问
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                .SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost");
            });
            var origs = configuration.GetSection("AllowOrigins").Get<string[]>() ?? Array.Empty<string>();
            if (origs != null)
            {
                DefaultCorsPolicyName = "with_origins";
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    builder.WithOrigins(origs)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .SetIsOriginAllowedToAllowWildcardSubdomains();
                });
            }
        });
        // 添加响应压缩
        services.AddResponseCaching();
        return services;
    }

    /// <summary>
    /// 添加跨域中间件
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseCorsAccessor(this IApplicationBuilder app)
    {
        // 配置跨域中间件
        app.UseCors(DefaultCorsPolicyName);

        // 添加压缩缓存
        app.UseResponseCaching();

        return app;
    }
}
