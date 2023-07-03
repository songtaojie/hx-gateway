// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Application.AutoMapper;
/// <summary>
/// 基础映射文件
/// </summary>
public class BaseMapper : IRegister
{
    public virtual void Register(TypeAdapterConfig config)
    {
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    protected string Serialize(object obj)
    {
        return obj == null ? string.Empty : JSON.Serialize(obj);
    }

    /// <summary>
    /// 反序列化
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="json"></param>
    /// <returns></returns>
    protected T Deserialize<T>(string json)where T:class,new()
    {
        return string.IsNullOrEmpty(json)? null : JSON.Deserialize<T>(json);
    }
}
