// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using Hx.Gateway.Core.Const;
using Hx.Gateway.Core.Options;
using Hx.Sdk.Cache;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Ocelot.Cache;

namespace Hx.Gateway.Core;
/// <summary>
/// 网关缓存
/// </summary>
/// <typeparam name="T"></typeparam>
public class OcelotCache<T> : IOcelotCache<T>
{
    private readonly OcelotSettingsOptions _ocelotSettings;
    private readonly ICache _cache;
    public OcelotCache(IOptions<OcelotSettingsOptions> options, ICache cache)
    {
        _ocelotSettings = options.Value;
        _cache = cache;
    }
    public void Add(string key, T value, TimeSpan ttl, string region)
    {
        var cacheKey = $"{GatewayCacheConst.OcelotCacheKey}{region}:{key}";
        _cache.Set(cacheKey, value, ttl);
    }
    public void AddAndDelete(string key, T value, TimeSpan ttl, string region)
    {
        Add(key, value, ttl, region);
    }

    public void ClearRegion(string region)
    {
        //_cache.RemoveByPrefixKey($"{GatewayCacheConst.OcelotCacheKey}{region}");
    }

    public T Get(string key, string region)
    {
        var cacheKey = $"{GatewayCacheConst.OcelotCacheKey}{region}:{key}";
        var result = _cache.Get<T>(key);
        if (result != null)
        {
            if (typeof(T) == typeof(CachedResponse))
            {//查看redis过期时间
                //var timeSpan = _cache.GetExpire(cacheKey);
                //if (timeSpan.TotalSeconds > 0)
                //{
                //    _cache.Set(key, result, timeSpan);
                //}
            }
            else
            {
                _cache.Set(key, result, TimeSpan.FromSeconds(_ocelotSettings.CacheTime));
            }
        }
        return result;
    }
}
