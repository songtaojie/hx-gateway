using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.Gateway.Core
{
    /// <summary>
    /// 下拉框
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseSelectDto<T>
    {
        /// <summary>
        /// 下拉框的值
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// 下拉框的标签
        /// </summary>
        public string Label { get; set; }
    }
}

