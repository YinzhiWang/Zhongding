using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;

namespace ZhongDing.Business.IRepositories
{
    public interface ISupplierCautionMoneyDeductionRepository : IBaseRepository<SupplierCautionMoneyDeduction>
    {
        IList<Domain.UIObjects.UISupplierCautionMoneyDeduction> GetUIList(Domain.UISearchObjects.UISearchSupplierCautionMoneyDeduction uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
    }
}
