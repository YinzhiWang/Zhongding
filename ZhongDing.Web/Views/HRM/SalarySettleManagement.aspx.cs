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

namespace ZhongDing.Web.Views.HRM
{
    public partial class SalarySettleManagement : WorkflowBasePage
    {
        protected override EWorkflow PagePermissionWorkflowID()
        {
            return EWorkflow.SalarySettleManagement;
        }
        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.SalarySettleManagement;
        }
        private ISalarySettleRepository _PageSalarySettleRepository;
        private ISalarySettleRepository PageSalarySettleRepository
        {
            get
            {
                if (_PageSalarySettleRepository == null)
                    _PageSalarySettleRepository = new SalarySettleRepository();

                return _PageSalarySettleRepository;
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
            this.Master.MenuItemID = (int)EMenuItem.SalarySettleManage;
            if (!IsPostBack)
            {
                BindDepartments();
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
                    _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewSalarySettle);
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
                    _CanAddUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewSalarySettle);

                return _CanAddUserIDs;
            }
        }
        private void BindSalarySettle(bool isNeedRebind)
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

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ApprovedByAdministrations))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ApprovedByAdministrations);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ApprovedByDeptManagers))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ApprovedByDeptManagers);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ApprovedByTreasurers))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ApprovedByTreasurers);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.Completed))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.Completed);
                }
            }

            UISearchSalarySettle uiSearchObj = new UISearchSalarySettle()
            {
                BeginDate = rdpBeginDate.SelectedDate.HasValue ? (new DateTime(rdpBeginDate.SelectedDate.Value.Year, rdpBeginDate.SelectedDate.Value.Month, 1)) : rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate.HasValue ? (new DateTime(rdpEndDate.SelectedDate.Value.Year, rdpEndDate.SelectedDate.Value.Month, 1)) : rdpEndDate.SelectedDate,
                //UnIncludeWorkflowStatusIDs = new List<int>() { (int)EWorkflowStatus.Paid },
                //NeedStatistics = true,
                //ClientName = txtClientName.Text.Trim(),
                //ProductName = txtProductName.Text.Trim(),
                DepartmentID = rcbxDepartment.SelectedValue.ToIntOrNull()

            };
            uiSearchObj.IncludeWorkflowStatusIDs = includeWorkflowStatusIDs;

            int totalRecords = 0;

            var uiSalarySettles = PageSalarySettleRepository.GetUIList(uiSearchObj, rgSalarySettles.CurrentPageIndex, rgSalarySettles.PageSize, out totalRecords);

            rgSalarySettles.VirtualItemCount = totalRecords;

            rgSalarySettles.DataSource = uiSalarySettles;

            if (isNeedRebind)
                rgSalarySettles.Rebind();
        }


        protected void rgSalarySettles_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindSalarySettle(false);
        }

        protected void rgSalarySettles_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;
            var id = editableItem.GetDataKeyValue("ID").ToIntOrNull();

            if (id.BiggerThanZero())
            {
                PageSalarySettleRepository.DeleteByID(id);
                PageSalarySettleRepository.Save();
                rgSalarySettles.Rebind();
            }


        }

        protected void rgSalarySettles_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
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

        protected void rgSalarySettles_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindSalarySettle(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpEndDate.SelectedDate = rdpBeginDate.SelectedDate = null;
            rcbxDepartment.SelectedValue = string.Empty;
            BindSalarySettle(true);
        }

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditSalarySettle);

                return _CanEditUserIDs;
            }
        }
        protected void rgSalarySettles_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
              || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                var uiEntity = (UISalarySettle)gridDataItem.DataItem;

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
                                case EWorkflowStatus.ApprovedByAdministrations:
                                case EWorkflowStatus.ApprovedByTreasurers:
                                    linkHtml += "审核";
                                    break;
                                case EWorkflowStatus.ApprovedByDeptManagers:
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

                    var editColumn = rgSalarySettles.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT);

                    if (editColumn != null)
                    {
                        var editCell = gridDataItem.Cells[editColumn.OrderIndex];

                        if (editCell != null)
                            editCell.Text = linkHtml;
                    }

                    var deleteColumn = rgSalarySettles.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE);

                    if (deleteColumn != null)
                    {
                        var deleteCell = gridDataItem.Cells[deleteColumn.OrderIndex];

                        if (deleteCell != null && !isShowDeleteLink)
                            deleteCell.Text = string.Empty;
                    }

                    //var stopColumn = rgSalarySettleReturnApplications.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_STOP);

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