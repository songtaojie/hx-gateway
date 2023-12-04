// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Hx.Gateway.Core;
using System.ComponentModel.DataAnnotations;

namespace Hx.Gateway.Admin.Services.Projects.Dto;

public class AddProjectInput
{
    /// <summary>
    /// 项目编码
    ///</summary>
    [Required(ErrorMessage = "项目编码不能为空")]
    public string Code { get; set; }

    /// <summary>
    /// 项目名称
    ///</summary>
    [Required(ErrorMessage = "项目名称不能为空")]
    public string Name { get; set; }

    /// <summary>
    /// 排序字段 
    ///</summary>
    public int SortIndex { get; set; }

    /// <summary>
    /// 启动/禁用
    /// </summary>
    public StatusEnum Status { get; set; }
}


public class UpdateProjectInput : AddProjectInput
{
    /// <summary>
    /// 主键id
    /// </summary>
    public Guid? Id { get; set; }
}