// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Application.SeedData;
public class TgDictItemSeedData : ISqlSugarEntitySeedData<TgDictItem>
{
    /// <summary>
    /// 种子数据
    /// </summary>
    /// <returns></returns>
    [IgnoreUpdate]
    public IEnumerable<TgDictItem> HasData()
    {

        return new[]
        {
            new TgDictItem
            {
                Id = 252885263000100,
                Code = "Consul KV配置名称",
                Value = "HxConsul",
                DictTypeId = 252885263000001,
                Remark = "服务发现Key/Value配置名称",
                SortIndex = 0,
                Status = StatusEnum.Enable
            },
            new TgDictItem
            {
                Id = 252885263000200,
                Code = "Dc",
                Value = "dc1",
                DictTypeId = 252885263000002,
                Remark = "服务发现Dc值",
                SortIndex = 0,
                Status = StatusEnum.Enable
            }
        };
    }
}
