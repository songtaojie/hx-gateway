// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

namespace Hx.Gateway.Admin.Services.Projects.Dto;

public class ListProjectOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 项目编码
    ///</summary>
    public string Code { get; set; }

    /// <summary>
    /// 项目名称 
    ///</summary>
    public string Name { get; set; }
}
