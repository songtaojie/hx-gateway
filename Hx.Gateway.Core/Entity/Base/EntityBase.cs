using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Hx.Gateway.Core.Entity
{

    /// <summary>
    /// 框架实体基类Id
    /// </summary>
    public abstract class EntityBase:IEntity<Guid>
    {
        /// <summary>
        /// 雪花Id
        /// </summary>
        [SugarColumn(ColumnDescription = "主键Id", IsPrimaryKey = true)]
        public virtual Guid Id { get; set; }
    }
}
