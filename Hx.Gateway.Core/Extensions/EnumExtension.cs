// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Core;
/// <summary>
/// 枚举扩展
/// </summary>
public static class EnumExtension
{
    /// <summary>
    ///  获取枚举的中文描述
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string GetDescription(this Enum obj)
    {
        string objName = obj.ToString();
        Type t = obj.GetType();
        FieldInfo fi = t.GetField(objName);
        DescriptionAttribute[] arrDesc = (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
        return arrDesc[0].Description;
    }
}
      