// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Core.Options;
/// <summary>
/// 数据库连接配置
/// </summary>
public sealed class DbConnectionConfig : ConnectionConfig
{
    /// <summary>
    /// 启用库表初始化
    /// </summary>
    public bool EnableInitDb { get; set; }

    /// <summary>
    /// 启用种子初始化
    /// </summary>
    public bool EnableInitSeed { get; set; }

    /// <summary>
    /// 启用驼峰转下划线
    /// </summary>
    public bool EnableUnderLine { get; set; }

    /// <summary>
    /// 启用Sql日志记录
    /// </summary>
    public bool EnableSqlLog { get; set; }

    internal ConnectionConfig ToConnectionConfig()
    {
        return new ConnectionConfig
        {
            AopEvents = AopEvents,
            ConfigId = ConfigId,
            ConfigureExternalServices = ConfigureExternalServices,
            ConnectionString = ConnectionString,
            DbLinkName = DbLinkName,
            DbType = DbType,
            IndexSuffix = IndexSuffix,
            IsAutoCloseConnection = IsAutoCloseConnection,
            LanguageType = LanguageType,
            MoreSettings = MoreSettings,
            SlaveConnectionConfigs = SlaveConnectionConfigs,
            SqlMiddle = SqlMiddle
        };
    }
}
