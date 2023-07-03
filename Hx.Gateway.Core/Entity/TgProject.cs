using System;
using System.Collections.Generic;
using System.Linq;
using SqlSugar;

namespace Hx.Gateway.Application.Entities
{
    /// <summary>
    /// 项目表
    ///</summary>
    [SugarTable(null, "项目表")]
    public class TgProject : EntityBase
    {
        /// <summary>
        /// 项目名称 
        ///</summary>
        [SugarColumn(ColumnDescription = "项目名称")]
        public string Name { get; set; }
        /// <summary>
        /// 排序字段 
        ///</summary>
        [SugarColumn(ColumnDescription = "排序字段")]
        public int SortIndex { get; set; }

        /// <summary>
        /// 状态字段 
        ///</summary>
        [SugarColumn(ColumnDescription = "状态字段")]
        public StatusEnum Status { get; set; }

        /// <summary>
        /// 路由配置
        /// </summary>
        [Navigate(NavigateType.OneToMany, nameof(TgRoute.ProjectId))]//一对多
        public List<TgRoute> Routes { get; set; }//只能是null不能赋默认值
    }
}
