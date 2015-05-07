using System;
using System.Collections;
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

namespace ZhongDing.Web.Views.Procures
{
    public partial class StockInMaintenance : WorkflowBasePage
    {
        #region Consts

        private const string DATA_KEY_NAME_STOCK_IN_DETAIL_ID = "ID";
        private const string DATA_KEY_NAME_PROCURE_ORDER_APP_ID = "ProcureOrderAppID";
        private const string DATA_KEY_NAME_PROCURE_ORDER_APP_DETAIL_ID = "ProcureOrderAppDetailID";
        private const string DATA_KEY_NAME_PRODUCT_ID = "ProductID";
        private const string DATA_KEY_NAME_PRODUCT_SPECIFICATION_ID = "ProductSpecificationID";
        private const string DATA_KEY_NAME_WAREHOUSE_ID = "WarehouseID";

        #endregion

        #region Members

        private IStockInRepository _PageStockInRepository;
        private IStockInRepository PageStockInRepository
        {
            get
            {
                if (_PageStockInRepository == null)
                    _PageStockInRepository = new StockInRepository();

                return _PageStockInRepository;
            }
        }

        private ISupplierRepository _PageSupplierRepository;
        private ISupplierRepository PageSupplierRepository
        {
            get
            {
                if (_PageSupplierRepository == null)
                    _PageSupplierRepository = new SupplierRepository();

                return _PageSupplierRepository;
            }
        }

        private IStockInDetailRepository _PageStockInDetailRepository;
        private IStockInDetailRepository PageStockInDetailRepository
        {
            get
            {
                if (_PageStockInDetailRepository == null)
                    _PageStockInDetailRepository = new StockInDetailRepository();

                return _PageStockInDetailRepository;
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

        private StockIn _CurrentEntity;
        private StockIn CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageStockInRepository.GetByID(this.CurrentEntityID);

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
                        _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewStockIn);
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
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditStockIn);

                return _CanEditUserIDs;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.StockIn;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.StockInManage;

            if (!IsPostBack)
            {
                BindSuppliers();

                LoadCurrentEntity();
            }
        }

        #region Private Methods

        private void BindSuppliers()
        {
            var suppliers = PageSupplierRepository.GetDropdownItems(new UISearchDropdownItem
            {
                Extension = new UISearchExtension
                {
                    CompanyID = CurrentUser.CompanyID
                }
            });

            rcbxSupplier.DataSource = suppliers;
            rcbxSupplier.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxSupplier.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxSupplier.DataBind();

            rcbxSupplier.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntity != null)
            {
                hdnCurrentEntityID.Value = this.CurrentEntity.ID.ToString();

                divComments.Visible = true;
                divOtherSections.Visible = true;

                txtCode.Text = this.CurrentEntity.Code;
                lblCreateBy.Text = PageUsersRepository.GetUserFullNameByID(this.CurrentEntity.CreatedBy.HasValue
                    ? this.CurrentEntity.CreatedBy.Value : GlobalConst.INVALID_INT);
                rdpEntryDate.SelectedDate = this.CurrentEntity.EntryDate;
                rcbxSupplier.SelectedValue = this.CurrentEntity.SupplierID.ToString();

                EWorkflowStatus workfolwStatus = (EWorkflowStatus)this.CurrentEntity.WorkflowStatusID;

                switch (workfolwStatus)
                {
                    case EWorkflowStatus.ToBeInWarehouse:
                    case EWorkflowStatus.InWarehouse:
                        btnPrint.Visible = true;
                        break;
                }

                //if (CanEditUserIDs.Contains(CurrentUser.UserID))
                //{
                //    btnSave.Visible = true;
                //    btnSubmit.Visible = false;
                //    ShowEntryStockControls(false);
                //}
                //else
                //{
                    switch (workfolwStatus)
                    {
                        case EWorkflowStatus.TemporarySave:
                            #region 暂存（订单创建者或有修改权限的用户才能修改）
                            if (CurrentUser.UserID == this.CurrentEntity.CreatedBy
                                || this.CanEditUserIDs.Contains(CurrentUser.UserID))
                                ShowSaveButtons(true);
                            else
                                DisabledBasicInfoControls();

                            ShowEntryStockControls(false);

                            #endregion

                            break;
                        case EWorkflowStatus.ToBeInWarehouse:
                            #region 已提交，待入库

                            DisabledBasicInfoControls();

                            if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                                ShowEntryStockControls(true);
                            else
                                ShowEntryStockControls(false);

                            #endregion

                            break;
                        case EWorkflowStatus.InWarehouse:
                            #region 已入库,不能修改

                            DisabledBasicInfoControls();
                            ShowEntryStockControls(false);

                            #endregion
                            break;
                    }
                //}

                var uiSearchStockInDetailObj = new UISearchStockInDetail()
                {
                    StockInID = this.CurrentEntityID.HasValue ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT,
                };

                var stockInDetails = PageStockInDetailRepository.GetUIList(uiSearchStockInDetailObj);

                Session[WebUtility.WebSessionNames.StockInDetailData] = stockInDetails;
            }
            else
            {
                InitDefaultData();

                if (!this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                {
                    this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                    this.Master.BaseNotification.AutoCloseDelay = 1000;
                    this.Master.BaseNotification.Show("您没有权限新增采购订单");

                    return;
                }

                Session[WebUtility.WebSessionNames.StockInDetailData] = new List<UIStockInDetail>();
            }
        }

