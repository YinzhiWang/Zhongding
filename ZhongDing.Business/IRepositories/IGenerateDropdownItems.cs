using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.IRepositories
{
    /// <summary>
    /// 接口：需要生成下拉列表项集合
    /// </summary>
    public interface IGenerateDropdownItems
    {
        /// <summary>
        /// 获取下拉列表项集合
        /// </summary>
        /// <param name="uiSearchObj">数据过滤的对象.</param>
        /// <returns>下拉列表项集合</returns>
        IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null);
    }
}
