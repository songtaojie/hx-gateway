using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Core
{
    /// <summary>
    /// 基础树
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseTreeDto<T>
    {
        public string Label { get; set; }

        public T Value { get; set; }

        public T ParentId { get; set; }

        public List<BaseTreeDto<T>> Children { get; set; }
    }
}
