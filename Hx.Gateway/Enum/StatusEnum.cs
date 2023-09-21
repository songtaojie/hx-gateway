// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using System.ComponentModel;

namespace Hx.Gateway.Enum;

/// <summary>
/// 通用状态枚举
/// </summary>
public enum StatusEnum
{
    /// <summary>
    /// 启用
    /// </summary>
    [Description("启用")]
    Enable = 1,

    /// <summary>
    /// 停用
    /// </summary>
    [Description("停用")]
    Disable = 2,
}
