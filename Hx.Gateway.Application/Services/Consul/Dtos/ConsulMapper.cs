using Hx.Gateway.Application.Services.Consul.Dtos;

namespace Hx.Gateway.Application.Services.Consul.Dto
{
    public class ConsulMapper : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.ForType<TgSettingBak, SettingBakOutput>().Map(dest => dest.BakTime, src => src.BakTime.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}