// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Hx.Gateway.Core;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 跨域访问服务拓展类
    /// </summary>
    public static class CorsAccessorServiceCollectionExtensions
    {
        /// <summary>
        /// 默认跨域导出响应头 Key
        /// </summary>
        /// <remarks>解决 ajax，XMLHttpRequest，axios 不能获取请求头问题</remarks>
        private static readonly string[] _defaultExposedHeaders = new[]
        {
            "access-token",
            "x-access-token"
        };
        /// <summary>
        /// 配置跨域
        /// </summary>
        /// <param name="services">服务集合</param>
        /// <param name="configuration"></param>
        /// <returns>服务集合</returns>
        public static IServiceCollection AddCorsAccessor(this IServiceCollection services,IConfiguration configuration)
        {
            ConfigureCorsAccessorSettings(services);
            // 添加跨域服务
            services.AddCors(options =>
            {
                var corsAccessorSettings = GetCorsAccessorSettings(configuration);
                // 获取选项
                // 添加策略跨域
                options.AddPolicy(name: corsAccessorSettings.PolicyName, builder =>
                {
                    // 判断是否设置了来源，因为 AllowAnyOrigin 不能和 AllowCredentials一起公用
                    var isNotSetOrigins = corsAccessorSettings.WithOrigins == null || corsAccessorSettings.WithOrigins.Length == 0;

                    var enabledSignalR = corsAccessorSettings.EnabledSignalR == true;

                    // 如果没有配置来源，则允许所有来源
                    if (isNotSetOrigins)
                    {
                        // 解决 SignarlR  不能配置允许所有源问题
                        if (!enabledSignalR) builder.AllowAnyOrigin();
                    }
                    else builder.WithOrigins(corsAccessorSettings.WithOrigins)
                                      .SetIsOriginAllowedToAllowWildcardSubdomains();

                    // 如果没有配置请求标头，则允许所有表头
                    if (corsAccessorSettings.WithHeaders == null || corsAccessorSettings.WithHeaders.Length == 0) builder.AllowAnyHeader();
                    else builder.WithHeaders(corsAccessorSettings.WithHeaders);

                    // 如果没有配置任何请求谓词，则允许所有请求谓词
                    if (corsAccessorSettings.WithMethods == null || corsAccessorSettings.WithMethods.Length == 0)
                    {
                        builder.AllowAnyMethod();
                    }
                    else
                    {
                        // 解决 SignarlR 必须允许 GET POST 问题
                        if (enabledSignalR)
                        {
                            builder.WithMethods(corsAccessorSettings.WithMethods.Concat(new[] { "GET", "POST" }).Distinct(StringComparer.OrdinalIgnoreCase).ToArray());
                        }
                        else
                        {
                            builder.WithMethods(corsAccessorSettings.WithMethods);
                        }
                    }

                    // 配置跨域凭据
                    if ((corsAccessorSettings.AllowCredentials == true && !isNotSetOrigins) || enabledSignalR)
                    {
                        builder.AllowCredentials();
                    }

                    // 配置响应头，如果前端不能获取自定义的 header 信息，必须配置该项，默认配置了 access-token 和 x-access-token，可取消默认行为
                    IEnumerable<string> exposedHeaders = corsAccessorSettings.FixedToken == true
                        ? _defaultExposedHeaders
                        : Array.Empty<string>();
                    if (corsAccessorSettings.WithExposedHeaders != null && corsAccessorSettings.WithExposedHeaders.Length > 0)
                        exposedHeaders.Concat(corsAccessorSettings.WithExposedHeaders).Distinct(StringComparer.OrdinalIgnoreCase);
                    if (exposedHeaders.Any()) builder.WithExposedHeaders(exposedHeaders.ToArray());
                    // 设置预检过期时间
                    builder.SetPreflightMaxAge(TimeSpan.FromSeconds(corsAccessorSettings.SetPreflightMaxAge ?? 24 * 60 * 60));
                });
            });
            return services;
        }

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="services"></param>
        private static void ConfigureCorsAccessorSettings(IServiceCollection services)
        {
            // 添加跨域配置选项
            services.AddOptions<CorsAccessorSettingsOptions>()
                  .BindConfiguration("CorsAccessorSettings", options =>
                  {
                      options.BindNonPublicProperties = true; // 绑定私有变量
                  })
                  .PostConfigure(options =>
                  {
                      options.PostConfigure("CorsAccessorSettingsOptions", options);
                  });
        }

        private static CorsAccessorSettingsOptions GetCorsAccessorSettings(IConfiguration configuration)
        {
            CorsAccessorSettingsOptions corsAccessorSettings = configuration.GetValue<CorsAccessorSettingsOptions>("CorsAccessorSettings");
            if (corsAccessorSettings == null)
            {
                corsAccessorSettings = new CorsAccessorSettingsOptions();
                corsAccessorSettings.PostConfigure("CorsAccessorSettingsOptions", corsAccessorSettings);
            }
            return corsAccessorSettings;
        }
    }
}
