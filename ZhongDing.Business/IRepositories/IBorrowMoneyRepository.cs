using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface IBorrowMoneyRepository : IBaseRepository<BorrowMoney>
    {
        IList<UIBorrowMoney> GetUIList(Domain.UISearchObjects.UISearchBorrowMoney uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        UIBorrowMoneyBalance CalculateBalance(Domain.UISearchObjects.UISearchBorrowMoney uiSearchObj);
    }
}
