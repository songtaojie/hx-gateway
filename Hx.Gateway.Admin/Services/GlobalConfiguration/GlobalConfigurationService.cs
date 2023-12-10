using Hx.Gateway.Admin.Services.GlobalConfiguration.Dtos;
using Hx.Gateway.Admin.Services.Projects.Dto;
using Hx.Gateway.Application.Services.GlobalConfiguration.Dtos;
using Hx.Gateway.Application.Services.Projects;
using Hx.Gateway.Core;
using Hx.Gateway.Core.Entity;
using Hx.Gateway.Core.Options.Ocelot;
using System.Text.Json;
using System.Xml.Linq;

namespace Hx.Gateway.Application.Services.GlobalConfiguration
{
    public class GlobalConfigurationService
    {
        private readonly ISqlSugarRepository<TgGlobalConfiguration> _rep;
        public GlobalConfigurationService(ISqlSugarRepository<TgGlobalConfiguration> rep)
        {
            _rep = rep;
        }

        #region 查询
        /// <summary>
        /// 获取项目的全局配置
        /// </summary>
        /// <param name="projectCode">项目编码</param>
        /// <returns></returns>
        public async Task<OcelotGlobalConfigurationNode> GetByProjectIdAsync(Guid peojectId)
        {
            var entity = await _rep.Context.Queryable<TgGlobalConfiguration>()
                .Where(u => u.ProjectId == peojectId && u.Status == StatusEnum.Enable)
                .FirstAsync();
            if (entity == null) return null;

            return new OcelotGlobalConfigurationNode()
            {
                BaseUrl = entity.BaseUrl,
                DownstreamHttpVersion = entity.DownstreamHttpVersion,
                DownstreamScheme = entity.DownstreamScheme,
                RequestIdKey = entity.RequestIdKey,
                LoadBalancerOptions = string.IsNullOrEmpty(entity.LoadBalancerOptions)
                    ? new Hx.Gateway.Core.Options.Ocelot.LoadBalancerOptions()
                    : JsonSerializer.Deserialize<Hx.Gateway.Core.Options.Ocelot.LoadBalancerOptions>(entity.LoadBalancerOptions),
                HttpHandlerOptions = string.IsNullOrEmpty(entity.LoadBalancerOptions)
                    ? new Hx.Gateway.Core.Options.Ocelot.HttpHandlerOptions()
                    : JsonSerializer.Deserialize<Hx.Gateway.Core.Options.Ocelot.HttpHandlerOptions>(entity.LoadBalancerOptions),
                QoSOptions = string.IsNullOrEmpty(entity.QoSOptions)
                    ? new Hx.Gateway.Core.Options.Ocelot.QoSOptions()
                    : JsonSerializer.Deserialize<Hx.Gateway.Core.Options.Ocelot.QoSOptions>(entity.QoSOptions),
                ServiceDiscoveryProvider = string.IsNullOrEmpty(entity.ServiceDiscoveryProviderOptions)
                    ? new Hx.Gateway.Core.Options.Ocelot.ServiceDiscoveryProviderOptions()
                    : JsonSerializer.Deserialize<Hx.Gateway.Core.Options.Ocelot.ServiceDiscoveryProviderOptions>(entity.ServiceDiscoveryProviderOptions),
                RateLimitOptions = string.IsNullOrEmpty(entity.RateLimitOptions)
                    ? new Hx.Gateway.Core.Options.Ocelot.RateLimitOptions()
                    : JsonSerializer.Deserialize<Hx.Gateway.Core.Options.Ocelot.RateLimitOptions>(entity.RateLimitOptions)
            };
        }

        /// <summary>
        /// 查询全局配置信息
        /// </summary>
        /// <returns></returns>
        public async Task<GlobalConfigurationOutput> GetByIdAsync(Guid id)
        {
            var entity = await _rep.Context.Queryable<TgGlobalConfiguration>()
                .Where(u => u.Id == id)
                .OrderByDescending(u => u.CreateTime)
                .FirstAsync();
            if(entity == null) return new GlobalConfigurationOutput();

            return new GlobalConfigurationOutput()
            { 
                Id = entity.Id,
                BaseUrl = entity.BaseUrl,
                DownstreamHttpVersion = entity.DownstreamHttpVersion,
                DownstreamScheme = entity.DownstreamScheme,
                Name = entity.Name,
                ProjectId = entity.ProjectId,
                RequestIdKey = entity.RequestIdKey,
                Status = entity.Status,
                LoadBalancerOptions = string.IsNullOrEmpty(entity.LoadBalancerOptions)
                    ?new Hx.Gateway.Core.Options.Ocelot.LoadBalancerOptions()
                    :JsonSerializer.Deserialize<Hx.Gateway.Core.Options.Ocelot.LoadBalancerOptions>(entity.LoadBalancerOptions),
                HttpHandlerOptions = string.IsNullOrEmpty(entity.LoadBalancerOptions)
                    ? new Hx.Gateway.Core.Options.Ocelot.HttpHandlerOptions()
                    : JsonSerializer.Deserialize<Hx.Gateway.Core.Options.Ocelot.HttpHandlerOptions>(entity.LoadBalancerOptions),
                QoSOptions = string.IsNullOrEmpty(entity.QoSOptions)
                    ?   new Hx.Gateway.Core.Options.Ocelot.QoSOptions()
                    : JsonSerializer.Deserialize<Hx.Gateway.Core.Options.Ocelot.QoSOptions>(entity.QoSOptions),
                ServiceDiscoveryProviderOptions = string.IsNullOrEmpty(entity.ServiceDiscoveryProviderOptions) 
                    ? new Hx.Gateway.Core.Options.Ocelot.ServiceDiscoveryProviderOptions() 
                    : JsonSerializer.Deserialize<Hx.Gateway.Core.Options.Ocelot.ServiceDiscoveryProviderOptions>(entity.ServiceDiscoveryProviderOptions),
                RateLimitOptions = string.IsNullOrEmpty(entity.RateLimitOptions) 
                    ? new Hx.Gateway.Core.Options.Ocelot.RateLimitOptions() 
                    : JsonSerializer.Deserialize<Hx.Gateway.Core.Options.Ocelot.RateLimitOptions>(entity.RateLimitOptions)
            };
        }

