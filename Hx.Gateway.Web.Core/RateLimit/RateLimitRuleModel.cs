using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Gateway.Web.Core.RateLimit
{
    public class RateLimitRuleModel
    {
        /// <summary>
        /// 是否启用限流
        /// </summary>
        public bool RateLimit { get; set; }

        /// <summary>
        /// 限流配置信息
        /// </summary>
        public List<DiffClientRateLimitOptions> RateLimitOptions { get; set; }
    }
}
