using Hx.Gateway.Admin.Enum;
using Hx.Sdk.Common;
using SqlSugar;

namespace Hx.Gateway.Entity
{
    /// <summary>
    /// 用户表
    ///</summary>
    [SugarTable(null, "用户表")]
    public class TgUserAccount : EntityBase
    {
        /// <summary>
        /// 账户
        ///</summary>
        [SugarColumn(ColumnDescription = "账户", Length = 64)]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        ///</summary>
        [SugarColumn(ColumnDescription = "密码", Length = 200)]
        public string Password { get; set; }

        /// <summary>
        /// OpenId
        /// </summary>
        [SugarColumn(ColumnDescription = "OpenId", IsNullable = true, Length = 200)]
        public string OpenId { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        [SugarColumn(ColumnDescription = "姓名", Length = 64)]
        public string Name { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        [SugarColumn(ColumnDescription = "状态")]
        public StatusEnum Status { get; set; }
    }
}
