using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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

namespace ZhongDing.Web.Views.Sales
{
    public partial class ClientSaleAppMaintenance : WorkflowBasePage
    {
        #region Members

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

        private ISalesOrderAppDetailRepository _PageSalesOrderAppDetailRepository;
        private ISalesOrderAppDetailRepository PageSalesOrderAppDetailRepository
        {
            get
            {
                if (_PageSalesOrderAppDetailRepository == null)
                    _PageSalesOrderAppDetailRepository = new SalesOrderAppDetailRepository();

                return _PageSalesOrderAppDetailRepository;
            }
        }

        private IClientUserRepository _PageClientUserRepository;
        private IClientUserRepository PageClientUserRepository
        {
            get
            {
                if (_PageClientUserRepository == null)
                    _PageClientUserRepository = new ClientUserRepository();

                return _PageClientUserRepository;
            }
        }

        private IClientCompanyRepository _PageClientCompanyRepository;
        private IClientCompanyRepository PageClientCompanyRepository
        {
            get
            {
                if (_PageClientCompanyRepository == null)
                    _PageClientCompanyRepository = new ClientCompanyRepository();

                return _PageClientCompanyRepository;
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

        private IClientInfoContactRepository _PageClientInfoContactRepository;
        private IClientInfoContactRepository PageClientInfoContactRepository
        {
            get
            {
                if (_PageClientInfoContactRepository == null)
                    _PageClientInfoContactRepository = new ClientInfoContactRepository();

                return _PageClientInfoContactRepository;
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

        private ISaleOrderTypeRepository _PageSaleOrderTypeRepository;
        private ISaleOrderTypeRepository PageSaleOrderTypeRepository
        {
            get
            {
                if (_PageSaleOrderTypeRepository == null)
                    _PageSaleOrderTypeRepository = new SaleOrderTypeRepository();

                return _PageSaleOrderTypeRepository;
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

        private int? SaleOrderTypeID
        {
            get
            {
                if (this.CurrentEntity == null)
                    return WebUtility.GetIntFromQueryString("SaleOrderTypeID");
                else
                    return CurrentEntity.SalesOrderApplication == null
                        ? GlobalConst.INVALID_INT : CurrentEntity.SalesOrderApplication.SaleOrderTypeID;
            }
        }

        private ClientSaleApplication _CurrentEntity;
        private ClientSaleApplication CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageClientSaleAppRepository.GetByID(this.CurrentEntityID);

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
                        _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewClientOrder);
                    else
                        _CanAccessUserIDs = PageWorkflowStatusRepository.GetCanAccessUserIDsByID(this.CurrentWorkFlowID, this.CurrentEntity.WorkflowStatusID);
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
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditClientOrder);

                return _CanEditUserIDs;
            }
        }

        private IList<int> _CanStopUserIDs;
        private IList<int> CanStopUserIDs
        {
            get
            {
                if (_CanStopUserIDs == null)
                    _CanStopUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.StopClientOrder);

                return _CanStopUserIDs;
            }
        }

        private IList<int> _CanAuditUserIDs;
        private IList<int> CanAuditUserIDs
        {
            get
            {
                if (_CanAuditUserIDs == null)
                    _CanAuditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.AuditClientOrder);

                return _CanAuditUserIDs;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.ClientOrder;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ClientOrderManage;

            if (this.SaleOrderTypeID.HasValue && this.SaleOrderTypeID > 0
                && (this.SaleOrderTypeID == (int)ESaleOrderType.AttractBusinessMode
                || this.SaleOrderTypeID == (int)ESaleOrderType.AttachedMode))
            {
                if (!IsPostBack)
                {
                    BindClientUsers();

                    BindDeliveryModes();

                    BindReceivingBankAccounts();

                    BindGuaranteebyUsers();

                    LoadCurrentEntity();
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

        #region Private Methods

        private void BindClientUsers()
        {
            var clientUsers = PageClientUserRepository.GetDropdownItems(new UISearchDropdownItem
            {
                Extension = new UISearchExtension { OnlyIncludeValidClientUser = true }
            });

            rcbxClientUser.DataSource = clientUsers;
            rcbxClientUser.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientUser.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientUser.DataBind();

            rcbxClientUser.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindClientCompanies()
        {
            ddlClientCompany.Items.Clear();

            var uiSearchObj = new UISearchDropdownItem();

            if (!string.IsNullOrEmpty(rcbxClientUser.SelectedValue))
            {
                int clientUserID;
                if (int.TryParse(rcbxClientUser.SelectedValue, out clientUserID))
                    uiSearchObj.Extension = new UISearchExtension { ClientUserID = clientUserID };
            }

            var clientCompanies = PageClientCompanyRepository.GetDropdownItems(uiSearchObj);
            ddlClientCompany.DataSource = clientCompanies;
            ddlClientCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            ddlClientCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            ddlClientCompany.DataBind();

            ddlClientCompany.Items.Insert(0, new DropDownListItem("", ""));
        }

        private void BindDeliveryModes()
        {
            ddlDeliveryMode.Items.Add(new DropDownListItem(GlobalConst.DeliveryModes.RECEIPTED_DELIVERY,
                ((int)EDeliveryMode.ReceiptedDelivery).ToString()));
            ddlDeliveryMode.Items.Add(new DropDownListItem(GlobalConst.DeliveryModes.GUARANTEE_DELIVERY,
                ((int)EDeliveryMode.GuaranteeDelivery).ToString()));

            ddlDeliveryMode.DataBind();
        }

        private void BindClientContacts()
        {
            rcbxClientContact.ClearSelection();
            rcbxClientContact.Items.Clear();

            if (!string.IsNullOrEmpty(rcbxClientUser.SelectedValue)
                && !string.IsNullOrEmpty(ddlClientCompany.SelectedValue))
            {
                var uiSearchObj = new UISearchDropdownItem();

                int clientUserID;
                int clientCompanyID;

                if (int.TryParse(rcbxClientUser.SelectedValue, out clientUserID)
                    && int.TryParse(ddlClientCompany.SelectedValue, out clientCompanyID))
                    uiSearchObj.Extension = new UISearchExtension
                    {
                        ClientUserID = clientUserID,
                        ClientCompanyID = clientCompanyID
                    };

                var clientContacts = PageClientInfoContactRepository.GetDropdownItems(uiSearchObj);

                rcbxClientContact.DataSource = clientContacts;
                rcbxClientContact.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                rcbxClientContact.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                rcbxClientContact.DataBind();

                rcbxClientContact.Items.Insert(0, new RadComboBoxItem("", ""));
            }
        }

        private void ClearReceiverInfo()
        {
            lblReceiverName.Text = string.Empty;
            lblReceiverPhone.Text = string.Empty;
            lblReceiverAddress.Text = string.Empty;
            lblReceiverFax.Text = string.Empty;
        }

        private void BindReceivingBankAccounts()
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
            rcbxReceivingBankAccount.DataSource = bankAccounts;
            rcbxReceivingBankAccount.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxReceivingBankAccount.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxReceivingBankAccount.DataBind();
        }

        private void BindGuaranteebyUsers()
        {
            var users = PageUsersRepository.GetDropdownItems();
            rcbxGuaranteeby.DataSource = users;
            rcbxGuaranteeby.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxGuaranteeby.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxGuaranteeby.DataBind();

            rcbxGuaranteeby.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntity != null)
            {
                hdnCurrentEntityID.Value = this.CurrentEntity.ID.ToString();

                int saleOrderTypeID = 0;

                if (CurrentEntity.SalesOrderApplication != null)
                {
                    var salesOrderApp = CurrentEntity.SalesOrderApplication;

                    txtOrderCode.Text = salesOrderApp.OrderCode;
                    rdpOrderDate.SelectedDate = salesOrderApp.OrderDate;
                    lblSalesOrderType.Text = salesOrderApp.SaleOrderType == null
                        ? string.Empty : salesOrderApp.SaleOrderType.TypeName;
                    cbxIsStop.Checked = salesOrderApp.IsStop;

                    saleOrderTypeID = salesOrderApp.SaleOrderTypeID;

                    hdnSaleOrderTypeID.Value = saleOrderTypeID.ToString();
                }

                lblCreateBy.Text = PageUsersRepository.GetUserFullNameByID(this.CurrentEntity.CreatedBy.HasValue
                    ? this.CurrentEntity.CreatedBy.Value : GlobalConst.INVALID_INT);

                rcbxClientUser.SelectedValue = CurrentEntity.ClientUserID.ToString();

                BindClientCompanies();
                ddlClientCompany.SelectedValue = CurrentEntity.ClientCompanyID.ToString();

                BindClientContacts();
                rcbxClientContact.SelectedValue = CurrentEntity.ClientContactID.ToString();

                if (saleOrderTypeID > 0)
                {
                    var eSaleOrderType = (ESaleOrderType)saleOrderTypeID;

                    switch (eSaleOrderType)
                    {
                        case ESaleOrderType.AttractBusinessMode:
                            if (CurrentEntity.DeliveryModeID.HasValue)
                            {
                                ddlDeliveryMode.SelectedValue = CurrentEntity.DeliveryModeID.Value.ToString();

                                var eDeliveryMode = (EDeliveryMode)CurrentEntity.DeliveryModeID.Value;

                                switch (eDeliveryMode)
                                {
                                    case EDeliveryMode.ReceiptedDelivery:
                                        foreach (RadComboBoxItem item in rcbxReceivingBankAccount.Items)
                                        {
                                            int bankAccountID = int.Parse(item.Value);

                                            if (CurrentEntity.ClientSaleAppBankAccount.Any(x => x.IsDeleted == false && x.ReceiverBankAccountID == bankAccountID))
                                            {
                                                item.Checked = true;

                                                HtmlTableRow trBankAccount = new HtmlTableRow();

                                                trBankAccount.Cells.Add(new HtmlTableCell() { InnerText = item.Text });

                                                tblReceivingBankAccounts.Rows.Add(trBankAccount);

                                            }
                                        }

                                        break;
                                    case EDeliveryMode.GuaranteeDelivery:
                                        rdpGuaranteeExpiration.SelectedDate = CurrentEntity.GuaranteeExpirationDate;
                                        if (CurrentEntity.Guaranteeby.HasValue)
                                            rcbxGuaranteeby.SelectedValue = CurrentEntity.Guaranteeby.Value.ToString();

                                        break;
                                }
                            }

                            break;
                        case ESaleOrderType.AttachedMode:

                            foreach (RadComboBoxItem item in rcbxReceivingBankAccount.Items)
                            {
                                int bankAccountID = int.Parse(item.Value);

                                if (CurrentEntity.ClientSaleAppBankAccount.Any(x => x.ReceiverBankAccountID == bankAccountID))
                                    item.Checked = true;
                            }

                            break;
                    }
                }

                lblReceiverName.Text = this.CurrentEntity.ReceiverName;
                lblReceiverPhone.Text = this.CurrentEntity.ReceiverPhone;
                lblReceiverAddress.Text = this.CurrentEntity.ReceiverAddress;
                lblReceiverFax.Text = this.CurrentEntity.ReceiverFax;

                EWorkflowStatus workfolwStatus = (EWorkflowStatus)this.CurrentEntity.WorkflowStatusID;

                if (CanEditUserIDs.Contains(CurrentUser.UserID))
                {
                    btnSave.Visible = true;
                    btnSubmit.Visible = false;
                    ShowAuditControls(false);

                    if (CurrentEntity.DeliveryModeID != (int)EDeliveryMode.GuaranteeDelivery)
                        divAppPayments.Visible = true;

                    #region 审核通过和发货中的订单，只能中止
                    switch (workfolwStatus)
                    {
                        case EWorkflowStatus.ApprovedBasicInfo:
                        case EWorkflowStatus.Shipping:
                            if (CanStopUserIDs.Contains(CurrentUser.UserID))
                                cbxIsStop.Enabled = true;
                            break;
                    }
                    #endregion
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
                            #region 已提交，待审核

                            DisabledBasicInfoControls();

                            if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                            {
                                ShowAuditControls(true);

                                if (CurrentEntity.DeliveryModeID != (int)EDeliveryMode.GuaranteeDelivery)
                                    divAppPayments.Visible = true;
                                else
                                    divAppPayments.Visible = false;

                            }
                            else
                                ShowAuditControls(false);

                            #endregion

                            break;

                        case EWorkflowStatus.ApprovedBasicInfo:
                        case EWorkflowStatus.Shipping:
                            #region 审核通过和发货中的订单，只能中止

                            DisabledBasicInfoControls();
                            ShowAuditControls(false);

                            if (CanStopUserIDs.Contains(CurrentUser.UserID)
                                && CurrentEntity.SalesOrderApplication != null
                                && CurrentEntity.SalesOrderApplication.IsStop == false)
                            {
                                btnSave.Visible = true;
                                btnSubmit.Visible = false;
                                cbxIsStop.Enabled = true;
                            }
                            #endregion
                            break;

                        case EWorkflowStatus.Completed:
                            #region 完成的订单，不能修改

                            DisabledBasicInfoControls();

                            ShowSaveButtons(false);

                            ShowAuditControls(false);

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
                    this.Master.BaseNotification.Show("您没有权限新增客户订单");
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
            divAppPayments.Visible = isShow;
            btnAudit.Visible = isShow;
            btnReturn.Visible = isShow;
        }

        /// <summary>
        /// 禁用基础信息相关控件
        /// </summary>
        private void DisabledBasicInfoControls()
        {
            rdpOrderDate.Enabled = false;
            rcbxClientUser.Enabled = false;
            ddlClientCompany.Enabled = false;
            ddlDeliveryMode.Enabled = false;
            cbxIsStop.Enabled = false;
            rcbxClientContact.Enabled = false;
            rdpGuaranteeExpiration.Enabled = false;
            rcbxGuaranteeby.Enabled = false;
            rcbxReceivingBankAccount.Visible = false;
            tblReceivingBankAccounts.Visible = true;

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
            divComment.Visible = false;
            divComments.Visible = false;
            divOtherSections.Visible = false;
            divStop.Visible = false;

            txtOrderCode.Text = Utility.GenerateAutoSerialNo(PageClientSaleAppRepository.GetMaxEntityID(),
                GlobalConst.EntityAutoSerialNo.SerialNoPrefix.CLIENT_ORDER);

            rdpOrderDate.SelectedDate = DateTime.Now;
            lblCreateBy.Text = CurrentUser.FullName;

            var saleOrderType = PageSaleOrderTypeRepository.GetByID(SaleOrderTypeID);
            if (saleOrderType != null)
                lblSalesOrderType.Text = saleOrderType.TypeName;

            hdnSaleOrderTypeID.Value = this.SaleOrderTypeID.ToString();
        }

        /// <summary>
        /// 保存客户订单基本信息
        /// </summary>
        private void SaveClientSaleAppBasicData(ClientSaleApplication currentEntity)
        {
            currentEntity.SalesOrderApplication.OrderDate = rdpOrderDate.SelectedDate.HasValue
                ? rdpOrderDate.SelectedDate.Value : DateTime.Now;

            currentEntity.ClientUserID = int.Parse(rcbxClientUser.SelectedValue);
            currentEntity.ClientCompanyID = int.Parse(ddlClientCompany.SelectedValue);

            var clientInfo = PageClientInfoRepository.GetList(x => x.ClientUserID == currentEntity.ClientUserID
                                && x.ClientCompanyID == currentEntity.ClientCompanyID).FirstOrDefault();

            if (clientInfo != null)
            {
                currentEntity.ReceiverName = clientInfo.ReceiverName;
                currentEntity.ReceiverPhone = clientInfo.PhoneNumber;
                currentEntity.ReceiverAddress = clientInfo.ReceiverAddress;
                currentEntity.ReceiverFax = clientInfo.Fax;
            }

            currentEntity.ClientContactID = int.Parse(rcbxClientContact.SelectedValue);
            var clientContact = PageClientInfoContactRepository.GetByID(currentEntity.ClientContactID);
            if (clientContact != null)
                currentEntity.ClientContactPhone = clientContact.PhoneNumber;

            var eSaleOrderType = (ESaleOrderType)this.SaleOrderTypeID;

            switch (eSaleOrderType)
            {
                case ESaleOrderType.AttractBusinessMode:
                    currentEntity.DeliveryModeID = int.Parse(ddlDeliveryMode.SelectedValue);
                    currentEntity.IsGuaranteed = false;
                    currentEntity.Guaranteeby = null;
                    currentEntity.GuaranteeExpirationDate = null;

                    if (currentEntity.DeliveryModeID == (int)EDeliveryMode.ReceiptedDelivery)
                    {
                        SaveReceivingBankAccounts(currentEntity);
                    }
                    else if (currentEntity.DeliveryModeID == (int)EDeliveryMode.GuaranteeDelivery)
                    {
                        currentEntity.IsGuaranteed = true;

                        currentEntity.GuaranteeExpirationDate = rdpGuaranteeExpiration.SelectedDate;
                        currentEntity.Guaranteeby = int.Parse(rcbxGuaranteeby.SelectedValue);
                    }

                    break;
                case ESaleOrderType.AttachedMode:
                    currentEntity.DeliveryModeID = null;
                    currentEntity.IsGuaranteed = false;
                    currentEntity.Guaranteeby = null;
                    currentEntity.GuaranteeExpirationDate = null;

                    SaveReceivingBankAccounts(currentEntity);

                    break;
            }

            PageClientSaleAppRepository.Save();

            if (!string.IsNullOrEmpty(txtComment.Text.Trim()))
            {
                var appNote = new ApplicationNote();
                appNote.WorkflowID = (int)EWorkflow.ClientOrder;
                appNote.NoteTypeID = (int)EAppNoteType.Comment;

                if (CanEditUserIDs.Contains(CurrentUser.UserID))
                    appNote.WorkflowStepID = (int)EWorkflowStep.EditClientOrder;
                else
                    appNote.WorkflowStepID = (int)EWorkflowStep.NewClientOrder;

                appNote.ApplicationID = currentEntity.ID;
                appNote.Note = txtComment.Text.Trim();

                PageAppNoteRepository.Add(appNote);

                PageAppNoteRepository.Save();
            }
        }

        private void SaveReceivingBankAccounts(ClientSaleApplication currentEntity)
        {
            foreach (RadComboBoxItem item in rcbxReceivingBankAccount.Items)
            {
                var curBankAccountID = int.Parse(item.Value);

                var matchedBankAccount = currentEntity.ClientSaleAppBankAccount
                    .FirstOrDefault(x => x.IsDeleted == false && x.ReceiverBankAccountID == curBankAccountID);

                if (item.Checked)
                {
                    //选中并且没有保存过，则新增并保存
                    if (matchedBankAccount == null)
                    {
                        matchedBankAccount = new ClientSaleAppBankAccount()
                        {
                            ReceiverBankAccountID = curBankAccountID
                        };

                        currentEntity.ClientSaleAppBankAccount.Add(matchedBankAccount);
                    }
                }
                else
                {
                    //未选中，但之前有保存过，则删除
                    if (matchedBankAccount != null)
                        matchedBankAccount.IsDeleted = true;
                }
            }
        }

        private void ValidControls()
        {
            if (this.SaleOrderTypeID == (int)ESaleOrderType.AttractBusinessMode)
            {
                if (!string.IsNullOrEmpty(ddlDeliveryMode.SelectedValue))
                {
                    int deliveryModeID;

                    if (int.TryParse(ddlDeliveryMode.SelectedValue, out deliveryModeID))
                    {
                        EDeliveryMode eDeliveryMode = (EDeliveryMode)deliveryModeID;

                        switch (eDeliveryMode)
                        {
                            case EDeliveryMode.ReceiptedDelivery:

                                if (rcbxReceivingBankAccount.CheckedItems.Count == 0)
                                    cvReceivingBankAccount.IsValid = false;

                                break;
                            case EDeliveryMode.GuaranteeDelivery:

                                if (!rdpGuaranteeExpiration.SelectedDate.HasValue)
                                    cvGuaranteeExpiration.IsValid = false;

                                if (string.IsNullOrEmpty(rcbxGuaranteeby.SelectedValue))
                                    cvGuaranteeby.IsValid = false;

                                break;
                        }
                    }
                }
            }
            else if (this.SaleOrderTypeID == (int)ESaleOrderType.AttachedMode)
            {
                if (rcbxReceivingBankAccount.CheckedItems.Count == 0)
                    cvReceivingBankAccount.IsValid = false;
            }
        }


        #endregion


        protected void rcbxClientUser_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ClearReceiverInfo();

            BindClientCompanies();

            BindClientContacts();
        }

        protected void ddlClientCompany_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            ClearReceiverInfo();

            if (!string.IsNullOrEmpty(rcbxClientUser.SelectedValue)
                && !string.IsNullOrEmpty(ddlClientCompany.SelectedValue))
            {
                int clientUserID;
                int clientCompanyID;

                if (int.TryParse(rcbxClientUser.SelectedValue, out clientUserID)
                    && int.TryParse(ddlClientCompany.SelectedValue, out clientCompanyID))
                {
                    var clientInfo = PageClientInfoRepository.GetList(x => x.ClientUserID == clientUserID
                        && x.ClientCompanyID == clientCompanyID).FirstOrDefault();

                    if (clientInfo != null)
                    {
                        lblReceiverName.Text = clientInfo.ReceiverName;
                        lblReceiverPhone.Text = clientInfo.PhoneNumber;
                        lblReceiverAddress.Text = clientInfo.ReceiverAddress;
                        lblReceiverFax.Text = clientInfo.Fax;
                    }
                }
            }

            BindClientContacts();
        }

        protected void rcbxClientContact_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            var dataItem = (UIDropdownItem)e.Item.DataItem;

            if (dataItem != null)
                e.Item.Attributes["Extension"] = Utility.JsonSeralize(dataItem.Extension);
        }

        #region Events

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
            var uiSearchObj = new UISearchSalesOrderAppDetail
            {
                SalesOrderApplicationID = CurrentEntity != null
                ? CurrentEntity.SalesOrderApplicationID : GlobalConst.INVALID_INT
            };

            int totalRecords;

            var requestProducts = PageSalesOrderAppDetailRepository
                .GetUIList(uiSearchObj, rgOrderProducts.CurrentPageIndex, rgOrderProducts.PageSize, out totalRecords);

            rgOrderProducts.DataSource = requestProducts;
        }

        protected void rgOrderProducts_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                PageSalesOrderAppDetailRepository.DeleteByID(id);
                PageSalesOrderAppDetailRepository.Save();
            }

