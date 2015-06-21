using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface ISalarySettleRepository : IBaseRepository<SalarySettle>
    {
        IList<UISalarySettle> GetUIList(Domain.UISearchObjects.UISearchSalarySettle uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        bool HasSalarySettle(int departmentID, DateTime settleDate);
    }
}