        /// <summary>
        /// 初始化默认值
        /// </summary>
        private void InitDefaultData()
        {
            btnSubmit.Visible = false;
            btnEntryStock.Visible = false;
            divComment.Visible = false;
            divComments.Visible = false;
            divOtherSections.Visible = false;

            rdpEntryDate.SelectedDate = DateTime.Now;

            lblCreateBy.Text = CurrentUser.FullName;

            txtCode.Text = Utility.GenerateAutoSerialNo(PageStockInRepository.GetMaxEntityID(),
                GlobalConst.EntityAutoSerialNo.SerialNoPrefix.STOCK_IN);
        }

        /// <summary>
        /// 禁用基础信息相关控件
        /// </summary>
        private void DisabledBasicInfoControls()
        {
            rdpEntryDate.Enabled = false;
            rcbxSupplier.Enabled = false;
            txtComment.Enabled = false;

            btnSave.Visible = false;
            btnSubmit.Visible = false;

            //目的是禁用Cell编辑
            rgStockInDetails.MasterTableView.EditMode = GridEditMode.InPlace;
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
        /// 显示或隐藏入库相关控件
        /// </summary>
        private void ShowEntryStockControls(bool isShow)
        {
            btnEntryStock.Visible = isShow;
        }

        /// <summary>
        /// 保存入库单基本信息
        /// </summary>
        /// <param name="currentEntity">The current entity.</param>
        private void SaveStockInBasicData(StockIn currentEntity)
        {
            currentEntity.EntryDate = rdpEntryDate.SelectedDate;
            currentEntity.SupplierID = Convert.ToInt32(rcbxSupplier.SelectedValue);

            PageStockInRepository.Save();

            if (!string.IsNullOrEmpty(txtComment.Text.Trim()))
            {
                var appNote = new ApplicationNote();
                appNote.WorkflowID = (int)EWorkflow.StockIn;
                appNote.WorkflowStepID = (int)EWorkflowStep.NewStockIn;
                appNote.NoteTypeID = (int)EAppNoteType.Comment;
                appNote.ApplicationID = currentEntity.ID;
                appNote.Note = txtComment.Text.Trim();

                PageAppNoteRepository.Add(appNote);

                PageAppNoteRepository.Save();
            }
        }

        private int GetDataKeyValue(GridBatchEditingCommand command, string dataKeyName)
        {
            int dataKeyValue = GlobalConst.INVALID_INT;

            if (command.NewValues[dataKeyName] != null)
            {
                string sdataKeyValue = command.NewValues[dataKeyName].ToString();

                if (int.TryParse(sdataKeyValue, out dataKeyValue))
                    return dataKeyValue;
            }

            return dataKeyValue;
        }

        #endregion

        #region Events

        #region Grid Events

        protected void rgAppNotes_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

        protected void rgStockInDetails_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            var stockInDetailData = (List<UIStockInDetail>)Session[WebUtility.WebSessionNames.StockInDetailData];

            rgStockInDetails.DataSource = stockInDetailData.Where(x => x.IsDeleted == false);
        }

