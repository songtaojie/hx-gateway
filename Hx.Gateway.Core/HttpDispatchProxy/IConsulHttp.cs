// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using Furion.RemoteRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Application;
/// <summary>
/// Consul Http客户端
/// </summary>
public interface IConsulHttp : IHttpDispatchProxy
{
    /// <summary>
    /// 读取Consul指定配置
    /// </summary>
    /// <param name="settingPath"></param>
    /// <param name="dc"></param>
    /// <returns></returns>
    [Get("#(ConsulConfigs:Address)/v1/kv/{settingPath}?dc={dc}")]
    Task<List<GetConsulKeyValueOutput>> GetConsulKeyValueAsync(string settingPath, string dc);


    [Put("#(ConsulConfigs:Address)/v1/kv/{settingPath}?dc={dc}&flags=0")]
    Task<bool> SaveConsulKeyValueAsync(string settingPath, string dc, [Body] string jsonObj);
}
