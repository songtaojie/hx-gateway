using Hx.Gateway.Core.Entity;
using Hx.Gateway.Core.Enum;
using Hx.Gateway.Core.Options;
using Hx.Sdk.Sqlsugar;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.Responses;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace Hx.Gateway.Core.Configuration.Repository
{
    /// <summary>
    /// 数据库配置文件读取
    /// </summary>
    public class DbFileConfigurationRepository : IFileConfigurationRepository
    {
        private readonly OcelotSettingsOptions _ocelotSettings;
        private readonly IServiceProvider _provider;
        public DbFileConfigurationRepository(IServiceProvider provider,
            IOptions<OcelotSettingsOptions> options)
        {
            _provider = provider;
            _ocelotSettings = options.Value;
        }

        /// <summary>
        /// 从数据库中获取配置信息
        /// </summary>
        /// <returns></returns>
        public async Task<Response<FileConfiguration>> Get()
        {
            #region 提取配置信息

            using (var serviceScope = _provider.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var rep = services.GetRequiredService<ISqlSugarRepository<TgGlobalConfiguration>>();
                var file = new FileConfiguration();
                //提取默认启用的路由配置信息
                var globalConfiguration = await rep.AsQueryable()
                    .LeftJoin<TgProject>((u, p) => u.ProjectId == p.Id)
                    .WhereIF(!string.IsNullOrEmpty(_ocelotSettings.ProjectCode), (u, p) => p.Code == _ocelotSettings.ProjectCode)
                    .Where((u, p) => u.Status == StatusEnum.Enable && p.Status == StatusEnum.Enable)
                    //.OrderByDescending((u, p) => u.)
                    .FirstAsync();

                if (globalConfiguration != null)
                {
                    //赋值全局信息
                    file.GlobalConfiguration = new FileGlobalConfiguration
                    {
                        BaseUrl = globalConfiguration.BaseUrl,
                        DownstreamScheme = globalConfiguration.DownstreamScheme,
                        DownstreamHttpVersion = globalConfiguration.DownstreamHttpVersion,
                        RequestIdKey = globalConfiguration.RequestIdKey,
                        //RateLimitOptions
                    };
                    if (!string.IsNullOrEmpty(globalConfiguration.HttpHandlerOptions))
                    {
                        file.GlobalConfiguration.HttpHandlerOptions = JsonSerializer.Deserialize<FileHttpHandlerOptions>(globalConfiguration.HttpHandlerOptions);
                    }
                    if (!string.IsNullOrEmpty(globalConfiguration.LoadBalancerOptions))
                    {
                        file.GlobalConfiguration.LoadBalancerOptions = JsonSerializer.Deserialize<FileLoadBalancerOptions>(globalConfiguration.LoadBalancerOptions);
                    }
                    if (!string.IsNullOrEmpty(globalConfiguration.QoSOptions))
                    {
                        file.GlobalConfiguration.QoSOptions = JsonSerializer.Deserialize<FileQoSOptions>(globalConfiguration.QoSOptions);
                    }
                    if (!string.IsNullOrEmpty(globalConfiguration.ServiceDiscoveryProviderOptions))
                    {
                        file.GlobalConfiguration.ServiceDiscoveryProvider = JsonSerializer.Deserialize<FileServiceDiscoveryProvider>(globalConfiguration.ServiceDiscoveryProviderOptions);
                    }
                    if (!string.IsNullOrEmpty(globalConfiguration.RateLimitOptions))
                    {
                        file.GlobalConfiguration.RateLimitOptions = JsonSerializer.Deserialize<FileRateLimitOptions>(globalConfiguration.RateLimitOptions);
                    }
                    //提取所有路由信息
                    var routeList = await rep.Context.Queryable<TgRoute>()
                         .LeftJoin<TgProject>((u, p) => u.ProjectId == p.Id)
                         .WhereIF(!string.IsNullOrEmpty(_ocelotSettings.ProjectCode), (u, p) => p.Code == _ocelotSettings.ProjectCode)
                         .Where((u, p) => p.Status == StatusEnum.Enable && u.Status == StatusEnum.Enable)
                         //.OrderByDescending((u, p) => u.CreateTime)
                         .Select((u, p) => u)
                         .ToListAsync();
                    if (!routeList.Any())
                        throw new Exception("未监测到任何可用的配置信息");
                    var reroutelist = new List<FileRoute>();
                    routeList.ForEach(r =>
                    {
                        var fileRoute = new FileRoute()
                        {
                            Key = r.RequestIdKey,
                            RequestIdKey = r.RequestIdKey,
                            DownstreamPathTemplate = r.DownstreamPathTemplate,
                            DownstreamScheme = r.DownstreamScheme,
                            Priority = r.Priority ?? 0,
                            ServiceName = r.ServiceName,
                            ServiceNamespace = r.ServiceNamespace,
                            UpstreamHost = r.UpstreamHost,
                            UpstreamPathTemplate = r.UpstreamPathTemplate,
                        };
                        if (!string.IsNullOrEmpty(r.AuthenticationOptions))
                        {
                            fileRoute.AuthenticationOptions = JsonSerializer.Deserialize<FileAuthenticationOptions>(r.AuthenticationOptions);
                        }
                        if (!string.IsNullOrEmpty(r.FileCacheOptions))
                        {
                            fileRoute.FileCacheOptions = JsonSerializer.Deserialize<FileCacheOptions>(r.FileCacheOptions);
                        }
                        if (!string.IsNullOrEmpty(r.DelegatingHandlers))
                        {
                            fileRoute.DelegatingHandlers = JsonSerializer.Deserialize<List<string>>(r.DelegatingHandlers);
                        }
                        if (!string.IsNullOrEmpty(r.LoadBalancerOptions))
                        {
                            fileRoute.LoadBalancerOptions = JsonSerializer.Deserialize<FileLoadBalancerOptions>(r.LoadBalancerOptions);
                        }
                        if (!string.IsNullOrEmpty(r.QoSOptions))
                        {
                            fileRoute.QoSOptions = JsonSerializer.Deserialize<FileQoSOptions>(r.QoSOptions);
                        }
                        if (r.DownstreamHostAndPorts != null)
                        {
                            fileRoute.DownstreamHostAndPorts = r.DownstreamHostAndPorts.Select(s => new FileHostAndPort
                            {
                                Host = s.Host,
                                Port = s.Port,
                            }).ToList();
                        }
                        if (!string.IsNullOrEmpty(r.UpstreamHttpMethod))
                        {
                            fileRoute.UpstreamHttpMethod = JsonSerializer.Deserialize<List<string>>(r.UpstreamHttpMethod);
                        }
                        //开始赋值
                        reroutelist.Add(fileRoute);
                    });
                    file.Routes = reroutelist;
                }
                #endregion
                if (file.Routes == null || !file.Routes.Any())
                    throw new Exception("未找到任何可用的配置信息");
                //{
                //    return new OkResponse<FileConfiguration>(null);
                //}
                return new OkResponse<FileConfiguration>(file);
            }
        }

        public async Task<Response> Set(FileConfiguration fileConfiguration)
        {
            using (var serviceScope = _provider.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var rep = services.GetRequiredService<ISqlSugarRepository<TgGlobalConfiguration>>();
                if (fileConfiguration != null && fileConfiguration.GlobalConfiguration != null && !string.IsNullOrEmpty(fileConfiguration.GlobalConfiguration.RequestIdKey))
                {
                    var globalConfiguration = fileConfiguration.GlobalConfiguration;
                    var tgGlobalConfiguration = new TgGlobalConfiguration
                    {
                        RequestIdKey = globalConfiguration.RequestIdKey,
                        BaseUrl = globalConfiguration.BaseUrl,
                        DownstreamHttpVersion = globalConfiguration.DownstreamHttpVersion,
                        DownstreamScheme = globalConfiguration.DownstreamScheme,
                        HttpHandlerOptions = globalConfiguration.HttpHandlerOptions == null
                                ? string.Empty
                                : JsonSerializer.Serialize(globalConfiguration.HttpHandlerOptions),
                        LoadBalancerOptions = globalConfiguration.LoadBalancerOptions == null
                                ? string.Empty
                                : JsonSerializer.Serialize(globalConfiguration.LoadBalancerOptions),
                        RateLimitOptions = globalConfiguration.RateLimitOptions == null
                                ? string.Empty
                                : JsonSerializer.Serialize(globalConfiguration.RateLimitOptions),
                        ServiceDiscoveryProviderOptions = globalConfiguration.ServiceDiscoveryProvider == null
                                ? string.Empty
                                : JsonSerializer.Serialize(globalConfiguration.ServiceDiscoveryProvider),
                        QoSOptions = globalConfiguration.QoSOptions == null
                                ? string.Empty
                                : JsonSerializer.Serialize(globalConfiguration.QoSOptions)
                    };
                    await rep.Context.Updateable<TgGlobalConfiguration>()
                        .SetColumns(u => new TgGlobalConfiguration
                        {
                            BaseUrl = tgGlobalConfiguration.BaseUrl,
                            DownstreamScheme = tgGlobalConfiguration.DownstreamScheme,
                            ServiceDiscoveryProviderOptions = tgGlobalConfiguration.ServiceDiscoveryProviderOptions,
                            LoadBalancerOptions = tgGlobalConfiguration.LoadBalancerOptions,
                            HttpHandlerOptions = tgGlobalConfiguration.HttpHandlerOptions,
                            QoSOptions = tgGlobalConfiguration.QoSOptions
                        })
                        .Where(u => u.RequestIdKey == globalConfiguration.RequestIdKey)
                        .ExecuteCommandAsync();
                    var routes = fileConfiguration.Routes;
                    if (routes != null && routes.Count > 0)
                    {
                        foreach (var fileRoute in routes)
                        {
                            var tgRoute = new TgRoute
                            {
                                RequestIdKey = fileRoute.RequestIdKey,
                                UpstreamPathTemplate = fileRoute.UpstreamPathTemplate,
                                UpstreamHost = fileRoute.UpstreamHost,
                                DownstreamHostAndPorts = fileRoute.DownstreamHostAndPorts == null
                                    ? null
                                    : fileRoute.DownstreamHostAndPorts.Select(r => new TgRouteHostPort { Host = r.Host, Port = r.Port }).ToList(),
                                AuthenticationOptions = fileRoute.AuthenticationOptions == null
                                    ? string.Empty
                                    : JsonSerializer.Serialize(fileRoute.AuthenticationOptions),
                                DelegatingHandlers = fileRoute.DelegatingHandlers == null
                                    ? string.Empty
                                    : JsonSerializer.Serialize(fileRoute.DelegatingHandlers),
                                DangerousAcceptAnyServerCertificateValidator = fileRoute.DangerousAcceptAnyServerCertificateValidator,
                                DownstreamHttpMethod = fileRoute.DownstreamHttpMethod,
                                DownstreamHttpVersion = fileRoute.DownstreamHttpVersion,
                                DownstreamPathTemplate = fileRoute.DownstreamPathTemplate,
                                DownstreamScheme = fileRoute.DownstreamScheme,
                                FileCacheOptions = fileRoute.FileCacheOptions == null
                                    ? string.Empty
                                    : JsonSerializer.Serialize(fileRoute.FileCacheOptions),
                                HttpHandlerOptions = fileRoute.HttpHandlerOptions == null
                                    ? string.Empty
                                    : JsonSerializer.Serialize(fileRoute.HttpHandlerOptions),
                                LoadBalancerOptions = fileRoute.LoadBalancerOptions == null
                                    ? string.Empty
                                    : JsonSerializer.Serialize(fileRoute.LoadBalancerOptions),
                                QoSOptions = fileRoute.QoSOptions == null
                                    ? string.Empty
                                    : JsonSerializer.Serialize(fileRoute.QoSOptions),
                                RateLimitOptions = fileRoute.RateLimitOptions == null
                                    ? string.Empty
                                    : JsonSerializer.Serialize(fileRoute.RateLimitOptions),
                                RouteIsCaseSensitive = fileRoute.RouteIsCaseSensitive,
                                ServiceName = fileRoute.ServiceName,
                                ServiceNamespace = fileRoute.ServiceNamespace
                            };
                            await rep.Context.Updateable<TgRoute>()
                            .SetColumns(u => new TgRoute
                            {
                                UpstreamPathTemplate = tgRoute.UpstreamPathTemplate,
                                UpstreamHost = tgRoute.UpstreamHost,
                                DownstreamHostAndPorts = tgRoute.DownstreamHostAndPorts,
                                AuthenticationOptions = tgRoute.AuthenticationOptions,
                                DelegatingHandlers = tgRoute.DelegatingHandlers,
                                DangerousAcceptAnyServerCertificateValidator = tgRoute.DangerousAcceptAnyServerCertificateValidator,
                                DownstreamHttpMethod = tgRoute.DownstreamHttpMethod,
                                DownstreamHttpVersion = tgRoute.DownstreamHttpVersion,
                                DownstreamPathTemplate = tgRoute.DownstreamPathTemplate,
                                DownstreamScheme = tgRoute.DownstreamScheme,
                                FileCacheOptions = tgRoute.FileCacheOptions,
                                HttpHandlerOptions = tgRoute.HttpHandlerOptions,
                                LoadBalancerOptions = tgRoute.LoadBalancerOptions,
                                QoSOptions = tgRoute.QoSOptions,
                                RateLimitOptions = tgRoute.RateLimitOptions,
                                RouteIsCaseSensitive = tgRoute.RouteIsCaseSensitive,
                                ServiceName = tgRoute.ServiceName,
                                ServiceNamespace = tgRoute.ServiceNamespace,
                                RouteProperties = tgRoute.RouteProperties,
                            })
                            .Where(u => u.RequestIdKey == tgRoute.RequestIdKey)
                            .ExecuteCommandAsync();
                        }
                    }
                }
                return await Task.FromResult<Response>(new OkResponse());
            }
                
        }
    }
}
