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
        /// <value>The parent item value.</value>
        public int? ParentItemValue { get; set; }

        /// <summary>
        /// 需筛选的项值的列表
        /// </summary>
        /// <value>The item values.</value>
        public IList<int> ItemValues { get; set; }

        /// <summary>
        /// 需筛选的项的文本
        /// </summary>
        /// <value>The item text.</value>
        public string ItemText { get; set; }

        /// <summary>
        /// 其他扩展实体ID
        /// </summary>
        /// <value>例如:users表需要根据DepartmentID过滤user.</value>
        public int ExtensionEntityID { get; set; }

    }
}
