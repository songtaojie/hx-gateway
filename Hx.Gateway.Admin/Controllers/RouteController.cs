// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Hx.Gateway.Admin.Services.Projects.Dto;
using Hx.Gateway.Admin.Services;
using Hx.Gateway.Application.Services.Projects;
using Hx.Gateway.Core;
using Microsoft.AspNetCore.Mvc;
using Hx.Gateway.Application.Services.Routes;
using Hx.Gateway.Application.Services.Routes.Dtos;
using Hx.Gateway.Admin.Services.GlobalConfiguration.Dtos;
using Hx.Gateway.Admin.Services.Routes.Dtos;
using Hx.Gateway.Application.Services.GlobalConfiguration.Dtos;

namespace Hx.Gateway.Admin.Controllers;

/// <summary>
/// 项目
/// </summary>
public class RouteController : BaseControllerBase
{
    private readonly RouteService _service;
    public RouteController(RouteService service)
    {
        _service = service;
    }

    /// <summary>
    /// 分页查询项目信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedListResult<PageRouteOutput>> GetPageAsync([FromQuery] PageRouteInput input)
    {
        var result = await _service.GetPageAsync(input);
        return result;
    }

    /// <summary>
    /// 新增项目信息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> AddAsync(AddRouteInput request)
    {
        return await _service.AddAsync(request);
    }

    /// <summary>
    /// 编辑项目信息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<bool> UpdateAsync(UpdateRouteInput request)
    {
        return await _service.UpdateAsync(request);
    }

    /// <summary>
    /// 编辑项目信息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPatch]
    public async Task<bool> UpdateStatusAsync(UpdateRouteStatusInput request)
    {
        return await _service.UpdateStatusAsync(request);
    }

    /// <summary>
    /// 删除项目信息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(DeleteRouteInput request)
    {
        return await _service.DeleteAsync(request);
    }

    /// <summary>
    /// 获取全局路由配置
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<DetailRouteOutput> GetDetailAsync([FromRoute] Guid id)
    {
        return await _service.GetByIdAsync(id);
    }
}