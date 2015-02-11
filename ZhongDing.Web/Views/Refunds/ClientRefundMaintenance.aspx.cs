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
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Web.Extensions;

namespace ZhongDing.Web.Views.Refunds
{
    public partial class ClientRefundMaintenance : WorkflowBasePage
    {
        #region Fields

        private int? ClientSaleAppID
        {
            get
            {
                return WebUtility.GetValueFromQueryString("ClientSaleAppID");
            }
        }

        #endregion

        #region Members

        private IClientRefundApplicationRepository _PageClientRefundAppRepository;
        private IClientRefundApplicationRepository PageClientRefundAppRepository
        {
            get
            {
                if (_PageClientRefundAppRepository == null)
                    _PageClientRefundAppRepository = new ClientRefundApplicationRepository();

                return _PageClientRefundAppRepository;
            }
        }

        private IClientSaleApplicationRepository _PageClientSaleAppRepository;
        private IClientSaleApplicationRepository PageClientSaleAppRepository
        {
            get
            {
                if (_PageClientSaleAppRepository == null)
                    _PageClientSaleAppRepository = new ClientSaleApplicationRepository();

                return _PageClientSaleAppRepository;
            }
        }

        private IProductHighPriceRepository _PageProductHighPriceRepository;
        private IProductHighPriceRepository PageProductHighPriceRepository
        {
            get
            {
                if (_PageProductHighPriceRepository == null)
                    _PageProductHighPriceRepository = new ProductHighPriceRepository();

                return _PageProductHighPriceRepository;
            }
        }

        private IClientRefundAppDetailRepository _PageClientRefundAppDetailRepository;
        private IClientRefundAppDetailRepository PageClientRefundAppDetailRepository
        {
            get
            {
                if (_PageClientRefundAppDetailRepository == null)
                    _PageClientRefundAppDetailRepository = new ClientRefundAppDetailRepository();

                return _PageClientRefundAppDetailRepository;
            }
        }

        private IApplicationPaymentRepository _PageAppPaymentRepository;
        private IApplicationPaymentRepository PageAppPaymentRepository
        {
            get
            {
                if (_PageAppPaymentRepository == null)
                    _PageAppPaymentRepository = new ApplicationPaymentRepository();

                return _PageAppPaymentRepository;
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

        private IClientInfoRepository _PageClientInfoRepository;
        private IClientInfoRepository PageClientInfoRepository
        {
            get
            {
                if (_PageClientInfoRepository == null)
                    _PageClientInfoRepository = new ClientInfoRepository();

                return _PageClientInfoRepository;
            }
        }

        private IClientInfoBankAccountRepository _PageClientInfoBankAccountRepository;
        private IClientInfoBankAccountRepository PageClientInfoBankAccountRepository
        {
            get
            {
                if (_PageClientInfoBankAccountRepository == null)
                    _PageClientInfoBankAccountRepository = new ClientInfoBankAccountRepository();

                return _PageClientInfoBankAccountRepository;
            }
        }

        private ClientRefundApplication _CurrentEntity;
        private ClientRefundApplication CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageClientRefundAppRepository.GetByID(this.CurrentEntityID);

                return _CurrentEntity;
            }
        }

