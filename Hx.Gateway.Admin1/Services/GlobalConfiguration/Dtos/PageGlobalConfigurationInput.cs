// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Hx.Gateway.Core;
using Hx.Gateway.Core.Entity;
using SqlSugar;

namespace Hx.Gateway.Admin.Services.GlobalConfiguration.Dtos;

public class PageGlobalConfigurationInput: BasePageParam
{
    /// <summary>
    /// 状态
    ///</summary>
    public StatusEnum? Status { get; set; }
}
