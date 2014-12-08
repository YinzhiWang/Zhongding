using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    /// <summary>
    /// 类：下拉列表查询
    /// </summary>
    public class UISearchDropdownItem
    {

        /// <summary>
        /// 父级项值
        /// </summary>
        public int? ParentItemValue { get; set; }

        /// <summary>
        /// 需包含的项值列表
        /// </summary>
        public IList<int> IncludeItemValues { get; set; }

        /// <summary>
        /// 需排除的项值列表
        /// </summary>
        public IList<int> ExcludeItemValues { get; set; }

        /// <summary>
        /// 需筛选的项的文本
        /// </summary>
        public string ItemText { get; set; }

        /// <summary>
        /// 扩展查询参数
        /// </summary>
        /// <value>The extension.</value>
        public UISearchExtension Extension { get; set; }

    }
}
