﻿using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;

namespace Hx.Gateway.Application.Entities
{
    /// <summary>
    /// 路由属性表
    ///</summary>
    [SugarTable(null, "路由属性表")]
    public class TgRouteProperty: EntityBaseId
    {
        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnDescription = "Key")]
        public string Key { get; set; }

        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnDescription = "Value")]
        public string Value { get; set; }

        /// <summary>
        /// 路由Id 
        ///</summary>
        [SugarColumn(ColumnDescription = "RouteId")]
        public long RouteId { get; set; }

        /// <summary>
        ///  
        ///</summary>
        [SugarColumn(ColumnDescription = "Type")]
        public int Type { get; set; }
    }
}
