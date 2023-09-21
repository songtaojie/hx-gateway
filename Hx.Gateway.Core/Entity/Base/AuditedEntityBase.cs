﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Gateway.Core.Entity
{
    /// <summary>
    /// 带有更新信息的实体(泛型)
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public abstract class AuditedEntityBase<TKey> :CreationEntityBase<TKey>
    {
        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 更新者id
        /// </summary>
        public virtual TKey UpdaterId { get; set; }
    }

    /// <summary>
    /// 带有更新信息的实体(非泛型，默认主键为雪花id)
    /// </summary>
    public abstract class AuditedEntityBase : CreationEntityBase
    {
        /// <summary>
        /// 更新时间
        /// </summary>
        public virtual DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 更新者id
        /// </summary>
        public virtual long? UpdaterId { get; set; }
    }
}
