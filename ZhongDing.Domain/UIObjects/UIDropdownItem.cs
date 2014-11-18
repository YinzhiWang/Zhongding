using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDropdownItem
    {
        public int ItemValue { get; set; }

        public string ItemText { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        /// <value>The extension.</value>
        public object Extension { get; set; }
    }
}
