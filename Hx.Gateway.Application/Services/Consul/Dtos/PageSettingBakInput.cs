using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Application.Services.Consul.Dtos
{
    public class PageSettingBakInput : BasePageInput
    {
        public string ConsulKey { get; set; }

        public string ConsulDc { get; set; }

        public DateTime? BakTime { get; set; }
    }
}
