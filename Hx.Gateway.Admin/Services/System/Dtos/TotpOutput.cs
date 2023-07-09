using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Application.Services.System.Dtos
{
    public class TotpOutput
    {
        public string TotpPwd { get; set; }

        public int CountDown { get; set; }
    }
}
