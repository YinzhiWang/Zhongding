using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Common.Extension;

namespace ZhongDing.Web.Views.Basics
{
    public partial class MoneyManagement : BasePage
    {
        #region Members

        private IBankAccountRepository _PageBankAccountRepository;
        private IBankAccountRepository PageBankAccountRepository
        {
            get
            {
                if (_PageBankAccountRepository == null)
                {
                    _PageBankAccountRepository = new BankAccountRepository();
                }

                return _PageBankAccountRepository;
            }
        }
        private IApplicationPaymentRepository _PagePageApplicationPaymentRepository;
        private IApplicationPaymentRepository PageApplicationPaymentRepository
        {
            get
            {
                if (_PagePageApplicationPaymentRepository == null)
                {
                    _PagePageApplicationPaymentRepository = new ApplicationPaymentRepository();
                }

                return _PagePageApplicationPaymentRepository;
            }
        }


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.MoneyManage;

            if (!IsPostBack)
            {
                BindBankAccounts();
            }

        }

        #region Private Methods

        private void BindBankAccounts()
        {


            var uiSearchObj = new UISearchDropdownItem
            {
                Extension = new UISearchExtension
                {
                    OwnerTypeID = (int)EOwnerType.Company,
                    CompanyID = CurrentUser.CompanyID,
                    //AccountTypeID = (int)EAccountType.Company
                }
            };

            var bankAccounts = PageBankAccountRepository.GetDropdownItems(uiSearchObj);
            rcbxFromAccount.DataSource = bankAccounts;
            rcbxFromAccount.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxFromAccount.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxFromAccount.DataBind();
            rcbxFromAccount.SelectedIndex = 0;

        }

        private void BindBankAccounts(bool isNeedRebind)
        {
            UISearchApplicationPayment uiSearchObj = new UISearchApplicationPayment()
            {
                PaymentStatusID = (int)EPaymentStatus.Paid,
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
                BankAccountID = rcbxFromAccount.SelectedValue.ToIntOrNull(),
                CompanyID = CurrentUser.CompanyID
            };
            if (!uiSearchObj.BankAccountID.HasValue)
            {
                var searchObj = new UISearchDropdownItem
                {
                    Extension = new UISearchExtension
                    {
                        OwnerTypeID = (int)EOwnerType.Company,
                        CompanyID = CurrentUser.CompanyID
                    }
                };
                var bankAccounts = PageBankAccountRepository.GetDropdownItems(searchObj);
                uiSearchObj.IncludeBankAccountIDs = bankAccounts.Select(x => x.ItemValue).ToList();
            }

            int totalRecords;

            var companies = PageApplicationPaymentRepository.GetUIListForMoneyManagement(uiSearchObj, rgApplicationPayments.CurrentPageIndex, rgApplicationPayments.PageSize - 1, out totalRecords);

            rgApplicationPayments.VirtualItemCount = totalRecords;

            rgApplicationPayments.DataSource = companies;

            if (isNeedRebind)
                rgApplicationPayments.Rebind();
        }


        #endregion

        #region Events

        protected void rgApplicationPayments_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindBankAccounts(false);
        }

        protected void rgApplicationPayments_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                PageBankAccountRepository.DeleteByID(id);
                PageBankAccountRepository.Save();
            }

            rgApplicationPayments.Rebind();
        }

        protected void rgApplicationPayments_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            base.PermissionOptionCheckGridCreate(e.Item);
        }

        protected void rgApplicationPayments_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            //base.PermissionOptionCheckGridDelete(e.OwnerTableView.Columns);
        }

        protected void rgApplicationPayments_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindBankAccounts(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

            //rcbxFromAccount.ClearSelection();
            rdpEndDate.SelectedDate = rdpBeginDate.SelectedDate = null;
            BindBankAccounts(true);
        }

        #endregion

        protected override EPermission PagePermissionID()
        {
            return EPermission.MoneyManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Edit;
        }
    }
}