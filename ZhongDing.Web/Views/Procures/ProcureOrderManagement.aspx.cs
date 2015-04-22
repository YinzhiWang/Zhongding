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
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Procures
{
    public partial class ProcureOrderManagement : WorkflowBasePage
    {
        #region Members

        private IProcureOrderApplicationRepository _PageProcureOrderAppRepository;
        private IProcureOrderApplicationRepository PageProcureOrderAppRepository
        {
            get
            {
                if (_PageProcureOrderAppRepository == null)
                    _PageProcureOrderAppRepository = new ProcureOrderApplicationRepository();

                return _PageProcureOrderAppRepository;
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

        private IList<int> _CanAddUserIDs;
        /// <summary>
        /// 可新增订单的用户
        /// </summary>
        private IList<int> CanAddUserIDs
        {
            get
            {
                if (_CanAddUserIDs == null)
                    _CanAddUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewProcureOrder);

                return _CanAddUserIDs;
            }
        }

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditProcureOrder);

                return _CanEditUserIDs;
            }
        }

        private IList<int> _CanStopUserIDs;
        private IList<int> CanStopUserIDs
        {
            get
            {
                if (_CanStopUserIDs == null)
                    _CanStopUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.StopProcureOrder);

                return _CanStopUserIDs;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.ProcureOrder;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ProcureOrderManage;

            if (!IsPostBack)
            {
                BindSuppliers();

                BindWorkflowStatus();
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

        private void BindWorkflowStatus()
        {
            var uiSearchObj = new UISearchDropdownItem();

            IList<int> includeItemValues = new List<int>();
            includeItemValues.Add((int)EWorkflowStatus.TemporarySave);
            includeItemValues.Add((int)EWorkflowStatus.Submit);
            includeItemValues.Add((int)EWorkflowStatus.ApprovedBasicInfo);
            includeItemValues.Add((int)EWorkflowStatus.ReturnBasicInfo);
            includeItemValues.Add((int)EWorkflowStatus.AuditingOfPaymentInfo);
            includeItemValues.Add((int)EWorkflowStatus.ToBePaid);
            includeItemValues.Add((int)EWorkflowStatus.ReturnPaymentInfo);
            includeItemValues.Add((int)EWorkflowStatus.Paid);
            includeItemValues.Add((int)EWorkflowStatus.Shipping);
            includeItemValues.Add((int)EWorkflowStatus.Completed);

            uiSearchObj.IncludeItemValues = includeItemValues;

            var workflowStatus = PageWorkflowStatusRepository.GetDropdownItems(uiSearchObj);

            rcbxWorkflowStatus.DataSource = workflowStatus;
            rcbxWorkflowStatus.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxWorkflowStatus.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxWorkflowStatus.DataBind();

            rcbxWorkflowStatus.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindEntities(bool isNeedRebind)
        {
            UISearchProcureOrderApplication uiSearchObj = new UISearchProcureOrderApplication()
            {
                CompanyID = CurrentUser.CompanyID,
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
            };

            IList<int> includeWorkflowStatusIDs = PageWorkflowStatusRepository
                .GetCanAccessIDsByUserID(CurrentWorkFlowID, CurrentUser.UserID);

            if (includeWorkflowStatusIDs == null)
            {
                includeWorkflowStatusIDs = new List<int>();
                includeWorkflowStatusIDs.Add((int)EWorkflowStatus.Paid);
            }
            else
            {
                if (this.CanAddUserIDs.Contains(CurrentUser.UserID) || this.CanEditUserIDs.Contains(CurrentUser.UserID))
                {
                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.TemporarySave))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.TemporarySave);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.Submit))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.Submit);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ApprovedBasicInfo))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ApprovedBasicInfo);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ReturnBasicInfo))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ReturnBasicInfo);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.AuditingOfPaymentInfo))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.AuditingOfPaymentInfo);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ToBePaid))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ToBePaid);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ReturnPaymentInfo))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ReturnPaymentInfo);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.Paid))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.Paid);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.Shipping))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.Shipping);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.Completed))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.Completed);
                }
            }

            uiSearchObj.IncludeWorkflowStatusIDs = includeWorkflowStatusIDs;

            if (!string.IsNullOrEmpty(rcbxSupplier.SelectedValue))
            {
                int supplierID;
                if (int.TryParse(rcbxSupplier.SelectedValue, out supplierID))
                    uiSearchObj.SupplierID = supplierID;
            }

            if (!string.IsNullOrEmpty(rcbxWorkflowStatus.SelectedValue))
            {
                int workflowStatusID;
                if (int.TryParse(rcbxWorkflowStatus.SelectedValue, out workflowStatusID))
                    uiSearchObj.WorkflowStatusID = workflowStatusID;
            }

            int totalRecords;

            var entities = PageProcureOrderAppRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;

            rgEntities.DataSource = entities;

            if (isNeedRebind)
                rgEntities.Rebind();
        }

        #endregion


        protected void rgEntities_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindEntities(false);
        }

        protected void rgEntities_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IProcureOrderApplicationRepository orderAppRepository = new ProcureOrderApplicationRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();
                    IApplicationPaymentRepository appPaymentRepository = new ApplicationPaymentRepository();

                    orderAppRepository.SetDbModel(db);
                    appNoteRepository.SetDbModel(db);
                    appPaymentRepository.SetDbModel(db);

                    var currentEntity = orderAppRepository.GetByID(id);

                    if (currentEntity != null)
                    {
                        foreach (var item in currentEntity.ProcureOrderAppDetail)
                        {
                            item.IsDeleted = true;
                        }

                        orderAppRepository.Delete(currentEntity);

                        var appNotes = appNoteRepository.GetList(x => x.WorkflowID == CurrentWorkFlowID && x.ApplicationID == currentEntity.ID);
                        foreach (var item in appNotes)
                        {
                            appNoteRepository.Delete(item);
                        }

                        var appPayments = appPaymentRepository.GetList(x => x.WorkflowID == CurrentWorkFlowID && x.ApplicationID == currentEntity.ID);

                        foreach (var item in appPayments)
                        {
                            appPaymentRepository.Delete(item);
                        }

                        unitOfWork.SaveChanges();
                    }
                }

                rgEntities.Rebind();
            }
        }

        protected void rgEntities_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem commandItem = e.Item as GridCommandItem;
                Panel plAddCommand = commandItem.FindControl("plAddCommand") as Panel;

                if (plAddCommand != null)
                {
                    if (this.CanAddUserIDs.Contains(CurrentUser.UserID))
                        plAddCommand.Visible = true;
                    else
                        plAddCommand.Visible = false;
                }
            }
        }

        protected void rgEntities_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (this.CanAddUserIDs.Contains(CurrentUser.UserID) || CanEditUserIDs.Contains(CurrentUser.UserID))
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = true;
            else
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = false;

            if (this.CanStopUserIDs.Contains(CurrentUser.UserID))
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_STOP).Visible = true;
            else
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_STOP).Visible = false;
        }

        protected void rgEntities_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                UIProcureOrderApplication uiEntity = (UIProcureOrderApplication)gridDataItem.DataItem;

                if (uiEntity != null)
                {
                    string linkHtml = "<a href=\"javascript:void(0);\" onclick=\"redirectToMaintenancePage(" + uiEntity.ID + ")\">";

                    var canAccessUserIDs = PageWorkflowStatusRepository.GetCanAccessUserIDsByID(this.CurrentWorkFlowID, uiEntity.WorkflowStatusID);

                    bool isCanAccessUser = false;
                    if (canAccessUserIDs.Contains(CurrentUser.UserID))
                        isCanAccessUser = true;

                    bool isCanEditUser = false;
                    if (CanEditUserIDs.Contains(CurrentUser.UserID)
                        || uiEntity.CreatedByUserID == CurrentUser.UserID)
                        isCanEditUser = true;

                    bool isShowDeleteLink = false;
                    bool isShowStopLink = false;

                    EWorkflowStatus workflowStatus = (EWorkflowStatus)uiEntity.WorkflowStatusID;

                    if (CanEditUserIDs.Contains(CurrentUser.UserID))
                    {
                        linkHtml += "编辑";

                        switch (workflowStatus)
                        {
                            case EWorkflowStatus.TemporarySave:
                            case EWorkflowStatus.ReturnBasicInfo:
                            case EWorkflowStatus.ReturnPaymentInfo:
                                isShowDeleteLink = true;
                                break;
                        }
                    }
                    else
                    {
                        if (isCanAccessUser)
                        {
                            switch (workflowStatus)
                            {
                                case EWorkflowStatus.TemporarySave:
                                case EWorkflowStatus.ReturnBasicInfo:
                                    if (isCanEditUser)
                                    {
                                        linkHtml += "编辑";
                                        isShowDeleteLink = true;
                                    }
                                    else
                                        linkHtml += "查看";
                                    break;

                                case EWorkflowStatus.Submit:
                                    linkHtml += "审核";
                                    break;

                                case EWorkflowStatus.ApprovedBasicInfo:
                                    if (isCanEditUser)
                                        linkHtml += "维护支付信息";
                                    else
                                        linkHtml += "查看";
                                    break;

                                case EWorkflowStatus.ReturnPaymentInfo:
                                    if (isCanEditUser)
                                    {
                                        linkHtml += "维护支付信息";
                                        isShowDeleteLink = true;
                                    }
                                    else
                                        linkHtml += "查看";

                                    break;

                                case EWorkflowStatus.AuditingOfPaymentInfo:
                                    linkHtml += "审核支付信息";
                                    break;

                                case EWorkflowStatus.ToBePaid:
                                    linkHtml += "支付";
                                    break;

                                case EWorkflowStatus.Paid:
                                case EWorkflowStatus.InWarehouse:
                                case EWorkflowStatus.Shipping:
                                case EWorkflowStatus.Completed:
                                    linkHtml += "查看";
                                    break;
                            }
                        }
                        else
                            linkHtml += "查看";
                    }

                    linkHtml += "</a>";

                    if (this.CanStopUserIDs.Contains(CurrentUser.UserID)
                        && uiEntity.IsStop == false)
                    {
                        switch (workflowStatus)
                        {
                            case EWorkflowStatus.Shipping:
                                isShowStopLink = true;
                                break;
                        }
                    }

                    var editColumn = rgEntities.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT);

                    if (editColumn != null)
                    {
                        var editCell = gridDataItem.Cells[editColumn.OrderIndex];

                        if (editCell != null)
                            editCell.Text = linkHtml;
                    }

                    var deleteColumn = rgEntities.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE);

                    if (deleteColumn != null)
                    {
                        var deleteCell = gridDataItem.Cells[deleteColumn.OrderIndex];

                        if (deleteCell != null && !isShowDeleteLink)
                            deleteCell.Text = string.Empty;
                    }

                    var stopColumn = rgEntities.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_STOP);

                    if (stopColumn != null)
                    {
                        var stopCell = gridDataItem.Cells[stopColumn.OrderIndex];

                        if (stopCell != null && !isShowStopLink)
                            stopCell.Text = string.Empty;
                    }
                }
            }
        }

        protected void rgEntities_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == GlobalConst.GridColumnUniqueNames.COLUMN_STOP)
            {
                GridEditableItem editableItem = e.Item as GridEditableItem;

                String sid = editableItem.GetDataKeyValue("ID").ToString();

                int id = 0;
                if (int.TryParse(sid, out id))
                {
                    var currentEntity = PageProcureOrderAppRepository.GetByID(id);

                    if (currentEntity != null)
                    {
                        currentEntity.IsStop = true;
                        currentEntity.StoppedBy = CurrentUser.UserID;
                        currentEntity.StoppedOn = DateTime.Now;

                        PageProcureOrderAppRepository.Save();
                    }
                }

                rgEntities.Rebind();
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindEntities(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpBeginDate.Clear();
            rdpEndDate.Clear();

            rcbxSupplier.ClearSelection();
            rcbxWorkflowStatus.ClearSelection();

            BindEntities(true);
        }

        
    }
}