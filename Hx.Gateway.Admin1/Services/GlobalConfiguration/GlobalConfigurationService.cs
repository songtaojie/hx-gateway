using Hx.Gateway.Admin.Services.GlobalConfiguration.Dtos;
using Hx.Gateway.Admin.Services.Projects.Dto;
using Hx.Gateway.Application.Services.GlobalConfiguration.Dtos;
using Hx.Gateway.Application.Services.Projects;
using Hx.Gateway.Core;
using Hx.Gateway.Core.Entity;
using System.Text.Json;

namespace Hx.Gateway.Application.Services.GlobalConfiguration
{
    public class GlobalConfigurationService
    {
        private readonly ISqlSugarRepository<TgGlobalConfiguration> _repository;
        public GlobalConfigurationService(ISqlSugarRepository<TgGlobalConfiguration> repository)
        {
            _repository = repository;
        }

        #region 查询

        /// <summary>
        /// 查询全局配置信息
        /// </summary>
        /// <returns></returns>
        public async Task<GlobalConfigurationOutput> GetByIdAsync(Guid id)
        {
            var entity = await _repository.Context.Queryable<TgGlobalConfiguration>()
                .Where(u => u.Id == id)
                .OrderByDescending(u => u.CreateTime)
                .FirstAsync();
            if(entity == null) return new GlobalConfigurationOutput();

            return new GlobalConfigurationOutput()
            { 
                LoadBalancerOptions = new Hx.Gateway.Core.Options.Ocelot.LoadBalancerOptions(),
                HttpHandlerOptions = new Hx.Gateway.Core.Options.Ocelot.HttpHandlerOptions(),
                QoSOptions = new Hx.Gateway.Core.Options.Ocelot.QoSOptions(),
                ServiceDiscoveryProviderOptions = new Hx.Gateway.Core.Options.Ocelot.ServiceDiscoveryProviderOptions(),
                RateLimitOptions = new Hx.Gateway.Core.Options.Ocelot.RateLimitOptions()
            };
        }

        /// <summary>
        /// 分页查询项目信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedListResult<PageGlobalConfigurationOutput>> GetPageAsync(PageGlobalConfigurationInput input)
        {
            return await _repository.AsQueryable()
                .LeftJoin<TgProject>((u,p) => u.ProjectId == p.Id)
                .WhereIF(input.Status.HasValue, u => u.Status == input.Status)
                .OrderByDescending((u, p) => u.CreateTime)
                .Select((u, p) => new PageGlobalConfigurationOutput
                { 
                    Id = u.Id,
                    Name = u.Name,
                    ProjectId = u.ProjectId,
                    ProjectName = p.Name,
                    Status = u.Status
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
        public async Task<int> AddAsync(GlobalConfigurationOutput request)
        {
            var tgGlobalConfiguration = new TgGlobalConfiguration
            { 
                Id = Guid.NewGuid(),
                ProjectId = request.ProjectId,
                BaseUrl = request.BaseUrl,
                Name = request.Name,
                Status = request.Status,    
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
            var insertResult = await _repository.Context.Insertable(tgGlobalConfiguration).ExecuteReturnIdentityAsync();
            if (insertResult > 0)
            {
                return insertResult;
            }
            else
            {
                throw new Exception("新增全局配置失败");
            }
        }

        #endregion

        #region 编辑

        /// <summary>
        /// 更新全局配置信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<string> UpdateAsync(GlobalConfigurationOutput request)
        {
            var tgGlobalConfiguration = new TgGlobalConfiguration
            {

            }; 
            var result = await _repository.Context.Updateable(tgGlobalConfiguration)
                   .ExecuteCommandHasChangeAsync();
            if (result)
            {
                return "编辑全局配置成功";
            }
            else
            {
                throw new Exception("编辑全局配置失败");
            }
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除全局配置信息
        /// </summary>
        /// <param name="globalConfigurationId">全局配置Id</param>
        /// <returns></returns>
        public async Task<string> DeletGlobalConfigurationAsync(Guid globalConfigurationId)
        {
            var result = await _repository.Context.Deleteable<TgGlobalConfiguration>()
                .Where(it => it.Id == globalConfigurationId)
                .ExecuteCommandHasChangeAsync();
            if (result)
            {
                return "删除全局配置成功";
            }
            else
            {
                throw new Exception("删除全局配置失败");
            }
        }

        #endregion
    }
}