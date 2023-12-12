using Hx.Gateway.Core.Entity;
using Hx.Gateway.Core.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Ocelot.Configuration.File;
using Ocelot.Configuration.Repository;
using Ocelot.Responses;
using System.Text.Json;

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
            if (string.IsNullOrEmpty(_ocelotSettings.ProjectCode))
                throw new Exception("Missing project code configuration");
            using (var serviceScope = _provider.CreateScope())
            {
                var services = serviceScope.ServiceProvider;

                var rep = services.GetRequiredService<ISqlSugarRepository<TgGlobalConfiguration>>();
                var file = new FileConfiguration();
                var project = await rep.Context.Queryable<TgProject>()
                    .Where(u => u.Code == _ocelotSettings.ProjectCode)
                    .Select(u => new { u.Id,u.Status })
                    .FirstAsync();
                if(project == null)
                    throw new Exception($"未找到编码为【{_ocelotSettings.ProjectCode}】的项目信息");
                if (project.Status == StatusEnum.Disable)
                    throw new Exception($"项目【{_ocelotSettings.ProjectCode}】已停用");
                //提取默认启用的路由配置信息
                var globalConfiguration = await rep.AsQueryable()
                    .Where(u => u.ProjectId == project.Id && u.Status == StatusEnum.Enable)
                    .FirstAsync();
                if (globalConfiguration != null)
                {
                    //赋值全局信息
                    file.GlobalConfiguration = GetFileGlobalConfiguration(globalConfiguration);
                }
                //提取所有路由信息
                var routeList = await rep.Context.Queryable<TgRoute>()
                   .Includes(u => u.DownstreamHostAndPorts)
                   .Includes(u => u.RouteProperties)
                   .Where(u => u.ProjectId == project.Id && u.Status == StatusEnum.Enable)
                   .ToListAsync();
                if (!routeList.Any())
                    throw new Exception($"项目【{_ocelotSettings.ProjectCode}】中未找到任何可用的配置信息");
                var reroutelist = new List<FileRoute>();
                routeList.ForEach(r =>
                {
                    reroutelist.Add(GetFileRoute(r));
                });
                file.Routes = reroutelist;
                #endregion
                return new OkResponse<FileConfiguration>(file);
            }
        }

        public async Task<Response> Set(FileConfiguration fileConfiguration)
        {
            using var serviceScope = _provider.CreateScope();
            var services = serviceScope.ServiceProvider;

            var rep = services.GetRequiredService<ISqlSugarRepository<TgGlobalConfiguration>>();
            if (fileConfiguration != null && fileConfiguration.GlobalConfiguration != null && !string.IsNullOrEmpty(fileConfiguration.GlobalConfiguration.RequestIdKey))
            {
                var project = await rep.Context.Queryable<TgProject>()
                    .Where(u => u.Code == _ocelotSettings.ProjectCode)
                    .Select(u => new { u.Id, u.Status })
                    .FirstAsync();
                if (project == null)
                    return await Task.FromResult<Response>(new OkResponse());

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


        private FileGlobalConfiguration GetFileGlobalConfiguration(TgGlobalConfiguration globalConfiguration)
        {
            var result = new FileGlobalConfiguration
            {
                BaseUrl = globalConfiguration.BaseUrl,
                DownstreamScheme = globalConfiguration.DownstreamScheme,
                DownstreamHttpVersion = globalConfiguration.DownstreamHttpVersion,
                RequestIdKey = globalConfiguration.RequestIdKey,
            };
            if (!string.IsNullOrEmpty(globalConfiguration.HttpHandlerOptions))
            {
                result.HttpHandlerOptions = JsonSerializer.Deserialize<FileHttpHandlerOptions>(globalConfiguration.HttpHandlerOptions);
            }
            if (!string.IsNullOrEmpty(globalConfiguration.LoadBalancerOptions))
            {
                result.LoadBalancerOptions = JsonSerializer.Deserialize<FileLoadBalancerOptions>(globalConfiguration.LoadBalancerOptions);
            }
            if (!string.IsNullOrEmpty(globalConfiguration.QoSOptions))
            {
                var options = JsonSerializer.Deserialize<Options.Ocelot.QoSOptions>(globalConfiguration.QoSOptions);
                if (options != null && options.Enabled == true)
                {
                    result.QoSOptions = new FileQoSOptions
                    {
                        ExceptionsAllowedBeforeBreaking = options.ExceptionsAllowedBeforeBreaking ?? 0,
                        DurationOfBreak = options.DurationOfBreak ?? 0,
                        TimeoutValue = options.TimeoutValue ?? 0,
                    };
                }
            }
            if (!string.IsNullOrEmpty(globalConfiguration.ServiceDiscoveryProviderOptions))
            {
                var options = JsonSerializer.Deserialize<Options.Ocelot.ServiceDiscoveryProviderOptions>(globalConfiguration.ServiceDiscoveryProviderOptions);
                if (options != null && options.Enabled == true)
                {
                    result.ServiceDiscoveryProvider = new FileServiceDiscoveryProvider
                    {
                        ConfigurationKey = options.ConfigurationKey,
                        Host = options.Host,
                        Namespace = options.Namespace,
                        PollingInterval = options.PollingInterval ?? 0,
                        Port = options.Port ?? 0,
                        Scheme = options.Scheme,
                        Token = options.Token,
                        Type = options.Type,
                    };
                }
            }
            if (!string.IsNullOrEmpty(globalConfiguration.RateLimitOptions))
            {
                var options = JsonSerializer.Deserialize<Options.Ocelot.RateLimitOptions>(globalConfiguration.RateLimitOptions);
                if (options != null && options.EnableRateLimiting == true)
                {
                    result.RateLimitOptions = new FileRateLimitOptions
                    {
                    };
                }
            }
            return result;
        }

        private FileRoute GetFileRoute(TgRoute route)
        {
            var fileRoute = new FileRoute()
            {
                Key = route.RequestIdKey,
                RequestIdKey = route.RequestIdKey,
                DownstreamPathTemplate = route.DownstreamPathTemplate,
                DownstreamScheme = route.DownstreamScheme,
                Priority = route.Priority ?? 0,
                ServiceName = route.ServiceName,
                ServiceNamespace = route.ServiceNamespace,
                UpstreamHost = route.UpstreamHost,
                UpstreamPathTemplate = route.UpstreamPathTemplate,
            };
            if (!string.IsNullOrEmpty(route.AuthenticationOptions))
            {
                fileRoute.AuthenticationOptions = JsonSerializer.Deserialize<FileAuthenticationOptions>(route.AuthenticationOptions);
            }
            if (!string.IsNullOrEmpty(route.FileCacheOptions))
            {
                fileRoute.FileCacheOptions = JsonSerializer.Deserialize<FileCacheOptions>(route.FileCacheOptions);
            }
            if (!string.IsNullOrEmpty(route.DelegatingHandlers))
            {
                fileRoute.DelegatingHandlers = JsonSerializer.Deserialize<List<string>>(route.DelegatingHandlers);
            }
            if (!string.IsNullOrEmpty(route.LoadBalancerOptions))
            {
                fileRoute.LoadBalancerOptions = JsonSerializer.Deserialize<FileLoadBalancerOptions>(route.LoadBalancerOptions);
            }
            if (!string.IsNullOrEmpty(route.QoSOptions))
            {
                var options = JsonSerializer.Deserialize<Options.Ocelot.QoSOptions>(route.QoSOptions);
                if (options != null && options.Enabled == true)
                {
                    fileRoute.QoSOptions = new FileQoSOptions
                    {
                        ExceptionsAllowedBeforeBreaking = options.ExceptionsAllowedBeforeBreaking ?? 0,
                        DurationOfBreak = options.DurationOfBreak ?? 0,
                        TimeoutValue = options.TimeoutValue ?? 0,
                    };
                }
            }
            if (route.DownstreamHostAndPorts != null)
            {
                fileRoute.DownstreamHostAndPorts = route.DownstreamHostAndPorts.Select(s => new FileHostAndPort
                {
                    Host = s.Host,
                    Port = s.Port,
                }).ToList();
            }
            if (!string.IsNullOrEmpty(route.UpstreamHttpMethod))
            {
                fileRoute.UpstreamHttpMethod = route.UpstreamHttpMethod.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            }
            return fileRoute;
        }
    }
}
