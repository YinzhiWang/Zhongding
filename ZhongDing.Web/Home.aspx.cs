using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories.Reminders;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web
{
    public partial class Home : BasePage
    {
        #region Repository s
        private IReminderRepository _PageReminderRepository;
        protected IReminderRepository PageReminderRepository
        {
            get
            {
                if (_PageReminderRepository == null)
                    _PageReminderRepository = new ReminderRepository();

                return _PageReminderRepository;
            }
        }
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.Home;
            if (!IsPostBack)
            {
                BindTabStrips();
                BindLableCounts();
            }
        }

        private void BindLableCounts()
        {
            bool hasPermission = false;
            hasPermission = inventoryReminder.HasPermissionCheck(PermossionIDAndValues);
            if (hasPermission)
            {
                liInventory.Visible = true;
                UISearchInventoryReminder uiSearchObj = new UISearchInventoryReminder()
                {
                    CompanyID = CurrentUser.CompanyID
                };
                lblInventory.Text = PageReminderRepository.GetUIInventoryReminderCount(uiSearchObj).ToString();
            }
            else
            {
                liInventory.Visible = false;
            }
            hasPermission = cautionMoneyReminder.HasPermissionCheck(PermossionIDAndValues);
            if (hasPermission)
            {
                liCautionMoney.Visible = true;
                UISearchCautionMoneyReminder uiSearchObj = new UISearchCautionMoneyReminder()
                {
                };
                lblCautionMoney.Text = PageReminderRepository.GetUICautionMoneyReminderCount(uiSearchObj).ToString();
            }
            else
            {
                liCautionMoney.Visible = false;
            }
            hasPermission = productInfoExpiredReminder.HasPermissionCheck(PermossionIDAndValues);
            if (hasPermission)
            {
                liProductInfoExpired.Visible = true;
                UISearchProductInfoExpiredReminder uiSearchObj = new UISearchProductInfoExpiredReminder()
                {
                    CompanyID = CurrentUser.CompanyID
                };
                lblProductInfoExpired.Text = PageReminderRepository.GetUIProductInfoExpiredReminderCount(uiSearchObj).ToString();
            }
            else
            {
                liProductInfoExpired.Visible = false;
            }
            hasPermission = clientInfoReminder.HasPermissionCheck(PermossionIDAndValues);
            if (hasPermission)
            {
                liClientInfo.Visible = true;
                UISearchClientInfoReminder uiSearchObj = new UISearchClientInfoReminder()
                {
                };
                lblClientInfo.Text = PageReminderRepository.GetUIClientInfoReminderCount(uiSearchObj).ToString();
            }
            else
            {
                liClientInfo.Visible = false;
            }
            hasPermission = supplierInfoReminder.HasPermissionCheck(PermossionIDAndValues);
            if (hasPermission)
            {
                liSupplier.Visible = true;
                UISearchSupplierInfoReminder uiSearchObj = new UISearchSupplierInfoReminder()
                {
                };
                lblSupplier.Text = PageReminderRepository.GetUISupplierInfoReminderCount(uiSearchObj).ToString();
            }
            else
            {
                liSupplier.Visible = false;
            }
            hasPermission = productExpiredReminder.HasPermissionCheck(PermossionIDAndValues);
            if (hasPermission)
            {
                liProductExpired.Visible = true;
                UISearchProductExpiredReminder uiSearchObj = new UISearchProductExpiredReminder()
                {
                    CompanyID = CurrentUser.CompanyID
                };
                lblProductExpired.Text = PageReminderRepository.GetUIProductExpiredReminderCount(uiSearchObj).ToString();
            }
            else
            {
                liProductExpired.Visible = false;
            }
            hasPermission = borrowMoneyExpiredReminder.HasPermissionCheck(PermossionIDAndValues);
            if (hasPermission)
            {
                liBorrowMoneyExpired.Visible = true;
                UISearchBorrowMoneyExpiredReminder uiSearchObj = new UISearchBorrowMoneyExpiredReminder()
                {
                };
                lblBorrowMoneyExpired.Text = PageReminderRepository.GetUIBorrowMoneyExpiredReminderCount(uiSearchObj).ToString();
            }
            else
            {
                liBorrowMoneyExpired.Visible = false;
            }
            hasPermission = guaranteeReceiptExpiredReminder.HasPermissionCheck(PermossionIDAndValues);
            if (hasPermission)
            {
                liGuaranteeReceiptExpired.Visible = true;
                UISearchGuaranteeReceiptExpiredReminder uiSearchObj = new UISearchGuaranteeReceiptExpiredReminder()
                {
                };
                uiSearchObj.IncludeWorkflowStatusIDs = new List<int>() { (int)EWorkflowStatus.Completed, (int)EWorkflowStatus.Shipping };
                lblGuaranteeReceiptExpired.Text = PageReminderRepository.GetUIGuaranteeReceiptExpiredReminderCount(uiSearchObj).ToString();
            }
            else
            {
                liGuaranteeReceiptExpired.Visible = false;
            }
        }

        private void BindTabStrips()
        {

            bool hasPermission = false;
            hasPermission = PermissionCheckTab(inventoryReminder, "pvInventory");
            hasPermission = PermissionCheckTab(cautionMoneyReminder, "pvCautionMoney");
            hasPermission = PermissionCheckTab(productInfoExpiredReminder, "pvProductInfoExpired");
            hasPermission = PermissionCheckTab(clientInfoReminder, "pvClientInfo");
            hasPermission = PermissionCheckTab(supplierInfoReminder, "pvSupplierInfo");
            hasPermission = PermissionCheckTab(productExpiredReminder, "pvProductExpired");
            hasPermission = PermissionCheckTab(borrowMoneyExpiredReminder, "pvBorrowMoneyExpired");
            hasPermission = PermissionCheckTab(guaranteeReceiptExpiredReminder, "pvGuaranteeReceiptExpired");

        }
        IDictionary<int, int> _permossionIDAndValues;
        IDictionary<int, int> PermossionIDAndValues
        {
            get
            {
                if (_permossionIDAndValues == null)
                {
                    int userID = CurrentUser.UserID;
                    _permossionIDAndValues = PagePermissionRepository.GetPermossionIDAndValues(userID);
                }
                return _permossionIDAndValues;
            }
        }
        private bool PermissionCheckTab(Views.Reminder.UserControls.BaseReminder reminder, string pageViewID)
        {


            if (reminder.HasPermissionCheck(PermossionIDAndValues))
            {
                if (tabStrips.SelectedTab.PageViewID == pageViewID)
                    reminder.BindData(true);
                else
                    reminder.BindData(false);
                return true;
            }
            else
            {
                reminder.BindData(false);
                tabStrips.Tabs.Remove(tabStrips.Tabs.First(x => x.PageViewID == pageViewID));
            }
            return false;
        }

        protected void tabStrips_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
        {
            BindTabStrips();
        }
    }
}