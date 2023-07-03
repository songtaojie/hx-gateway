using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;

namespace Hx.Gateway.Application.Entities
{
    /// <summary>
    /// 字典项
    ///</summary>
    [SugarTable(null, "字典项")]
    public class TgDictItem:EntityBaseId
    {
        /// <summary>
        /// 字典类型Id
        ///</summary>
        [SugarColumn(ColumnDescription = "字典Id")]
        public long DictTypeId { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [SugarColumn(ColumnDescription = "值", Length = 128)]
        public string Value { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        [SugarColumn(ColumnDescription = "编码", Length = 64)]
        public string Code { get; set; }

        /// <summary>
        /// 备注
        ///</summary>
        [SugarColumn(ColumnDescription = "备注")]
        public string? Remark { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(ColumnDescription = "排序")]
        public int SortIndex { get; set; } = 100;

        /// <summary>
        /// 状态
        ///</summary>
        [SugarColumn(ColumnDescription = "状态")]
        public StatusEnum Status { get; set; } = StatusEnum.Enable;
    }
}
