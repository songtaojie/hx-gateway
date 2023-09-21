using Hx.Sdk.Common;
using SqlSugar;

namespace Hx.Gateway.Entity
{
    /// <summary>
    /// 路由地址配置表
    ///</summary>
    [SugarTable(null, "路由地址配置表")]
    public class TgRouteHostPort : EntityBase
    {
        /// <summary>
        ///  路由主机
        ///</summary>
        [SugarColumn(ColumnDescription = "路由主机", Length = 64)]
        public string Host { get; set; }

        /// <summary>
        ///  端口
        ///</summary>
        [SugarColumn(ColumnDescription = "路由端口")]
        public int Port { get; set; }

        /// <summary>
        /// 路由Id 
        ///</summary>
        [SugarColumn(ColumnDescription = "路由Id")]
        public long RouteId { get; set; }
    }
}
