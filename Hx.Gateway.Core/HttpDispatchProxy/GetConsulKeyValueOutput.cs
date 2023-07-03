// Apache-2.0 License
// Copyright (c) 2021-2022 songtaojie
// 电话/微信：stj15638116256  Email：stjworkemail@163.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Application;
public class GetConsulKeyValueOutput
{
    public int LockIndex { get; set; }

    public string Key { get; set; }

    public int Flags { get; set; }

    public string Value { get; set; }

    public long CreateIndex { get; set; }

    public long ModifyIndex { get; set; }
}
