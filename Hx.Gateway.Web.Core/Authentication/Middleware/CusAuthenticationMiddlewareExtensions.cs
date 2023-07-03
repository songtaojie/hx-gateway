using Microsoft.AspNetCore.Builder;

namespace Hx.Gateway.Web.Core.Authentication.Middleware
{
    public static class CusAuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseCusAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CusAuthenticationMiddleware>();
        }
    }
}
