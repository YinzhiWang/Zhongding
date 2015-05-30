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
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Web.Extensions;

namespace ZhongDing.Web.Views.Settlements.Editors
{
    public partial class DBClientSettleBonusPayment : WorkflowBasePage
    {
        #region Fields

        /// <summary>
        /// 所属的实体ID
        /// </summary>
        /// <value>The owner entity ID.</value>
        private int? OwnerEntityID
        {
            get
            {
                return WebUtility.GetIntFromQueryString("OwnerEntityID");
            }
        }

        #endregion

        #region Members

        private IDBClientSettleBonusRepository _PageDBClientSettleBonusRepository;
        private IDBClientSettleBonusRepository PageDBClientSettleBonusRepository
        {
            get
            {
                if (_PageDBClientSettleBonusRepository == null)
                    _PageDBClientSettleBonusRepository = new DBClientSettleBonusRepository();

                return _PageDBClientSettleBonusRepository;
            }
        }

        private IDBClientSettlementRepository _PageDBClientSettlementRepository;
        private IDBClientSettlementRepository PageDBClientSettlementRepository
        {
            get
            {
                if (_PageDBClientSettlementRepository == null)
                    _PageDBClientSettlementRepository = new DBClientSettlementRepository();

                return _PageDBClientSettlementRepository;
            }
        }

        private IBankAccountRepository _PageBankAccountRepository;
        private IBankAccountRepository PageBankAccountRepository
        {
            get
            {
                if (_PageBankAccountRepository == null)
                    _PageBankAccountRepository = new BankAccountRepository();

                return _PageBankAccountRepository;
            }
        }

        private DBClientSettlement _CurrentOwnerEntity;
        private DBClientSettlement CurrentOwnerEntity
        {
            get
            {
                if (_CurrentOwnerEntity == null)
                    if (this.OwnerEntityID.HasValue && this.OwnerEntityID > 0)
                        _CurrentOwnerEntity = PageDBClientSettlementRepository.GetByID(this.OwnerEntityID);

                return _CurrentOwnerEntity;
            }
        }

        private IList<int> _CanPayUserIDs;
        private IList<int> CanPayUserIDs
        {
            get
            {
                if (_CanPayUserIDs == null)
                    _CanPayUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.PayDBClientSettleBonus);

                return _CanPayUserIDs;
            }
        }

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditDBClientSettleBonus);

                return _CanEditUserIDs;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.DBClientSettleBonus;
        }
        protected override EWorkflow PagePermissionWorkflowID()
        {
            return EWorkflow.DBClientSettleBonus;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!hasPermission())
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_NO_PERMISSION_CLOSE_WIN);

                return;
            }

            if (!IsPostBack)
            {
                hdnGridClientID.Value = GridClientID;

                BindBankAccount();
            }
        }

        #region Private Methods

        private void BindBankAccount()
        {
            var uiSearchObj = new UISearchDropdownItem
            {
                Extension = new UISearchExtension
                {
                    OwnerTypeID = (int)EOwnerType.Company,
                    CompanyID = CurrentUser.CompanyID
                }
            };

            var bankAccounts = PageBankAccountRepository.GetDropdownItems(uiSearchObj);
            rcbxFromAccount.DataSource = bankAccounts;
            rcbxFromAccount.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxFromAccount.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxFromAccount.DataBind();
        }

        private bool hasPermission()
        {
            if (this.CurrentOwnerEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                    || this.CanPayUserIDs.Contains(CurrentUser.UserID))
                && (this.CurrentOwnerEntity.WorkflowStatusID == (int)EWorkflowStatus.ApprovedByDeptManagers))
            {
                return true;
            }
            return false;
        }

        #endregion

        protected void rgDBClientSettleBonus_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchDBClientSettleBonus
            {
                DBClientSettlementID = this.OwnerEntityID.HasValue
                ? OwnerEntityID.Value : GlobalConst.INVALID_INT
            };

            var uiEntities = PageDBClientSettleBonusRepository.GetNeedPayUIList(uiSearchObj);

            rgDBClientSettleBonus.DataSource = uiEntities;
        }

        protected void rgDBClientSettleBonus_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgDBClientSettleBonus_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            var selectedItems = rgDBClientSettleBonus.SelectedItems;

            if (selectedItems.Count == 0)
            {
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show("请选择要支付的大包提成");

                return;
            }

            if (this.CurrentOwnerEntity != null)
            {
                var payDate = rdpPayDate.SelectedDate;
                var fromBankAccountID = Convert.ToInt32(rcbxFromAccount.SelectedValue);
                var strFromBankAccount = rcbxFromAccount.SelectedItem.Text;

                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IDBClientSettleBonusRepository dbClientSettleBonusRepository = new DBClientSettleBonusRepository();
                    IApplicationPaymentRepository appPaymentRepository = new ApplicationPaymentRepository();

                    dbClientSettleBonusRepository.SetDbModel(db);
                    appPaymentRepository.SetDbModel(db);

                    foreach (var item in selectedItems)
                    {
                        var editableItem = ((GridEditableItem)item);
                        int id = Convert.ToInt32(editableItem.GetDataKeyValue("ID").ToString());

                        decimal totalPayAmount;
                        decimal.TryParse(editableItem.GetDataKeyValue("TotalPayAmount").ToString(), out totalPayAmount);

                        var curDBClientSettleBonus = dbClientSettleBonusRepository.GetByID(id);

                        if (curDBClientSettleBonus != null)
                        {
                            ApplicationPayment appPayment = new ApplicationPayment();

                            appPayment.PayDate = payDate;
                            appPayment.FromBankAccountID = fromBankAccountID;
                            appPayment.FromAccount = strFromBankAccount;
                            appPayment.Amount = totalPayAmount;

                            var clientBankAccount = curDBClientSettleBonus.DBClientBonus.ClientUser.BankAccount;
                            if (clientBankAccount != null)
                            {
                                appPayment.ToBankAccountID = clientBankAccount.ID;
                                appPayment.ToAccount = clientBankAccount.AccountName + " "
                                    + clientBankAccount.BankBranchName + "" + clientBankAccount.Account;
                            }

                            var txtFee = (RadNumericTextBox)editableItem.FindControl("txtFee");
                            decimal? fee = (decimal?)txtFee.Value.Value;
                            appPayment.Fee = fee;

                            appPayment.ApplicationID = CurrentOwnerEntity.ID;
                            appPayment.WorkflowID = this.CurrentWorkFlowID;
                            appPayment.PaymentStatusID = (int)EPaymentStatus.Paid;
                            appPayment.PaymentTypeID = (int)EPaymentType.Expend;
                            appPayment.Comment = "支付对应大包提成（DBClientBonus.ID）:" + curDBClientSettleBonus.DBClientBonus.ID;

                            appPaymentRepository.Add(appPayment);

                            curDBClientSettleBonus.LastModifiedBy = CurrentUser.UserID;
                            curDBClientSettleBonus.LastModifiedOn = DateTime.Now;
                            curDBClientSettleBonus.DBClientBonus.IsSettled = true;
                            curDBClientSettleBonus.DBClientBonus.SettledDate = DateTime.Now;
                        }
                    }

                    unitOfWork.SaveChanges();

                    this.Master.BaseNotification.OnClientHidden = "onClientHidden";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_CLOSE_WIN);

                }

            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "onError";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_CLOSE_WIN);

                return;
            }
        }
    }
}