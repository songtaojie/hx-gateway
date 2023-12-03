// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using Hx.Gateway.Core.Entity;
using System.Collections.Generic;

namespace Hx.Gateway.Core;

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
                Id = Guid.Parse("a2e92532-b47f-40d8-8b6c-ab12517a9787"),
                Account="admin",
                Password= MD5Encryption.Encrypt("123456"),
                Name="系统管理员",
                Status = StatusEnum.Enable
            },
        };
    }
}
