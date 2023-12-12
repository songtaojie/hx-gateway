// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Hx.Gateway.Admin.Services.Routes.Dtos;
using Hx.Gateway.Application.Services.GlobalConfiguration;
using Hx.Gateway.Application.Services.Routes.Dtos;
using Hx.Gateway.Core;
using Hx.Gateway.Core.Entity;
using Hx.Gateway.Core.Options.Ocelot;
using System.Text.Json;

namespace Hx.Gateway.Application.Services.Routes
{
    public class RouteService
    {
        private readonly ISqlSugarRepository<TgRoute> _rep;
        private readonly GlobalConfigurationService _globalConfigurationService;
        public RouteService(ISqlSugarRepository<TgRoute> rep,
            GlobalConfigurationService globalConfigurationService)
        {
            _rep = rep;
            _globalConfigurationService = globalConfigurationService;
        }

        #region 查询

        /// <summary>
        /// 预览路由信息
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetRoutePreviewAsync(string projectCode)
        {
            var ocelotRoot = new OcelotRoot();
            var project = await _rep.Context.Queryable<TgProject>()
               .Where(u => u.Code == projectCode && u.Status == StatusEnum.Enable)
               .Select(u => new { u.Id })
               .FirstAsync();
            if (project == null) JsonSerializer.Serialize(ocelotRoot);

            // 获取全局变量
            var globalConfiguration = await _globalConfigurationService.GetByProjectIdAsync(project.Id);
            if (globalConfiguration != null)
            {
                ocelotRoot.GlobalConfiguration = globalConfiguration;
            }
            ocelotRoot.Routes = await GetRouteByProjectIdAsync(project.Id);
            return JsonSerializer.Serialize(ocelotRoot);
        }


