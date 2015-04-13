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

                            if (this.CanAccessUserIDs.Contains(CurrentUser.UserID))
                                ShowSaveButtons(true);
                            else
                                ShowSaveButtons(false);

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
                    this.Master.BaseNotification.Show("您没有权申请结算");
                }
            }
        }

        /// <summary>
        /// 显示或隐藏保存和提交按钮
        /// </summary>
        private void ShowSaveButtons(bool isShow)
        {
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

        }

        protected void rgAppPayments_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgAppPayments_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgAppPayments_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!IsValid) return;

            CurrentEntity.SettlementOperateDate = DateTime.Now;
            CurrentEntity.WorkflowStatusID = (int)EWorkflowStatus.Submit;
            CurrentEntity.CreatedBy = CurrentUser.UserID;

            PageDBClientSettlementRepository.Save();

            if (!string.IsNullOrEmpty(txtComment.Text.Trim()))
            {
                var appNote = new ApplicationNote();
                appNote.WorkflowID = (int)EWorkflow.ClientOrder;
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
                        //decimal totalPayAmount = 0;
                        //decimal totalPaidAmount = 0;//已支付

                        //if (currentEntity.DBClientSettleBonus != null)
                        //    totalPayAmount = currentEntity.DBClientSettleBonus.Sum(x => x.TotalPayAmount ?? 0);

                        //var appPayments = appPaymentRepository.GetList(x => x.WorkflowID == CurrentWorkFlowID
                        //    && x.ApplicationID == currentEntity.ID);

                        //totalPaidAmount += appPayments.Sum(x => ((x.Amount.HasValue ? x.Amount.Value : 0) + (x.Fee.HasValue ? x.Fee.Value : 0)));

                        //if (totalPayAmount != totalPaidAmount)
                        //{
                        //    this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                        //    this.Master.BaseNotification.AutoCloseDelay = 1000;
                        //    this.Master.BaseNotification.Show("货品总金额必须等于收款总金额");

                        //    return;
                        //}

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
    }
}