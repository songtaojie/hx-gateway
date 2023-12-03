// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Hx.Gateway.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// 跨域中间件拓展
    /// </summary>
    public static class CorsAccessorApplicationBuilderExtensions
    {
        /// <summary>
        /// 添加跨域中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseCorsAccessor(this IApplicationBuilder app)
        {

            // 获取选项
            var options = app.ApplicationServices.GetService<IOptions<CorsAccessorSettingsOptions>>();
            if (options == null)
                throw new ArgumentNullException(nameof(options), "Add the AddCorsAccessor method to services");
            var corsAccessorSettings = options.Value;

            // 配置跨域中间件
            app.UseCors(corsAccessorSettings.PolicyName);

            // 添加压缩缓存
            app.UseResponseCaching();

            return app;
        }
    }
}
