using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Common.Extension;
using Telerik.Web.UI;
using ZhongDing.Web.Extensions;
using ZhongDing.Common;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Web.Views.CautionMoneys
{
    public partial class ClientCautionMoneyReturnApplyManagement : WorkflowBasePage
    {
        #region Members
        protected override EWorkflow PagePermissionWorkflowID()
        {
            return EWorkflow.ClientCautionMoneyReturnApply;
        }
        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.ClientCautionMoneyReturnApply;
        }
        private IClientCautionMoneyRepository _PageClientCautionMoneyRepository;
        private IClientCautionMoneyRepository PageClientCautionMoneyRepository
        {
            get
            {
                if (_PageClientCautionMoneyRepository == null)
                    _PageClientCautionMoneyRepository = new ClientCautionMoneyRepository();

                return _PageClientCautionMoneyRepository;
            }
        }
        private IClientCautionMoneyReturnApplicationRepository _PageClientCautionMoneyReturnApplicationRepository;
        private IClientCautionMoneyReturnApplicationRepository PageClientCautionMoneyReturnApplicationRepository
        {
            get
            {
                if (_PageClientCautionMoneyReturnApplicationRepository == null)
                    _PageClientCautionMoneyReturnApplicationRepository = new ClientCautionMoneyReturnApplicationRepository();

                return _PageClientCautionMoneyReturnApplicationRepository;
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


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ClientCautionMoneyReturnApplyManage;
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
                    _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewClientCautionMoneyReturnApply);
                }

                return _CanAccessUserIDs;
            }
        }
        private void BindClientCautionMoney(bool isNeedRebind)
        {
            IList<int> includeWorkflowStatusIDs = PageWorkflowStatusRepository
       .GetCanAccessIDsByUserID(this.CurrentWorkFlowID, CurrentUser.UserID);

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

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.Completed))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.Completed);
                }
            }

            UISearchClientCautionMoneyReturnApplication uiSearchObj = new UISearchClientCautionMoneyReturnApplication()
            {
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
                //UnIncludeWorkflowStatusIDs = new List<int>() { (int)EWorkflowStatus.Paid },
                //NeedStatistics = true,
                ClientName = txtClientName.Text.Trim(),
                ProductName = txtProductName.Text.Trim(),
                DepartmentID = rcbxDepartment.SelectedValue.ToIntOrNull()
            };
            uiSearchObj.IncludeWorkflowStatusIDs = includeWorkflowStatusIDs;

            int totalRecords = 0;

            var uiClientCautionMoneys = PageClientCautionMoneyReturnApplicationRepository.GetUIListForClientCautionMoneyReturnApplyManagement(uiSearchObj, rgClientCautionMoneys.CurrentPageIndex, rgClientCautionMoneys.PageSize, out totalRecords);

            rgClientCautionMoneys.VirtualItemCount = totalRecords;

            rgClientCautionMoneys.DataSource = uiClientCautionMoneys;

            if (isNeedRebind)
                rgClientCautionMoneys.Rebind();
        }


        protected void rgClientCautionMoneys_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindClientCautionMoney(false);
        }

        protected void rgClientCautionMoneys_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;
            var ClientCautionMoneyID = editableItem.GetDataKeyValue("ID").ToIntOrNull();

            if (ClientCautionMoneyID.BiggerThanZero())
            {

                PageClientCautionMoneyRepository.Save();
                rgClientCautionMoneys.Rebind();
            }


        }

        protected void rgClientCautionMoneys_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgClientCautionMoneys_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }


        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindClientCautionMoney(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpEndDate.SelectedDate = rdpBeginDate.SelectedDate = null;
            rcbxDepartment.SelectedValue = string.Empty;

            txtProductName.Text = txtClientName.Text = string.Empty;

            BindClientCautionMoney(true);
        }

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditClientCautionMoneyReturnApply);

                return _CanEditUserIDs;
            }
        }
        protected void rgClientCautionMoneys_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
              || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                var uiEntity = (UIClientCautionMoneyReturnApplication)gridDataItem.DataItem;

                if (uiEntity != null)
                {
                    string linkHtml = "<a href=\"javascript:void(0);\" onclick=\"redirectToClientCautionMoneyReturnApplyMaintenancePage(" + uiEntity.ID + "," + uiEntity.ClientCautionMoneyID + ")\">";

                    var canAccessUserIDs = PageWorkflowStatusRepository.GetCanAccessUserIDsByID((int)EWorkflow.ClientCautionMoneyReturnApply, uiEntity.WorkflowStatusID);

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
                                    linkHtml += "审核";
                                    break;
                                case EWorkflowStatus.ApprovedByTreasurers:
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

                    var editColumn = rgClientCautionMoneys.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT);

                    if (editColumn != null)
                    {
                        var editCell = gridDataItem.Cells[editColumn.OrderIndex];

                        if (editCell != null)
                            editCell.Text = linkHtml;
                    }

                    var deleteColumn = rgClientCautionMoneys.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE);

                    if (deleteColumn != null)
                    {
                        var deleteCell = gridDataItem.Cells[deleteColumn.OrderIndex];

                        if (deleteCell != null && !isShowDeleteLink)
                            deleteCell.Text = string.Empty;
                    }

                    //var stopColumn = rgClientCautionMoneyReturnApplications.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_STOP);

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