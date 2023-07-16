using Hx.Sdk.Common;
using SqlSugar;

namespace Hx.Gateway.Core.Entity
{
    /// <summary>
    /// 全局配置表
    ///</summary>
    [SugarTable(null, "全局配置表")]
    public class TgGlobalConfiguration : EntityBase
    {
        /// <summary>
        /// 项目Id 
        ///</summary>
        [SugarColumn(ColumnDescription = "项目Id")]
        public long ProjectId { get; set; }

        /// <summary>
        /// 基础地址 
        ///</summary>
        [SugarColumn(ColumnDescription = "基础地址", IsNullable = true, Length = 200)]
        public string BaseUrl { get; set; }

        /// <summary>
        /// 请求ID 
        ///</summary>
        [SugarColumn(ColumnDescription = "请求Key", IsNullable = true, Length = 20)]
        public string RequestIdKey { get; set; }

        /// <summary>
        /// Http协议（http,https） 
        ///</summary>
        [SugarColumn(ColumnDescription = "Http协议（http,https）", IsNullable = true, Length = 20)]
        public string DownstreamScheme { get; set; }
        /// <summary>
        /// Http版本（1.0，1.1，2.0） 
        ///</summary>
        [SugarColumn(ColumnDescription = "Http版本（1.0，1.1，2.0）", IsNullable = true, Length = 20)]
        public string DownstreamHttpVersion { get; set; }
        /// <summary>
        /// 负载均衡
        ///</summary>
        ///<see cref="Options.Ocelot.LoadBalancerOptions"/>
        [SugarColumn(ColumnDescription = "负载均衡", IsNullable = true, Length = 64)]
        public string LoadBalancerOptions { get; set; }

        /// <summary>
        /// HttpHandler配置 
        ///</summary>   
        ///<see cref="Options.Ocelot.HttpHandlerOptions"/>
        [SugarColumn(ColumnDescription = "HttpHandler配置", IsNullable = true, Length = 64)]
        public string HttpHandlerOptions { get; set; }

        /// <summary>
        /// 服务质量控制
        ///</summary>
        ///<see cref="Options.Ocelot.QoSOptions"/>
        [SugarColumn(ColumnDescription = "服务质量控制", IsNullable = true, Length = 64)]
        public string QoSOptions { get; set; }

        /// <summary>
        /// 全局限流配置 
        ///</summary>
        ///<see cref="Options.Ocelot.GlobalRateLimitOptions"/>
        [SugarColumn(ColumnDescription = "全局限流配置", IsNullable = true, Length = 200)]
        public string RateLimitOptions { get; set; }

        /// <summary>
        /// 服务发现代理配置
        /// </summary>
        /// <see cref="Options.Ocelot.ServiceDiscoveryProviderOptions"/>
        [SugarColumn(ColumnDescription = "服务发现代理配置", IsNullable = true, Length = 200)]
        public string ServiceDiscoveryProviderOptions { get; set; }

        /// <summary>
        /// 状态
        ///</summary>
        [SugarColumn(ColumnDescription = "状态")]
        public StatusEnum Status { get; set; } = StatusEnum.Enable;
    }
}
