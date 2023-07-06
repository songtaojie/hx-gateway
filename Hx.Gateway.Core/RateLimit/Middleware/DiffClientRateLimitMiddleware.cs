using Hx.Gateway.Core.Options;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Ocelot.Logging;
using Ocelot.Middleware;
using System.Linq;
using System.Threading.Tasks;

namespace Hx.Gateway.Core.RateLimit.Middleware
{
    /// <summary>
    /// 自定义客户端限流中间件
    /// </summary>
    public class DiffClientRateLimitMiddleware : OcelotMiddleware
    {
        private readonly IClientRateLimitProcessor _clientRateLimitProcessor;
        private readonly RequestDelegate _next;
        private readonly OcelotSettingsOptions _ocelotSettings;
        public DiffClientRateLimitMiddleware(RequestDelegate next,
            IOcelotLoggerFactory loggerFactory,
            IClientRateLimitProcessor clientRateLimitProcessor,
            IOptions<OcelotSettingsOptions> options)
            : base(loggerFactory.CreateLogger<DiffClientRateLimitMiddleware>())
        {
            _next = next;
            _clientRateLimitProcessor = clientRateLimitProcessor;
            _ocelotSettings = options.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var clientId = "client_cjy"; //使用默认的客户端
            if (!_ocelotSettings.ClientRateLimit)
            {
                Logger.LogInformation($"未启用客户端限流中间件");
                await _next.Invoke(httpContext);
            }
            else
            {
                var downstreamRoute = httpContext.Items.DownstreamRoute();
                //非认证的渠道
                if (!downstreamRoute.IsAuthenticated)
                {
                    if (httpContext.Request.Headers.ContainsKey(_ocelotSettings.ClientKey))
                    {
                        clientId = httpContext.Request.Headers[_ocelotSettings.ClientKey].First();
                    }
                }
                else
                {//认证过的渠道，从Claim中提取
                    var clientKeyValue = httpContext.User.Claims.FirstOrDefault(p => p.Type == _ocelotSettings.ClientKey)?.Value;
                    if (!string.IsNullOrEmpty(clientKeyValue)) clientId = clientKeyValue;
                }
                var path = downstreamRoute.UpstreamPathTemplate.OriginalValue;
                if (await _clientRateLimitProcessor.CheckClientRateLimitResultAsync(clientId, path))
                {
                    await _next.Invoke(httpContext);
                }
                else
                {
                    var error = new RateLimitOptionsError($"请求路由 {httpContext.Request.Path}触发限流策略");
                    Logger.LogWarning($"路由地址 {httpContext.Request.Path} 触发限流策略. {error}");
                    httpContext.Items.SetError(error);
                }
            }
        }
    }
}
