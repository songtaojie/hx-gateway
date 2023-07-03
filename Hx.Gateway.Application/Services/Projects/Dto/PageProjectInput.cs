using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Application.Services.Projects
{
    /// <summary>
    /// 分页查询项目
    /// </summary>
    public class PageProjectInput : BasePageInput
    {
        /// <summary>
        /// 启动/禁用
        /// </summary>
        public StatusEnum? Status { get; set; }
    }
}
