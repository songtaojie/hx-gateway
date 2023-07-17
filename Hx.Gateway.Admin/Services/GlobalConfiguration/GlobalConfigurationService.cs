using Hx.Gateway.Admin.Enum;
using Hx.Gateway.Application.Services.GlobalConfiguration.Dtos;
using Hx.Gateway.Core.Entity;
using Hx.Sdk.Sqlsugar;
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
        public async Task<GlobalConfigurationOutput> GetGlobalConfigurationAsync()
        {
            var entity = await _repository.Context.Queryable<TgGlobalConfiguration>()
                .Where(u => u.Status == StatusEnum.Enable)
                .OrderByDescending(u => u.CreateTime)
                .FirstAsync();
            if(entity == null) return new GlobalConfigurationOutput();

            return new GlobalConfigurationOutput()
            { 
                LoadBalancerOptions = new Admin.Options.Ocelot.LoadBalancerOptions(),
                HttpHandlerOptions = new Admin.Options.Ocelot.HttpHandlerOptions(),
                QoSOptions = new Admin.Options.Ocelot.QoSOptions(),
                ServiceDiscoveryProviderOptions = new Admin.Options.Ocelot.ServiceDiscoveryProviderOptions(),
                RateLimitOptions = new Admin.Options.Ocelot.RateLimitOptions()
            };
        }

        #endregion

        #region 新增

        /// <summary>
        /// 新增全局配置信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<int> AddGlobalConfigurationAsync(AddGlobalConfigurationInput request)
        {
            var tgGlobalConfiguration = new TgGlobalConfiguration
            { 
                BaseUrl = request.BaseUrl,
                DownstreamHttpVersion = request.DownstreamHttpVersion,
                DownstreamScheme = request.DownstreamScheme,
                HttpHandlerOptions = request.HttpHandlerOptions == null ?string.Empty:JsonSerializer.Serialize(request.HttpHandlerOptions),
                //ProjectId = request.ProjectId
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
        public async Task<string> UpdateGlobalConfigurationAsync(UpdateGlobalConfigurationInput request)
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
        public async Task<string> DeletGlobalConfigurationAsync(int globalConfigurationId)
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