using Hx.Gateway.Core;
using Hx.Gateway.Core.Entity;

namespace Hx.Gateway.Application.Services.Routes.Dtos
{
    /// <summary>
    /// 分页查询路由
    /// </summary>
    public class PageRouteInput : BasePageParam
    {
        /// <summary>
        /// 项目Id
        /// </summary>
        public long ProjectId { get; set; }

        /// <summary>
        /// 启动/禁用
        /// </summary>
        public StatusEnum? Status { get; set; }

        /// <summary>
        /// 下游的路由模板，即真实处理请求的路径模板
        /// </summary>
        public string DownstreamPathTemplate { get; set; }

        /// <summary>
        /// 上游请求的模板，即用户真实请求的链接
        /// </summary>
        public string UpstreamPathTemplate { get; set; }

        /// <summary>
        /// 请求Id
        /// </summary>
        public string RequestIdKey { get; set; }
    }
}
