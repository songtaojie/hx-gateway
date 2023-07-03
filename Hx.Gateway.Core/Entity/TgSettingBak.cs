using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;

namespace Hx.Gateway.Application.Entities
{
    /// <summary>
    /// 配置备份
    ///</summary>
    [SugarTable(null, "配置备份")]
    public class TgSettingBak: EntityBaseId
    {
        /// <summary>
        /// 备份时间
        ///</summary>
        [SugarColumn(ColumnDescription = "备份时间")]
        public DateTime BakTime { get; set; }

        /// <summary>
        /// 备份内容
        ///</summary>
        [SugarColumn(ColumnDescription = "备份内容")]
        public string BakJson { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(ColumnDescription = "备注")]
        public string Remark { get; set; }

        /// <summary>
        /// ConsulKey
        /// </summary>
        [SugarColumn(ColumnDescription = "ConsulKey")]
        public string ConsulKey { get; set; }

        /// <summary>
        /// ConsulDc
        /// </summary>
        [SugarColumn(ColumnDescription = "ConsulDc")]
        public string ConsulDc { get; set; }
    }
}
