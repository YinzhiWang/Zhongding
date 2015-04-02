using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface IReportRepository
    {
        IList<UICompany> GetCompanyReport();
        IList<UIProcureOrderReport> GetProcureOrderReport(Domain.UISearchObjects.UISearchProcureOrderReport uiSearchObj);

        IList<UIProcureOrderReport> GetProcureOrderReport(Domain.UISearchObjects.UISearchProcureOrderReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords);


        IList<UIProcureOrderApplicationPaymentReport> GetProcureOrderApplicationPaymentReport(Domain.UISearchObjects.UISearchProcureOrderApplicationPaymentReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        IList<UIProcureOrderApplicationPaymentReport> GetProcureOrderApplicationPaymentReport(Domain.UISearchObjects.UISearchProcureOrderApplicationPaymentReport uiSearchObj);

        IList<UIClientSaleAppReport> GetClientSaleAppReport(UISearchClientSaleAppReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        IList<UIClientSaleAppReport> GetClientSaleAppReport(UISearchClientSaleAppReport uiSearchObj);

        IList<UIStockOutDetailReport> GetStockOutDetailReport(UISearchStockOutDetailReport uiSearchObj);

        IList<UIStockOutDetailReport> GetStockOutDetailReport(UISearchStockOutDetailReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        IList<UIStockInDetailReport> GetStockInDetailReport(UISearchStockInDetailReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        IList<UIStockInDetailReport> GetStockInDetailReport(UISearchStockInDetailReport uiSearchObj);
    }
}
