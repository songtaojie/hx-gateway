// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Hx.Gateway.Core;
using SqlSugar;

namespace Hx.Gateway.Admin.Services.GlobalConfiguration.Dtos;

public class PageGlobalConfigurationOutput
{
    /// <summary>
    /// 
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 项目Id 
    ///</summary>
    public Guid ProjectId { get; set; }

    /// <summary>
    /// 项目名字
    ///</summary>
    public string ProjectName { get; set; }

    /// <summary>
    /// 全局配置名称 
    ///</summary>
    public string Name { get; set; }

    /// <summary>
    /// 状态
    ///</summary>
    public StatusEnum Status { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreateTime { get; set; }

}
