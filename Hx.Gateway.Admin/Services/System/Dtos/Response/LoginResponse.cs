namespace Ocelot.Admin.Api.Application
{
    public class LoginResponse
    {
        /// <summary>
        /// 授权token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 刷新token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 用户OPENID
        /// </summary>
        public string OpenId { get; set; }
    }
}
