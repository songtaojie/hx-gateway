// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using Hx.Gateway.Core.Entity;
using Hx.Gateway.Core.Enum;
using Hx.Sdk.SqlSugar;
using System.Collections.Generic;

namespace Hx.Gateway.Core.SeedData;

/// <summary>
/// 系统用户表种子数据
/// </summary>
public class TgUserAccountSeedData : ISqlSugarEntitySeedData<TgUserAccount>
{
    /// <summary>
    /// 种子数据
    /// </summary>
    /// <returns></returns>
    //[IgnoreUpdate]
    public IEnumerable<TgUserAccount> HasData()
    {
        return new[]
        {
            new TgUserAccount
            {
                Id=252885263000000,
                Account="admin",
                Password="e10adc3949ba59abbe56e057f20f883e",
                Name="系统管理员",
                Status = StatusEnum.Enable
            },
        };
    }
}
