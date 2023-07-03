using Hx.Gateway.Application.Options.Ocelot;
using Hx.Gateway.Application.Services.GlobalConfiguration;
using Hx.Gateway.Application.Services.Routes;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hx.Gateway.Application.Services.Consul.Hubs
{
    public class ConsulHub : Hub
    {
        private readonly ConsulService _consulService;
        private readonly GlobalConfigurationService _globalConfigurationService;
        private readonly ProjectService _projectService;
        private readonly RouteService _routeService;

        public ConsulHub(ConsulService consulService, 
            GlobalConfigurationService globalConfigurationService,
            ProjectService projectService, 
            RouteService routeService)
        {
            _consulService = consulService;
            _globalConfigurationService = globalConfigurationService;
            _projectService = projectService;
            _routeService = routeService;
        }

        public async Task SysnToConsulAsync(string key, string dc)
        {
            if (string.IsNullOrWhiteSpace(key) || string.IsNullOrWhiteSpace(dc))
            {
                await Clients.Caller.SendAsync("SysnToConsulMessage", $"consul配置未选中");
                return;
            }
            var ocelotRoot = new OcelotRoot();
            // 获取全局变量
            var globalConfiguration = await _globalConfigurationService.GetGlobalConfigurationAsync();
            if (globalConfiguration != null)
            {
                await Clients.Caller.SendAsync("SysnToConsulMessage", $"获取全局配置成功，当前全局配置{globalConfiguration.Status.GetDescription()}");
                if (globalConfiguration.Status == StatusEnum.Enable)
                {
                    ocelotRoot.GlobalConfiguration = globalConfiguration.Adapt<OcelotGlobalConfigurationNode>();
                }
            }
            var projects = await _projectService.GetProjectList();
            if (projects.Count == 0)
            {
                await Clients.Caller.SendAsync("SysnToConsulMessage", $"未查询到可用项目，停止同步，请检查后重试");
                return;
            }
            var allRoutes = projects.SelectMany(r => r.Routes);
           
            ocelotRoot.Routes = allRoutes.Adapt<List<OcelotRouteNode>>();

            var jSetting = new JsonSerializerOptions 
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented= true,
            };
            var ocelotConfigJson = JSON.Serialize(ocelotRoot, jSetting);
            await Clients.Caller.SendAsync("SysnToConsulMessage", $"构造ocelot配置成功");
            var settingBak = await _consulService.GetLastSettingBakAsync(key, dc);
            var settingJsonHashCode = settingBak != null ? settingBak.BakJson.GetHashCode() : 0;
            var currentJson = await _consulService.GetConsulKeyValueAsync(key, dc);
            //如果当前配置与要设置的配置一致，则不进行同步
            if (ocelotConfigJson.GetHashCode() == currentJson.GetHashCode())
            {
                await Clients.Caller.SendAsync("SysnToConsulMessage", $"与线上内容一致，同步取消");
                return;
            }

            if (settingJsonHashCode == currentJson.GetHashCode())
            {
                await Clients.Caller.SendAsync("SysnToConsulMessage", $"与上次备份内容一致，跳过备份");
            }
            else
            {
                await Clients.Caller.SendAsync("SysnToConsulMessage", $"进行配置备份");
                var bakResult = await _consulService.InsertSettingBakAsync(key, dc, currentJson, "同步备份");
                await Clients.Caller.SendAsync("SysnToConsulMessage", bakResult ? "备份完成" : "备份失败，同步中断");
                if (!bakResult)
                    return;
            }

            var editResult = await _consulService.SaveConsulKeyValueAsync(key, dc, ocelotConfigJson);
            await Clients.Caller.SendAsync("SysnToConsulMessage", editResult ? "同步完成" : "同步失败");
        }
    }
}
