namespace Hx.Gateway.Application.Services.System.Dtos
{
    /// <summary>
    /// 登录请求
    /// </summary>
    public class LoginInput
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
