using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface ISalarySettleDetailRepository : IBaseRepository<SalarySettleDetail>
    {
        IList<UISalarySettleDetail> GetUIList(Domain.UISearchObjects.UISearchSalarySettleDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        IList<UISalarySettleDetail> GetUIListForAppPayment(Domain.UISearchObjects.UISearchSalarySettleDetail uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
    }
}
