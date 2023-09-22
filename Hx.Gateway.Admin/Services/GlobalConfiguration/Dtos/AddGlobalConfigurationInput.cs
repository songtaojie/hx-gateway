using Hx.Gateway.Core.Options.Ocelot;

namespace Hx.Gateway.Application.Services.GlobalConfiguration.Dtos
{
    /// <summary>
    /// 新增全局配置
    /// </summary>
    public class AddGlobalConfigurationInput
    {
        /// <summary>
        /// 基础地址 
        ///</summary>
        public string BaseUrl { get; set; }

        /// <summary>
        /// 请求ID 
        ///</summary>
        public string RequestIdKey { get; set; }

        /// <summary>
        /// Http协议（http,https） 
        ///</summary>
        public string DownstreamScheme { get; set; }
        /// <summary>
        /// Http版本（1.0，1.1，2.0） 
        ///</summary>
        public string DownstreamHttpVersion { get; set; }
        /// <summary>
        /// 负载均衡
        ///</summary>
        ///<see cref=" Hx.Gateway.Core.Options.Ocelot.LoadBalancerOptions"/>
        public LoadBalancerOptions LoadBalancerOptions { get; set; }

        /// <summary>
        /// HttpHandler配置 
        ///</summary>   
        ///<see cref=" Hx.Gateway.Core.Options.Ocelot.HttpHandlerOptions"/>
        public HttpHandlerOptions HttpHandlerOptions { get; set; }

        /// <summary>
        /// 服务质量控制
        ///</summary>
        ///<see cref=" Hx.Gateway.Core.Options.Ocelot.QoSOptions"/>
        public QoSOptions QoSOptions { get; set; }

        /// <summary>
        /// 全局限流配置 
        ///</summary>
        ///<see cref=" Hx.Gateway.Core.Options.Ocelot.RateLimitOptions"/>
        public RateLimitOptions RateLimitOptions { get; set; }

        /// <summary>
        /// 服务发现代理配置
        /// </summary>
        /// <see cref=" Hx.Gateway.Core.Options.Ocelot.ServiceDiscoveryProviderOptions"/>
        public ServiceDiscoveryProviderOptions ServiceDiscoveryProviderOptions { get; set; }
    }
}
