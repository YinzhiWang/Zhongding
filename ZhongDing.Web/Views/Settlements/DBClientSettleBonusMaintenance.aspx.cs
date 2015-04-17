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
using ZhongDing.Common.Extension;
using ZhongDing.Common.NPOIHelper.Excel;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Settlements
{
    public partial class DBClientSettleBonusMaintenance : WorkflowBasePage
    {
        #region Members

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

        private DBClientSettlement _CurrentEntity;
        private DBClientSettlement CurrentEntity
        {
            get
            {
                if (_CurrentEntity == null)
                    if (this.CurrentEntityID.HasValue && this.CurrentEntityID > 0)
                        _CurrentEntity = PageDBClientSettlementRepository.GetByID(this.CurrentEntityID);

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
                        _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.ApplyDBClientSettleBonus);
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
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditDBClientSettleBonus);

                return _CanEditUserIDs;
            }
        }

        private IList<int> _CanAuditUserIDs;
        private IList<int> CanAuditUserIDs
        {
            get
            {
                if (_CanAuditUserIDs == null)
                    _CanAuditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.AuditDBClientSettleBonusByDeptManagers);

                return _CanAuditUserIDs;
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

        #endregion


        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.DBClientSettleBonus;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.DBClientSettleBonusManage;

            if (!IsPostBack)
            {
                LoadCurrentEntity();
            }
        }


        #region Private Methods

        private void LoadCurrentEntity()
        {
            if (this.CurrentEntity != null)
            {
                hdnCurrentEntityID.Value = this.CurrentEntity.ID.ToString();

                lblCreatedOn.Text = CurrentEntity.CreatedOn.ToString("yyyy/MM/dd");
                lblCreateBy.Text = PageUsersRepository.GetUserFullNameByID((CurrentEntity.CreatedBy.HasValue && CurrentEntity.CreatedBy > 0)
                    ? this.CurrentEntity.CreatedBy.Value : CurrentUser.UserID);
                lblSettlementOperateDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
                lblHospitalType.Text = CurrentEntity.HospitalType.TypeName;

                EWorkflowStatus workfolwStatus = (EWorkflowStatus)this.CurrentEntity.WorkflowStatusID;

                if (CanEditUserIDs.Contains(CurrentUser.UserID))
                {
                    btnSubmit.Visible = false;
                    ShowAuditControls(false);
                }
                else
                {
                    switch (workfolwStatus)
                    {
                        case EWorkflowStatus.ToBeSettle:
                        case EWorkflowStatus.ReturnBasicInfo:
                            #region 未结算和基础信息退回（订单创建者才能修改）

                            if (this.CanAccessUserIDs.Contains(CurrentUser.UserID)
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
                                ShowAuditControls(true);
                            else
                                ShowAuditControls(false);

                            #endregion

                            break;

                        case EWorkflowStatus.ApprovedByDeptManagers:
                            #region 审核通过

                            DisabledBasicInfoControls();
                            ShowAuditControls(false);
                            ShowSaveButtons(false);

                            if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                            {
                                divAppPayments.Visible = true;
                                btnConfirmPay.Visible = true;
                            }

                            #endregion
                            break;

                        case EWorkflowStatus.Paid:
                            #region 已支付，不能修改

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
                    this.Master.BaseNotification.Show("您没有权限申请结算");
                }
            }
        }

        /// <summary>
        /// 显示或隐藏保存和提交按钮
        /// </summary>
        private void ShowSaveButtons(bool isShow)
        {
            btnSubmit.Visible = isShow;
            btnExport.Visible = isShow;
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

            lblCreateBy.Text = CurrentUser.FullName;
            lblSettlementOperateDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
        }

        private void BindSettleBonus(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchDBClientSettleBonus
            {
                DBClientSettlementID = this.CurrentEntityID.HasValue
                ? CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            if (!string.IsNullOrEmpty(txtClientUserName.Text.Trim()))
                uiSearchObj.ClientUserName = txtClientUserName.Text.Trim();

            if (CanPayUserIDs.Contains(CurrentUser.UserID))
                uiSearchObj.OnlyIncludeNeedSettlement = true;

            int totalRecords;

            var uiEntities = PageDBClientSettleBonusRepository.GetUIList(uiSearchObj, rgDBClientSettleBonus.CurrentPageIndex, rgDBClientSettleBonus.PageSize, out totalRecords);

            rgDBClientSettleBonus.VirtualItemCount = totalRecords;
            rgDBClientSettleBonus.DataSource = uiEntities;

            if (isNeedRebind)
                rgDBClientSettleBonus.Rebind();
        }



        #endregion



        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindSettleBonus(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtClientUserName.Text = string.Empty;

            BindSettleBonus(true);
        }

        protected void rgDBClientSettleBonus_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindSettleBonus(false);
        }

        protected void rgDBClientSettleBonus_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                    || this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                && (this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ToBeSettle
                    || this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ReturnBasicInfo))
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = true;
            }
            else
            {
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = false;
            }

            if (CanPayUserIDs.Contains(CurrentUser.UserID))
            {
                e.OwnerTableView.Columns.FindByUniqueName("ClientDBBankAccount").Visible = true;
                e.OwnerTableView.Columns.FindByUniqueName("IsSettled").Visible = true;

                e.OwnerTableView.Columns.FindByUniqueName("Hospitals").Visible = false;
                e.OwnerTableView.Columns.FindByUniqueName("ProductName").Visible = false;
                e.OwnerTableView.Columns.FindByUniqueName("Specification").Visible = false;
                e.OwnerTableView.Columns.FindByUniqueName("BonusAmount").Visible = false;
                e.OwnerTableView.Columns.FindByUniqueName("IsNeedSettlement").Visible = false;
                e.OwnerTableView.Columns.FindByUniqueName("PerformanceAmount").Visible = false;
            }
            else
            {
                e.OwnerTableView.Columns.FindByUniqueName("ClientDBBankAccount").Visible = false;
                e.OwnerTableView.Columns.FindByUniqueName("IsSettled").Visible = false;
            }
        }

        protected void rgDBClientSettleBonus_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                var uiEntity = (UIDBClientSettleBonus)gridDataItem.DataItem;

                if (uiEntity != null)
                {
                    string linkEditHtml = string.Empty;

                    if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                        || this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                            && (this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ToBeSettle
                            || this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ReturnBasicInfo))
                    {
                        if (uiEntity.IsSettled != true)
                        {
                            if (uiEntity.IsNeedSettlement != true
                                && uiEntity.IsManualSettled != true)
                            {
                                linkEditHtml += "<a href=\"javascript:void(0);\" onclick=\"openManualSettleWindow("
                                    + uiEntity.ID + "," + (int)EManualSettleActionType.IncludeSettlement + ")\">加入结算</a>";
                            }
                            else if (uiEntity.IsManualSettled == true)
                            {
                                linkEditHtml += "<a href=\"javascript:void(0);\" onclick=\"openManualSettleWindow("
                                    + uiEntity.ID + "," + (int)EManualSettleActionType.ExcludeSettlement + ")\">取消结算</a>";
                            }
                        }
                    }

                    var editColumn = rgDBClientSettleBonus.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT);

                    if (editColumn != null)
                    {
                        var editCell = gridDataItem.Cells[editColumn.OrderIndex];

                        if (editCell != null)
                            editCell.Text = linkEditHtml;
                    }
                }
            }
        }

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

        protected void rgAppPayments_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var uiSearchObj = new UISearchApplicationPayment
            {
                WorkflowID = this.CurrentWorkFlowID,
                ApplicationID = this.CurrentEntityID.HasValue
                ? this.CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            int totalRecords;

            var appPayments = PageDBClientSettlementRepository.GetPayments(uiSearchObj, rgAppPayments.CurrentPageIndex, rgAppPayments.PageSize, out totalRecords);

            rgAppPayments.DataSource = appPayments;
            rgAppPayments.VirtualItemCount = totalRecords;
        }

        protected void rgAppPayments_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem commandItem = e.Item as GridCommandItem;
                Panel plAddCommand = commandItem.FindControl("plAddCommand") as Panel;

                if (plAddCommand != null)
                {
                    if (this.CurrentEntity != null && (this.CanEditUserIDs.Contains(CurrentUser.UserID)
                        || (this.CanPayUserIDs.Contains(CurrentUser.UserID)
                            && this.CurrentEntity.WorkflowStatusID == (int)EWorkflowStatus.ApprovedByDeptManagers)))
                        plAddCommand.Visible = true;
                    else
                        plAddCommand.Visible = false;
                }
            }
        }

        #region Button Events

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (CurrentEntity.DBClientSettleBonus.Count == 0)
            {
                this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.Show("没有结算明细无需申请结算");

                return;
            }


            if (!IsValid) return;

            CurrentEntity.SettlementOperateDate = DateTime.Now;
            CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.Submit;
            CurrentEntity.CreatedBy = CurrentUser.UserID;

            PageDBClientSettlementRepository.Save();

            if (!string.IsNullOrEmpty(txtComment.Text.Trim()))
            {
                var appNote = new ApplicationNote();
                appNote.WorkflowID = CurrentWorkFlowID;
                appNote.NoteTypeID = (int)EAppNoteType.Comment;

                if (CanEditUserIDs.Contains(CurrentUser.UserID))
                    appNote.WorkflowStepID = (int)EWorkflowStep.EditDBClientSettleBonus;
                else
                    appNote.WorkflowStepID = (int)EWorkflowStep.ApplyDBClientSettleBonus;

                appNote.ApplicationID = CurrentEntity.ID;
                appNote.Note = txtComment.Text.Trim();

                PageAppNoteRepository.Add(appNote);

                PageAppNoteRepository.Save();
            }

            this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
        }

        protected void btnAudit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtAuditComment.Text.Trim()))
                cvAuditComment.IsValid = false;

            if (!IsValid) return;

            if (this.CurrentEntity != null)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IDBClientSettlementRepository dbClientSettlementRepository = new DBClientSettlementRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    dbClientSettlementRepository.SetDbModel(db);
                    appNoteRepository.SetDbModel(db);

                    var currentEntity = dbClientSettlementRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        var appNote = new ApplicationNote();
                        appNote.WorkflowID = CurrentWorkFlowID;
                        appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;
                        appNoteRepository.Add(appNote);

                        switch (currentEntity.WorkflowStatusID)
                        {
                            case (int)EWorkflowStatus.Submit:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditDBClientSettleBonusByDeptManagers;
                                appNote.ApplicationID = currentEntity.ID;
                                appNote.Note = txtAuditComment.Text.Trim();

                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ApprovedByDeptManagers;

                                break;
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

            if (this.CurrentEntity != null)
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IDBClientSettlementRepository dbClientSettlementRepository = new DBClientSettlementRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    dbClientSettlementRepository.SetDbModel(db);
                    appNoteRepository.SetDbModel(db);

                    var currentEntity = dbClientSettlementRepository.GetByID(this.CurrentEntityID);

                    if (currentEntity != null)
                    {
                        var appNote = new ApplicationNote();
                        appNote.WorkflowID = CurrentWorkFlowID;
                        appNote.NoteTypeID = (int)EAppNoteType.AuditOpinion;
                        appNoteRepository.Add(appNote);

                        switch (currentEntity.WorkflowStatusID)
                        {
                            case (int)EWorkflowStatus.Submit:
                                appNote.WorkflowStepID = (int)EWorkflowStep.AuditDBClientSettleBonusByDeptManagers;
                                appNote.ApplicationID = currentEntity.ID;
                                appNote.Note = txtAuditComment.Text.Trim();

                                currentEntity.WorkflowStatusID = (int)EWorkflowStatus.ReturnBasicInfo;

                                break;
                        }

                        unitOfWork.SaveChanges();

                        this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
                        this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
                        this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
                    }
                }
            }
        }

        protected void btnConfirmPay_Click(object sender, EventArgs e)
        {
            var totalNeedPayAmount = PageDBClientSettleBonusRepository
                .GetList(x => x.DBClientSettlementID == this.CurrentEntity.ID
                && x.IsNeedSettlement == true).Sum(x => x.TotalPayAmount);

            var totalPaidAmount = PageAppPaymentRepository
                .GetList(x => x.WorkflowID == CurrentWorkFlowID
                && x.ApplicationID == this.CurrentEntity.ID).Sum(x => x.Amount);

            if (totalNeedPayAmount != totalPaidAmount)
            {
                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.AutoCloseDelay = 1000;
                this.Master.BaseNotification.Show("支付总额不等于应发总额，请确认");

                return;
            }

            this.CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.Paid;

            PageDBClientSettlementRepository.Save();

            this.Master.BaseNotification.OnClientHidden = "redirectToManagementPage";
            this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_SUCCESS;
            this.Master.BaseNotification.Show(GlobalConst.NotificationSettings.MSG_SUCCESS_OPERATE_REDIRECT);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            var uiSearchObj = new UISearchDBClientSettleBonus
            {
                DBClientSettlementID = this.CurrentEntityID.HasValue
                ? CurrentEntityID.Value : GlobalConst.INVALID_INT
            };

            if (!string.IsNullOrEmpty(txtClientUserName.Text.Trim()))
                uiSearchObj.ClientUserName = txtClientUserName.Text.Trim();

            if (CanPayUserIDs.Contains(CurrentUser.UserID))
                uiSearchObj.OnlyIncludeNeedSettlement = true;

            var uiEntities = PageDBClientSettleBonusRepository.GetUIList(uiSearchObj);

            var excelPath = Server.MapPath("~/App_Data/") + "TempDBClientSettleBonusExcel.xls";

            NPOIHelper nPOIHelper = new Common.NPOIHelper.Excel.NPOIHelper();
            UIDBClientSettleBonus model = new UIDBClientSettleBonus();

            List<ExcelHeader> excelHeaders = new List<ExcelHeader>() { 
                new ExcelHeader(model.GetName(() => model.ClientUserName),"客户"), 
                new ExcelHeader(model.GetName(() => model.Hospitals),"医院"),
                new ExcelHeader(model.GetName(() => model.ProductName),"货品"),
                new ExcelHeader(model.GetName(() => model.Specification),"规格"),
                new ExcelHeader(model.GetName(() => model.SettlementDate),"年月"),
                new ExcelHeader(model.GetName(() => model.BonusAmount),"提成"),
                new ExcelHeader(model.GetName(() => model.IsNeedSettlement),"需结算"),
                new ExcelHeader(model.GetName(() => model.PerformanceAmount),"绩效"),
                new ExcelHeader(model.GetName(() => model.TotalPayAmount),"应发")
            };

            Queue<ExcelHeader> excelHeadersQueue = new Queue<ExcelHeader>(excelHeaders);
            Root excelRoot = new Root()
            {
                root = new HeadInfo()
                {
                    rowspan = 2,
                    sheetname = "大包客户提成结算表",
                    defaultheight = null,
                    defaultwidth = 20,
                    head = new List<AttributeList>(){
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,0,0"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,1,1"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,2,2"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,3,3"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,4,4"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,5,5"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,6,6"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,7,7"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,8,8"},

                    }
                }
            };

            List<Func<UIDBClientSettleBonus, string>> fieldFuncs = new List<Func<UIDBClientSettleBonus, string>>();

            fieldFuncs.Add(x => x.ClientUserName);
            fieldFuncs.Add(x => x.Hospitals);
            fieldFuncs.Add(x => x.ProductName);
            fieldFuncs.Add(x => x.Specification);
            fieldFuncs.Add(x => x.SettlementDate.ToString("yyyy/MM"));
            fieldFuncs.Add(x => x.BonusAmount.ToString("C2"));
            fieldFuncs.Add(x => (x.IsNeedSettlement.HasValue && x.IsNeedSettlement == true)
                ? GlobalConst.BoolChineseDescription.TRUE : GlobalConst.BoolChineseDescription.FALSE);
            fieldFuncs.Add(x => x.PerformanceAmount.ToString("C2"));
            fieldFuncs.Add(x => x.TotalPayAmount.ToString("C2"));


            nPOIHelper.ExportToExcel<UIDBClientSettleBonus>(
                (List<UIDBClientSettleBonus>)uiEntities,
                excelPath,
                excelHeaders.Select(x => x.Key).ToArray(),
                excelRoot,
                fieldFuncs.ToArray());
            Response.ContentType = "application/x-zip-compressed";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + ("大包客户提成结算表" + "_"
                + this.CurrentEntity.HospitalType.TypeName).UrlEncode() + "_" + this.CurrentEntity.SettlementDate.ToString("yyyyMMdd") + ".xls");
            string filename = excelPath;
            Response.TransmitFile(filename);

        }

        #endregion
    }
}