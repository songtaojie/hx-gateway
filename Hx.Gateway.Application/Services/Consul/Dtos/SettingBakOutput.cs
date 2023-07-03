using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Application.Services.Consul.Dtos
{
    public class SettingBakOutput
    {
        /// <summary>
        /// 主键Id 
        ///</summary>
        public int Id { get; set; }

        /// <summary>
        /// 备份时间
        ///</summary>
        public string BakTime { get; set; }

        /// <summary>
        /// 备份内容
        ///</summary>
        public string BakJson { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// ConsulKey
        /// </summary>
        public string ConsulKey { get; set; }

        /// <summary>
        /// ConsulDc
        /// </summary>
        public string ConsulDc { get; set; }
    }
}
