using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface IReportRepository
    {
        IList<UICompany> GetCompanyReport();
        IList<UIProcureOrderReport> GetProcureOrderReport(Domain.UISearchObjects.UISearchProcureOrderReport uiSearchObj);

        IList<UIProcureOrderReport> GetProcureOrderReport(Domain.UISearchObjects.UISearchProcureOrderReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
    }
}
