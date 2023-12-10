// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Application.Services.Routes.Dtos;
/// <summary>
/// 更新路由信息
/// </summary>
public class UpdateRouteInput:AddRouteInput
{
    /// <summary>
    /// 路由id
    /// </summary>
    public Guid Id { get; set; }
}
