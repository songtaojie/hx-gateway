using Hx.Gateway.Core;
using Hx.Gateway.Core.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlSugar;

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
            DbConnectionConfig dbConnectionConfig = new DbConnectionConfig();
            SqlSugarConfigProvider.SetDbConfig(dbConnectionConfig);
            action?.Invoke(dbConnectionConfig);
       
            //注册SqlSugar用AddScoped
            services.AddScoped<ISqlSugarClient>(provider =>
            {
                //Scoped用SqlSugarClient 
                SqlSugarClient sqlSugar = new SqlSugarClient(dbConnectionConfig.ToConnectionConfig(),
                db =>
                {
                    //每次上下文都会执行
                    SqlSugarConfigProvider.InitDatabase(db, dbConnectionConfig);
                    SqlSugarConfigProvider.SetDataExecuting(db);
                    var log = provider.GetService<ILogger<ISqlSugarClient>>();
                    if (dbConnectionConfig.EnableSqlLog) SqlSugarConfigProvider.SetAopLog(db, log);
                });
                return sqlSugar;
            });

            // 注册非泛型仓储
            services.AddScoped<ISqlSugarRepository, SqlSugarRepository>();
            // 注册 SqlSugar 仓储
            services.AddScoped(typeof(ISqlSugarRepository<>), typeof(SqlSugarRepository<>));
            return services;
        }

        /// <summary>
        /// 添加 SqlSugar 拓展
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugar(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<DbConnectionConfig>(config.GetSection("DbSettings"))
                .PostConfigure<DbConnectionConfig>(options => 
                {
                    SqlSugarConfigProvider.SetDbConfig(options);
                });
            services.AddHttpContextAccessor();
            //注册SqlSugar用AddScoped
            services.AddScoped<ISqlSugarClient>(provider =>
            {
                var options = provider.GetService<IOptions<DbConnectionConfig>>();
                var dbConnectionConfig = options.Value;
                //Scoped用SqlSugarClient 
                SqlSugarClient sqlSugar = new SqlSugarClient(dbConnectionConfig.ToConnectionConfig(),
                db =>
                {
                    //每次上下文都会执行
                    SqlSugarConfigProvider.InitDatabase(db, dbConnectionConfig);
                    SqlSugarConfigProvider.SetDataExecuting(db);
                    var log = provider.GetService<ILogger<ISqlSugarClient>>();
                    if (dbConnectionConfig.EnableSqlLog) SqlSugarConfigProvider.SetAopLog(db, log);
                });
                return sqlSugar;
            });

            // 注册非泛型仓储
            services.AddScoped<ISqlSugarRepository, SqlSugarRepository>();
            // 注册 SqlSugar 仓储
            services.AddScoped(typeof(ISqlSugarRepository<>), typeof(SqlSugarRepository<>));
            return services;
        }
    }
}
