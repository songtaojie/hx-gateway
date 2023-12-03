using System.Collections.Generic;
using SqlSugar;
namespace Hx.Gateway.Core.Entity
{
    /// <summary>
    /// 路由表
    ///</summary>
    [SugarTable(null, "路由表")]
    public class TgRoute : EntityBase
    {
        /// <summary>
        /// 项目Id 
        ///</summary>
        [SugarColumn(ColumnDescription = "项目Id")]
        public Guid ProjectId { get; set; }

        /// <summary>
        /// 下游的路由模板，即真实处理请求的路径模板 
        ///</summary>
        [SugarColumn(ColumnDescription = "下游的路由模板，即真实处理请求的路径模板 ", IsNullable = true, Length = 200)]
        public string DownstreamPathTemplate { get; set; }

        /// <summary>
        /// 上游请求的模板，即用户真实请求的链接 
        ///</summary>
        [SugarColumn(ColumnDescription = "上游请求的模板，即用户真实请求的链接 ", IsNullable = true, Length = 200)]
        public string UpstreamPathTemplate { get; set; }

        /// <summary>
        /// 上游请求的http方法（数组：GET、POST、PUT） 
        ///</summary>
        [SugarColumn(ColumnDescription = "上游请求的http方法（数组：GET、POST、PUT） ", IsNullable = true, Length = 64)]
        public string UpstreamHttpMethod { get; set; }

        /// <summary>
        /// 下游请求的http方法（数组：GET、POST、PUT） 
        ///</summary>
        [SugarColumn(ColumnDescription = "下游请求的http方法（数组：GET、POST、PUT） ", IsNullable = true, Length = 64)]
        public string DownstreamHttpMethod { get; set; }

        /// <summary>
        ///  下游Http版本 
        ///</summary>
        [SugarColumn(ColumnDescription = "下游Http版本 ", IsNullable = true, Length = 64)]
        public string DownstreamHttpVersion { get; set; }

        /// <summary>
        ///  请求Id 
        ///</summary>
        [SugarColumn(ColumnDescription = "请求Id", IsNullable = true, Length = 64)]
        public string RequestIdKey { get; set; }

        /// <summary>
        ///  开启上下游路由模板大小写匹配 
        ///</summary>
        [SugarColumn(ColumnDescription = "开启上下游路由模板大小写匹配 ")]
        public bool? RouteIsCaseSensitive { get; set; }

        /// <summary>
        ///  服务名 
        ///</summary>
        [SugarColumn(ColumnDescription = "服务名", IsNullable = true, Length = 64)]
        public string ServiceName { get; set; }

        /// <summary>
        ///  服务命名空间 
        ///</summary>
        [SugarColumn(ColumnDescription = "服务命名空间 ", IsNullable = true, Length = 128)]
        public string ServiceNamespace { get; set; }

        /// <summary>
        /// 请求的方式，如：http,htttps 
        ///</summary>
        [SugarColumn(ColumnDescription = "请求的方式，如：http,htttps ", IsNullable = true, Length = 20)]
        public string DownstreamScheme { get; set; }

        /// <summary>
        /// 缓存配置
        ///</summary>
        ///<see cref="Options.Ocelot.FileCacheOptions"/>
        [SugarColumn(ColumnDescription = "缓存配置", IsNullable = true, Length = 64)]
        public string FileCacheOptions { get; set; }

        /// <summary>
        /// 服务质量
        /// </summary>
        /// <see cref="Options.Ocelot.QoSOptions"/>
        [SugarColumn(ColumnDescription = "服务质量配置", IsNullable = true, Length = 64)]
        public string QoSOptions { get; set; }

        /// <summary>
        ///  负载均衡
        ///</summary>
        ///<see cref="Options.Ocelot.LoadBalancerOptions"/>
        [SugarColumn(ColumnDescription = "负载均衡配置", IsNullable = true, Length = 64)]
        public string LoadBalancerOptions { get; set; }

        /// <summary>
        /// 限制配置
        ///</summary>
        ///<see cref="Options.Ocelot.RateLimitOptions"/>
        [SugarColumn(ColumnDescription = "限制配置", IsNullable = true, Length = 64)]
        public string RateLimitOptions { get; set; }

        /// <summary>
        ///  身份认证
        ///</summary>
        ///<see cref="Options.Ocelot.AuthenticationOptions"/>
        [SugarColumn(ColumnDescription = "身份认证", IsNullable = true, Length = 64)]
        public string AuthenticationOptions { get; set; }

        /// <summary>
        ///  HttpHandler配置
        ///</summary>
        ///<see cref="Options.Ocelot.HttpHandlerOptions"/>
        [SugarColumn(ColumnDescription = "HttpHandler配置", IsNullable = true, Length = 64)]
        public string HttpHandlerOptions { get; set; }

        /// <summary>
        ///  该特性允许您拥有基于上游主机的路由。
        ///  它的工作原理是查看客户端使用的主机头，然后将其作为我们用来标识路由的信息的一部分
        ///</summary>
        [SugarColumn(ColumnDescription = "该特性允许您拥有基于上游主机的路由")]
        public string UpstreamHost { get; set; }

        /// <summary>
        ///  委托处理程序，需要实现DelegatingHandler类
        ///  数组[ "FakeHandlerTwo", "FakeHandler"]
        ///</summary>
        [SugarColumn(ColumnName = "DelegatingHandlers")]
        public string DelegatingHandlers { get; set; }

        /// <summary>
        ///  你可以通过在ocelot.json中包含“Priority”属性来定义你想要的路由匹配Upstream HttpRequest的顺序
        ///</summary>
        [SugarColumn(ColumnDescription = "你可以通过在ocelot.json中包含“Priority”属性来定义你想要的路由匹配Upstream HttpRequest的顺序")]
        public int? Priority { get; set; }

        /// <summary>
        /// 评估危险服务验证 
        /// 如果要忽略SSL警告/错误，请在路由配置中设置以下参数
        ///</summary>
        [SugarColumn(ColumnDescription = "如果要忽略SSL警告/错误，请在路由配置中设置该参数")]
        public bool? DangerousAcceptAnyServerCertificateValidator { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(ColumnDescription = "排序")]
        public int? Sort { get; set; }

        /// <summary>
        ///  状态
        ///</summary>
        [SugarColumn(ColumnDescription = "状态")]
        public StatusEnum Status { get; set; } = StatusEnum.Enable;

        /// <summary>
        /// 路由地址配置
        /// </summary>
        [Navigate(NavigateType.OneToMany, nameof(TgRouteHostPort.RouteId))]
        public List<TgRouteHostPort> DownstreamHostAndPorts { get; set; }

        /// <summary>
        /// 路由属性
        /// </summary>
        [Navigate(NavigateType.OneToMany, nameof(TgRouteProperty.RouteId))]
        public List<TgRouteProperty> RouteProperties { get; set; }
    }
}
