using Hx.Gateway.Application.Const;
using Hx.Gateway.Application.Options;
using Hx.Gateway.Application.RateLimit;
using Microsoft.Extensions.Options;
using Ocelot.Cache;

namespace Hx.Gateway.Web.Core.Authentication
{
    /// <summary>
    /// 实现自定义授权处理器逻辑
    /// </summary>
    public class CusAuthenticationProcessor : ICusAuthenticationProcessor
    {
        private readonly IClientAuthenticationRepository _clientAuthenticationRepository;
        private readonly OcelotSettingsOptions ocelotSettings;
        private readonly IOcelotCache<ClientRoleModel> _ocelotCache;
        public CusAuthenticationProcessor(IClientAuthenticationRepository clientAuthenticationRepository,
            IOptions<OcelotSettingsOptions> options,
            IOcelotCache<ClientRoleModel> ocelotCache)
        {
            _clientAuthenticationRepository = clientAuthenticationRepository;
            ocelotSettings = options.Value;
            _ocelotCache = ocelotCache;
        }
        /// <summary>
        /// 校验当前的请求地址客户端是否有权限访问
        /// </summary>
        /// <param name="clientid">客户端ID</param>
        /// <param name="path">请求地址</param>
        /// <returns></returns>
        public async Task<bool> CheckClientAuthenticationAsync(string clientid, string path)
        {
            var cachePrefix = $"{GatewayCacheConst.OcelotCacheKey}ClientAuthentication";
            var key = $"{cachePrefix}:{clientid}:{path}";
            var cacheResult = _ocelotCache.Get(key, cachePrefix);
            if (cacheResult != null)
            {//提取缓存数据
                return cacheResult.Role;
            }
            else
            {//重新获取认证信息
                var result = await _clientAuthenticationRepository.ClientAuthenticationAsync(clientid, path);
                //添加到缓存里
                _ocelotCache.Add(key, new ClientRoleModel() { CacheTime = DateTime.Now, Role = result }, TimeSpan.FromMinutes(ocelotSettings.CacheTime), cachePrefix);
                return result;
            }
        }
    }
}
