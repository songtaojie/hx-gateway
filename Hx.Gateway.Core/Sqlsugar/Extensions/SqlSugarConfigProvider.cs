﻿// Apache-2.0 License
// Copyright (c) 2021-2022 
// 作者:songtaojie
// 电话/微信：stjworkemail@163.com

using Hx.Gateway.Core.Options;
using Hx.Sdk.Sqlsugar;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SqlSugar;
using System.Runtime.Loader;
using FreeRedis;

namespace Hx.Gateway.Core;

/// <summary>
/// SqlSugar配置初始化
/// </summary>
internal static class SqlSugarConfigProvider
{
    /// <summary>
    /// 应用有效程序集
    /// </summary>
    private static IEnumerable<Assembly> _assemblies;

    private static IEnumerable<Type> _effectiveTypes;

    static SqlSugarConfigProvider()
    {
        _assemblies = GetAssemblies();
    }
    /// <summary>
    /// 获取应用有效程序集
    /// </summary>
    /// <returns>IEnumerable</returns>
    private static IEnumerable<Assembly> GetAssemblies()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();
        return assemblies.Where(r => !r.FullName.StartsWith("System"));
    }
    /// <summary>
    /// 设置实体所在的程序集
    /// </summary>
    public static void SetAssemblies(IEnumerable<Assembly> assemblies)
    {
        _assemblies = assemblies;
        _effectiveTypes = null;
    }
    /// <summary>
    /// 有效程序集类型
    /// </summary>
    internal static IEnumerable<Type> EffectiveTypes
    {
        get
        {
            if (_effectiveTypes != null) return _effectiveTypes;
            if (_effectiveTypes == null && _assemblies != null)
            {
                _effectiveTypes = _assemblies.SelectMany(GetTypes);
            }
            else
            {
                _effectiveTypes = Array.Empty<Type>();
            }
            return _effectiveTypes;
        }
    }

    /// <summary>
    /// 加载程序集中的所有类型
    /// </summary>
    /// <param name="ass"></param>
    /// <returns></returns>
    private static IEnumerable<Type> GetTypes(Assembly ass)
    {
        var types = Array.Empty<Type>();

        try
        {
            types = ass.GetTypes();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error load `{ass.FullName}` assembly,{ex.Message}\r\n{ex.StackTrace}");
            Console.ForegroundColor = ConsoleColor.DarkRed;
        }
        return types.Where(u => u.IsPublic);
    }


    /// <summary>
    /// 配置连接属性
    /// </summary>
    /// <param name="config"></param>
    public static DbConnectionConfig SetDbConfig(DbConnectionConfig config)
    {
        var configureExternalServices = new ConfigureExternalServices
        {
            EntityNameService = (type, entity) => // 处理表
            {
                if (config.EnableUnderLine && !entity.DbTableName.Contains('_'))
                    entity.DbTableName = UtilMethods.ToUnderLine(entity.DbTableName); // 驼峰转下划线
            },
            EntityService = (type, column) => // 处理列
            {
                if (new NullabilityInfoContext().Create(type).WriteState is NullabilityState.Nullable)
                    column.IsNullable = true;
                if (config.EnableUnderLine && !column.IsIgnore && !column.DbColumnName.Contains('_'))
                    column.DbColumnName = UtilMethods.ToUnderLine(column.DbColumnName); // 驼峰转下划线

                if (config.DbType == SqlSugar.DbType.Oracle)
                {
                    if (type.PropertyType == typeof(long) || type.PropertyType == typeof(long?))
                        column.DataType = "number(18)";
                    if (type.PropertyType == typeof(bool) || type.PropertyType == typeof(bool?))
                        column.DataType = "number(1)";
                }
            },
            //DataInfoCacheService = new SqlSugarCache(),
        };
        config.ConfigureExternalServices = configureExternalServices;
        config.InitKeyType = InitKeyType.Attribute;
        config.IsAutoCloseConnection = true;
        config.MoreSettings = new ConnMoreSettings
        {
            IsAutoRemoveDataCache = true,
            SqlServerCodeFirstNvarchar = true // 采用Nvarchar
        };
        return config;
    }

    /// <summary>
    /// 配置Aop日志
    /// </summary>
    /// <param name="db"></param>
    /// <param name="logger"></param>
    public static void SetAopLog(ISqlSugarClient db,ILogger logger = null)
    {
        var config = db.CurrentConnectionConfig;

        // 设置超时时间
        db.Ado.CommandTimeOut = 30;

        // 打印SQL语句
        db.Aop.OnLogExecuting = (sql, pars) =>
        {
            var originColor = Console.ForegroundColor;
            if (sql.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase))
                Console.ForegroundColor = ConsoleColor.Green;
            if (sql.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase) || sql.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase))
                Console.ForegroundColor = ConsoleColor.Yellow;
            if (sql.StartsWith("DELETE", StringComparison.OrdinalIgnoreCase))
                Console.ForegroundColor = ConsoleColor.Red;
            if (logger == null)
            {
                Console.WriteLine($"【{DateTime.Now}——执行SQL】\r\n{UtilMethods.GetNativeSql(sql, pars)}\r\n");
                Console.ForegroundColor = originColor;
            }
            else
            {
                logger.LogInformation($"【{DateTime.Now}——执行SQL】\r\n{UtilMethods.GetNativeSql(sql, pars)}\r\n");
                Console.ForegroundColor = originColor;
            }
            
        };
        db.Aop.OnError = ex =>
        {
            if (ex.Parametres == null) return;
            if (logger == null)
            {
                Console.WriteLine($"【{DateTime.Now}——错误SQL】\r\n {UtilMethods.GetNativeSql(ex.Sql, (SugarParameter[])ex.Parametres)} \r\n");
                Console.ForegroundColor = ConsoleColor.DarkRed;
            }
            else
            {
                logger.LogInformation($"【{DateTime.Now}——错误SQL】\r\n {UtilMethods.GetNativeSql(ex.Sql, (SugarParameter[])ex.Parametres)} \r\n");
            }
            
        };

        
    }

    /// <summary>
    /// 设置数据审计
    /// </summary>
    /// <param name="db"></param>
    public static void SetDataExecuting(ISqlSugarClient db)
    {
        // 数据审计
        db.Aop.DataExecuting = (oldValue, entityInfo) =>
        {
            if (entityInfo.OperationType == DataFilterType.InsertByObject)
            {
                // 主键(long类型)且没有值的---赋值雪花Id
                if (entityInfo.EntityColumnInfo.IsPrimarykey && entityInfo.EntityColumnInfo.PropertyInfo.PropertyType == typeof(Guid))
                {
                    var id = entityInfo.EntityColumnInfo.PropertyInfo.GetValue(entityInfo.EntityValue);
                    if (id == null || (Guid)id == Guid.Empty)
                        entityInfo.SetValue(Guid.NewGuid());
                }
                if (entityInfo.PropertyName == "CreateTime")
                    entityInfo.SetValue(DateTime.Now);
            }
            if (entityInfo.OperationType == DataFilterType.UpdateByObject)
            {
                if (entityInfo.PropertyName == "UpdateTime")
                    entityInfo.SetValue(DateTime.Now);
            }
        };

    }

    private static bool _initDb = false;
    private static bool _initSeed = false;
    /// <summary>
    /// 初始化数据库和种子数据
    /// DbConnectionConfig需开启相应的开关
    /// </summary>
    /// <param name="dbProvider"></param>
    /// <param name="config"></param>
    /// <param name="logger"></param>
    public static void InitDatabase(ISqlSugarClient dbProvider, DbConnectionConfig config,ILogger logger)
    {
        if (!_initDb)
        {
            _initDb = true;
            // 创建数据库
            if (config.DbType != DbType.Oracle)
                dbProvider.DbMaintenance.CreateDatabase();

            // 获取所有实体表-初始化表结构
            var entityTypes = EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass && u.IsDefined(typeof(SugarTable), false)).ToList();
            if (!entityTypes.Any()) return;
            foreach (var entityType in entityTypes)
            {
                var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
                if (tAtt != null && tAtt.configId.ToString() != config.ConfigId) continue;

                var splitTable = entityType.GetCustomAttribute<SplitTableAttribute>();
                if (splitTable == null)
                    dbProvider.CodeFirst.InitTables(entityType);
                else
                    dbProvider.CodeFirst.SplitTables().InitTables(entityType);
            }
        }

        if (!_initSeed && config.EnableInitSeed)
        {
            _initSeed = true;
            // 获取所有种子配置-初始化数据
            var seedDataTypes = EffectiveTypes.Where(u => !u.IsInterface && !u.IsAbstract && u.IsClass
                && u.GetInterfaces().Any(i => i.HasImplementedRawGeneric(typeof(ISqlSugarEntitySeedData<>)))).ToList();
            if (!seedDataTypes.Any()) return;
            foreach (var seedType in seedDataTypes)
            {
                var instance = Activator.CreateInstance(seedType);

                var hasDataMethod = seedType.GetMethod("HasData");
                if (hasDataMethod == null) continue;
                var seedData = ((IEnumerable)hasDataMethod?.Invoke(instance, null))?.Cast<object>();
                if (seedData == null) continue;

                var entityType = seedType.GetInterfaces().First().GetGenericArguments().First();
                var tAtt = entityType.GetCustomAttribute<TenantAttribute>();
                if (tAtt != null && tAtt.configId.ToString() != config.ConfigId) continue;
                //if (tAtt == null && config.ConfigId != SqlSugarConst.ConfigId) continue;

                var entityInfo = dbProvider.EntityMaintenance.GetEntityInfo(entityType);
                if (entityInfo.Columns.Any(u => u.IsPrimarykey))
                {
                    // 按主键进行批量增加和更新
                    var storage = dbProvider.StorageableByObject(seedData.ToList()).ToStorage();
                    storage.AsInsertable.ExecuteCommand();
                    var ignoreUpdate = hasDataMethod.GetCustomAttribute<IgnoreSeedUpdateAttribute>();
                    if (ignoreUpdate == null) storage.AsUpdateable.ExecuteCommand();
                }
                else
                {
                    // 无主键则只进行插入
                    if (!dbProvider.Queryable(entityInfo.DbTableName, entityInfo.DbTableName).Any())
                        dbProvider.InsertableByObject(seedData.ToList()).ExecuteCommand();
                }
            }
        }
    }

}
