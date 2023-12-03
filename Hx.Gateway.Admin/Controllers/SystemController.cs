// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ocelot.Admin.Api.Application;

namespace Hx.Gateway.Admin.Controllers;

public class SystemController:BaseControllerBase
{
    private readonly SystemService _systemService;
    public SystemController(SystemService systemService)
    {
        _systemService = systemService;
    }

    /// <summary>
    /// 授权登录
    /// </summary>
    /// <param name="loginRequest">登录请求</param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost]
    public async Task<LoginResponse> LoginAsync(LoginRequest loginRequest)
    {
        return await _systemService.LoginAsync(loginRequest);
    }
}
