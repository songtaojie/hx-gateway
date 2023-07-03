// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Application.SeedData;
public class TgDictTypeSeedData : ISqlSugarEntitySeedData<TgDictType>
{
    /// <summary>
    /// 种子数据
    /// </summary>
    /// <returns></returns>
    [IgnoreUpdate]
    public IEnumerable<TgDictType> HasData()
    {
        return new[]
        {
            new TgDictType { Id = 252885263000001, Type = DictTypeEnum.ConsulSettingKey, Name = "服务发现KV名字" },
            new TgDictType { Id = 252885263000002, Type = DictTypeEnum.ConsulDC, Name = "服务发现Dc" }
        };
    }
}

