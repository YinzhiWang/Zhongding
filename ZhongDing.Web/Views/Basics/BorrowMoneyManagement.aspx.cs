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
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Web.Views.Basics
{
    public partial class BorrowMoneyManagement : WorkflowBasePage
    {
        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.BorrowMoneyManagement;
        }
        #region Members

        private IBorrowMoneyRepository _PageBorrowMoneyRepository;
        private IBorrowMoneyRepository PageBorrowMoneyRepository
        {
            get
            {
                if (_PageBorrowMoneyRepository == null)
                {
                    _PageBorrowMoneyRepository = new BorrowMoneyRepository();
                }

                return _PageBorrowMoneyRepository;
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
            this.Master.MenuItemID = (int)EMenuItem.BorrowMoneyManage;

            if (!IsPostBack)
            {
                BindBorrowMoneys();
            }

        }

        #region Private Methods

        private void BindBorrowMoneys()
        {


            //var uiSearchObj = new UISearchDropdownItem
            //{
            //    Extension = new UISearchExtension
            //    {
            //        OwnerTypeID = (int)EOwnerType.Company,
            //        CompanyID = CurrentUser.CompanyID,
            //        //AccountTypeID = (int)EAccountType.Company
            //    }
            //};

            //var BorrowMoneys = PageBorrowMoneyRepository.GetDropdownItems(uiSearchObj);
            //rcbxFromAccount.DataSource = BorrowMoneys;
            //rcbxFromAccount.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            //rcbxFromAccount.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            //rcbxFromAccount.DataBind();
            //rcbxFromAccount.SelectedIndex = 0;

        }

        private void BindBorrowMoneys(bool isNeedRebind)
        {
            UISearchBorrowMoney uiSearchObj = new UISearchBorrowMoney()
            {
                //PaymentStatusID = (int)EPaymentStatus.Paid,
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
                Status = rcbxStatus.SelectedValue.ToIntOrNull(),
                BorrowName = txtBorrowName.Text.Trim(),
                //BorrowMoneyID = rcbxFromAccount.SelectedValue.ToIntOrNull(),
                //CompanyID = CurrentUser.CompanyID
            };
            int totalRecords;

            var borrowMoneys = PageBorrowMoneyRepository.GetUIList(uiSearchObj, rgBorrowMoneys.CurrentPageIndex, rgBorrowMoneys.PageSize - 1, out totalRecords);

            rgBorrowMoneys.VirtualItemCount = totalRecords;

            rgBorrowMoneys.DataSource = borrowMoneys;

            if (isNeedRebind)
                rgBorrowMoneys.Rebind();
            BindPaymentSummary();
        }
        private void BindPaymentSummary()
        {
            UISearchBorrowMoney uiSearchObj = new UISearchBorrowMoney()
            {
                //PaymentStatusID = (int)EPaymentStatus.Paid,
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
                Status = rcbxStatus.SelectedValue.ToIntOrNull(),
                BorrowName = txtBorrowName.Text.Trim(),
            };


            UIBorrowMoneyBalance borrowMoneyBalance = PageBorrowMoneyRepository.CalculateBalance(uiSearchObj);

            lblTotalPaymentAmount.Text = "总计借款：" + borrowMoneyBalance.TotalBorrowAmount.ToString("C2")
                + "  已还款：" + borrowMoneyBalance.TotalReturnedAmount.ToString("C2");
            //lblCapitalTotalPaymentAmount.Text = totalPaymentAmount.ToString().ConvertToChineseMoney();

        }


        #endregion

        #region Events

        protected void rgBorrowMoneys_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindBorrowMoneys(false);
        }

        protected void rgBorrowMoneys_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            int? id = editableItem.GetDataKeyValue("ID").ToString().ToIntOrNull();
            if (id.BiggerThanZero())
            {

                var hasApplicationPaymen = PageApplicationPaymentRepository.GetList(x => x.WorkflowID == (int)EWorkflow.BorrowMoneyManagement && x.ApplicationID == id
                     && x.PaymentStatusID == (int)EPaymentStatus.Paid).Any();
                if (hasApplicationPaymen)
                {
                    ShowErrorMessage("此借款单已经有交易产生，不能删除");
                }
                else
                {
                    PageBorrowMoneyRepository.DeleteByID(id);
                    PageBorrowMoneyRepository.Save();
                }
            }

            rgBorrowMoneys.Rebind();
        }

        protected void rgBorrowMoneys_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            base.PermissionOptionCheckGridCreate(e.Item);
        }

        protected void rgBorrowMoneys_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            base.PermissionOptionCheckGridDelete(e.OwnerTableView.Columns);
        }

        protected void rgBorrowMoneys_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindBorrowMoneys(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

            //rcbxFromAccount.ClearSelection();
            rdpEndDate.SelectedDate = rdpBeginDate.SelectedDate = null;
            rcbxStatus.SelectedValue = string.Empty;
            txtBorrowName.Text = string.Empty;
            BindBorrowMoneys(true);
        }

        #endregion

        protected override EPermission PagePermissionID()
        {
            return EPermission.BorrowMoneyManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Edit;
        }
    }
}