// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

namespace Hx.Gateway.Admin.Options.Ocelot;
/// <summary>
/// Ocelot目前支持CacheManager项目提供的一些非常基本的缓存。
/// 这是一个了不起的项目，解决了很多缓存问题。我
/// 建议使用这个包来缓存Ocelot
/// Install-Package Ocelot.Cache.CacheManager
/// </summary>
public class FileCacheOptions
{
    /// <summary>
    /// 如设置为15，这意味着缓存将在15秒后过期
    /// </summary>
    public int? TtlSeconds { get; set; }

    /// <summary>
    /// 区域
    /// </summary>
    public string Region { get; set; }
}
