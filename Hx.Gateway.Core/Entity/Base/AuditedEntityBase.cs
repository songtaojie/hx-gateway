using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Gateway.Core.Entity
{
    /// <summary>
    /// 带有更新信息的实体(非泛型，默认主键为雪花id)
    /// </summary>
    public abstract class AuditedEntityBase : CreationEntityBase
    {
        /// <summary>
        /// 更新时间
        /// </summary>
        [SugarColumn(ColumnDescription = "更新时间", IsOnlyIgnoreInsert = true)]
        public virtual DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 更新者id
        /// </summary>
        [SugarColumn(ColumnDescription = "更新者id", IsOnlyIgnoreInsert = true)]
        public virtual Guid? UpdaterId { get; set; }
    }
}