        /// <summary>
        /// 分页查询项目信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedListResult<PageGlobalConfigurationOutput>> GetPageAsync(PageGlobalConfigurationInput input)
        {
            return await _rep.AsQueryable()
                .LeftJoin<TgProject>((u,p) => u.ProjectId == p.Id)
                .WhereIF(input.ProjectId.HasValue,u => u.ProjectId == input.ProjectId)
                .WhereIF(input.Status.HasValue, u => u.Status == input.Status)
                .OrderByDescending((u, p) => u.CreateTime)
                .Select((u, p) => new PageGlobalConfigurationOutput
                { 
                    Id = u.Id,
                    Name = u.Name,
                    ProjectId = u.ProjectId,
                    ProjectName = p.Name,
                    Status = u.Status,
                    CreateTime = u.CreateTime
                })
                .ToPagedListAsync(input.Page, input.PageSize);
        }

        #endregion

        #region 新增

        /// <summary>
        /// 新增全局配置信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> AddAsync(AddGlobalConfigurationInput request)
        {
            var tgGlobalConfiguration = new TgGlobalConfiguration
            { 
                Id = Guid.NewGuid(),
                ProjectId = request.ProjectId,
                BaseUrl = request.BaseUrl,
                Name = request.Name,
                CreateTime = DateTime.Now,
                RequestIdKey = request.RequestIdKey,
                DownstreamHttpVersion = request.DownstreamHttpVersion,
                DownstreamScheme = request.DownstreamScheme,
                HttpHandlerOptions = request.HttpHandlerOptions == null 
                    ?string.Empty
                    :JsonSerializer.Serialize(request.HttpHandlerOptions),
                LoadBalancerOptions = request.LoadBalancerOptions == null
                     ? string.Empty
                    : JsonSerializer.Serialize(request.LoadBalancerOptions),
                QoSOptions = request.QoSOptions == null
                     ? string.Empty
                    : JsonSerializer.Serialize(request.QoSOptions),
                RateLimitOptions = request.RateLimitOptions == null
                     ? string.Empty
                    : JsonSerializer.Serialize(request.RateLimitOptions),
                ServiceDiscoveryProviderOptions = request.ServiceDiscoveryProviderOptions == null
                     ? string.Empty
                    : JsonSerializer.Serialize(request.ServiceDiscoveryProviderOptions),
            };
            var insertResult = await _rep.Context.Insertable(tgGlobalConfiguration).ExecuteReturnIdentityAsync();
            return insertResult > 0;
        }

        #endregion

        #region 编辑

        /// <summary>
        /// 更新全局配置信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(UpdateGlobalConfigurationInput request)
        {
            var entity = await _rep.Context.Queryable<TgGlobalConfiguration>()
                .Where(u => u.Id == request.Id)
                .FirstAsync();
            if (entity == null)
                throw new UserFriendlyException("配置信息不存在");
            entity.RequestIdKey = request.RequestIdKey;
            entity.ProjectId = request.ProjectId;
            entity.BaseUrl = request.BaseUrl;
            entity.Name = request.Name;
            entity.DownstreamHttpVersion = request.DownstreamHttpVersion;
            entity.DownstreamScheme = request.DownstreamScheme;
            entity.HttpHandlerOptions = request.HttpHandlerOptions == null
                    ? string.Empty
                    : JsonSerializer.Serialize(request.HttpHandlerOptions);
            entity.LoadBalancerOptions = request.LoadBalancerOptions == null
                     ? string.Empty
                    : JsonSerializer.Serialize(request.LoadBalancerOptions);
            entity.QoSOptions = request.QoSOptions == null
                     ? string.Empty
                    : JsonSerializer.Serialize(request.QoSOptions);
            entity.RateLimitOptions = request.RateLimitOptions == null
                     ? string.Empty
                    : JsonSerializer.Serialize(request.RateLimitOptions);
            entity.ServiceDiscoveryProviderOptions = request.ServiceDiscoveryProviderOptions == null
                     ? string.Empty
                    : JsonSerializer.Serialize(request.ServiceDiscoveryProviderOptions);
            return await _rep.Context.Updateable(entity)
                   .ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// 更新全局配置信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<bool> UpdateStatusAsync(UpdateGlobalConfigurationStatusInput request)
        {
            var entity = await _rep.Context.Queryable<TgGlobalConfiguration>()
                .Where(u => u.Id == request.Id)
                .FirstAsync();
            if (entity == null)
                throw new UserFriendlyException("配置信息不存在");
            entity.Status = request.Status;
            return await _rep.Context.Updateable(entity)
                .UpdateColumns(u => new {u.Status,u.UpdaterId,u.UpdateTime })
                .ExecuteCommandHasChangeAsync();
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除全局配置信息
        /// </summary>
        /// <param name="input">全局配置Id</param>
        /// <returns></returns>
        public async Task<bool> DeletAsync(DeleteGlobalConfigurationInput input)
        {
            return await _rep.Context.Deleteable<TgGlobalConfiguration>()
                .Where(it => it.Id == input.Id)
                .ExecuteCommandHasChangeAsync();
        }

        #endregion
    }
}