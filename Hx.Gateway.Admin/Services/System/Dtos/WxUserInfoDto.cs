using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Application.Services.System.Dtos
{
    public class WxUserInfoDto
    {
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string OpenId { get; set; }

        public int Errcode { get; set; }


        public string ErrMsg { get; set; }
    }

}
