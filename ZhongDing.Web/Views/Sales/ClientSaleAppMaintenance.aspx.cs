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
                if (_CanAccessUserIDs == null)
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

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ClientOrderManage;
            this.CurrentWorkFlowID = (int)EWorkflow.ClientOrder;

            if (!IsPostBack)
            {
                BindClientUsers();

                BindDeliveryModes();

                BindReceivingBankAccounts();

                BindGuaranteebyUsers();

                LoadCurrentEntity();
            }
        }

        #region Private Methods

        private void BindClientUsers()
        {
            var clientUsers = PageClientUserRepository.GetDropdownItems();
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
                    AccountTypeID = (int)EAccountType.Company,
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

                if (CurrentEntity.SalesOrderApplication != null)
                {
                    txtOrderCode.Text = CurrentEntity.SalesOrderApplication.OrderCode;
                    rdpOrderDate.SelectedDate = CurrentEntity.SalesOrderApplication.OrderDate;
                }

                lblCreateBy.Text = PageUsersRepository.GetUserFullNameByID(this.CurrentEntity.CreatedBy.HasValue
                    ? this.CurrentEntity.CreatedBy.Value : GlobalConst.INVALID_INT);

                lblSalesModel.Text = CurrentEntity.SalesModel == null
                    ? string.Empty : CurrentEntity.SalesModel.SalesModelName;

                rcbxClientUser.SelectedValue = CurrentEntity.ClientUserID.ToString();

                BindClientCompanies();
                ddlClientCompany.SelectedValue = CurrentEntity.ClientCompanyID.ToString();

                BindClientContacts();
                rcbxClientContact.SelectedValue = CurrentEntity.ClientContactID.ToString();

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

                                if (CurrentEntity.ClientSaleAppBankAccount.Any(x => x.ReceiverBankAccountID == bankAccountID))
                                    item.Checked = true;
                            }

                            break;
                        case EDeliveryMode.GuaranteeDelivery:
                            rdpGuaranteeExpiration.SelectedDate = CurrentEntity.GuaranteeExpirationDate;
                            if (CurrentEntity.Guaranteeby.HasValue)
                                rcbxGuaranteeby.SelectedValue = CurrentEntity.Guaranteeby.Value.ToString();

                            break;
                    }
                }

                lblReceiverName.Text = this.CurrentEntity.ReceiverName;
                lblReceiverPhone.Text = this.CurrentEntity.ReceiverPhone;
                lblReceiverAddress.Text = this.CurrentEntity.ReceiverAddress;
                lblReceiverFax.Text = this.CurrentEntity.ReceiverFax;

                if (CanEditUserIDs.Contains(CurrentUser.UserID))
                {
                    btnSave.Visible = true;
                    btnSubmit.Visible = false;
                    ShowAuditControls(false);
                }
                else
                {
                    EWorkflowStatus workfolwStatus = (EWorkflowStatus)this.CurrentEntity.WorkflowStatusID;

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

                            #endregion

                            break;
                        case EWorkflowStatus.Submit:
                            #region 已提交，待审核

                            DisabledBasicInfoControls();

                            if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                                ShowAuditControls(true);
                            else
                                ShowAuditControls(false);

                            #endregion

                            break;

                        case EWorkflowStatus.ExportToDBOrder:
                            #region 已生成配送订单，不能修改

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
            rcbxReceivingBankAccount.Enabled = false;

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

        protected void rgOrderProducts_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {

        }

        protected void rgOrderProducts_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {

        }

        protected void rgOrderProducts_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgOrderProducts_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
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

            if (!IsValid) return;


        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

        }

        protected void btnAudit_Click(object sender, EventArgs e)
        {

        }

        protected void btnReturn_Click(object sender, EventArgs e)
        {

        }

    }
}