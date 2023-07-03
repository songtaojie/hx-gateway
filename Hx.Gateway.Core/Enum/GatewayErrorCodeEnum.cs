// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Application.Enum;

/// <summary>
/// 网关异常代码
/// </summary>
[ErrorCodeType]
public enum GatewayErrorCodeEnum
{
    [ErrorCodeItemMetadata("新增雇员失败")]
    INSERT_EMPLOYEE_FAIL = 9000,

    [ErrorCodeItemMetadata("新增角色失败")]
    INSERT_ROLE_FAIL,

    [ErrorCodeItemMetadata("重置密码失败")]
    RESET_PASSWORD_FAIL,

    [ErrorCodeItemMetadata("用户名或密码错误")]
    ACCOUNT_OR_PASSWORD_ERROR,

    [ErrorCodeItemMetadata("用户不存在")]
    ACCOUNT_NON_EXISTENT,

    [ErrorCodeItemMetadata("当前用户已被管理员停用")]
    ACCOUNT_DISABLED,

    [ErrorCodeItemMetadata("删除雇员失败")]
    DELETE_EMPLOYEE_FAIL,

    [ErrorCodeItemMetadata("删除雇员角色失败")]
    DELETE_GLOBAL_CONFIGURATION_FAIL,

    [ErrorCodeItemMetadata("编辑全局配置失败")]
    UPDATE_GLOBAL_CONFIGURATION_FAIL,

    [ErrorCodeItemMetadata("新增全局配置失败")]
    INSERT_GLOBAL_CONFIGURATION_FAIL,

    [ErrorCodeItemMetadata("新增项目失败")]
    INSERT_PROJECT_FAIL,

    [ErrorCodeItemMetadata("删除项目失败")]
    DELETE_PROJECT_FAIL,

    [ErrorCodeItemMetadata("编辑项目失败")]
    UPDATE_PROJECT_FAIL,

    [ErrorCodeItemMetadata("启动项目失败")]
    ENABLE_PROJECT_FAIL,

    [ErrorCodeItemMetadata("禁用项目失败")]
    DISABLE_PROJECT_FAIL,

    [ErrorCodeItemMetadata("删除路由失败")]
    DELETE_ROUTE_FAIL,

    [ErrorCodeItemMetadata("启动路由失败")]
    ENABLE_ROUTE_FAIL,

    [ErrorCodeItemMetadata("禁用路由失败")]
    DISABLE_ROUTE_FAIL,
}
