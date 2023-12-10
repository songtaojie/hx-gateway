// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Hx.Gateway.Core;
using SqlSugar;

namespace Hx.Gateway.Admin.Services.Routes.Dtos;

public class UpdateRouteStatusInput
{
    /// <summary>
    /// 路由id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///  状态
    ///</summary>
    public StatusEnum Status { get; set; }
}
