// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using Hx.Core;
using Hx.Gateway.Application.Options;
using Hx.Gateway.Application.RateLimit;
using Microsoft.Extensions.Caching.Memory;
using NewLife.Caching;
using Ocelot.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Application;
/// <summary>
/// 网关缓存
/// </summary>
/// <typeparam name="T"></typeparam>
public class OcelotCache<T> : IOcelotCache<T>
{
    private readonly OcelotSettingsOptions _ocelotSettings;
    private readonly CacheManager _cacheManager;
    public OcelotCache(IOptions<OcelotSettingsOptions> options, CacheManager cacheManager)
    {
        _ocelotSettings = options.Value;
        _cacheManager = cacheManager;
    }
    public void Add(string key, T value, TimeSpan ttl, string region)
    {
        var cacheKey = $"{GatewayCacheConst.OcelotCacheKey}{region}:{key}";
        _cacheManager.Set(cacheKey, value, ttl);
    }
    public void AddAndDelete(string key, T value, TimeSpan ttl, string region)
    {
        Add(key, value, ttl, region);
    }

    public void ClearRegion(string region)
    {
        _cacheManager.RemoveByPrefixKey($"{GatewayCacheConst.OcelotCacheKey}{region}");
    }

    public T Get(string key, string region)
    {
        var cacheKey = $"{GatewayCacheConst.OcelotCacheKey}{region}:{key}";
        var result = _cacheManager.Get<T>(key);
        if (result != null)
        {
            if (typeof(T) == typeof(CachedResponse))
            {//查看redis过期时间
                var timeSpan = _cacheManager.GetExpire(cacheKey);
                if (timeSpan.TotalSeconds > 0)
                {
                    _cacheManager.Set(key, result, timeSpan);
                }
            }
            else
            {
                _cacheManager.Set(key, result, TimeSpan.FromSeconds(_ocelotSettings.CacheTime));
            }
        }
        return result;
    }
}
