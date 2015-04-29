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
    public interface ISupplierBankAccountRepository : IBaseRepository<SupplierBankAccount>, IGenerateDropdownItems
    {
        IList<UIDropdownItem> GetDropdownItems(UISearchDropdownItem uiSearchObj = null);
    }
}
