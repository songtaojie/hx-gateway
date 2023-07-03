using Hx.Gateway.Application.Services.GlobalConfiguration.Dtos;

namespace Hx.Gateway.Application.Services.GlobalConfiguration
{
    public class GlobalConfigurationService : IDynamicApiController, ITransient
    {
        private readonly SqlSugarRepository<TgGlobalConfiguration> _repository;
        public GlobalConfigurationService(SqlSugarRepository<TgGlobalConfiguration> repository)
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
                .With(SqlWith.NoLock)
                .Where(u => u.Status == StatusEnum.Enable)
                .OrderByDescending(u => u.CreateTime)
                .FirstAsync();
            return entity?.Adapt<GlobalConfigurationOutput>() ?? new GlobalConfigurationOutput()
            { 
                LoadBalancerOptions = new Options.Ocelot.LoadBalancerOptions(),
                HttpHandlerOptions = new Options.Ocelot.HttpHandlerOptions(),
                QoSOptions = new Options.Ocelot.QoSOptions(),
                ServiceDiscoveryProviderOptions = new Options.Ocelot.ServiceDiscoveryProviderOptions(),
                RateLimitOptions = new Options.Ocelot.RateLimitOptions()
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
            var TgGlobalConfiguration = request.Adapt<TgGlobalConfiguration>();
            var insertResult = await _repository.Context.Insertable(TgGlobalConfiguration).ExecuteReturnIdentityAsync();
            if (insertResult > 0)
            {
                return insertResult;
            }
            else
            {
                throw Oops.Oh(GatewayErrorCodeEnum.INSERT_GLOBAL_CONFIGURATION_FAIL).StatusCode((int)GatewayErrorCodeEnum.INSERT_GLOBAL_CONFIGURATION_FAIL);
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
            var TgGlobalConfiguration = request.Adapt<TgGlobalConfiguration>();
            var result = await _repository.Context.Updateable(TgGlobalConfiguration)
                   .ExecuteCommandHasChangeAsync();
            if (result)
            {
                return "编辑全局配置成功";
            }
            else
            {
                throw Oops.Oh(GatewayErrorCodeEnum.UPDATE_GLOBAL_CONFIGURATION_FAIL).StatusCode((int)GatewayErrorCodeEnum.UPDATE_GLOBAL_CONFIGURATION_FAIL);
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
                throw Oops.Oh(GatewayErrorCodeEnum.DELETE_GLOBAL_CONFIGURATION_FAIL).StatusCode((int)GatewayErrorCodeEnum.DELETE_GLOBAL_CONFIGURATION_FAIL);
            }
        }

        #endregion
    }
}