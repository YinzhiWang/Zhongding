using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface IReminderRepository
    {
        int GetUIInventoryReminderCount(Domain.UISearchObjects.UISearchInventoryReminder uiSearchObj);
        IList<UIInventoryReminder> GetUIInventoryReminder(Domain.UISearchObjects.UISearchInventoryReminder uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
        int GetUICautionMoneyReminderCount(Domain.UISearchObjects.UISearchCautionMoneyReminder uiSearchObj);
        IList<UICautionMoneyReminder> GetUICautionMoneyReminder(Domain.UISearchObjects.UISearchCautionMoneyReminder uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
        int GetUIProductInfoExpiredReminderCount(Domain.UISearchObjects.UISearchProductInfoExpiredReminder uiSearchObj);
        IList<UIProductInfoExpiredReminder> GetUIProductInfoExpiredReminder(Domain.UISearchObjects.UISearchProductInfoExpiredReminder uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
        int GetUIClientInfoReminderCount(Domain.UISearchObjects.UISearchClientInfoReminder uiSearchObj);
        IList<UIClientInfoReminder> GetUIClientInfoReminder(Domain.UISearchObjects.UISearchClientInfoReminder uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
        int GetUISupplierInfoReminderCount(Domain.UISearchObjects.UISearchSupplierInfoReminder uiSearchObj);
        IList<UISupplierInfoReminder> GetUISupplierInfoReminder(Domain.UISearchObjects.UISearchSupplierInfoReminder uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
        int GetUIBorrowMoneyExpiredReminderCount(Domain.UISearchObjects.UISearchBorrowMoneyExpiredReminder uiSearchObj);
        IList<UIBorrowMoneyExpiredReminder> GetUIBorrowMoneyExpiredReminder(Domain.UISearchObjects.UISearchBorrowMoneyExpiredReminder uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
        int GetUIGuaranteeReceiptExpiredReminderCount(Domain.UISearchObjects.UISearchGuaranteeReceiptExpiredReminder uiSearchObj);
        IList<UIGuaranteeReceiptExpiredReminder> GetUIGuaranteeReceiptExpiredReminder(Domain.UISearchObjects.UISearchGuaranteeReceiptExpiredReminder uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
        int GetUIProductExpiredReminderCount(Domain.UISearchObjects.UISearchProductExpiredReminder uiSearchObj);
        IList<UIProductExpiredReminder> GetUIProductExpiredReminder(Domain.UISearchObjects.UISearchProductExpiredReminder uiSearchObj, int pageIndex, int pageSize, out int totalRecords);  
    }
}
