﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Application.Services.GlobalConfiguration.Dtos
{
    /// <summary>
    /// 编辑全局配置
    /// </summary>
    public class UpdateGlobalConfigurationInput : AddGlobalConfigurationInput
    {
        /// <summary>
        /// 主键Id 
        ///</summary>
        [Required(ErrorMessage ="主键标识不能为空")]
        public long Id { get; set; }
    }
}