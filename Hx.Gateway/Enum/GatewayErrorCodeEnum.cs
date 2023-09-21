// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Core.Enum;

/// <summary>
/// 网关异常代码
/// </summary>
public enum GatewayErrorCodeEnum
{
    /// <summary>
    /// 新增雇员失败
    /// </summary>
    INSERT_EMPLOYEE_FAIL = 9000,

    /// <summary>
    /// 新增角色失败
    /// </summary>
    INSERT_ROLE_FAIL,

    /// <summary>
    /// 重置密码失败
    /// </summary>
    RESET_PASSWORD_FAIL,

    /// <summary>
    /// 用户名或密码错误
    /// </summary>
    ACCOUNT_OR_PASSWORD_ERROR,

    /// <summary>
    /// 用户不存在
    /// </summary>
    ACCOUNT_NON_EXISTENT,

    /// <summary>
    /// 当前用户已被管理员停用
    /// </summary>
    ACCOUNT_DISABLED,

    /// <summary>
    /// 删除雇员失败
    /// </summary>
    DELETE_EMPLOYEE_FAIL,

    /// <summary>
    /// 删除雇员角色失败
    /// </summary>
    DELETE_GLOBAL_CONFIGURATION_FAIL,

    /// <summary>
    /// /编辑全局配置失败
    /// </summary>
    UPDATE_GLOBAL_CONFIGURATION_FAIL,

    /// <summary>
    /// 新增全局配置失败
    /// </summary>
    INSERT_GLOBAL_CONFIGURATION_FAIL,

    /// <summary>
    /// 新增项目失败
    /// </summary>
    INSERT_PROJECT_FAIL,

    /// <summary>
    /// 删除项目失败
    /// </summary>
    DELETE_PROJECT_FAIL,

    /// <summary>
    /// 编辑项目失败
    /// </summary>
    UPDATE_PROJECT_FAIL,

    /// <summary>
    /// 启动项目失败
    /// </summary>
    ENABLE_PROJECT_FAIL,

    /// <summary>
    /// 禁用项目失败
    /// </summary>
    DISABLE_PROJECT_FAIL,

    /// <summary>
    /// 删除路由失败
    /// </summary>
    DELETE_ROUTE_FAIL,

    /// <summary>
    /// 启动路由失败
    /// </summary>
    ENABLE_ROUTE_FAIL,

    /// <summary>
    /// 禁用路由失败
    /// </summary>
    DISABLE_ROUTE_FAIL,
}