            rgOrderProducts.Rebind();
        }

        protected void rgOrderProducts_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem commandItem = e.Item as GridCommandItem;
                Panel plAddCommand = commandItem.FindControl("plAddCommand") as Panel;

                if (plAddCommand != null)
                {
                    if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                            || (this.CurrentEntity.CreatedBy == CurrentUser.UserID
                                && (this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.TemporarySave
                                    || this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ReturnBasicInfo))))
                        plAddCommand.Visible = true;
                    else
                        plAddCommand.Visible = false;
                }
            }
        }

        protected void rgOrderProducts_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                    || (this.CurrentEntity.CreatedBy == CurrentUser.UserID
                        && (this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.TemporarySave
                            || this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ReturnBasicInfo))))
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = true;
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = true;
            }
            else
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = false;
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = false;
            }

            if (this.CanAuditUserIDs.Contains(CurrentUser.UserID)
                && this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.Submit)
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = true;
            }

            if (this.CurrentEntity != null && this.CurrentEntity.SalesOrderApplication.SaleOrderTypeID == (int)ESaleOrderType.AttachedMode)
                e.OwnerTableView.Columns.FindByUniqueName("InvoicePrice").Visible = true;
            else
                e.OwnerTableView.Columns.FindByUniqueName("InvoicePrice").Visible = false;
        }

        protected void rgOrderProducts_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                var uiEntity = (UISalesOrderAppDetail)gridDataItem.DataItem;

                if (uiEntity != null)
                {
                    string linkEditHtml = "<a href=\"javascript:void(0);\" onclick=\"openOrderProductWindow(" + uiEntity.ID + ")\">";

                    if (this.CanAuditUserIDs.Contains(CurrentUser.UserID)
                        && this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.Submit)
                        linkEditHtml += "修改单价";
                    else
                        linkEditHtml += "编辑";

                    linkEditHtml += "</a>";

                    var editColumn = rgOrderProducts.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT);

                    if (editColumn != null)
                    {
                        var editCell = gridDataItem.Cells[editColumn.OrderIndex];

                        if (editCell != null)
                            editCell.Text = linkEditHtml;
                    }
                }
            }
        }

        #endregion

        #region rgAppPayments

        protected void rgAppPayments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchApplicationPayment
            {
                WorkflowID = this.CurrentWorkFlowID,
                ApplicationID = this.CurrentEntityID.HasValue
                ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            int totalRecords;

            var appPayments = PageClientSaleAppRepository.GetPayments(uiSearchObj, rgAppPayments.CurrentPageIndex, rgAppPayments.PageSize, out totalRecords);

            rgAppPayments.DataSource = appPayments;
            rgAppPayments.VirtualItemCount = totalRecords;
        }

        protected void rgAppPayments_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem commandItem = e.Item as GridCommandItem;
                Panel plAddCommand = commandItem.FindControl("plAddCommand") as Panel;

                if (plAddCommand != null)
                {
                    if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                        || (this.CanAuditUserIDs.Contains(CurrentUser.UserID)
                            && this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.Submit)))
                        plAddCommand.Visible = true;
                    else
                        plAddCommand.Visible = false;
                }
            }
        }

        protected void rgAppPayments_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                || (this.CanAuditUserIDs.Contains(CurrentUser.UserID)
                    && this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.Submit)))
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = true;
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = true;
            }
            else
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = false;
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = false;
            }
        }

        protected void rgAppPayments_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            var editableItem = ((GridEditableItem)e.Item);
            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                string sPaymentMethodID = editableItem.GetDataKeyValue("PaymentMethodID").ToString();

                int iPaymentMethodID;

                if (int.TryParse(sPaymentMethodID, out iPaymentMethodID))
                {
                    if (iPaymentMethodID == (int)EPaymentMethod.BankTransfer)
                    {
                        PageAppPaymentRepository.DeleteByID(id);
                        PageAppPaymentRepository.Save();
                    }
                    else if (iPaymentMethodID == (int)EPaymentMethod.Deduction)
                    {

                    }
                }

                rgAppPayments.Rebind();
            }
        }

        #endregion

        #endregion

        #region Button Events

        protected void btnSave_Click(object sender, EventArgs e)
        {
            #region 验证数据是否有效

            ValidControls();

            #endregion

            if (!IsValid) return;

            ClientSaleApplication currentEntity = this.CurrentEntity;

            if (currentEntity == null)
            {
                currentEntity = new ClientSaleApplication();
                currentEntity.CompanyID = CurrentUser.CompanyID;
                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.TemporarySave;

                var salesOrderApp = new SalesOrderApplication()
                {
                    SaleOrderTypeID = this.SaleOrderTypeID.Value,
                    OrderCode = Utility.GenerateAutoSerialNo(PageClientSaleAppRepository.GetMaxEntityID(),
                                GlobalConst.EntityAutoSerialNo.SerialNoPrefix.CLIENT_ORDER)
                };

                currentEntity.SalesOrderApplication = salesOrderApp;

                PageClientSaleAppRepository.Add(currentEntity);
            }

            SaveClientSaleAppBasicData(currentEntity);

            hdnCurrentEntityID.Value = currentEntity.ID.ToString();

            if (this.CurrentEntity != null)
            {
                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REDIRECT);
            }
            else
            {
                this.Master.BaseNotification.OnClientHidden = "refreshMaintenancePage";
                this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_SAEVED_REFRESH);
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            #region 验证数据是否有效

            ValidControls();

            #endregion

            if (!IsValid) return;

            if (this.CurrentEntity != null)
            {
                EWorkflowStatus workfolwStatus = (EWorkflowStatus)this.CurrentEntity.WorkflowStatusID;

                switch (workfolwStatus)
                {
                    case EWorkflowStatus.TemporarySave:
                    case EWorkflowStatus.ReturnBasicInfo:
                        if (this.CurrentEntity.SalesOrderApplication.SalesOrderAppDetail.Where(x => x.IsDeleted == false).Count() > 0)
                        {
                            this.CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.Submit;

                            SaveClientSaleAppBasicData(this.CurrentEntity);

                            this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                        }
                        else
                        {
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                            this.Master.BaseNotification.AutoCloseDelay = 1000;
                            this.Master.BaseNotification.Show("货品为空，订单不能提交");
                        }

                        break;
                }
            }
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

                    IClientSaleApplicationRepository clientSaleAppRepository = new ClientSaleApplicationRepository();
                    IApplicationPaymentRepository appPaymentRepository = new ApplicationPaymentRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    clientSaleAppRepository.SetDbModel(db);
                    appPaymentRepository.SetDbModel(db);
                    appNoteRepository.SetDbModel(db);

                    var currentEntity = clientSaleAppRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        decimal totalOrderAmount = 0;
                        decimal totalPayAmount = 0;

                        if (currentEntity.SalesOrderApplication != null)
                            totalOrderAmount = currentEntity.SalesOrderApplication.SalesOrderAppDetail.Sum(x => x.TotalSalesAmount);

                        var appPayments = appPaymentRepository.GetList(x => x.WorkflowID == CurrentWorkFlowID
                            && x.ApplicationID == currentEntity.ID);

                        if (currentEntity.DeliveryModeID == (int)EDeliveryMode.GuaranteeDelivery)
                        {
                            if (totalOrderAmount > WebConfig.MaxGuaranteeAmount)
                            {
                                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                                this.Master.BaseNotification.AutoCloseDelay = 1000;
                                this.Master.BaseNotification.Show("已超过最大担保金额：" + WebConfig.MaxGuaranteeAmount + "元");

                                return;
                            }
                        }
                        else
                        {
                            totalPayAmount += appPayments.Count() > 0
                                ? appPayments.Sum(x => ((x.Amount.HasValue ? x.Amount.Value : 0) + (x.Fee.HasValue ? x.Fee.Value : 0)))
                                : 0;

                            //还需要加上抵扣的总金额
                            //totalPayAmount += 

                            if (totalOrderAmount != totalPayAmount)
                            {
                                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                                this.Master.BaseNotification.AutoCloseDelay = 1000;
                                this.Master.BaseNotification.Show("货品总金额必须等于收款总金额");

                                return;
                            }
                        }

                        var appNote = new ApplicationNote();
                        appNote.WorkflowID = CurrentWorkFlowID;
                        appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;
                        appNoteRepository.Add(appNote);

                        switch (currentEntity.WorkflowStatusID)
                        {
                            case (int)EWorkflowStatus.Submit:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditClientOrder;
                                appNote.ApplicationID = currentEntity.ID;
                                appNote.Note = txtAuditComment.Text.Trim();

                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ApprovedBasicInfo;

                                break;
                        }

                        foreach (var item in appPayments)
                        {
                            item.PaymentStatusID = (int)EPaymentStatus.Paid;
                        }

                        unitOfWork.SaveChanges();

                        this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                        this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
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

                    IClientSaleApplicationRepository clientSaleAppRepository = new ClientSaleApplicationRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    clientSaleAppRepository.SetDbModel(db);
                    appNoteRepository.SetDbModel(db);

                    var currentEntity = clientSaleAppRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        var appNote = new ApplicationNote();
                        appNote.WorkflowID = CurrentWorkFlowID;
                        appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;
                        appNoteRepository.Add(appNote);

                        switch (currentEntity.WorkflowStatusID)
                        {
                            case (int)EWorkflowStatus.Submit:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditClientOrder;
                                appNote.ApplicationID = currentEntity.ID;
                                appNote.Note = txtAuditComment.Text.Trim();

                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ReturnBasicInfo;

                                break;
                        }

                        unitOfWork.SaveChanges();

                        this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                        this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                    }
                }
            }
        }

        #endregion

        #endregion

    }
}