        protected void rgStockInDetails_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem commandItem = e.Item as GridCommandItem;

                if (commandItem != null && commandItem.Cells.Count > 0
                    && commandItem.Cells[0].Controls.Count > 0)
                {
                    var commandItemTable = ((Table)(commandItem.Cells[0].Controls[0]));

                    if (commandItemTable != null && commandItemTable.Rows.Count > 1
                        && commandItemTable.Rows[1].Cells.Count > 0)
                    {
                        var saveChangesCell = commandItemTable.Rows[1].Cells[0];

                        if (saveChangesCell != null)
                        {
                            if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                                || (this.CurrentEntity.CreatedBy == CurrentUser.UserID
                                    && (this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.TemporarySave))))
                                saveChangesCell.Visible = true;
                            else
                                saveChangesCell.Visible = false;
                        }
                    }
                }
            }
        }

        protected void rgStockInDetails_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                    || (this.CurrentEntity.CreatedBy == CurrentUser.UserID
                        && (this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.TemporarySave))))
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = true;
            }
            else
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = false;
            }
        }

        protected void rgStockInDetails_BatchEditCommand(object sender, GridBatchEditingEventArgs e)
        {
            if (rgStockInDetails.Items.Count > 0)
            {
                bool isNeedValidate = false;
                bool isNeedSave = false;

                var stockInDetailData = (List<UIStockInDetail>)Session[WebUtility.WebSessionNames.StockInDetailData];

                #region 暂存当前修改的实体

                var uiSearchObj = new UISearchDropdownItem
                {
                    Extension = new UISearchExtension
                    {
                        CompanyID = CurrentUser.CompanyID
                    }
                };

                var warehouses = PageWarehouseRepository.GetDropdownItems(uiSearchObj);

                foreach (var command in e.Commands)
                {
                    int id = GetDataKeyValue(command, DATA_KEY_NAME_STOCK_IN_DETAIL_ID);
                    int procureOrderAppID = GetDataKeyValue(command, DATA_KEY_NAME_PROCURE_ORDER_APP_ID);
                    int procureOrderAppDetailID = GetDataKeyValue(command, DATA_KEY_NAME_PROCURE_ORDER_APP_DETAIL_ID);
                    int productID = GetDataKeyValue(command, DATA_KEY_NAME_PRODUCT_ID);
                    int warehouseID = GetDataKeyValue(command, DATA_KEY_NAME_WAREHOUSE_ID);

                    var curEntity = stockInDetailData.FirstOrDefault(x => x.ID == id && x.ProcureOrderAppDetailID == procureOrderAppDetailID);

                    if (curEntity != null)
                    {
                        isNeedSave = true;

                        #region 处理当前实体

                        var curWarehouse = warehouses.FirstOrDefault(x => x.ItemValue == warehouseID);
                        if (curWarehouse != null)
                            curEntity.Warehouse = curWarehouse.ItemText;

                        switch (command.Type)
                        {
                            case GridBatchEditingCommandType.Delete:
                                if (curEntity != null)
                                    curEntity.IsDeleted = true;
                                break;

                            case GridBatchEditingCommandType.Insert:
                                break;
                            case GridBatchEditingCommandType.Update:

                                isNeedValidate = true;

                                curEntity.WarehouseID = warehouseID;

                                string sInQty = command.NewValues["InQty"] == null
                                    ? string.Empty : command.NewValues["InQty"].ToString();
                                string sBatchNumber = command.NewValues["BatchNumber"] == null
                                    ? string.Empty : command.NewValues["BatchNumber"].ToString();
                                string sExpirationDate = command.NewValues["ExpirationDate"] == null
                                    ? string.Empty : command.NewValues["ExpirationDate"].ToString();
                                string sLicenseNumber = command.NewValues["LicenseNumber"] == null
                                    ? string.Empty : command.NewValues["LicenseNumber"].ToString();
                                string sIsMortgagedProduct = command.NewValues["IsMortgagedProduct"] == null
                                    ? string.Empty : command.NewValues["IsMortgagedProduct"].ToString();

                                if (!string.IsNullOrEmpty(sInQty))
                                {
                                    int iInQty;
                                    if (int.TryParse(sInQty, out iInQty))
                                        curEntity.InQty = iInQty;
                                }

                                if (!string.IsNullOrEmpty(sBatchNumber))
                                    curEntity.BatchNumber = sBatchNumber;

                                if (!string.IsNullOrEmpty(sExpirationDate))
                                {
                                    DateTime expirationDate;
                                    if (DateTime.TryParse(sExpirationDate, out expirationDate))
                                        curEntity.ExpirationDate = expirationDate;
                                }

                                if (!string.IsNullOrEmpty(sLicenseNumber))
                                    curEntity.LicenseNumber = sLicenseNumber;

                                if (!string.IsNullOrEmpty(sIsMortgagedProduct))
                                {
                                    bool isMortgagedProduct;

                                    if (bool.TryParse(sIsMortgagedProduct, out isMortgagedProduct))
                                        curEntity.IsMortgagedProduct = isMortgagedProduct;
                                }

                                break;
                        }

                        #endregion
                    }
                }

                Session[WebUtility.WebSessionNames.StockInDetailData] = stockInDetailData;

                #endregion

                if (isNeedValidate)
                {
                    int inValidCount = 0;

                    foreach (var item in stockInDetailData)
                    {
                        if (item.InQty <= 0 || string.IsNullOrEmpty(item.BatchNumber)
                            || string.IsNullOrEmpty(item.LicenseNumber) || !item.ExpirationDate.HasValue)
                        {
                            inValidCount++;

                            break;
                        }
                    }

                    if (inValidCount > 0)
                    {
                        cvStockInDetails.IsValid = false;
                        cvStockInDetails.ErrorMessage = "请更正以下错误:<br />基本数量、货品批号、过期日期、批准文号均为必填项";

                        return;
                    }
                }

                if (isNeedSave)
                {
                    foreach (var item in stockInDetailData)
                    {
                        StockInDetail stockInDetail = null;

                        if (item.ID > 0)
                        {
                            stockInDetail = PageStockInDetailRepository.GetByID(item.ID);

                            if (item.IsDeleted)
                                stockInDetail.IsDeleted = item.IsDeleted;
                        }
                        else
                        {
                            if (!item.IsDeleted)
                            {
                                stockInDetail = new StockInDetail
                                {
                                    StockInID = item.StockInID,
                                    ProcureOrderAppID = item.ProcureOrderAppID,
                                    ProcureOrderAppDetailID = item.ProcureOrderAppDetailID,
                                    ProductID = item.ProductID,
                                    ProductSpecificationID = item.ProductSpecificationID
                                };

                                PageStockInDetailRepository.Add(stockInDetail);
                            }
                        }

                        if (stockInDetail != null && !item.IsDeleted)
                        {
                            stockInDetail.WarehouseID = item.WarehouseID;
                            stockInDetail.InQty = item.InQty;
                            stockInDetail.BatchNumber = item.BatchNumber;
                            stockInDetail.ExpirationDate = item.ExpirationDate;
                            stockInDetail.LicenseNumber = item.LicenseNumber;
                            stockInDetail.IsMortgagedProduct = item.IsMortgagedProduct;
                            stockInDetail.ProcurePrice = item.ProcurePrice;
                        }
                    }

                    PageStockInDetailRepository.Save();

                    Session[WebUtility.WebSessionNames.StockInDetailData] = PageStockInDetailRepository.GetUIList(new UISearchStockInDetail
                    {
                        StockInID = this.CurrentEntity.ID
                    });

                    hdnGridCellValueChangedCount.Value = "0";

                    rgStockInDetails.Rebind();
                }
            }
        }

        protected void rcbxWarehouse_Load(object sender, EventArgs e)
        {
            RadComboBox rcbxWarehouse = (RadComboBox)sender;

            rcbxWarehouse.Items.Clear();

            var uiSearchObj = new UISearchDropdownItem
            {
                Extension = new UISearchExtension
                {
                    CompanyID = CurrentUser.CompanyID
                }
            };

            var warehouses = PageWarehouseRepository.GetDropdownItems(uiSearchObj);

            rcbxWarehouse.DataSource = warehouses;
            rcbxWarehouse.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxWarehouse.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxWarehouse.DataBind();

        }

        #endregion

        #region Buttons Events

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            StockIn currentEntity = null;

            if (this.CurrentEntityID.HasValue
            && this.CurrentEntityID > 0)
                currentEntity = PageStockInRepository.GetByID(this.CurrentEntityID);

            if (currentEntity == null)
            {
                currentEntity = new StockIn();
                currentEntity.Code = Utility.GenerateAutoSerialNo(PageStockInRepository.GetMaxEntityID(),
                GlobalConst.EntityAutoSerialNo.SerialNoPrefix.STOCK_IN);
                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.TemporarySave;
                PageStockInRepository.Add(currentEntity);
            }

            SaveStockInBasicData(currentEntity);

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
                        if (this.CurrentEntity.StockInDetail.Where(x => x.IsDeleted == false).Count() > 0)
                        {
                            this.CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.ToBeInWarehouse;

                            SaveStockInBasicData(this.CurrentEntity);

                            this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                        }
                        else
                        {
                            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                            this.Master.BaseNotification.AutoCloseDelay = 1000;
                            this.Master.BaseNotification.Show("入库货品为空，入库单不能提交");
                        }

                        break;

                    case EWorkflowStatus.ToBeInWarehouse:
                        break;

                    case EWorkflowStatus.InWarehouse:
                        break;
                }

                hdnGridCellValueChangedCount.Value = "0";
            }
        }

        protected void btnEntryStock_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            if (this.CurrentEntity != null)
            {
                if (this.CurrentEntity.Supplier != null)
                {
                    if (this.CurrentEntity.Supplier.SupplierCertificate.Any(x => x.Certificate.EffectiveTo < DateTime.Now))
                    {
                        this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                        this.Master.BaseNotification.AutoCloseDelay = 1000;
                        this.Master.BaseNotification.Show("供应商有证照过期，不能入库");

                        return;
                    }
                }

                if (this.CurrentEntity.StockInDetail.Any(x => x.Product.ProductCertificate.Any(y => y.Certificate.EffectiveTo < DateTime.Now)))
                {
                    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                    this.Master.BaseNotification.AutoCloseDelay = 1000;
                    this.Master.BaseNotification.Show("货品有证照过期，不能入库");

                    return;
                }

                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IStockInRepository stockInRepository = new StockInRepository();
                    IProcureOrderApplicationRepository procureOrderAppRepository = new ProcureOrderApplicationRepository();

                    procureOrderAppRepository.SetDbModel(db);
                    stockInRepository.SetDbModel(db);

                    var currentEntity = stockInRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        var procureOrderAppIDs = currentEntity.StockInDetail.Where(x => x.IsDeleted == false)
                            .Select(x => x.ProcureOrderAppID).Distinct();

                        foreach (var procureOrderAppID in procureOrderAppIDs)
                        {
                            var procureOrderApp = procureOrderAppRepository.GetByID(procureOrderAppID);

                            if (procureOrderApp != null)
                            {
                                var totalProcureQty = procureOrderApp.ProcureOrderAppDetail
                                    .Where(x => x.IsDeleted == false).Sum(x => x.ProcureCount);

                                var totalInQty = procureOrderApp.StockInDetail
                                    .Where(x => x.IsDeleted == false).Sum(x => x.InQty);

                                if (totalInQty > 0)
                                {
                                    if (totalProcureQty == totalInQty)
                                        procureOrderApp.WorkflowStatusID = (int)EWorkflowStatus.Completed;
                                    else
                                        procureOrderApp.WorkflowStatusID = (int)EWorkflowStatus.Shipping;
                                }
                            }
                        }

                        currentEntity.WorkflowStatusID = (int)EWorkflowStatus.InWarehouse;

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