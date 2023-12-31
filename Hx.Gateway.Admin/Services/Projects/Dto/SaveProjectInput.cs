﻿using Hx.Gateway.Core;
using System.ComponentModel.DataAnnotations;

namespace Hx.Gateway.Application.Services.Projects;
public class SaveProjectInput
{
    /// <summary>
    /// 主键id
    /// </summary>
    public Guid? Id { get; set; }

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
    /// 启动/禁用
    /// </summary>
    public StatusEnum Status { get; set; }
   
}

