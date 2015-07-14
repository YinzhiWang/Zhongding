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
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Common.Extension;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Web.Views.Basics
{
    public partial class ReimbursementManagement : WorkflowBasePage
    {
        protected override EWorkflow PagePermissionWorkflowID()
        {
            return EWorkflow.ReimbursementManagement;
        }
        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.ReimbursementManagement;
        }
        private IReimbursementRepository _PageReimbursementRepository;
        private IReimbursementRepository PageReimbursementRepository
        {
            get
            {
                if (_PageReimbursementRepository == null)
                    _PageReimbursementRepository = new ReimbursementRepository();

                return _PageReimbursementRepository;
            }
        }


        private IDepartmentRepository _PageDepartmentRepository;
        private IDepartmentRepository PageDepartmentRepository
        {
            get
            {
                if (_PageDepartmentRepository == null)
                    _PageDepartmentRepository = new DepartmentRepository();

                return _PageDepartmentRepository;
            }
        }




        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ReimbursementManage;
            if (!IsPostBack)
            {
                BindDepartments();
                BindWorkflowStatus();
            }
        }
        private void BindDepartments()
        {
            var departments = PageDepartmentRepository.GetDropdownItems();

            rcbxDepartment.DataSource = departments;
            rcbxDepartment.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDepartment.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDepartment.DataBind();

            rcbxDepartment.Items.Insert(0, new RadComboBoxItem("", ""));
        }
        private IList<int> _CanAccessUserIDs;
        private IList<int> CanAccessUserIDs
        {
            get
            {
                if (_CanAccessUserIDs == null || _CanAccessUserIDs.Count == 0)
                {
                    _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewReimbursement);
                }

                return _CanAccessUserIDs;
            }
        }
        private IList<int> _CanAddUserIDs;
        /// <summary>
        /// 可新增的用户
        /// </summary>
        private IList<int> CanAddUserIDs
        {
            get
            {
                if (_CanAddUserIDs == null)
                    _CanAddUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewReimbursement);

                return _CanAddUserIDs;
            }
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
        private void BindReimbursement(bool isNeedRebind)
        {
            IList<int> includeWorkflowStatusIDs = PageWorkflowStatusRepository.GetCanAccessIDsByUserID(this.CurrentWorkFlowID, CurrentUser.UserID);

            if (includeWorkflowStatusIDs == null)
            {
                includeWorkflowStatusIDs = new List<int>();
                includeWorkflowStatusIDs.Add((int)EWorkflowStatus.Completed);
            }
            else
            {
                if (this.CanAccessUserIDs.Contains(CurrentUser.UserID) || this.CanEditUserIDs.Contains(CurrentUser.UserID))
                {
                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.TemporarySave))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.TemporarySave);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.Submit))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.Submit);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ReturnBasicInfo))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ReturnBasicInfo);



                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ApprovedByDeptManagers))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ApprovedByDeptManagers);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ApprovedByTreasurers))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ApprovedByTreasurers);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ApprovedByTreasurers))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ApprovedByTreasurers);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.Completed))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.Completed);
                }
            }

            UISearchReimbursement uiSearchObj = new UISearchReimbursement()
            {
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
                //UnIncludeWorkflowStatusIDs = new List<int>() { (int)EWorkflowStatus.Paid },
                //NeedStatistics = true,
                //ClientName = txtClientName.Text.Trim(),
                //ProductName = txtProductName.Text.Trim(),
                //DepartmentID = rcbxDepartment.SelectedValue.ToIntOrNull()

            };
            uiSearchObj.IncludeWorkflowStatusIDs = includeWorkflowStatusIDs;

            int totalRecords = 0;

            var uiReimbursements = PageReimbursementRepository.GetUIList(uiSearchObj, rgReimbursements.CurrentPageIndex, rgReimbursements.PageSize, out totalRecords);

            rgReimbursements.VirtualItemCount = totalRecords;

            rgReimbursements.DataSource = uiReimbursements;

            if (isNeedRebind)
                rgReimbursements.Rebind();
        }


        protected void rgReimbursements_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindReimbursement(false);
        }

        protected void rgReimbursements_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;
            var id = editableItem.GetDataKeyValue("ID").ToIntOrNull();

            if (id.BiggerThanZero())
            {
                PageReimbursementRepository.DeleteByID(id);
                PageReimbursementRepository.Save();
                rgReimbursements.Rebind();
            }


        }

        protected void rgReimbursements_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
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

        protected void rgReimbursements_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindReimbursement(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpEndDate.SelectedDate = rdpBeginDate.SelectedDate = null;
            rcbxDepartment.SelectedValue = string.Empty;
            BindReimbursement(true);
        }

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditReimbursement);

                return _CanEditUserIDs;
            }
        }
        protected void rgReimbursements_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
              || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                var uiEntity = (UIReimbursement)gridDataItem.DataItem;

                if (uiEntity != null)
                {
                    string linkHtml = "<a href=\"javascript:void(0);\" onclick=\"redirectToMaintenancePage(" + uiEntity.ID + ")\">";

                    var canAccessUserIDs = PageWorkflowStatusRepository.GetCanAccessUserIDsByID(CurrentWorkFlowID, uiEntity.WorkflowStatusID);

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
                                case EWorkflowStatus.ApprovedByDeptManagers:
                                case EWorkflowStatus.ApprovedByTreasurers:
                                    linkHtml += "审核";
                                    break;
                                case EWorkflowStatus.ApprovedByManagers:
                                    linkHtml += "支付";
                                    break;
                            }
                        }
                        else
                            linkHtml += "查看";
                    }

                    linkHtml += "</a>";

                    //if (this.CanStopUserIDs.Contains(CurrentUser.UserID)
                    //    && uiEntity.IsStop == false)
                    //{
                    //    switch (workflowStatus)
                    //    {
                    //        case EWorkflowStatus.ApprovedBasicInfo:
                    //        case EWorkflowStatus.Shipping:
                    //            isShowStopLink = true;
                    //            break;
                    //    }
                    //}

                    var editColumn = rgReimbursements.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT);

                    if (editColumn != null)
                    {
                        var editCell = gridDataItem.Cells[editColumn.OrderIndex];

                        if (editCell != null)
                            editCell.Text = linkHtml;
                    }

                    var deleteColumn = rgReimbursements.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE);

                    if (deleteColumn != null)
                    {
                        var deleteCell = gridDataItem.Cells[deleteColumn.OrderIndex];

                        if (deleteCell != null && !isShowDeleteLink)
                            deleteCell.Text = string.Empty;
                    }

                    //var stopColumn = rgReimbursementReturnApplications.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_STOP);

                    //if (stopColumn != null)
                    //{
                    //    var stopCell = gridDataItem.Cells[stopColumn.OrderIndex];

                    //    if (stopCell != null && !isShowStopLink)
                    //        stopCell.Text = string.Empty;
                    //}
                }
            }
        }
    }
}