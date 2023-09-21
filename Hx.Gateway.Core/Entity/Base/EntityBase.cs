﻿using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;

namespace Hx.Gateway.Core.Entity
{

    /// <summary>
    /// 框架实体基类Id
    /// </summary>
    public abstract class EntityBase<TKey> : IEntity<TKey>
    {
        /// <summary>
        /// Id
        /// </summary>
        public virtual TKey Id { get; set; }
    }

    /// <summary>
    /// 框架实体基类Id
    /// </summary>
    public abstract class EntityBase:IEntity<long>
    {
        /// <summary>
        /// 雪花Id
        /// </summary>
        public virtual long Id { get; set; }
    }
}
