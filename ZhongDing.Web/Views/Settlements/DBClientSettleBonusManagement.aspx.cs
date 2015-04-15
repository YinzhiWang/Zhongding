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

namespace ZhongDing.Web.Views.Settlements
{
    public partial class DBClientSettleBonusManagement : WorkflowBasePage
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

        private IHospitalTypeRepository _PageHospitalTypeRepository;
        private IHospitalTypeRepository PageHospitalTypeRepository
        {
            get
            {
                if (_PageHospitalTypeRepository == null)
                    _PageHospitalTypeRepository = new HospitalTypeRepository();

                return _PageHospitalTypeRepository;
            }
        }

        private IList<int> _CanAddUserIDs;
        private IList<int> CanAddUserIDs
        {
            get
            {
                if (_CanAddUserIDs == null)
                    _CanAddUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.ApplyDBClientSettleBonus);

                return _CanAddUserIDs;
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
                BindHospitalTypes();

                BindWorkflowStatus();
            }

        }

        #region Private Methods

        private void BindHospitalTypes()
        {
            var clientUsers = PageHospitalTypeRepository.GetDropdownItems();
            rcbxHospitalType.DataSource = clientUsers;
            rcbxHospitalType.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxHospitalType.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxHospitalType.DataBind();

            rcbxHospitalType.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindWorkflowStatus()
        {
            var uiSearchObj = new UISearchDropdownItem();

            IList<int> includeItemValues = new List<int>();
            includeItemValues.Add((int)EWorkflowStatus.ToBeSettle);
            includeItemValues.Add((int)EWorkflowStatus.Submit);
            includeItemValues.Add((int)EWorkflowStatus.ReturnBasicInfo);
            includeItemValues.Add((int)EWorkflowStatus.ApprovedByDeptManagers);
            includeItemValues.Add((int)EWorkflowStatus.Paid);
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
            var uiSearchObj = new UISearchDBClientSettlement();

            if (!string.IsNullOrEmpty(rcbxHospitalType.SelectedValue))
            {
                int hospitalTypeID;
                if (int.TryParse(rcbxHospitalType.SelectedValue, out hospitalTypeID))
                    uiSearchObj.HospitalTypeID = hospitalTypeID;
            }

            if (rmypSettlementDate.SelectedDate.HasValue)
            {
                var tempSettlementDate = rmypSettlementDate.SelectedDate.Value;
                uiSearchObj.SettlementDate = new DateTime(tempSettlementDate.Year, tempSettlementDate.Month, 1);
            }

            IList<int> includeWorkflowStatusIDs = PageWorkflowStatusRepository
                .GetCanAccessIDsByUserID(this.CurrentWorkFlowID, CurrentUser.UserID);

            if (includeWorkflowStatusIDs == null)
            {
                includeWorkflowStatusIDs = new List<int>();
                includeWorkflowStatusIDs.Add((int)EWorkflowStatus.Completed);
            }
            else
            {
                if (this.CanAddUserIDs.Contains(CurrentUser.UserID) || this.CanEditUserIDs.Contains(CurrentUser.UserID))
                {
                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ToBeSettle))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ToBeSettle);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.Submit))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.Submit);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ReturnBasicInfo))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ReturnBasicInfo);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ApprovedByDeptManagers))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ApprovedByDeptManagers);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.Paid))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.Paid);
                }
            }

            uiSearchObj.IncludeWorkflowStatusIDs = includeWorkflowStatusIDs;

            if (!string.IsNullOrEmpty(rcbxWorkflowStatus.SelectedValue))
            {
                int workflowStatusID;
                if (int.TryParse(rcbxWorkflowStatus.SelectedValue, out workflowStatusID))
                    uiSearchObj.WorkflowStatusID = workflowStatusID;
            }

            int totalRecords;

            var uiEntities = PageDBClientSettlementRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;
            rgEntities.DataSource = uiEntities;

            if (isNeedRebind)
                rgEntities.Rebind();
        }

        #endregion


        protected void rgEntities_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindEntities(false);
        }

        protected void rgEntities_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                var uiEntity = (UIDBClientSettlement)gridDataItem.DataItem;

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

                    EWorkflowStatus workflowStatus = (EWorkflowStatus)uiEntity.WorkflowStatusID;

                    if (CanEditUserIDs.Contains(CurrentUser.UserID))
                    {
                        linkHtml += "编辑";
                    }
                    else
                    {
                        if (isCanAccessUser)
                        {
                            switch (workflowStatus)
                            {
                                case EWorkflowStatus.ToBeSettle:
                                        linkHtml += "申请结算";
                                    break;
                                case EWorkflowStatus.ReturnBasicInfo:
                                    if (isCanEditUser)
                                        linkHtml += "编辑";
                                    else
                                        linkHtml += "查看";
                                    break;

                                case EWorkflowStatus.Submit:
                                        linkHtml += "审核";
                                    break;

                                case EWorkflowStatus.ApprovedByDeptManagers:
                                        linkHtml += "支付";
                                    break;
                                case EWorkflowStatus.Paid:
                                    linkHtml += "查看";
                                    break;
                            }
                        }
                        else
                            linkHtml += "查看";
                    }

                    linkHtml += "</a>";


                    var editColumn = rgEntities.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT);

                    if (editColumn != null)
                    {
                        var editCell = gridDataItem.Cells[editColumn.OrderIndex];

                        if (editCell != null)
                            editCell.Text = linkHtml;
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindEntities(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rmypSettlementDate.Clear();
            rcbxHospitalType.ClearSelection();
            rcbxWorkflowStatus.ClearSelection();

            BindEntities(true);
        }
    }
}