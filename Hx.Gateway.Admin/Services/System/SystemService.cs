
using Hx.Gateway.Core.Entity;
using Hx.Gateway.Core;
using System.Security.Claims;
using Hx.Gateway.Admin.Authentication;

namespace Ocelot.Admin.Api.Application
{
    public class SystemService 
    {
        private readonly ISqlSugarRepository<TgUserAccount> _rep;
        private readonly IConfiguration _config;

        public SystemService(ISqlSugarRepository<TgUserAccount> rep, 
            IConfiguration config)
        {
            _rep = rep;
            _config = config;
        }

        /// <summary>
        /// 登录授权
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var userAccount = await _rep.AsQueryable()
                .FirstAsync(o => o.Account == request.Account);
            if (userAccount == null)
                throw new UserFriendlyException("用户名或密码错误");
            if(!MD5Encryption.Compare(request.Password, userAccount.Password))
                throw new UserFriendlyException("用户名或密码错误");
            if (userAccount.Status == StatusEnum.Disable)
                throw new UserFriendlyException("该用户已被禁用");

            var accessToken = JWTEncryption.Encrypt(new Dictionary<string, object>()
            {
                {"userId",userAccount.Id },
                {"fullName",userAccount.Name },
                {"openId",userAccount.OpenId }
            });
            var refreshExpirdTime = _config.GetValue<int?>("JWTSettings:RefreshTokenExpiredTime", null);
            var refreshToken = JWTEncryption.GenerateRefreshToken(accessToken, refreshExpirdTime ?? 43200);
            return new LoginResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                FullName = userAccount.Name,
                Account = userAccount.Account,
            };
        }
    }
}