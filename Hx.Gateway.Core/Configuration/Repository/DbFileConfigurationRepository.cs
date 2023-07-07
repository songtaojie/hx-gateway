using Hx.Gateway.Core.Entity;
using Hx.Gateway.Core.Enum;
using Hx.Gateway.Core.Options;
using Hx.Sdk.Sqlsugar.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Ocelot.Configuration.ChangeTracking;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Core.Configuration.Repository
{
    /// <summary>
    /// 数据库配置文件读取
    /// </summary>
    public class DbFileConfigurationRepository : IFileConfigurationRepository
    {
        private readonly SqlSugarRepository<TgGlobalConfiguration> _rep;
        private readonly OcelotSettingsOptions _ocelotSettings;
        public DbFileConfigurationRepository(SqlSugarRepository<TgGlobalConfiguration> rep,
            IOptions<OcelotSettingsOptions> options)
        {
            _rep = rep;
            _ocelotSettings = options.Value;
        }

        /// <summary>
        /// 从数据库中获取配置信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response<FileConfiguration>> Get()
        {
            #region 提取配置信息
            var file = new FileConfiguration();
            //提取默认启用的路由配置信息
            var globalConfiguration = await _rep.AsQueryable()
                .LeftJoin<TgProject>((u,p) => u.ProjectId == p.Id)
                .WhereIF(!string.IsNullOrEmpty(_ocelotSettings.ProjectCode), (u, p) =>p.Code == _ocelotSettings.ProjectCode)
                .Where((u, p) =>  u.Status == StatusEnum.Enable && p.Status == StatusEnum.Enable)
                .OrderByDescending((u, p) => u.CreateTime)
                .FirstAsync();
            if (globalConfiguration != null)
            {
                //赋值全局信息
                file.GlobalConfiguration = globalConfiguration.Adapt<FileGlobalConfiguration>();

                //提取所有路由信息
                var routeList = await _rep.Context.Queryable<TgRoute>()
                     .LeftJoin<TgProject>((u, p) => u.ProjectId == p.Id)
                     .WhereIF(!string.IsNullOrEmpty(_ocelotSettings.ProjectCode), (u, p) => p.Code == _ocelotSettings.ProjectCode)
                     .Where((u, p) => p.Status == StatusEnum.Enable && u.Status == StatusEnum.Enable)
                     .OrderByDescending((u, p) => u.CreateTime)
                     .Select((u, p) => u)
                     .ToListAsync();
                file.Routes = routeList.Adapt<List<FileRoute>>();
            }
            #endregion
            if (file.Routes == null || !file.Routes.Any())
            {
                return new OkResponse<FileConfiguration>(null);
            }
            return new OkResponse<FileConfiguration>(file);
        }

        public async Task<Response> Set(FileConfiguration fileConfiguration)
        {
            if (fileConfiguration != null && fileConfiguration.GlobalConfiguration != null && !string.IsNullOrEmpty(fileConfiguration.GlobalConfiguration.RequestIdKey))
            {
                var tgGlobalConfiguration = fileConfiguration.GlobalConfiguration.Adapt<TgGlobalConfiguration>();
                await _rep.Context.Updateable<TgGlobalConfiguration>()
                    .SetColumns(u => new TgGlobalConfiguration
                    {
                        BaseUrl = tgGlobalConfiguration.BaseUrl,
                        DownstreamScheme = tgGlobalConfiguration.DownstreamScheme,
                        ServiceDiscoveryProviderOptions = tgGlobalConfiguration.ServiceDiscoveryProviderOptions,
                        LoadBalancerOptions = tgGlobalConfiguration.LoadBalancerOptions,
                        HttpHandlerOptions = tgGlobalConfiguration.HttpHandlerOptions,
                        QoSOptions = tgGlobalConfiguration.QoSOptions
                    })
                    .Where(u => u.RequestIdKey == tgGlobalConfiguration.RequestIdKey)
                    .ExecuteCommandAsync();
                var reRoutes = fileConfiguration.Routes;
                if (reRoutes != null && reRoutes.Count > 0)
                {
                    foreach (var item in reRoutes)
                    {
                        var route = item.Adapt<TgRoute>();
                        await _rep.Context.Updateable<TgRoute>()
                        .SetColumns(u => new TgRoute
                        {
                            UpstreamPathTemplate = route.UpstreamPathTemplate,
                            UpstreamHost = route.UpstreamHost,
                            DownstreamHostAndPorts = route.DownstreamHostAndPorts,
                            AuthenticationOptions = route.AuthenticationOptions,
                            DelegatingHandlers = route.DelegatingHandlers,
                            DangerousAcceptAnyServerCertificateValidator = route.DangerousAcceptAnyServerCertificateValidator,
                            DownstreamHttpMethod = route.DownstreamHttpMethod,
                            DownstreamHttpVersion = route.DownstreamHttpVersion,
                            DownstreamPathTemplate = route.DownstreamPathTemplate,
                            DownstreamScheme = route.DownstreamScheme,
                            FileCacheOptions = route.FileCacheOptions,
                            HttpHandlerOptions = route.HttpHandlerOptions,
                            LoadBalancerOptions = route.LoadBalancerOptions,
                            QoSOptions = route.QoSOptions,
                            RateLimitOptions = route.RateLimitOptions,
                            RouteIsCaseSensitive = route.RouteIsCaseSensitive,
                            ServiceName = route.ServiceName,
                            ServiceNamespace = route.ServiceNamespace,
                            RouteProperties = route.RouteProperties,
                        })
                        .Where(u => u.RequestIdKey == item.RequestIdKey)
                        .ExecuteCommandAsync();
                    }
                }
            }
            return await Task.FromResult<Response>(new OkResponse());
        }
    }
}
