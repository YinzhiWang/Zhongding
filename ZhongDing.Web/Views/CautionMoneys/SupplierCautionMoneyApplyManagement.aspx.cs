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
    public partial class SupplierCautionMoneyApplyManagement : WorkflowBasePage
    {
        #region Members
        protected override EWorkflow PagePermissionWorkflowID()
        {
            return EWorkflow.SupplierCautionMoneyApply;
        }
        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.SupplierCautionMoneyApply;
        }
        private ISupplierCautionMoneyRepository _PageSupplierCautionMoneyRepository;
        private ISupplierCautionMoneyRepository PageSupplierCautionMoneyRepository
        {
            get
            {
                if (_PageSupplierCautionMoneyRepository == null)
                    _PageSupplierCautionMoneyRepository = new SupplierCautionMoneyRepository();

                return _PageSupplierCautionMoneyRepository;
            }
        }

        private IWorkflowStatusRepository _PageWorkflowStatusRepository;
        protected IWorkflowStatusRepository PageWorkflowStatusRepository
        {
            get
            {
                if (_PageWorkflowStatusRepository == null)
                    _PageWorkflowStatusRepository = new WorkflowStatusRepository();

                return _PageWorkflowStatusRepository;
            }
        }
        private IWorkflowStepRepository _PageWorkflowStepRepository;
        protected IWorkflowStepRepository PageWorkflowStepRepository
        {
            get
            {
                if (_PageWorkflowStepRepository == null)
                    _PageWorkflowStepRepository = new WorkflowStepRepository();

                return _PageWorkflowStepRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.SupplierCautionMoneyApplyManage;
            if (!IsPostBack)
            {

            }
        }
        private IList<int> _CanAccessUserIDs;
        private IList<int> CanAccessUserIDs
        {
            get
            {
                if (_CanAccessUserIDs == null || _CanAccessUserIDs.Count == 0)
                {
                    _CanAccessUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewSupplierCautionMoneyApply);
                }

                return _CanAccessUserIDs;
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
                    _CanAddUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewSupplierCautionMoneyApply);

                return _CanAddUserIDs;
            }
        }

        private void BindSupplierCautionMoney(bool isNeedRebind)
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
            UISearchSupplierCautionMoney uiSearchObj = new UISearchSupplierCautionMoney()
            {
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
                //WorkflowStatusIDs = new int[] { (int)EWorkflowStatus.TemporarySave, (int)EWorkflowStatus.ReturnBasicInfo,
                //(int)EWorkflowStatus.Submit,(int)EWorkflowStatus.ApprovedByDeptManagers,
                //(int)EWorkflowStatus.ApprovedByTreasurers},
                SupplierName = txtSupplierName.Text.Trim(),
                ProductName = txtProductName.Text.Trim()
            };
            uiSearchObj.WorkflowStatusIDs = includeWorkflowStatusIDs.ToArray();



            int totalRecords = 0;

            var uiSupplierCautionMoneys = PageSupplierCautionMoneyRepository.GetUIList(uiSearchObj, rgSupplierCautionMoneys.CurrentPageIndex, rgSupplierCautionMoneys.PageSize, out totalRecords);

            rgSupplierCautionMoneys.VirtualItemCount = totalRecords;

            rgSupplierCautionMoneys.DataSource = uiSupplierCautionMoneys;


            if (isNeedRebind)
                rgSupplierCautionMoneys.Rebind();
        }


        protected void rgSupplierCautionMoneys_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindSupplierCautionMoney(false);
        }

        protected void rgSupplierCautionMoneys_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;
            var supplierCautionMoneyID = editableItem.GetDataKeyValue("ID").ToIntOrNull();

            if (supplierCautionMoneyID.BiggerThanZero())
            {
                PageSupplierCautionMoneyRepository.DeleteByID(supplierCautionMoneyID);
                PageSupplierCautionMoneyRepository.Save();
                rgSupplierCautionMoneys.Rebind();
            }


        }

        protected void rgSupplierCautionMoneys_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
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

        protected void rgSupplierCautionMoneys_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }
        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditSupplierCautionMoneyApply);

                return _CanEditUserIDs;
            }
        }
        protected void rgSupplierCautionMoneys_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
              || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                var uiEntity = (UISupplierCautionMoney)gridDataItem.DataItem;

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

                    var editColumn = rgSupplierCautionMoneys.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT);

                    if (editColumn != null)
                    {
                        var editCell = gridDataItem.Cells[editColumn.OrderIndex];

                        if (editCell != null)
                            editCell.Text = linkHtml;
                    }

                    var deleteColumn = rgSupplierCautionMoneys.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE);

                    if (deleteColumn != null)
                    {
                        var deleteCell = gridDataItem.Cells[deleteColumn.OrderIndex];

                        if (deleteCell != null && !isShowDeleteLink)
                            deleteCell.Text = string.Empty;
                    }

                    //var stopColumn = rgSupplierCautionMoneys.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_STOP);

                    //if (stopColumn != null)
                    //{
                    //    var stopCell = gridDataItem.Cells[stopColumn.OrderIndex];

                    //    if (stopCell != null && !isShowStopLink)
                    //        stopCell.Text = string.Empty;
                    //}
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindSupplierCautionMoney(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpEndDate.SelectedDate = rdpBeginDate.SelectedDate = null;

            txtProductName.Text = txtSupplierName.Text = string.Empty;

            BindSupplierCautionMoney(true);
        }
    }
}