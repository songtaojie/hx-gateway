using Hx.Sdk.Sqlsugar.Repositories;
using Hx.Sdk.Sqlsugar;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Options;
using System.Reflection;
using System.Runtime;
using Microsoft.Extensions.Configuration;
using Hx.Gateway.Core;
using Microsoft.AspNetCore.Http.Connections;
using Hx.Gateway.Core.Options;
using FreeRedis;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// SqlSugar 拓展类
    /// </summary>
    public static class SqlSugarServiceCollectionExtensions
    {
        /// <summary>
        /// 添加 SqlSugar 拓展
        /// </summary>
        /// <param name="services"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugar(this IServiceCollection services,Action<DbConnectionConfig> action = default)
        {
            DbConnectionConfig config = new DbConnectionConfig();
            SqlSugarConfigProvider.SetDbConfig(config);
            action?.Invoke(config);
            SqlSugarScope sqlSugar = new(config.ToConnectionConfig(), db =>
            {
                var dbProvider = db.GetConnectionScope(config.ConfigId);
                if(config.EnableSqlLog) SqlSugarConfigProvider.SetAopLog(dbProvider);
            });

            // 初始化数据库表结构及种子数据
            SqlSugarConfigProvider.InitDatabase(sqlSugar, config);
            services.AddSingleton<ISqlSugarClient>(sqlSugar);
            // 注册非泛型仓储
            services.AddScoped<ISqlSugarRepository, SqlSugarRepository>();
            // 注册 SqlSugar 仓储
            services.AddScoped(typeof(ISqlSugarRepository<>), typeof(SqlSugarRepository<>));
            return services;
        }
    }
}
