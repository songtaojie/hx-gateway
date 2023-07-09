// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Hx.Gateway.Core.Const;
using Hx.Sdk.Cache;
using Ocelot.Cache;

namespace Hx.Gateway.Core.Cache;

/// <summary>
/// 自定义缓存实现
/// </summary>
/// <typeparam name="T"></typeparam>
public class RedisOcelotCache<T> : IOcelotCache<T>
{
    private readonly ICache _cache;
    public RedisOcelotCache(ICache cache)
    {
        _cache = cache;
    }

    /// <summary>
    /// 添加缓存信息
    /// </summary>
    /// <param name="key">缓存的key</param>
    /// <param name="value">缓存的实体</param>
    /// <param name="ttl">过期时间</param>
    /// <param name="region">缓存所属分类，可以指定分类缓存过期</param>
    public void Add(string key, T value, TimeSpan ttl, string region)
    {
        if (ttl.TotalMilliseconds <= 0)
            return;
        key = GetKey(region, key);
        _cache.Set(key, value, TimeSpan.FromSeconds(ttl.TotalSeconds));
    }


    public void AddAndDelete(string key, T value, TimeSpan ttl, string region)
    {
        Add(key, value, ttl, region);
    }

    /// <summary>
    /// 批量移除regin开头的所有缓存记录
    /// </summary>
    /// <param name="region">缓存分类</param>
    public void ClearRegion(string region)
    {
        _cache.RemoveByPrefix($"{CacheConst.Prefix}:{region}:");
    }

    /// <summary>
    /// 获取执行的缓存信息
    /// </summary>
    /// <param name="key">缓存key</param>
    /// <param name="region">缓存分类</param>
    /// <returns></returns>
    public T Get(string key, string region)
    {
        key = GetKey(region, key);
        return _cache.Get<T>(key);
    }

    /// <summary>
    /// 获取格式化后的key
    /// </summary>
    /// <param name="region">分类标识</param>
    /// <param name="key">key</param>
    /// <returns></returns>
    private string GetKey(string region, string key)
    {
        return $"{CacheConst.Prefix}:{region}:{key}";
    }
}
