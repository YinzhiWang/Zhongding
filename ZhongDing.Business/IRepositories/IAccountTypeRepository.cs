using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface IAccountTypeRepository : IBaseRepository<AccountType>
    {
        /// <summary>
        /// 获取UI List，不分页
        /// </summary>
        /// <returns>IList{UIAccountType}.</returns>
        IList<UIAccountType> GetUIList();

    }
}
