using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface ICompanyRepository : IBaseRepository<Company>
    {
        /// <summary>
        /// 获取UI List，不分页
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <returns>IList{UICompany}.</returns>
        IList<UICompany> GetUIList(UISearchCompany uiSearchObj = null);

        /// <summary>
        /// 获取UI List，分页
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <param name="totalRecords">The total records.</param>
        /// <returns>IList{UICompany}.</returns>
        IList<UICompany> GetUIList(UISearchCompany uiSearchObj, out int totalRecords);
    }
}
