using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Application.Services.Dictionary.Dtos
{
    public class AllSelectOutput
    {
        /// <summary>
        /// consul配置关键字
        ///</summary>
        public List<BaseSelectDto<string>> ConsulSettingKey { get; set; }

        /// <summary>
        /// consul DC 库 
        ///</summary>
        public List<BaseSelectDto<string>> ConsulDC { get; set; }
    }
}
