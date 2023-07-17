// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Admin.Options.Ocelot;
/// <summary>
/// 为了验证路由，然后使用Ocelot的任何基于声明的功能，如授权或使用令牌的值修改请求。
/// 用户必须像往常一样在他们的Startup.cs中注册身份验证服务，
/// 但每次注册时都提供一个方案(身份验证提供者密钥)。
/// </summary>
public class AuthenticationOptions
{
    /// <summary>
    /// 当Ocelot运行时，它会查看这个AuthenticationProviderKey并检查是否有一个使用给定密钥注册的身份验证提供程序。
    /// 如果没有，那么Ocelot将不会启动，如果有，那么Route将在执行时使用该提供程序
    /// </summary>
    public string AuthenticationProviderKey { get; set; }

    /// <summary>
    /// 允许的范围
    /// </summary>
    public List<string> AllowedScopes { get; set; }
}
