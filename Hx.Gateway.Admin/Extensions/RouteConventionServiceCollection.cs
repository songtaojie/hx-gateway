// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc;
using Hx.Gateway.Admin;

namespace Microsoft.Extensions.DependencyInjection;

public static class RouteConventionServiceCollection
{
    /// <summary>
    /// 使用自定义路由扩展方法
    /// </summary>
    /// <param name="opts"></param>
    /// <param name="routeAttribute"></param>
    public static void UseCentralRoutePrefix(this MvcOptions mvcOptions, IRouteTemplateProvider routeAttribute,IWebHostEnvironment webHostEnvironment)
    {
        // 添加我们自定义 实现IApplicationModelConvention的RouteConvention
        mvcOptions.Conventions.Insert(0, new RouteConvention(routeAttribute, webHostEnvironment));
    }
}