        /// <summary>
        /// 查询路由具体信息
        /// </summary>
        /// <param name="projectId">项目Id</param>
        /// <returns></returns>
        public async Task<IEnumerable<OcelotRouteNode>> GetRouteByProjectIdAsync(Guid peojectId)
        {
            var routeList = await _rep.Context.Queryable<TgRoute>()
                .Includes<TgRouteHostPort>(u => u.DownstreamHostAndPorts)
                .Includes(u => u.RouteProperties)
                .Where(u => u.ProjectId == peojectId && u.Status == StatusEnum.Enable)
                .ToListAsync();
            if (routeList == null || !routeList.Any())
                return Array.Empty<OcelotRouteNode>();
            return routeList.Select(route => new OcelotRouteNode
            {
                AuthenticationOptions = string.IsNullOrEmpty(route.AuthenticationOptions)
                    ? new AuthenticationOptions()
                    : JsonSerializer.Deserialize<AuthenticationOptions>(route.AuthenticationOptions),
                DangerousAcceptAnyServerCertificateValidator = route.DangerousAcceptAnyServerCertificateValidator,
                DelegatingHandlers = string.IsNullOrEmpty(route.DelegatingHandlers)
                    ? Array.Empty<string>()
                    : route.DelegatingHandlers.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries),
                DownstreamHostAndPorts = route.DownstreamHostAndPorts == null
                    ? Array.Empty<DownstreamHostAndPortOptions>()
                    : route.DownstreamHostAndPorts.Select(r => new DownstreamHostAndPortOptions { Host = r.Host, Port = r.Port }),
                DownstreamHttpMethod = route.DownstreamHttpMethod,
                DownstreamPathTemplate = route.DownstreamPathTemplate,
                DownstreamScheme = route.DownstreamScheme,
                FileCacheOptions = string.IsNullOrEmpty(route.FileCacheOptions)
                    ? null
                    : JsonSerializer.Deserialize<FileCacheOptions>(route.FileCacheOptions),
                HttpHandlerOptions = string.IsNullOrEmpty(route.HttpHandlerOptions)
                    ? null
                    : JsonSerializer.Deserialize<HttpHandlerOptions>(route.HttpHandlerOptions),
                LoadBalancerOptions = string.IsNullOrEmpty(route.LoadBalancerOptions)
                    ? null
                    : JsonSerializer.Deserialize<LoadBalancerOptions>(route.LoadBalancerOptions),
                QoSOptions = string.IsNullOrEmpty(route.QoSOptions)
                    ? null
                    : JsonSerializer.Deserialize<QoSOptions>(route.QoSOptions),
                RateLimitOptions = string.IsNullOrEmpty(route.RateLimitOptions)
                    ? null
                    : JsonSerializer.Deserialize<RateLimitOptions>(route.RateLimitOptions),
                RequestIdKey = route.RequestIdKey,
                RouteIsCaseSensitive = route.RouteIsCaseSensitive,
                ServiceName = route.ServiceName,
                UpstreamHttpMethod = string.IsNullOrEmpty(route.UpstreamHttpMethod)
                    ? Array.Empty<string>()
                    : route.UpstreamHttpMethod.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries),
                UpstreamPathTemplate = route.UpstreamPathTemplate,
                UseServiceDiscovery = route.UseServiceDiscovery
            });
        }

        /// <summary>
        /// 分页查询项目信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedListResult<PageRouteOutput>> GetPageAsync(PageRouteInput input)
        {
            return await _rep.Context.Queryable<TgRoute>()
                .LeftJoin<TgProject>((u,p) => u.ProjectId == p.Id)
                .WhereIF(input.ProjectId.HasValue, (u, p) => u.ProjectId == input.ProjectId)
                .WhereIF(input.Status.HasValue, (u, p) => u.Status == input.Status)
                .WhereIF(!string.IsNullOrWhiteSpace(input.UpstreamPathTemplate), (u, p) => u.UpstreamPathTemplate.Contains(input.UpstreamPathTemplate))
                .WhereIF(!string.IsNullOrWhiteSpace(input.DownstreamPathTemplate), (u, p) => u.DownstreamPathTemplate.Contains(input.DownstreamPathTemplate))
                .WhereIF(!string.IsNullOrWhiteSpace(input.RequestIdKey), (u, p) => u.RequestIdKey.Contains(input.RequestIdKey))
                .OrderByDescending((u, p) => u.CreateTime)
                .Select((u, p) => new PageRouteOutput
                { 
                    Id = u.Id,
                    DownstreamHttpMethod = u.DownstreamHttpMethod,
                    DownstreamHttpVersion = u.DownstreamHttpVersion,
                    DownstreamPathTemplate = u.DownstreamPathTemplate,
                    DownstreamScheme = u.DownstreamScheme,
                    ProjectName = p.Name,
                    UpstreamPathTemplate = u.UpstreamPathTemplate,
                    RequestIdKey = u.RequestIdKey,
                    Sort = u.Sort,
                    Status = u.Status,
                    UpstreamHttpMethod = u.UpstreamHttpMethod,
                })
                .ToPagedListAsync(input.Page, input.PageSize);
        }

        /// <summary>
        /// 查询路由具体信息
        /// </summary>
        /// <param name="routeId"></param>
        /// <returns></returns>
        public async Task<DetailRouteOutput> GetByIdAsync(Guid routeId)
        {
            var route = await _rep.Context.Queryable<TgRoute>()
                 .Includes(o => o.DownstreamHostAndPorts)
                 .Includes(o => o.RouteProperties)
                 .Where(o => o.Id == routeId)
                 .FirstAsync();
            if (route == null)
                throw new UserFriendlyException("当前路由信息不存在");
            return new DetailRouteOutput
            {
                Id = route.Id,
                ProjectId = route.ProjectId,
                DownstreamHttpVersion = route.DownstreamHttpVersion,
                Priority = route.Priority,
                RouteProperties = route.RouteProperties,
                ServiceNamespace = route.ServiceNamespace,
                UpstreamHost = route.UpstreamHost,
                AuthenticationOptions = string.IsNullOrEmpty(route.AuthenticationOptions)
                    ? new AuthenticationOptions()
                    : JsonSerializer.Deserialize<AuthenticationOptions>(route.AuthenticationOptions),
                DangerousAcceptAnyServerCertificateValidator = route.DangerousAcceptAnyServerCertificateValidator,
                DelegatingHandlers = string.IsNullOrEmpty(route.DelegatingHandlers)
                    ? Array.Empty<string>()
                    : route.DelegatingHandlers.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries),
                DownstreamHostAndPorts = route.DownstreamHostAndPorts,
                DownstreamHttpMethod = route.DownstreamHttpMethod,
                DownstreamPathTemplate = route.DownstreamPathTemplate,
                DownstreamScheme = route.DownstreamScheme,
                FileCacheOptions = string.IsNullOrEmpty(route.FileCacheOptions)
                    ? null
                    : JsonSerializer.Deserialize<FileCacheOptions>(route.FileCacheOptions),
                HttpHandlerOptions = string.IsNullOrEmpty(route.HttpHandlerOptions)
                    ? null
                    : JsonSerializer.Deserialize<HttpHandlerOptions>(route.HttpHandlerOptions),
                LoadBalancerOptions = string.IsNullOrEmpty(route.LoadBalancerOptions)
                    ? null
                    : JsonSerializer.Deserialize<LoadBalancerOptions>(route.LoadBalancerOptions),
                QoSOptions = string.IsNullOrEmpty(route.QoSOptions)
                    ? null
                    : JsonSerializer.Deserialize<QoSOptions>(route.QoSOptions),
                RateLimitOptions = string.IsNullOrEmpty(route.RateLimitOptions)
                    ? null
                    : JsonSerializer.Deserialize<RateLimitOptions>(route.RateLimitOptions),
                RequestIdKey = route.RequestIdKey,
                RouteIsCaseSensitive = route.RouteIsCaseSensitive,
                ServiceName = route.ServiceName,
                UpstreamHttpMethod = string.IsNullOrEmpty(route.UpstreamHttpMethod)
                    ? Array.Empty<string>()
                    : route.UpstreamHttpMethod.Split(new char[] { ',', '，' }, StringSplitOptions.RemoveEmptyEntries),
                UpstreamPathTemplate = route.UpstreamPathTemplate,
                UseServiceDiscovery = route.UseServiceDiscovery
            };
        }

        #endregion

        #region 新增

        /// <summary>
        /// 新增路由信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(AddRouteInput input)
        {
            var route = new TgRoute
            { 
                AuthenticationOptions = input.AuthenticationOptions == null ?string.Empty :JsonSerializer.Serialize(input.AuthenticationOptions),
                UseServiceDiscovery = input.UseServiceDiscovery,
                CreateTime = DateTime.Now,
                CreatorId = Guid.Empty,
                DangerousAcceptAnyServerCertificateValidator = input.DangerousAcceptAnyServerCertificateValidator,
                DelegatingHandlers = input.DelegatingHandlers == null ?string.Empty :string.Join(',', input.DelegatingHandlers),
                DownstreamHostAndPorts = input.DownstreamHostAndPorts,
                DownstreamHttpMethod = input.DownstreamHttpMethod,
                DownstreamHttpVersion = input.DownstreamHttpVersion,
                DownstreamPathTemplate = input.DownstreamPathTemplate,
                DownstreamScheme = input.DownstreamScheme,
                FileCacheOptions = input.FileCacheOptions == null ? string.Empty:JsonSerializer.Serialize(input.FileCacheOptions),
                HttpHandlerOptions = input.HttpHandlerOptions == null ? string.Empty : JsonSerializer.Serialize(input.FileCacheOptions),
                LoadBalancerOptions = input.LoadBalancerOptions == null ? string.Empty : JsonSerializer.Serialize(input.FileCacheOptions),
                Priority = input.Priority,
                ProjectId = input.ProjectId,
                QoSOptions = input.QoSOptions == null ? string.Empty : JsonSerializer.Serialize(input.FileCacheOptions),
                RateLimitOptions = input.RateLimitOptions == null ? string.Empty : JsonSerializer.Serialize(input.FileCacheOptions),
                RequestIdKey = input.RequestIdKey,
                RouteIsCaseSensitive = input.RouteIsCaseSensitive,
                RouteProperties = input.RouteProperties,
                ServiceName = input.ServiceName,
                ServiceNamespace = input.ServiceNamespace,
                Sort = input.Sort,
                UpstreamHost = input.UpstreamHost,
                UpstreamHttpMethod = input.UpstreamHttpMethod  == null? string.Empty:string.Join(",", input.UpstreamHttpMethod),
                UpstreamPathTemplate = input.UpstreamPathTemplate,

            };
            return await _rep.Context.InsertNav(route)
                .Include(o => o.DownstreamHostAndPorts)
                .Include(o => o.RouteProperties)
                .ExecuteCommandAsync();
        }

        #endregion

        #region 编辑

        /// <summary>
        /// 更新路由信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(UpdateRouteInput input)
        {
            var route = new TgRoute
            {
                Id = input.Id,
                AuthenticationOptions = input.AuthenticationOptions == null ? string.Empty : JsonSerializer.Serialize(input.AuthenticationOptions),
                UseServiceDiscovery = input.UseServiceDiscovery,
                CreateTime = DateTime.Now,
                CreatorId = Guid.Empty,
                DangerousAcceptAnyServerCertificateValidator = input.DangerousAcceptAnyServerCertificateValidator,
                DelegatingHandlers = input.DelegatingHandlers == null ? string.Empty : string.Join(',', input.DelegatingHandlers),
                DownstreamHostAndPorts = input.DownstreamHostAndPorts,
                DownstreamHttpMethod = input.DownstreamHttpMethod,
                DownstreamHttpVersion = input.DownstreamHttpVersion,
                DownstreamPathTemplate = input.DownstreamPathTemplate,
                DownstreamScheme = input.DownstreamScheme,
                FileCacheOptions = input.FileCacheOptions == null ? string.Empty : JsonSerializer.Serialize(input.FileCacheOptions),
                HttpHandlerOptions = input.HttpHandlerOptions == null ? string.Empty : JsonSerializer.Serialize(input.FileCacheOptions),
                LoadBalancerOptions = input.LoadBalancerOptions == null ? string.Empty : JsonSerializer.Serialize(input.FileCacheOptions),
                Priority = input.Priority,
                ProjectId = input.ProjectId,
                QoSOptions = input.QoSOptions == null ? string.Empty : JsonSerializer.Serialize(input.FileCacheOptions),
                RateLimitOptions = input.RateLimitOptions == null ? string.Empty : JsonSerializer.Serialize(input.FileCacheOptions),
                RequestIdKey = input.RequestIdKey,
                RouteIsCaseSensitive = input.RouteIsCaseSensitive,
                RouteProperties = input.RouteProperties,
                ServiceName = input.ServiceName,
                ServiceNamespace = input.ServiceNamespace,
                Sort = input.Sort,
                UpstreamHost = input.UpstreamHost,
                UpstreamHttpMethod = input.UpstreamHttpMethod == null ? string.Empty : string.Join(",", input.UpstreamHttpMethod),
                UpstreamPathTemplate = input.UpstreamPathTemplate,

            };
            return await _rep.Context.UpdateNav(route)
                .Include(o => o.DownstreamHostAndPorts)
                .Include(o => o.RouteProperties)
                .ExecuteCommandAsync();
        }

        /// <summary>
        /// 启用或禁用路由
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> UpdateStatusAsync(UpdateRouteStatusInput input)
        {
            var route = await _rep.Context.Queryable<TgRoute>()
                .Where(u => u.Id == input.Id)
                .Select(u => new TgRoute
                {
                   Id = u.Id
                })
                .FirstAsync();
            if (route == null)
                throw new UserFriendlyException("路由信息不存在");
            route.Status = input.Status;

            return await _rep.Context.Updateable<TgRoute>(route)
                .UpdateColumns(u => new {u.Status,u.UpdateTime,u.UpdaterId })
                .ExecuteCommandHasChangeAsync();
        }
        #endregion

        #region 删除

        /// <summary>
        /// 删除路由信息
        /// </summary>
        /// <param name="routeId">路由Id</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(DeleteRouteInput input)
        {
            return await _rep.Context.DeleteNav<TgRoute>(u => u.Id == input.Id)
                .Include(u => u.DownstreamHostAndPorts)
                .Include(u => u.RouteProperties)
                .ExecuteCommandAsync();
        }

        #endregion
    }
}