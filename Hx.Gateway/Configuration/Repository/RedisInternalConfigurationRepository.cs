// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Ocelot.Configuration.Repository;
using Ocelot.Configuration;
using Ocelot.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hx.Sdk.Cache;
using Hx.Gateway.Core.Const;
using Hx.Gateway.Core.Options;
using Microsoft.Extensions.Options;
using Ocelot.Cache;
using Ocelot.Configuration.Creator;

namespace Hx.Gateway.Core.Configuration.Repository;

/// <summary>
/// 使用redis存储内部配置信息
/// </summary>
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
        var cacheKey = $"{CacheConst.Prefix}:InterConfig";
        _ocelotCache.Add(cacheKey, (InternalConfiguration)internalConfiguration, TimeSpan.FromSeconds(_ocelotSettings.CacheTime), "");
        return new OkResponse();
    }

    /// <summary>
    /// 从缓存中获取配置信息
    /// </summary>
    /// <returns></returns>
    public Response<IInternalConfiguration> Get()
    {
        var cacheKey = $"{CacheConst.Prefix}:InterConfig";
        var result = _ocelotCache.Get(cacheKey, "");
        if (result != null)
        {
            return new OkResponse<IInternalConfiguration>(result);
        }
        var fileconfig = _fileConfigurationRepository.Get().GetAwaiter().GetResult();
        var internalConfig = _internalConfigurationCreator.Create(fileconfig.Data).GetAwaiter().GetResult();
        AddOrReplace(internalConfig.Data);
        return new OkResponse<IInternalConfiguration>(internalConfig.Data);
    }
}
