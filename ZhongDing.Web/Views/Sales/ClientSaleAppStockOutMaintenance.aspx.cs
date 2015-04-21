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
using ZhongDing.Common.Extension;

namespace ZhongDing.Web.Views.Sales
{
    public partial class ClientSaleAppStockOutMaintenance : WorkflowBasePage
    {
        #region Members

        private IStockOutRepository _PageStockOutRepository;
        private IStockOutRepository PageStockOutRepository
        {
            get
            {
                if (_PageStockOutRepository == null)
                    _PageStockOutRepository = new StockOutRepository();

                return _PageStockOutRepository;
            }
        }

        private IStockOutDetailRepository _PageStockOutDetailRepository;
        private IStockOutDetailRepository PageStockOutDetailRepository
        {
            get
            {
                if (_PageStockOutDetailRepository == null)
                    _PageStockOutDetailRepository = new StockOutDetailRepository();

                return _PageStockOutDetailRepository;
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
        private ITransportFeeRepository _PageTransportFeeRepository;
        private ITransportFeeRepository PageTransportFeeRepository
        {
            get
            {
                if (_PageTransportFeeRepository == null)
                    _PageTransportFeeRepository = new TransportFeeRepository();

                return _PageTransportFeeRepository;
            }
        }
        private ITransportFeeStockOutRepository _PageTransportFeeStockOutRepository;
        private ITransportFeeStockOutRepository PageTransportFeeStockOutRepository 
        {
            get
            {
                if (_PageTransportFeeStockOutRepository == null)
                    _PageTransportFeeStockOutRepository = new TransportFeeStockOutRepository();

                return _PageTransportFeeStockOutRepository;
            }
        }
        private StockOut _CurrentEntity;
        private StockOut CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageStockOutRepository.GetByID(this.CurrentEntityID);

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
                        _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewClientStockOut);
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
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditClientStockOut);

