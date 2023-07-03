// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using Furion.ConfigurableOptions;

namespace Hx.Gateway.Application.Options;
public class LoginSettingsOptions : IConfigurableOptions
{
    /// <summary>
    /// 是否使用Totp登录
    /// </summary>
    public bool TotpLogin { get; set; }

    public string AppId { get; set; }

    public string AppSecret { get; set; }
}
