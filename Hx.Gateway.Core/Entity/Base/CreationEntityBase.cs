using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Gateway.Core.Entity
{
    /// <summary>
    /// 带有创建信息的实体
    /// </summary>
    public abstract class CreationEntityBase : EntityBase
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(ColumnDescription = "创建时间", IsOnlyIgnoreUpdate = true)]
        public virtual DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建者Id
        /// </summary>
        [SugarColumn(ColumnDescription = "创建者Id", IsOnlyIgnoreUpdate = true)]
        public virtual Guid? CreatorId { get; set; }
    }
}