                return _CanEditUserIDs;
            }
        }


        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.ClientOrderStockOut;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ClientOrderStockOutManage;

            if (!IsPostBack)
            {
                BindClientUsers();

                BindInvoiceTypes();

                LoadCurrentEntity();

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

        private void BindInvoiceTypes()
        {
            ddlInvoiceType.Items.Add(new DropDownListItem(GlobalConst.InvoiceCategories.RECEIPT,
                ((int)EInvoiceCategory.Receipt).ToString()));

            ddlInvoiceType.Items.Add(new DropDownListItem(GlobalConst.InvoiceCategories.INVOICE,
                ((int)EInvoiceCategory.Invoice).ToString()));

            ddlInvoiceType.DataBind();
        }

        private void LoadCurrentEntity()
        {
            rdpBillDate.MaxDate = DateTime.Now;

            if (this.CurrentEntity != null)
            {
                rgTransportFees.Visible = true;

                //只能操作客户订单出库单
                if (this.CurrentEntity.ReceiverTypeID != (int)EReceiverType.ClientUser)
                {
                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                    this.Master.BaseNotification.AutoCloseDelay = 1000;
                    this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_PARAMETER_ERROR_REDIRECT);

                    return;
                }

                hdnCurrentEntityID.Value = this.CurrentEntity.ID.ToString();

                txtCode.Text = this.CurrentEntity.Code;
                rdpBillDate.SelectedDate = this.CurrentEntity.BillDate;
                rcbxClientUser.SelectedValue = this.CurrentEntity.ClientUserID.ToString();
                BindClientCompanies();
                ddlClientCompany.SelectedValue = this.CurrentEntity.ClientCompanyID.ToString();
                ddlInvoiceType.SelectedValue = this.CurrentEntity.InvoiceTypeID.ToString();

                if (this.CurrentEntity.ClientCompany != null)
                    lblClientCompanyRegistedAddress.Text = this.CurrentEntity.ClientCompany.Address;

                lblReceiverName.Text = this.CurrentEntity.ReceiverName;
                lblReceiverPhone.Text = this.CurrentEntity.ReceiverPhone;
                lblReceiverAddress.Text = this.CurrentEntity.ReceiverAddress;

                if (CanEditUserIDs.Contains(CurrentUser.UserID))
                {
                    btnSave.Visible = true;
                    btnSubmit.Visible = false;
                    btnOutStock.Visible = false;
                }
                else
                {
                    EWorkflowStatus workfolwStatus = (EWorkflowStatus)this.CurrentEntity.WorkflowStatusID;

                    switch (workfolwStatus)
                    {
                        case EWorkflowStatus.TemporarySave:
                            #region 暂存（订单创建者或有修改权限的用户才能修改）
                            if (CurrentUser.UserID == this.CurrentEntity.CreatedBy
                                || this.CanEditUserIDs.Contains(CurrentUser.UserID))
                                ShowSaveButtons(true);
                            else
                                DisabledBasicInfoControls();

                            btnOutStock.Visible = false;
                            btnPrint.Visible = false;
                            #endregion

                            break;
                        case EWorkflowStatus.ToBeOutWarehouse:
                            #region 已提交，待出库

                            DisabledBasicInfoControls();

                            if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                                ShowOutStockControls(true);
                            else
                                ShowOutStockControls(false);

                            btnPrint.Visible = true;

                            #endregion

                            break;
                        case EWorkflowStatus.OutWarehouse:
                            #region 已出库,不能修改

                            DisabledBasicInfoControls();
                            ShowOutStockControls(false);
                            btnPrint.Visible = true;

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
                    this.Master.BaseNotification.Show("您没有权限新增客户订单出库单");

                    return;
                }
            }
        }

        /// <summary>
        /// 初始化默认值
        /// </summary>
        private void InitDefaultData()
        {
            btnSubmit.Visible = false;
            btnOutStock.Visible = false;
            divComment.Visible = false;
            divComments.Visible = false;
            divOtherSections.Visible = false;

            rdpBillDate.SelectedDate = DateTime.Now;

            txtCode.Text = Utility.GenerateAutoSerialNo(PageStockOutRepository.GetMaxEntityID(),
                GlobalConst.EntityAutoSerialNo.SerialNoPrefix.STOCK_OUT_CLIENT);

            divTransportFees.Visible = false;
        }

        /// <summary>
        /// 禁用基础信息相关控件
        /// </summary>
        private void DisabledBasicInfoControls()
        {
            rdpBillDate.Enabled = false;
            rcbxClientUser.Enabled = false;
            ddlClientCompany.Enabled = false;
            ddlInvoiceType.Enabled = false;
            txtComment.Enabled = false;

            btnSave.Visible = false;
            btnSubmit.Visible = false;
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
        /// 显示或隐藏出库相关控件
        /// </summary>
        private void ShowOutStockControls(bool isShow)
        {
            btnOutStock.Visible = isShow;
        }


        /// <summary>
        /// 保存出库单基本信息
        /// </summary>
        /// <param name="currentEntity">The current entity.</param>
        private void SaveStockOutBasicData(StockOut currentEntity)
        {
            currentEntity.BillDate = rdpBillDate.SelectedDate.HasValue
                ? rdpBillDate.SelectedDate.Value : DateTime.Now;
            currentEntity.ClientUserID = Convert.ToInt32(rcbxClientUser.SelectedValue);
            currentEntity.ClientCompanyID = Convert.ToInt32(ddlClientCompany.SelectedValue);
            currentEntity.InvoiceTypeID = Convert.ToInt32(ddlInvoiceType.SelectedValue);

            var clientInfo = PageClientInfoRepository.GetList(x => x.ClientUserID == currentEntity.ClientUserID
                    && x.ClientCompanyID == currentEntity.ClientCompanyID).FirstOrDefault();

            if (clientInfo != null)
            {
                currentEntity.ReceiverName = clientInfo.ReceiverName;
                currentEntity.ReceiverPhone = clientInfo.PhoneNumber;
                currentEntity.ReceiverAddress = clientInfo.ReceiverAddress;
            }

            PageStockOutRepository.Save();

            if (!string.IsNullOrEmpty(txtComment.Text.Trim()))
            {
                var appNote = new ApplicationNote();
                appNote.WorkflowID = this.CurrentWorkFlowID;
                appNote.WorkflowStepID = (int)EWorkflowStep.NewClientStockOut;
                appNote.NoteTypeID = (int)EAppNoteType.Comment;
                appNote.ApplicationID = currentEntity.ID;
                appNote.Note = txtComment.Text.Trim();

                PageAppNoteRepository.Add(appNote);

                PageAppNoteRepository.Save();
            }
        }

        private void ClearReceiverInfo()
        {
            lblClientCompanyRegistedAddress.Text = string.Empty;
            lblReceiverName.Text = string.Empty;
            lblReceiverPhone.Text = string.Empty;
            lblReceiverAddress.Text = string.Empty;
        }

        #endregion

        protected void rcbxClientUser_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindClientCompanies();
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
                        lblClientCompanyRegistedAddress.Text = clientInfo.ClientCompany == null
                            ? string.Empty : clientInfo.ClientCompany.Address;

                        lblReceiverName.Text = clientInfo.ReceiverName;
                        lblReceiverPhone.Text = clientInfo.PhoneNumber;
                        lblReceiverAddress.Text = clientInfo.ReceiverAddress;
                    }
                }
            }
        }

        protected void rgAppNotes_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchApplicationNote()
            {
                WorkflowID = this.CurrentWorkFlowID,
                ApplicationID = this.CurrentEntityID.HasValue
                ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            var appNotes = PageAppNoteRepository.GetUIList(uiSearchObj);

            rgAppNotes.DataSource = appNotes;
        }

        protected void rgStockOutDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var uiSearchStockOutDetailObj = new UISearchStockOutDetail()
            {
                StockOutID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT,
            };

            var stockOutDetails = PageStockOutDetailRepository.GetUIList(uiSearchStockOutDetailObj);

            rgStockOutDetails.DataSource = stockOutDetails;
        }

        protected void rgStockOutDetails_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem commandItem = e.Item as GridCommandItem;
                Panel plAddCommand = commandItem.FindControl("plAddCommand") as Panel;

                if (plAddCommand != null)
                {
                    if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                        || (this.CurrentEntity.CreatedBy == CurrentUser.UserID
                            && this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.TemporarySave)))
                        plAddCommand.Visible = true;
                    else
                        plAddCommand.Visible = false;
                }
            }
        }

        protected void rgStockOutDetails_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                    || (this.CurrentEntity.CreatedBy == CurrentUser.UserID
                        && this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.TemporarySave)))
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = true;
            }
            else
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = false;
            }
        }

        protected void rgStockOutDetails_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                PageStockOutDetailRepository.DeleteByID(id);
                PageStockOutDetailRepository.Save();
            }

            rgStockOutDetails.Rebind();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            StockOut currentEntity = null;

            if (this.CurrentEntityID.HasValue
            && this.CurrentEntityID > 0)
                currentEntity = PageStockOutRepository.GetByID(this.CurrentEntityID);

            if (currentEntity == null)
            {
                currentEntity = new StockOut();
                currentEntity.Code = Utility.GenerateAutoSerialNo(PageStockOutRepository.GetMaxEntityID(),
                GlobalConst.EntityAutoSerialNo.SerialNoPrefix.STOCK_OUT_CLIENT);
                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.TemporarySave;
                currentEntity.ReceiverTypeID = (int)EReceiverType.ClientUser;
                currentEntity.CompanyID = CurrentUser.CompanyID;
                PageStockOutRepository.Add(currentEntity);
            }

            SaveStockOutBasicData(currentEntity);

            hdnCurrentEntityID.Value = currentEntity.ID.ToString();

            if (this.CurrentEntityID.HasValue
                && this.CurrentEntityID > 0)
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
            if (!IsValid) return;

            if (this.CurrentEntity != null)
            {
                EWorkflowStatus workfolwStatus = (EWorkflowStatus)this.CurrentEntity.WorkflowStatusID;

                switch (workfolwStatus)
                {
                    case EWorkflowStatus.TemporarySave:
                        if (this.CurrentEntity.StockOutDetail.Where(x => x.IsDeleted == false).Count() > 0)
                        {
                            this.CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.ToBeOutWarehouse;

                            SaveStockOutBasicData(this.CurrentEntity);

                            this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                        }
                        else
                        {
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                            this.Master.BaseNotification.AutoCloseDelay = 1000;
                            this.Master.BaseNotification.Show("出库货品为空，出库单不能提交");
                        }

                        break;
                }

            }
        }

        protected void btnOutStock_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            if (this.CurrentEntity != null)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IStockOutRepository stockOutRepository = new StockOutRepository();
                    ISalesOrderApplicationRepository salesOrderAppRepository = new SalesOrderApplicationRepository();

                    stockOutRepository.SetDbModel(db);
                    salesOrderAppRepository.SetDbModel(db);

                    var currentEntity = stockOutRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        var salesOrderAppIDs = currentEntity.StockOutDetail.Where(x => x.IsDeleted == false)
                            .Select(x => x.SalesOrderApplicationID).Distinct();

                        foreach (var salesOrderAppID in salesOrderAppIDs)
                        {
                            var salesOrderApp = salesOrderAppRepository.GetByID(salesOrderAppID);

                            if (salesOrderApp != null)
                            {
                                var salesTotalQty = salesOrderApp.SalesOrderAppDetail
                                    .Where(x => x.IsDeleted == false).Sum(x => x.Count + (x.GiftCount.HasValue ? x.GiftCount.Value : 0));

                                var outTotalQty = salesOrderApp.StockOutDetail
                                    .Where(x => x.IsDeleted == false).Sum(x => x.OutQty);

                                var clientSaleApp = salesOrderApp.ClientSaleApplication.FirstOrDefault(x => x.IsDeleted == false);

                                if (clientSaleApp != null)
                                {
                                    if (outTotalQty > 0)
                                    {
                                        if (salesTotalQty == outTotalQty)
                                            clientSaleApp.WorkflowStatusID = (int)EWorkflowStatus.Completed;
                                        else
                                            clientSaleApp.WorkflowStatusID = (int)EWorkflowStatus.Shipping;
                                    }
                                }
                            }
                        }

                        currentEntity.WorkflowStatusID = (int)EWorkflowStatus.OutWarehouse;
                        currentEntity.OutDate = DateTime.Now;

                        unitOfWork.SaveChanges();

                        this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                        this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                        this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                    }
                }
            }
        }

        private void BindTransportFee(bool isNeedRebind)
        {
            if (CurrentEntityID.BiggerThanZero())
            {
                var uiTransportFees = PageTransportFeeStockOutRepository.GetUIListForSaleAppStockOut(CurrentEntityID.Value);
                rgTransportFees.DataSource = uiTransportFees;

                if (isNeedRebind)
                    rgTransportFees.Rebind();
            }
        }


        protected void rgTransportFees_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindTransportFee(false);
        }
    }
}