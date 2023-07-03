using Hx.Gateway.Application.Options;
using Microsoft.Extensions.Options;
using Ocelot.Configuration;
using Ocelot.Logging;
using Ocelot.Middleware;
using System.Linq;

namespace Hx.Gateway.Web.Core.Authentication.Middleware
{
    /// <summary>
    /// 
    /// </summary>
    public class CusAuthenticationMiddleware : OcelotMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly OcelotSettingsOptions _ocelotSettings;
        private readonly ICusAuthenticationProcessor _processor;

        public CusAuthenticationMiddleware(RequestDelegate next,
            IOcelotLoggerFactory loggerFactory,
            ICusAuthenticationProcessor processor,
            IOptions<OcelotSettingsOptions> options)
            : base(loggerFactory.CreateLogger<CusAuthenticationMiddleware>())
        {
            _next = next;
            _processor = processor;
            _ocelotSettings = options.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext.Request.Method.ToUpper() == "OPTIONS")
            {
                await _next.Invoke(httpContext);
                return;
            }
            var downstreamRoute = httpContext.Items.DownstreamRoute();
            if (!IsAuthenticatedRoute(downstreamRoute))
            {
                await _next.Invoke(httpContext);
                return;
            }
            if (!_ocelotSettings.ClientAuthorization)
            {
                Logger.LogInformation($"未启用客户端认证中间件");
                await _next.Invoke(httpContext);
                return;
            }
            Logger.LogInformation($"{httpContext.Request.Path} 是认证路由. {MiddlewareName} 开始校验授权信息");
            var clientId = _ocelotSettings.ClientKey;
            var path = downstreamRoute.UpstreamPathTemplate.OriginalValue; //路由地址
            var clientKeyValue = httpContext.User.Claims.FirstOrDefault(p => p.Type == _ocelotSettings.ClientKey)?.Value;
            if (!string.IsNullOrEmpty(clientKeyValue)) // 从Claims中提取客户端id
            {
                clientId = clientKeyValue;
            }
            if (await _processor.CheckClientAuthenticationAsync(clientId, path))
            {
                await _next.Invoke(httpContext);
            }
            else // 未授权直接返回错误
            {
                var error = new UnauthenticatedError($"请求认证路由 {httpContext.Request.Path}客户端未授权");
                Logger.LogWarning($"路由地址 {httpContext.Request.Path} 自定义认证管道校验失败. {error}");
                httpContext.Items.SetError(error);
            }
        }
        private static bool IsAuthenticatedRoute(DownstreamRoute route)
        {
            return route.IsAuthenticated;
        }
    }
}
