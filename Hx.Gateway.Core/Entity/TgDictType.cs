using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;

namespace Hx.Gateway.Application.Entities
{
    /// <summary>
    /// 字典
    ///</summary>
    [SugarTable(null, "字典")]
    public class TgDictType:EntityBaseId
    {
        /// <summary>
        /// 字典名称 
        ///</summary>
        [SugarColumn(ColumnDescription = "字典名称")]
        public string Name { get; set; }

        /// <summary>
        /// 字典类型 
        ///</summary>
        [SugarColumn(ColumnDescription = "字典类型")]
        public DictTypeEnum Type { get; set; }

        /// <summary>
        /// 路由地址配置
        /// </summary>
        [Navigate(NavigateType.OneToMany, nameof(TgDictItem.DictTypeId))]
        public List<TgDictItem> DictionaryItems { get; set; }
    }
}
