﻿// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Core;
/// <summary>
/// MD5 加密
/// </summary>
public static class MD5Encryption
{
    /// <summary>
    /// MD5 比较
    /// </summary>
    /// <param name="text">加密文本</param>
    /// <param name="hash">MD5 字符串</param>
    /// <param name="uppercase">是否输出大写加密，默认 false</param>
    /// <param name="is16">是否输出 16 位</param>
    /// <returns>bool</returns>
    public static bool Compare(string text, string hash, bool uppercase = false, bool is16 = false)
    {
        if (string.IsNullOrEmpty(text)) return false;
        return Compare(Encoding.UTF8.GetBytes(text), hash, uppercase, is16);
    }

    /// <summary>
    /// MD5 加密
    /// </summary>
    /// <param name="text">加密文本</param>
    /// <param name="uppercase">是否输出大写加密，默认 false</param>
    /// <param name="is16">是否输出 16 位</param>
    /// <returns></returns>
    public static string Encrypt(string text, bool uppercase = false, bool is16 = false)
    {
        if (string.IsNullOrEmpty(text)) return text;
        return Encrypt(Encoding.UTF8.GetBytes(text), uppercase, is16);
    }

    /// <summary>
    /// MD5 加密
    /// </summary>
    /// <param name="bytes">字节数组</param>
    /// <param name="uppercase">是否输出大写加密，默认 false</param>
    /// <param name="is16">是否输出 16 位</param>
    /// <returns></returns>
    public static string Encrypt(byte[] bytes, bool uppercase = false, bool is16 = false)
    {
        var data = MD5.HashData(bytes);

        var stringBuilder = new StringBuilder();
        for (var i = 0; i < data.Length; i++)
        {
            stringBuilder.Append(data[i].ToString("x2"));
        }

        var md5String = stringBuilder.ToString();
        var hash = !is16 ? md5String : md5String.Substring(8, 16);
        return !uppercase ? hash : hash.ToUpper();
    }

    /// <summary>
    /// MD5 比较
    /// </summary>
    /// <param name="bytes">字节数组</param>
    /// <param name="hash">MD5 字符串</param>
    /// <param name="uppercase">是否输出大写加密，默认 false</param>
    /// <param name="is16">是否输出 16 位</param>
    /// <returns>bool</returns>
    public static bool Compare(byte[] bytes, string hash, bool uppercase = false, bool is16 = false)
    {
        var hashOfInput = Encrypt(bytes, uppercase, is16);
        return hash.Equals(hashOfInput, StringComparison.OrdinalIgnoreCase);
    }
}
