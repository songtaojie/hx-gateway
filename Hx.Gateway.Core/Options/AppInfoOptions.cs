using Furion.ConfigurableOptions;

namespace Hx.Gateway.Core
{
    public class AppInfoOptions : IConfigurableOptions
    {
        /// <summary>
        /// 是否使用Totp登录
        /// </summary>
        public bool TotpLogin { get; set; }

        public string AppId { get; set; }

        public string AppSecret { get; set; }
    }
}
