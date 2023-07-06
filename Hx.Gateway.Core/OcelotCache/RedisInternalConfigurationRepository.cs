using Hx.Gateway.Core.Const;
using Hx.Gateway.Core.Options;
using Microsoft.Extensions.Options;
using Ocelot.Cache;
using Ocelot.Configuration;
using Ocelot.Configuration.Creator;
using Ocelot.Configuration.Repository;
using Ocelot.Responses;

namespace Hx.Gateway.Core;
public class RedisInternalConfigurationRepository : IInternalConfigurationRepository
{
    private readonly OcelotSettingsOptions _ocelotSettings;
    private IFileConfigurationRepository _fileConfigurationRepository;
    private IInternalConfigurationCreator _internalConfigurationCreator;
    private readonly IOcelotCache<InternalConfiguration> _ocelotCache;
    public RedisInternalConfigurationRepository(IOptions<OcelotSettingsOptions> options,
        IFileConfigurationRepository fileConfigurationRepository,
        IInternalConfigurationCreator internalConfigurationCreator,
        IOcelotCache<InternalConfiguration> ocelotCache)
    {
        _fileConfigurationRepository = fileConfigurationRepository;
        _internalConfigurationCreator = internalConfigurationCreator;
        _ocelotSettings = options.Value;
        _ocelotCache = ocelotCache;
    }

    /// <summary>
    /// 设置配置信息
    /// </summary>
    /// <param name="internalConfiguration">配置信息</param>
    /// <returns></returns>
    public Response AddOrReplace(IInternalConfiguration internalConfiguration)
    {
        var cacheKey = $"{GatewayCacheConst.OcelotCacheKey}{nameof(InternalConfiguration)}";
        _ocelotCache.Add(cacheKey, (InternalConfiguration)internalConfiguration, TimeSpan.FromSeconds(_ocelotSettings.CacheTime), "");
        return new OkResponse();
    }

    /// <summary>
    /// 从缓存中获取配置信息
    /// </summary>
    /// <returns></returns>
    public Response<IInternalConfiguration> Get()
    {
        var cacheKey = $"{GatewayCacheConst.OcelotCacheKey}{nameof(InternalConfiguration)}";
        var result = _ocelotCache.Get(cacheKey, "");
        if (result != null)
        {
            return new OkResponse<IInternalConfiguration>(result);
        }
        var fileconfig = _fileConfigurationRepository.Get().Result;
        var internalConfig = _internalConfigurationCreator.Create(fileconfig.Data).Result;
        AddOrReplace(internalConfig.Data);
        return new OkResponse<IInternalConfiguration>(internalConfig.Data);
    }
}