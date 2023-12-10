// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Hx.Gateway.Core;
using System.ComponentModel.DataAnnotations;

namespace Hx.Gateway.Admin.Services.GlobalConfiguration.Dtos;

public class UpdateGlobalConfigurationStatusInput
{
    /// <summary>
    /// 主键Id 
    ///</summary>
    [Required(ErrorMessage = "主键标识不能为空")]
    public Guid Id { get; set; }

    /// <summary>
    /// 状态
    ///</summary>
    public StatusEnum Status { get; set; }
}
