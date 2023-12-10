// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using System.ComponentModel.DataAnnotations;

namespace Hx.Gateway.Admin.Services.GlobalConfiguration.Dtos;

public class DeleteGlobalConfigurationInput
{
    /// <summary>
    /// 主键Id 
    ///</summary>
    [Required(ErrorMessage = "主键标识不能为空")]
    public Guid Id { get; set; }
}
