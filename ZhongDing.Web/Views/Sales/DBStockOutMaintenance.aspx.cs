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

namespace ZhongDing.Web.Views.Sales
{
    public partial class DBStockOutMaintenance : WorkflowBasePage
    {
        #region Consts

        private const string DATA_KEY_NAME_STOCK_OUT_DETAIL_ID = "ID";
        private const string DATA_KEY_NAME_SALES_ORDER_APP_ID = "SalesOrderApplicationID";
        private const string DATA_KEY_NAME_SALES_ORDER_APP_DETAIL_ID = "SalesOrderAppDetailID";
        private const string DATA_KEY_NAME_PRODUCT_ID = "ProductID";
        private const string DATA_KEY_NAME_PRODUCT_SPECIFICATION_ID = "ProductSpecificationID";
        private const string DATA_KEY_NAME_WAREHOUSE_ID = "WarehouseID";

        #endregion

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

        private IDistributionCompanyRepository _PageDistributionCompanyRepository;
        private IDistributionCompanyRepository PageDistributionCompanyRepository
        {
            get
            {
                if (_PageDistributionCompanyRepository == null)
                    _PageDistributionCompanyRepository = new DistributionCompanyRepository();

                return _PageDistributionCompanyRepository;
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

        private IWarehouseRepository _PageWarehouseRepository;
        private IWarehouseRepository PageWarehouseRepository
        {
            get
            {
                if (_PageWarehouseRepository == null)
                    _PageWarehouseRepository = new WarehouseRepository();

                return _PageWarehouseRepository;
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
                        _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewDBStockOut);
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
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditDBStockOut);

                return _CanEditUserIDs;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.DBStockOut;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.DBOrderStockOutManage;

            if (!IsPostBack)
            {
                BindDistributionCompanies();

                LoadCurrentEntity();
            }
        }

        #region Private Methods

        private void BindDistributionCompanies()
        {
            var distributionCompanies = PageDistributionCompanyRepository.GetDropdownItems();

            rcbxDistributionCompany.DataSource = distributionCompanies;
            rcbxDistributionCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDistributionCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDistributionCompany.DataBind();

            rcbxDistributionCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void LoadCurrentEntity()
        {
            rdpBillDate.MaxDate = DateTime.Now;

            if (this.CurrentEntity != null)
            {
                //只能操作大包出库单
                if (this.CurrentEntity.ReceiverTypeID != (int)EReceiverType.DistributionCompany)
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
                rcbxDistributionCompany.SelectedValue = this.CurrentEntity.DistributionCompanyID.ToString();
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
                    this.Master.BaseNotification.Show("您没有权限新增大包出库单");

                    return;
                }
            }
        }

        /// <summary>
        /// 初始化默认值
        /// </summary>
        private void InitDefaultData()
        {
            //btnSearchOrders.Visible = false;
            btnSubmit.Visible = false;
            btnOutStock.Visible = false;
            divComment.Visible = false;
            divComments.Visible = false;
            divOtherSections.Visible = false;

            rdpBillDate.SelectedDate = DateTime.Now;

            txtCode.Text = Utility.GenerateAutoSerialNo(PageStockOutRepository.GetMaxEntityID(),
                GlobalConst.EntityAutoSerialNo.SerialNoPrefix.STOCK_OUT_DABAO);
        }

        /// <summary>
        /// 禁用基础信息相关控件
        /// </summary>
        private void DisabledBasicInfoControls()
        {
            rdpBillDate.Enabled = false;
            rcbxDistributionCompany.Enabled = false;
            txtComment.Enabled = false;

            btnSave.Visible = false;
            btnSubmit.Visible = false;

            //btnSearchOrders.Visible = false;

            //目的是禁用Cell编辑
            rgStockOutDetails.MasterTableView.EditMode = GridEditMode.InPlace;
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
            currentEntity.DistributionCompanyID = Convert.ToInt32(rcbxDistributionCompany.SelectedValue);

            var distCompany = PageDistributionCompanyRepository.GetByID(currentEntity.DistributionCompanyID);

            if (distCompany != null)
            {
                currentEntity.ReceiverName = distCompany.ReceiverName;
                currentEntity.ReceiverPhone = distCompany.PhoneNumber;
                currentEntity.ReceiverAddress = distCompany.Address;
            }

            PageStockOutRepository.Save();

            if (!string.IsNullOrEmpty(txtComment.Text.Trim()))
            {
                var appNote = new ApplicationNote();
                appNote.WorkflowID = (int)EWorkflow.DBStockOut;
                appNote.WorkflowStepID = (int)EWorkflowStep.NewDBStockOut;
                appNote.NoteTypeID = (int)EAppNoteType.Comment;
                appNote.ApplicationID = currentEntity.ID;
                appNote.Note = txtComment.Text.Trim();

                PageAppNoteRepository.Add(appNote);

                PageAppNoteRepository.Save();
            }
        }

        #endregion

        #region Events

        #region Grid Events

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

        #endregion

        #region Other Events

        protected void rcbxDistributionCompany_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            UIDropdownItem dataItem = (UIDropdownItem)e.Item.DataItem;

            if (dataItem != null)
                e.Item.Attributes["Extension"] = Utility.JsonSeralize(dataItem.Extension);
        }


        #endregion

        #region Buttons Events

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
                GlobalConst.EntityAutoSerialNo.SerialNoPrefix.STOCK_OUT_DABAO);
                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.TemporarySave;
                currentEntity.ReceiverTypeID = (int)EReceiverType.DistributionCompany;
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

            hdnGridCellValueChangedCount.Value = "0";
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

                hdnGridCellValueChangedCount.Value = "0";
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
                    //IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    stockOutRepository.SetDbModel(db);
                    //appNoteRepository.SetDbModel(db);

                    var currentEntity = stockOutRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        currentEntity.WorkflowStatusID = (int)EWorkflowStatus.OutWarehouse;
                        currentEntity.OutDate = DateTime.Now;

                        //var appNote = new ApplicationNote();
                        //appNote.WorkflowID = (int)EWorkflow.DBStockOut;
                        //appNote.WorkflowStepID = (int)EWorkflowStep.OutDBStockRoom;
                        //appNote.NoteTypeID = (int)EAppNoteType.Comment;
                        //appNote.ApplicationID = currentEntity.ID;
                        //appNote.Note = "出库单已出库（由系统自动生成）";
                        //appNoteRepository.Add(appNote);

                        unitOfWork.SaveChanges();

                        this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                        this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                        this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                    }
                }
            }
        }

        #endregion



        #endregion

    }
}