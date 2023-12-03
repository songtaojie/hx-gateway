using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ocelot.Admin.Api.Application
{
    public class TotpResponse
    {
        public string TotpPwd { get; set; }

        public int CountDown { get; set; }
    }
}