        private IList<int> _CanAccessUserIDs;
        private IList<int> CanAccessUserIDs
        {
            get
            {
                if (_CanAccessUserIDs == null || _CanAccessUserIDs.Count == 0)
                {
                    if (this.CurrentEntity == null)
                        _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewClientRefund);
                    else
                        _CanAccessUserIDs = PageWorkflowStatusRepository.GetCanAccessUserIDsByID((int)EWorkflow.ClientRefunds, this.CurrentEntity.WorkflowStatusID);
                }

                return _CanAccessUserIDs;
            }
        }


        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditClientRefund);

                return _CanEditUserIDs;
            }
        }

        private IList<int> _CanAuditUserIDs;
        private IList<int> CanAuditUserIDs
        {
            get
            {
                if (_CanAuditUserIDs == null)
                    _CanAuditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByIDs(new List<int>
                    {
                        (int)EWorkflowStep.AuditClientRefundByTreasurers,
                        (int)EWorkflowStep.AuditClientRefundByDeptManagers
                    });

                return _CanAuditUserIDs;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ClientRefundsManage;
            this.CurrentWorkFlowID = (int)EWorkflow.ClientRefunds;

            if (!IsPostBack)
            {
                //处理申请返款
                ApplyClientRefundApp();

                LoadCurrentEntity();
            }
        }

        #region Private Methods

        private void ApplyClientRefundApp()
        {
            if (this.ClientSaleAppID.HasValue && this.ClientSaleAppID > 0
                && (!this.CurrentEntityID.HasValue || (this.CurrentEntityID.HasValue && this.CurrentEntityID <= 0)))
            {
                var clientSaleApp = PageClientSaleAppRepository.GetByID(this.ClientSaleAppID);

                if (clientSaleApp != null && !clientSaleApp.ClientRefundApplication.Any(x => x.IsDeleted == false))
                {
                    if (clientSaleApp.IsGuaranteed && !clientSaleApp.GuaranteeLog.Any(x => x.IsDeleted == false && x.IsReceipted))
                    {
                        this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                        this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                        this.Master.BaseNotification.AutoCloseDelay = 1000;
                        this.Master.BaseNotification.Show("该担保模式订单的担保款还未收回，不能申请返款");
                    }

                    if (clientSaleApp.SalesOrderApplication != null)
                    {
                        var clientRefundApp = new ClientRefundApplication
                        {
                            ClientSaleAppID = clientSaleApp.ID,
                            CompanyID = clientSaleApp.CompanyID,
                            ClientUserID = clientSaleApp.ClientUserID,
                            ClientCompanyID = clientSaleApp.ClientCompanyID,
                            SaleOrderTypeID = clientSaleApp.SalesOrderApplication.SaleOrderTypeID,
                            DeliveryModeID = clientSaleApp.DeliveryModeID,
                            OrderCode = clientSaleApp.SalesOrderApplication.OrderCode,
                            OrderDate = clientSaleApp.SalesOrderApplication.OrderDate,
                            IsStop = clientSaleApp.SalesOrderApplication.IsStop,
                            WorkflowStatusID = (int)EWorkflowStatus.TemporarySave,
                        };

                        PageClientRefundAppRepository.Add(clientRefundApp);

                        foreach (var item in clientSaleApp.SalesOrderApplication.SalesOrderAppDetail
                            .Where(x => x.IsDeleted == false && x.Warehouse != null && x.Warehouse.SaleTypeID == (int)ESaleType.HighPrice))
                        {
                            var actualSalePrice = 0M;
                            var clientTaxRatio = 0M;

                            var productHighPrice = PageProductHighPriceRepository.GetList(x => x.IsDeleted == false
                                && x.ProductID == item.ProductID && x.ProductSpecificationID == item.ProductSpecificationID)
                                .FirstOrDefault();

                            if (productHighPrice != null)
                            {
                                actualSalePrice = productHighPrice.ActualSalePrice.HasValue
                                    ? productHighPrice.ActualSalePrice.Value : 0M;

                                clientTaxRatio = productHighPrice.ClientTaxRatio.HasValue
                                    ? productHighPrice.ClientTaxRatio.Value : 0M;
                            }

                            var clientRefundAppDetail = new ClientRefundAppDetail
                            {
                                WarehouseID = item.WarehouseID.Value,
                                ProductID = item.ProductID,
                                ProductSpecificationID = item.ProductSpecificationID,
                                Count = item.Count,
                                HighPrice = item.SalesPrice,
                                ActualSalePrice = actualSalePrice,
                                ClientTaxRatio = clientTaxRatio,
                                TotalSalesAmount = item.TotalSalesAmount,
                                RefundAmount = (item.SalesPrice - (item.SalesPrice - actualSalePrice) * clientTaxRatio - actualSalePrice) * item.Count
                            };

                            clientRefundApp.ClientRefundAppDetail.Add(clientRefundAppDetail);
                        }

                        clientRefundApp.RefundAmount = clientRefundApp.ClientRefundAppDetail.Sum(x => x.RefundAmount);

                        PageClientRefundAppRepository.Save();

                        Response.Redirect("ClientRefundMaintenance.aspx?EntityID=" + clientRefundApp.ID, true);
                    }
                }
                else
                {
                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                    this.Master.BaseNotification.AutoCloseDelay = 1000;
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_REDIRECT);
                }
            }
        }

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntity != null)
            {
                hdnCurrentEntityID.Value = this.CurrentEntity.ID.ToString();

                lblOrderCode.Text = CurrentEntity.OrderCode;
                lblCreateBy.Text = PageUsersRepository.GetUserFullNameByID(this.CurrentEntity.CreatedBy.HasValue
                    ? this.CurrentEntity.CreatedBy.Value : GlobalConst.INVALID_INT);

                lblOrderDate.Text = CurrentEntity.OrderDate.ToString("yyyy/MM/dd");
                lblSalesOrderType.Text = CurrentEntity.SaleOrderType.TypeName;
                lblClientUser.Text = CurrentEntity.ClientUser.ClientName;
                lblClientCompany.Text = CurrentEntity.ClientCompany.Name;

                if (CurrentEntity.DeliveryModeID.HasValue && CurrentEntity.DeliveryModeID > 0)
                {
                    if (CurrentEntity.DeliveryModeID == (int)EDeliveryMode.ReceiptedDelivery)
                        lblDeliveryMode.Text = GlobalConst.DeliveryModes.RECEIPTED_DELIVERY;
                    else if (CurrentEntity.DeliveryModeID == (int)EDeliveryMode.GuaranteeDelivery)
                        lblDeliveryMode.Text = GlobalConst.DeliveryModes.GUARANTEE_DELIVERY;
                }

                if (CurrentEntity.IsStop)
                    lblIsStop.Text = GlobalConst.BoolChineseDescription.TRUE;
                else
                    lblIsStop.Text = GlobalConst.BoolChineseDescription.FALSE;

                EWorkflowStatus workfolwStatus = (EWorkflowStatus)this.CurrentEntity.WorkflowStatusID;

                if (CanEditUserIDs.Contains(CurrentUser.UserID))
                {
                    btnSave.Visible = true;
                    btnSubmit.Visible = false;
                    ShowAuditControls(false);
                }
                else
                {
                    switch (workfolwStatus)
                    {
                        case EWorkflowStatus.TemporarySave:
                        case EWorkflowStatus.ReturnBasicInfo:
                            #region 暂存和基础信息退回（订单创建者才能修改）

                            if (CurrentUser.UserID == this.CurrentEntity.CreatedBy
                                || this.CanEditUserIDs.Contains(CurrentUser.UserID))
                                ShowSaveButtons(true);
                            else
                                DisabledBasicInfoControls();

                            btnAudit.Visible = false;
                            btnReturn.Visible = false;
                            divAudit.Visible = false;
                            divAppPayments.Visible = false;

                            #endregion

                            break;
                        case EWorkflowStatus.Submit:
                        case EWorkflowStatus.ApprovedByTreasurers:
                            #region 已提交或需进入下一级审核，待审核

                            DisabledBasicInfoControls();

                            divAppPayments.Visible = false;

                            if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                                ShowAuditControls(true);
                            else
                                ShowAuditControls(false);

                            #endregion

                            break;

                        case EWorkflowStatus.ApprovedByDeptManagers:
                            #region 审核通过，支付

                            DisabledBasicInfoControls();
                            ShowAuditControls(false);

                            if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                            {
                                btnSave.Visible = true;
                                btnSubmit.Visible = false;
                                btnPay.Visible = true;
                            }
                            else
                                rgAppPayments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                            #endregion
                            break;

                        case EWorkflowStatus.Paid:
                            #region 已支付的订单，不能修改

                            DisabledBasicInfoControls();

                            ShowSaveButtons(false);

                            ShowAuditControls(false);

                            rgAppPayments.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;

                            #endregion
                            break;
                    }
                }
            }
            else
            {
                InitDefaultData();

                if (!this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                {
                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                    this.Master.BaseNotification.AutoCloseDelay = 1000;
                    this.Master.BaseNotification.Show("您没有权限申请客户返款");
                }
            }
        }

        /// <summary>
        /// 显示或隐藏保存和提交按钮
        /// </summary>
        private void ShowSaveButtons(bool isShow)
        {
            btnSave.Visible = isShow;
            btnSubmit.Visible = isShow;
        }

        /// <summary>
        /// 显示或隐藏审核相关控件
        /// </summary>
        private void ShowAuditControls(bool isShow)
        {
            divAudit.Visible = isShow;
            btnAudit.Visible = isShow;
            btnReturn.Visible = isShow;
        }

        /// <summary>
        /// 禁用基础信息相关控件
        /// </summary>
        private void DisabledBasicInfoControls()
        {
            txtComment.Enabled = false;

            btnSave.Visible = false;
            btnSubmit.Visible = false;
        }

        /// <summary>
        /// 初始化默认值
        /// </summary>
        private void InitDefaultData()
        {
            btnSubmit.Visible = false;
            btnAudit.Visible = false;
            btnReturn.Visible = false;
            divComments.Visible = false;
            divAppPayments.Visible = false;
        }

        #endregion

        #region Grid Events

        #region App Notes

        protected void rgAppNotes_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchApplicationNote()
            {
                WorkflowID = this.CurrentWorkFlowID,
                NoteTypeID = (int)EAppNoteType.Comment,
                ApplicationID = this.CurrentEntityID.HasValue
                ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            var appNotes = PageAppNoteRepository.GetUIList(uiSearchObj);

            rgAppNotes.DataSource = appNotes;
        }

        protected void rgAuditNotes_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchApplicationNote()
            {
                WorkflowID = this.CurrentWorkFlowID,
                NoteTypeID = (int)EAppNoteType.AuditOpinion,
                ApplicationID = this.CurrentEntityID.HasValue
                ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            var appNotes = PageAppNoteRepository.GetUIList(uiSearchObj);

            rgAuditNotes.DataSource = appNotes;
        }

        #endregion

        #region rgOrderProducts

        protected void rgOrderProducts_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchClientRefundAppDetail
            {
                ClientRefundAppID = this.CurrentEntityID.HasValue
                ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            int totalRecords;

            var uiEntities = PageClientRefundAppDetailRepository.GetUIList(uiSearchObj, rgOrderProducts.CurrentPageIndex, rgOrderProducts.PageSize, out totalRecords);

            rgOrderProducts.VirtualItemCount = totalRecords;

            rgOrderProducts.DataSource = uiEntities;
        }

        protected void rgOrderProducts_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            if (CanEditUserIDs.Contains(CurrentUser.UserID)
                || (CanAuditUserIDs.Contains(CurrentUser.UserID)
                    && (CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.Submit
                    || CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ApprovedByTreasurers)))
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = true;
            else
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = false;
        }

        protected void rgOrderProducts_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.EditItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;

                UIClientRefundAppDetail uiEntity = null;

                if (e.Item.ItemIndex >= 0)
                    uiEntity = (UIClientRefundAppDetail)gridDataItem.DataItem;

                var txtActualSalePrice = (RadNumericTextBox)e.Item.FindControl("txtActualSalePrice");
                if (txtActualSalePrice != null && uiEntity != null)
                    txtActualSalePrice.DbValue = uiEntity.ActualSalePrice;
            }
        }

        protected void rgOrderProducts_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (IsValid)
            {
                var editableItem = ((GridEditableItem)e.Item);
                String sid = editableItem.GetDataKeyValue("ID").ToString();

                int id = 0;
                if (int.TryParse(sid, out id))
                {
                    if (e.Item is GridDataItem)
                    {
                        GridDataItem dataItem = e.Item as GridDataItem;

                        var clientRefundAppDetail = CurrentEntity.ClientRefundAppDetail.FirstOrDefault(x => x.ID == id);

                        if (clientRefundAppDetail != null)
                        {
                            var txtActualSalePrice = (RadNumericTextBox)e.Item.FindControl("txtActualSalePrice");
                            if (txtActualSalePrice != null)
                                clientRefundAppDetail.ActualSalePrice = (decimal)(txtActualSalePrice.Value.HasValue ? txtActualSalePrice.Value.Value : 0D);

                            clientRefundAppDetail.RefundAmount = (clientRefundAppDetail.HighPrice
                                - (clientRefundAppDetail.HighPrice - clientRefundAppDetail.ActualSalePrice)
                                    * clientRefundAppDetail.ClientTaxRatio
                                - clientRefundAppDetail.ActualSalePrice) * clientRefundAppDetail.Count;


                            CurrentEntity.RefundAmount = CurrentEntity.ClientRefundAppDetail
                            .Sum(x => x.RefundAmount);

                            PageClientRefundAppRepository.Save();
                        }
                    }

                    rgOrderProducts.Rebind();
                }
            }
        }

        #endregion

        #region rgAppPayments

        protected void rgAppPayments_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchApplicationPayment
            {
                WorkflowID = this.CurrentWorkFlowID,
                ApplicationID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            int totalRecords;

            var appPayments = PageAppPaymentRepository.GetUIList(uiSearchObj, rgAppPayments.CurrentPageIndex, rgAppPayments.PageSize, out totalRecords);

            rgAppPayments.DataSource = appPayments;
            rgAppPayments.VirtualItemCount = totalRecords;
        }

        protected void rgAppPayments_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                || (this.CanAccessUserIDs.Contains(CurrentUser.UserID)
                    && this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ApprovedByDeptManagers)))
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = true;
            }
            else
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = false;
            }
        }

        protected void rgAppPayments_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.EditItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;

                UIApplicationPayment uiEntity = null;

                if (e.Item.ItemIndex >= 0)
                    uiEntity = (UIApplicationPayment)gridDataItem.DataItem;

                var rcbxFromAccount = (RadComboBox)e.Item.FindControl("rcbxFromAccount");

                if (rcbxFromAccount != null)
                {
                    var uiSearchObj = new UISearchDropdownItem
                    {
                        Extension = new UISearchExtension
                        {
                            OwnerTypeID = (int)EOwnerType.Company,
                            CompanyID = CurrentEntity.CompanyID
                        }
                    };

                    if (e.Item.ItemIndex < 0)
                    {
                        var excludeItemValues = PageAppPaymentRepository
                        .GetList(x => x.WorkflowID == CurrentWorkFlowID && x.ApplicationID == this.CurrentEntityID)
                        .Select(x => x.FromBankAccountID.HasValue ? x.FromBankAccountID.Value : GlobalConst.INVALID_INT)
                        .ToList();

                        if (excludeItemValues.Count > 0)
                            uiSearchObj.ExcludeItemValues = excludeItemValues;
                    }

                    var bankAccounts = PageBankAccountRepository.GetDropdownItems(uiSearchObj);
                    rcbxFromAccount.DataSource = bankAccounts;
                    rcbxFromAccount.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                    rcbxFromAccount.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                    rcbxFromAccount.DataBind();

                    if (uiEntity != null)
                        rcbxFromAccount.SelectedValue = uiEntity.FromBankAccountID.ToString();
                }

                var rcbxToAccount = (RadComboBox)e.Item.FindControl("rcbxToAccount");

                if (rcbxToAccount != null)
                {
                    var uiSearchObj = new UISearchDropdownItem
                    {
                        Extension = new UISearchExtension
                        {
                            OwnerTypeID = (int)EOwnerType.Client,
                        }
                    };

                    if (e.Item.ItemIndex < 0)
                    {
                        var excludeItemValues = PageAppPaymentRepository
                            .GetList(x => x.WorkflowID == CurrentWorkFlowID && x.ApplicationID == this.CurrentEntityID)
                            .Select(x => x.ToBankAccountID.HasValue ? x.ToBankAccountID.Value : GlobalConst.INVALID_INT)
                            .ToList();

                        if (excludeItemValues.Count > 0)
                            uiSearchObj.ExcludeItemValues = excludeItemValues;
                    }

                    if (CurrentEntity.ClientUserID > 0 && CurrentEntity.ClientCompanyID > 0)
                    {
                        var clientInfo = PageClientInfoRepository.GetByConditions(CurrentEntity.ClientUserID, CurrentEntity.ClientCompanyID);

                        if (clientInfo != null)
                        {
                            var includeItemValues = clientInfo.ClientInfoBankAccount.Where(x => x.IsDeleted == false)
                                .Select(x => x.BankAccountID.HasValue ? x.BankAccountID.Value : GlobalConst.INVALID_INT)
                                .ToList();

                            if (includeItemValues.Count > 0)
                                uiSearchObj.IncludeItemValues = includeItemValues;
                        }
                    }

                    var bankAccounts = PageBankAccountRepository.GetDropdownItems(uiSearchObj);
                    rcbxToAccount.DataSource = bankAccounts;
                    rcbxToAccount.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                    rcbxToAccount.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                    rcbxToAccount.DataBind();

                    if (uiEntity != null)
                        rcbxToAccount.SelectedValue = uiEntity.ToBankAccountID.ToString();
                }

                var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                if (txtAmount != null && uiEntity != null)
                    txtAmount.DbValue = uiEntity.Amount;
            }
        }

        protected void rgAppPayments_InsertCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                GridDataItem dataItem = e.Item as GridDataItem;

                ApplicationPayment appPayment = new ApplicationPayment();

                var rcbxFromAccount = (RadComboBox)e.Item.FindControl("rcbxFromAccount");

                if (!string.IsNullOrEmpty(rcbxFromAccount.SelectedValue))
                {
                    int fromAccountID;
                    if (int.TryParse(rcbxFromAccount.SelectedValue, out fromAccountID))
                        appPayment.FromBankAccountID = fromAccountID;
                    appPayment.FromAccount = rcbxFromAccount.SelectedItem.Text;
                }

                var rcbxToAccount = (RadComboBox)e.Item.FindControl("rcbxToAccount");

                if (!string.IsNullOrEmpty(rcbxToAccount.SelectedValue))
                {
                    int toAccountID;
                    if (int.TryParse(rcbxToAccount.SelectedValue, out toAccountID))
                        appPayment.ToBankAccountID = toAccountID;
                    appPayment.ToAccount = rcbxToAccount.SelectedItem.Text;
                }

                var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                appPayment.Amount = (decimal?)txtAmount.Value;

                appPayment.ApplicationID = this.CurrentEntityID.Value;
                appPayment.WorkflowID = this.CurrentWorkFlowID;
                appPayment.PaymentStatusID = (int)EPaymentStatus.ToBePaid;
                appPayment.PaymentTypeID = (int)EPaymentType.Expend;

                PageAppPaymentRepository.Add(appPayment);

                PageAppPaymentRepository.Save();
            }

            rgAppPayments.Rebind();
        }

        protected void rgAppPayments_UpdateCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            var editableItem = ((GridEditableItem)e.Item);
            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;

                    var appPayment = PageAppPaymentRepository.GetByID(id);

                    var rcbxFromAccount = (RadComboBox)e.Item.FindControl("rcbxFromAccount");

                    if (!string.IsNullOrEmpty(rcbxFromAccount.SelectedValue))
                    {
                        int fromAccountID;
                        if (int.TryParse(rcbxFromAccount.SelectedValue, out fromAccountID))
                            appPayment.FromBankAccountID = fromAccountID;
                        appPayment.FromAccount = rcbxFromAccount.SelectedItem.Text;
                    }

                    var rcbxToAccount = (RadComboBox)e.Item.FindControl("rcbxToAccount");

                    if (!string.IsNullOrEmpty(rcbxToAccount.SelectedValue))
                    {
                        int toAccountID;
                        if (int.TryParse(rcbxToAccount.SelectedValue, out toAccountID))
                            appPayment.ToBankAccountID = toAccountID;
                        appPayment.ToAccount = rcbxToAccount.SelectedItem.Text;
                    }

                    var txtAmount = (RadNumericTextBox)e.Item.FindControl("txtAmount");
                    appPayment.Amount = (decimal?)txtAmount.Value;

                    PageAppPaymentRepository.Save();

                    rgAppPayments.Rebind();
                }
            }
        }

        #endregion

        #endregion

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtComment.Text.Trim()))
            {
                var appNote = new ApplicationNote();
                appNote.WorkflowID = CurrentWorkFlowID;
                appNote.WorkflowStepID = (int)EWorkflowStep.NewClientRefund;
                appNote.NoteTypeID = (int)EAppNoteType.Comment;
                appNote.ApplicationID = CurrentEntity.ID;
                appNote.Note = txtComment.Text.Trim();

                PageAppNoteRepository.Add(appNote);

                PageAppNoteRepository.Save();
            }

            this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (this.CurrentEntity != null)
            {
                this.CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.Submit;

                PageClientRefundAppRepository.Save();
            }

            if (!string.IsNullOrEmpty(txtComment.Text.Trim()))
            {
                var appNote = new ApplicationNote();
                appNote.WorkflowID = CurrentWorkFlowID;
                appNote.WorkflowStepID = (int)EWorkflowStep.NewClientRefund;
                appNote.NoteTypeID = (int)EAppNoteType.Comment;
                appNote.ApplicationID = CurrentEntity.ID;
                appNote.Note = txtComment.Text.Trim();

                PageAppNoteRepository.Add(appNote);

                PageAppNoteRepository.Save();
            }

            this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
        }

        protected void btnAudit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAuditComment.Text.Trim()))
                cvAuditComment.IsValid = false;

            if (!IsValid) return;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IClientRefundApplicationRepository clientRefundAppRepository = new ClientRefundApplicationRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    clientRefundAppRepository.SetDbModel(db);
                    appNoteRepository.SetDbModel(db);

                    var currentEntity = clientRefundAppRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        var appNote = new ApplicationNote();
                        appNote.WorkflowID = CurrentWorkFlowID;
                        appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;
                        appNote.ApplicationID = currentEntity.ID;
                        appNote.Note = txtAuditComment.Text.Trim();
                        appNoteRepository.Add(appNote);

                        switch (currentEntity.WorkflowStatusID)
                        {
                            case (int)EWorkflowStatus.Submit:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditClientRefundByTreasurers;
                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByTreasurers;

                                break;

                            case (int)EWorkflowStatus.ApprovedByTreasurers:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditClientRefundByDeptManagers;
                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByDeptManagers;

                                break;
                        }

                        unitOfWork.SaveChanges();

                        this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                        this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                    }
                }
            }
        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAuditComment.Text.Trim()))
                cvAuditComment.IsValid = false;

            if (!IsValid) return;

            if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IClientRefundApplicationRepository clientRefundAppRepository = new ClientRefundApplicationRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    clientRefundAppRepository.SetDbModel(db);
                    appNoteRepository.SetDbModel(db);

                    var currentEntity = clientRefundAppRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        var appNote = new ApplicationNote();
                        appNote.WorkflowID = CurrentWorkFlowID;
                        appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;
                        appNote.ApplicationID = currentEntity.ID;
                        appNote.Note = txtAuditComment.Text.Trim();
                        appNoteRepository.Add(appNote);

                        switch (currentEntity.WorkflowStatusID)
                        {
                            case (int)EWorkflowStatus.Submit:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditClientRefundByTreasurers;
                                break;

                            case (int)EWorkflowStatus.ApprovedByTreasurers:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditClientRefundByDeptManagers;
                                break;
                        }

                        currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ReturnBasicInfo;

                        unitOfWork.SaveChanges();

                        this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                        this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                    }
                }
            }
        }

        protected void btnPay_Click(object sender, EventArgs e)
        {
            if (CurrentEntity != null)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IClientRefundApplicationRepository clientRefundAppRepository = new ClientRefundApplicationRepository();
                    IApplicationPaymentRepository appPaymentRepository = new ApplicationPaymentRepository();

                    clientRefundAppRepository.SetDbModel(db);
                    appPaymentRepository.SetDbModel(db);

                    var currentEntity = clientRefundAppRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {

                        var appPayments = appPaymentRepository.GetList(x => x.WorkflowID == CurrentWorkFlowID
                            && x.ApplicationID == CurrentEntity.ID).ToList();

                        var totalRefundAmount = CurrentEntity.ClientRefundAppDetail.Sum(x => x.RefundAmount);

                        var totalPayAmount = appPayments.Sum(x => x.Amount.HasValue ? x.Amount.Value : 0M);

                        if (totalPayAmount != totalRefundAmount)
                        {
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                            this.Master.BaseNotification.AutoCloseDelay = 1000;
                            this.Master.BaseNotification.Show("支付总额不等于应返款总额，不能确认支付");
                        }
                        else
                        {
                            foreach (var item in appPayments)
                            {
                                item.PaymentStatusID = (int)EPaymentStatus.Paid;
                            }

                            currentEntity.WorkflowStatusID = (int)EWorkflowStatus.Paid;

                            unitOfWork.SaveChanges();

                            this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                        }
                    }
                }
            }
        }
    }
}