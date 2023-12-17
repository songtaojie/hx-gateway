// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json.Linq;
using System.Text.Json.Nodes;
using System.Text.Json;
using Newtonsoft.Json;

namespace Hx.Gateway.Admin;

public class RouteConvention : IApplicationModelConvention
{
    private readonly AttributeRouteModel _routePrefix;
    private readonly IWebHostEnvironment _webHostEnvironment;
    /// <summary>
    /// 构造方法
    /// </summary>
    /// <param name="routeTemplateProvider"></param>
    public RouteConvention(IRouteTemplateProvider routeTemplateProvider,
        IWebHostEnvironment webHostEnvironment)
    {
        _routePrefix = new AttributeRouteModel(routeTemplateProvider);
        _webHostEnvironment = webHostEnvironment;
    }

    /// <summary>
    /// 实现接口的Apply方法
    /// </summary>
    /// <param name="application"></param>
    public void Apply(ApplicationModel application)
    {
        //遍历所有的 Controller
        foreach (var controller in application.Controllers)
        {
            // 已经标记了 RouteAttribute 的 Controller
            var matchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel != null).ToList();
            if (matchedSelectors.Any())
            {
                foreach (var selectorModel in matchedSelectors)
                {
                    // 在当前路由上 再 添加一个路由前缀
                    selectorModel.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_routePrefix, selectorModel.AttributeRouteModel);
                }
            }

            // 没有标记 RouteAttribute 的 Controller
            var unmatchedSelectors = controller.Selectors.Where(x => x.AttributeRouteModel == null).ToList();
            if (unmatchedSelectors.Any())
            {
                foreach (var selectorModel in unmatchedSelectors)
                {
                    // 添加一个路由前缀
                    selectorModel.AttributeRouteModel = _routePrefix;
                }
            }
        }

        var webRootPath = _webHostEnvironment.WebRootPath;
        var configPath = Path.Combine(webRootPath, "config.js");
        if (File.Exists(configPath))
        {
            var lines = File.ReadAllLines(configPath);
            var configJs = File.ReadAllText(configPath);
            if (!string.IsNullOrEmpty(configJs))
            {
                var configArr = configJs.Split(new char[] { '=',';' }, StringSplitOptions.RemoveEmptyEntries);
                if (configArr != null && configArr.Length >= 2)
                {
                    try
                    {
                        JsonTextReader reader = new JsonTextReader(new StringReader(configArr[1]));
                        JObject jObject = (JObject)JToken.ReadFrom(reader);
                        if (jObject.ContainsKey(CommonConst.API_Prefix_Key))
                        {
                            jObject[CommonConst.API_Prefix_Key] = _routePrefix.Template;
                        }
                        configJs = $"{configArr[0]} = {jObject.ToString()}";
                        File.WriteAllText(configPath, configJs);
                    }
                    catch 
                    { 
                    }
                }
            }
        }
    }
}
