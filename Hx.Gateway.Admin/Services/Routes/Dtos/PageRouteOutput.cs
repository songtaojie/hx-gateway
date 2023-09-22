using Hx.Gateway.Core;

namespace Hx.Gateway.Application.Services.Routes.Dtos
{
    /// <summary>
    /// 分页查询路由
    /// </summary>
    public class PageRouteOutput
    {
        /// <summary>
        /// 主键ID 
        ///</summary>
        public long Id { get; set; }

        /// <summary>
        /// 下游的路由模板，即真实处理请求的路径模板 
        ///</summary>
        public string DownstreamPathTemplate { get; set; }

        /// <summary>
        /// 上游请求的模板，即用户真实请求的链接 
        ///</summary>
        public string UpstreamPathTemplate { get; set; }

        /// <summary>
        /// 上游请求的http方法（数组：GET、POST、PUT） 
        ///</summary>
        public string UpstreamHttpMethod { get; set; }

        /// <summary>
        /// 下游请求的http方法（数组：GET、POST、PUT） 
        ///</summary>
        public string DownstreamHttpMethod { get; set; }

        /// <summary>
        ///  下游Http版本 
        ///</summary>
        public string DownstreamHttpVersion { get; set; }

        /// <summary>
        ///  请求Id 
        ///</summary>
        public string RequestIdKey { get; set; }

        /// <summary>
        /// 请求的方式，如：http,htttps 
        ///</summary>
        public string DownstreamScheme { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int? Sort { get; set; }

        /// <summary>
        ///  状态
        ///</summary>
        public StatusEnum? Status { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status_V => Status.GetDescription();
    }
}
