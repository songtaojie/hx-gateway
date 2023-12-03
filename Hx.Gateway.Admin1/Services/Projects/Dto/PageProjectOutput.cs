// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Hx.Gateway.Core;

namespace Hx.Gateway.Admin.Services.Projects.Dto;

public class PageProjectOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 项目编码
    ///</summary>
    public string Code { get; set; }

    /// <summary>
    /// 项目名称 
    ///</summary>
    public string Name { get; set; }

    /// <summary>
    /// 排序字段 
    ///</summary>
    public int SortIndex { get; set; }

    /// <summary>
    /// 状态字段 
    ///</summary>
    public StatusEnum Status { get; set; }

    /// <summary>
    /// 状态字段 
    ///</summary>
    public string Status_V => Status.GetDescription();

    /// <summary>
    /// 创建时间
    /// </summary>    
    public DateTime? CreateTime { get; set; }

}
