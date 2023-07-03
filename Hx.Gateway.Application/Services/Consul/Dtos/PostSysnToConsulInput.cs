using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Application.Services.Consul.Dto
{
    public class PostSysnToConsulInput
    {
        public string Key { get; set; }

        public string Dc { get; set; }
    }
}
