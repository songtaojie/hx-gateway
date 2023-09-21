// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using Microsoft.Extensions.Options;

namespace Hx.Gateway.Core.Options;
public class OcelotSettingsOptions : IPostConfigureOptions<OcelotSettingsOptions>
{
    /// <summary>
    /// 项目编码
    /// </summary>
    public string ProjectCode { get; set; }

    /// <summary>
    /// 是否启用定时器，默认不启动
    /// </summary>
    public bool EnableTimer { get; set; } = false;

    /// <summary>
    /// 定时器周期，单位（毫秒），默认30分总自动更新一次
    /// </summary>
    public int TimerDelay { get; set; } = 30 * 60 * 1000;

    /// <summary>
    /// 是否启用集群环境，如果非集群环境直接本地缓存+数据库即可
    /// </summary>
    public bool ClusterEnvironment { get; set; } = false;

    /// <summary>
    /// 是否启用客户端授权,默认不开启
    /// </summary>
    public bool ClientAuthorization { get; set; } = false;

    /// <summary>
    /// 服务器缓存时间，默认30分钟
    /// </summary>
    public int CacheTime { get; set; } = 1800;

    /// <summary>
    /// 客户端标识，默认 client_id
    /// </summary>
    public string ClientKey { get; set; } = "client_id";

    /// <summary>
    /// 是否开启自定义限流，默认不开启
    /// </summary>
    public bool ClientRateLimit { get; set; } = false;

    /// <summary>
    /// 数据库连接配置
    /// </summary>
    public DbConnectionConfig DbConnectionConfig { get; set; }

    public void PostConfigure(string name, OcelotSettingsOptions options)
    {
    }
}
