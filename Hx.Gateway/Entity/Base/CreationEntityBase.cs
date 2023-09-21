﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Gateway.Entity
{
    /// <summary>
    /// 带有创建信息的实体(泛型)
    /// </summary>
    public abstract class CreationEntityBase<TKey> : EntityBase<TKey>
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建者Id
        /// </summary>
        public virtual TKey CreatorId { get; set; }
    }

    /// <summary>
    /// 带有创建信息的实体(非泛型，默认主键为雪花Id)
    /// </summary>
    public abstract class CreationEntityBase : EntityBase
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建者Id
        /// </summary>
        public virtual long? CreatorId { get; set; }
    }
}
