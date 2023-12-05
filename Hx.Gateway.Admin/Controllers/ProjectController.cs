// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Hx.Gateway.Admin.Services;
using Hx.Gateway.Admin.Services.Projects.Dto;
using Hx.Gateway.Application.Services.Projects;
using Hx.Gateway.Core;
using Hx.Gateway.Core.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ocelot.Admin.Api.Application;
using Ocelot.RequestId;

namespace Hx.Gateway.Admin.Controllers;

/// <summary>
/// 项目
/// </summary>
public class ProjectController:BaseControllerBase
{
    private readonly ProjectService _service;
    public ProjectController(ProjectService service)
    {
        _service = service;
    }

    /// <summary>
    /// 分页查询项目信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedListResult<PageProjectOutput>> GetPageAsync([FromQuery]PageProjectInput input)
    {
        var result = await _service.GetPageAsync(input);
        if (result != null) throw new Exception("sss");
        return result;
    }

    /// <summary>
    /// 新增项目信息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> AddAsync(AddProjectInput request)
    {
        return await _service.AddAsync(request);
    }

    /// <summary>
    /// 新增项目信息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> UpdateAsync(UpdateProjectInput request)
    {
        return await _service.UpdateAsync(request);
    }
}
