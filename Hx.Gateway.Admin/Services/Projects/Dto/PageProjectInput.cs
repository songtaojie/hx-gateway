using Hx.Gateway.Core;
using Hx.Gateway.Core.Entity;

namespace Hx.Gateway.Application.Services.Projects
{
    /// <summary>
    /// 分页查询项目
    /// </summary>
    public class PageProjectInput : BasePageParam
    {
        /// <summary>
        /// 启动/禁用
        /// </summary>
        public StatusEnum? Status { get; set; }
    }
}
