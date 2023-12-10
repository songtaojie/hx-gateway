// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Hx.Gateway.Admin.Services.Projects.Dto;
using Hx.Gateway.Admin.Services;
using Hx.Gateway.Application.Services.Projects;
using Hx.Gateway.Core;
using Microsoft.AspNetCore.Mvc;
using Hx.Gateway.Application.Services.GlobalConfiguration;
using Hx.Gateway.Admin.Services.GlobalConfiguration.Dtos;
using Hx.Gateway.Application.Services.GlobalConfiguration.Dtos;

namespace Hx.Gateway.Admin.Controllers;

/// <summary>
/// 项目
/// </summary>
public class GlobalConfigurationController : BaseControllerBase
{
    private readonly GlobalConfigurationService _service;
    public GlobalConfigurationController(GlobalConfigurationService service)
    {
        _service = service;
    }

    /// <summary>
    /// 分页查询项目信息
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<PagedListResult<PageGlobalConfigurationOutput>> GetPageAsync([FromQuery] PageGlobalConfigurationInput input)
    {
        var result = await _service.GetPageAsync(input);
        return result;
    }

    /// <summary>
    /// 新增全局配置
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<bool> AddAsync(AddGlobalConfigurationInput request)
    {
        return await _service.AddAsync(request);
    }

    /// <summary>
    /// 编辑项目信息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut]
    public async Task<bool> UpdateAsync(UpdateGlobalConfigurationInput request)
    {
        return await _service.UpdateAsync(request);
    }

    /// <summary>
    /// 编辑项目信息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPatch]
    public async Task<bool> UpdateStatusAsync(UpdateGlobalConfigurationStatusInput request)
    {
        return await _service.UpdateStatusAsync(request);
    }

    /// <summary>
    /// 删除项目信息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpDelete]
    public async Task<bool> DeleteAsync(DeleteGlobalConfigurationInput request)
    {
        return await _service.DeletAsync(request);
    }
    /// <summary>
    /// 获取全局路由配置
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<GlobalConfigurationOutput> GetDetailAsync([FromRoute]Guid id)
    {
        return await _service.GetByIdAsync(id);
    }
    
}
