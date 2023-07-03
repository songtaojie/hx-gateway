namespace Hx.Gateway.Web.Core
{
    public class JwtHandler : AppAuthorizeHandler
    {
        /// <summary>
        /// 验证管道
        /// </summary>
        /// <param name="context"></param>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public override async Task<bool> PipelineAsync(AuthorizationHandlerContext context, DefaultHttpContext httpContext)
        {
            // 这里写您的授权判断逻辑，授权通过返回 true，否则返回 false
            //目前仅校验Token的有效性和有效期
            return await Task.FromResult(true);
        }

        /// <summary>
        /// 重写 Handler 添加自动刷新收取逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override async Task HandleAsync(AuthorizationHandlerContext context)
        {
            var expiredTime = int.TryParse(App.Configuration["JWTSettings:ExpiredTime"], out int intExpreeTime) ? intExpreeTime : 60;
            var refreshExpirdTime = int.TryParse(App.Configuration["JWTSettings:RefreshTokenExpiredTime"], out int intRefreshTokenExpiredTime) ? intRefreshTokenExpiredTime : 43200;
            // 自动刷新 token
            if (JWTEncryption.AutoRefreshToken(context, context.GetCurrentHttpContext(), expiredTime, refreshExpirdTime))
            {
                await AuthorizeHandleAsync(context);
            }
            else context.Fail();    // 授权失败
        }
    }
}
