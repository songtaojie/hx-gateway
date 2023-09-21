using System;
using System.Collections.Generic;
using System.Text;

namespace Hx.Gateway.Entity
{
    /// <summary>
    /// 实体接口
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class IEntity<TKey>
    {
        /// <summary>
        /// 主键id
        /// </summary>
        TKey Id { get; set; }
    }
}
