using System;
using System.Collections.Generic;
using System.Text;
using SqlSugar;

namespace Hx.Gateway.Core.Entity
{
    /// <summary>
    /// 带有状态数据的实体（非泛型）
    /// </summary>
    public abstract class FullAuditedEntityBase : AuditedEntityBase
    {
        /// <summary>
        /// 是否删除
        /// </summary>
        [SugarColumn(ColumnDescription = "是否删除")]
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// 删除人id
        /// </summary>
        [SugarColumn(ColumnDescription = "删除人id")]
        public virtual Guid? DeleterId { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        [SugarColumn(ColumnDescription = "删除时间")]
        public virtual DateTime? DeleteTime { get; set; }
    }
}
