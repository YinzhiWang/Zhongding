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

namespace ZhongDing.Web.Views.Refunds
{
    public partial class ClientRefundManagement : WorkflowBasePage
    {
        #region Members

        private IClientRefundApplicationRepository _PageClientRefundAppRepository;
        private IClientRefundApplicationRepository PageClientRefundAppRepository
        {
            get
            {
                if (_PageClientRefundAppRepository == null)
                    _PageClientRefundAppRepository = new ClientRefundApplicationRepository();

                return _PageClientRefundAppRepository;
            }
        }

        private ICompanyRepository _PageCompanyRepository;
        private ICompanyRepository PageCompanyRepository
        {
            get
            {
                if (_PageCompanyRepository == null)
                {
                    _PageCompanyRepository = new CompanyRepository();
                }
                return _PageCompanyRepository;
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

        private IClientUserRepository _PageClientUserRepository;
        private IClientUserRepository PageClientUserRepository
        {
            get
            {
                if (_PageClientUserRepository == null)
                    _PageClientUserRepository = new ClientUserRepository();

                return _PageClientUserRepository;
            }
        }

        private IClientCompanyRepository _PageClientCompanyRepository;
        private IClientCompanyRepository PageClientCompanyRepository
        {
            get
            {
                if (_PageClientCompanyRepository == null)
                    _PageClientCompanyRepository = new ClientCompanyRepository();

                return _PageClientCompanyRepository;
            }
        }

        private IList<int> _CanAddUserIDs;
        private IList<int> CanAddUserIDs
        {
            get
            {
                if (_CanAddUserIDs == null)
                    _CanAddUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewClientRefund);

                return _CanAddUserIDs;
            }
        }

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditClientRefund);

                return _CanEditUserIDs;
            }
        }


        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ClientRefundsManage;
            this.CurrentWorkFlowID = (int)EWorkflow.ClientRefunds;

            if (!IsPostBack)
            {
                BindCompanies();

                BindClientUsers();

                BindWorkflowStatus();
            }
        }

        #region Private Methods

        private void BindCompanies()
        {
            var companies = PageCompanyRepository.GetDropdownItems();
            rcbxCompany.DataSource = companies;
            rcbxCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxCompany.DataBind();

            rcbxCompany.Items.Insert(0, new RadComboBoxItem("", ""));

            rcbxApplyCompany.DataSource = companies;
            rcbxApplyCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxApplyCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxApplyCompany.DataBind();

            rcbxApplyCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindWarehouses()
        {
            rcbxWarehouse.ClearSelection();
            rcbxWarehouse.Items.Clear();

            int companyID;

            if (int.TryParse(rcbxCompany.SelectedValue, out companyID))
            {
                var uiSearchObj = new UISearchDropdownItem
                {
                    Extension = new UISearchExtension
                    {
                        CompanyID = companyID,
                        SaleTypeID = (int)ESaleType.HighPrice
                    }
                };

                var warehouses = PageWarehouseRepository.GetDropdownItems(uiSearchObj);

                rcbxWarehouse.DataSource = warehouses;
                rcbxWarehouse.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                rcbxWarehouse.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                rcbxWarehouse.DataBind();

                rcbxWarehouse.Items.Insert(0, new RadComboBoxItem("", ""));
            }
        }

        private void BindClientUsers()
        {
            var clientUsers = PageClientUserRepository.GetDropdownItems(new UISearchDropdownItem
            {
                Extension = new UISearchExtension { OnlyIncludeValidClientUser = true }
            });

            rcbxClientUser.DataSource = clientUsers;
            rcbxClientUser.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientUser.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientUser.DataBind();

            rcbxClientUser.Items.Insert(0, new RadComboBoxItem("", ""));

            rcbxApplyClientUser.DataSource = clientUsers;
            rcbxApplyClientUser.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxApplyClientUser.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxApplyClientUser.DataBind();

            rcbxApplyClientUser.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindClientCompanies()
        {
            rcbxClientCompany.ClearSelection();
            rcbxClientCompany.Items.Clear();

            var uiSearchObj = new UISearchDropdownItem();

            if (!string.IsNullOrEmpty(rcbxClientUser.SelectedValue))
            {
                int clientUserID;
                if (int.TryParse(rcbxClientUser.SelectedValue, out clientUserID))
                    uiSearchObj.Extension = new UISearchExtension { ClientUserID = clientUserID };
            }

            var clientCompanies = PageClientCompanyRepository.GetDropdownItems(uiSearchObj);
            rcbxClientCompany.DataSource = clientCompanies;
            rcbxClientCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientCompany.DataBind();

            rcbxClientCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindNeedRefundOrders(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchClientNeedRefundOrder();

            if (!string.IsNullOrEmpty(rcbxCompany.SelectedValue))
            {
                int companyID;
                if (int.TryParse(rcbxCompany.SelectedValue, out companyID))
                    uiSearchObj.CompanyID = companyID;
            }

            if (!string.IsNullOrEmpty(rcbxWarehouse.SelectedValue))
            {
                int warehouseID;
                if (int.TryParse(rcbxWarehouse.SelectedValue, out warehouseID))
                    uiSearchObj.WarehouseID = warehouseID;
            }

            if (!string.IsNullOrEmpty(rcbxClientUser.SelectedValue))
            {
                int clientUserID;
                if (int.TryParse(rcbxClientUser.SelectedValue, out clientUserID))
                    uiSearchObj.ClientUserID = clientUserID;
            }

            if (!string.IsNullOrEmpty(rcbxClientCompany.SelectedValue))
            {
                int clientCompanyID;
                if (int.TryParse(rcbxClientCompany.SelectedValue, out clientCompanyID))
                    uiSearchObj.ClientCompanyID = clientCompanyID;
            }

            int totalRecords;

            var entities = PageClientRefundAppRepository.GetNeedRefundOrders(uiSearchObj, rgOrders.CurrentPageIndex, rgOrders.PageSize, out totalRecords);

            rgOrders.VirtualItemCount = totalRecords;

            rgOrders.DataSource = entities;

            if (isNeedRebind)
                rgOrders.Rebind();
        }

        private void BindApplyWarehouses()
        {
            rcbxApplyWarehouse.ClearSelection();
            rcbxApplyWarehouse.Items.Clear();

            int companyID;

            if (int.TryParse(rcbxApplyCompany.SelectedValue, out companyID))
            {
                var uiSearchObj = new UISearchDropdownItem
                {
                    Extension = new UISearchExtension
                    {
                        CompanyID = companyID,
                        SaleTypeID = (int)ESaleType.HighPrice
                    }
                };

                var warehouses = PageWarehouseRepository.GetDropdownItems(uiSearchObj);

                rcbxApplyWarehouse.DataSource = warehouses;
                rcbxApplyWarehouse.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                rcbxApplyWarehouse.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                rcbxApplyWarehouse.DataBind();

                rcbxApplyWarehouse.Items.Insert(0, new RadComboBoxItem("", ""));
            }
        }

        private void BindApplyClientCompanies()
        {
            rcbxApplyClientCompany.ClearSelection();
            rcbxApplyClientCompany.Items.Clear();

            var uiSearchObj = new UISearchDropdownItem();

            if (!string.IsNullOrEmpty(rcbxApplyClientUser.SelectedValue))
            {
                int clientUserID;
                if (int.TryParse(rcbxApplyClientUser.SelectedValue, out clientUserID))
                    uiSearchObj.Extension = new UISearchExtension { ClientUserID = clientUserID };
            }

            var clientCompanies = PageClientCompanyRepository.GetDropdownItems(uiSearchObj);
            rcbxApplyClientCompany.DataSource = clientCompanies;
            rcbxApplyClientCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxApplyClientCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxApplyClientCompany.DataBind();

            rcbxApplyClientCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindClientRefunds(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchClientRefundApplication
            {
                BeginDate = rdpApplyBeginDate.SelectedDate,
                EndDate = rdpApplyEndDate.SelectedDate,
            };

            if (!string.IsNullOrEmpty(rcbxApplyCompany.SelectedValue))
            {
                int companyID;
                if (int.TryParse(rcbxApplyCompany.SelectedValue, out companyID))
                    uiSearchObj.CompanyID = companyID;
            }

            if (!string.IsNullOrEmpty(rcbxApplyWarehouse.SelectedValue))
            {
                int warehouseID;
                if (int.TryParse(rcbxApplyWarehouse.SelectedValue, out warehouseID))
                    uiSearchObj.WarehouseID = warehouseID;
            }

            if (!string.IsNullOrEmpty(rcbxApplyClientUser.SelectedValue))
            {
                int clientUserID;
                if (int.TryParse(rcbxApplyClientUser.SelectedValue, out clientUserID))
                    uiSearchObj.ClientUserID = clientUserID;
            }

            if (!string.IsNullOrEmpty(rcbxApplyClientCompany.SelectedValue))
            {
                int clientCompanyID;
                if (int.TryParse(rcbxApplyClientCompany.SelectedValue, out clientCompanyID))
                    uiSearchObj.ClientCompanyID = clientCompanyID;
            }

            IList<int> includeWorkflowStatusIDs = PageWorkflowStatusRepository
                .GetCanAccessIDsByUserID(this.CurrentWorkFlowID, CurrentUser.UserID);

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

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ReturnBasicInfo))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ReturnBasicInfo);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ApprovedByTreasurers))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ApprovedByTreasurers);

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

            var uiEntities = PageClientRefundAppRepository.GetUIList(uiSearchObj, rgClientRefunds.CurrentPageIndex, rgClientRefunds.PageSize, out totalRecords);

            rgClientRefunds.VirtualItemCount = totalRecords;
            rgClientRefunds.DataSource = uiEntities;

            if (isNeedRebind)
                rgClientRefunds.Rebind();
        }

        private void BindWorkflowStatus()
        {
            var uiSearchObj = new UISearchDropdownItem();

            IList<int> includeItemValues = new List<int>();
            includeItemValues.Add((int)EWorkflowStatus.TemporarySave);
            includeItemValues.Add((int)EWorkflowStatus.Submit);
            includeItemValues.Add((int)EWorkflowStatus.ReturnBasicInfo);
            includeItemValues.Add((int)EWorkflowStatus.ApprovedByTreasurers);
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

        #endregion

        #region Need refund orders

        protected void rcbxCompany_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindWarehouses();
        }

        protected void rcbxClientUser_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindClientCompanies();
        }

        protected void btnSearchOrder_Click(object sender, EventArgs e)
        {
            BindNeedRefundOrders(true);
        }

        protected void btnResetOrder_Click(object sender, EventArgs e)
        {
            rcbxCompany.ClearSelection();

            rcbxWarehouse.ClearSelection();
            rcbxWarehouse.Items.Clear();

            rcbxClientUser.ClearSelection();

            rcbxClientCompany.ClearSelection();
            rcbxClientCompany.Items.Clear();

            BindNeedRefundOrders(true);

        }

        protected void rgOrders_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindNeedRefundOrders(false);
        }

        protected void rgOrders_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                var uiEntity = (UIClientNeedRefundOrder)gridDataItem.DataItem;

                if (uiEntity != null)
                {
                    if (uiEntity.IsGuaranteed
                        && !uiEntity.IsReceiptedGuaranteeAmount)
                    {
                        var editColumn = rgOrders.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT);

                        if (editColumn != null)
                        {
                            var editCell = gridDataItem.Cells[editColumn.OrderIndex];

                            if (editCell != null)
                                editCell.Text = string.Empty;
                        }
                    }
                }
            }
        }

        protected void rgOrders_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (this.CanAddUserIDs.Contains(CurrentUser.UserID) || CanEditUserIDs.Contains(CurrentUser.UserID))
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = true;
            else
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT).Visible = false;
        }

        #endregion

        #region Client refunds

        protected void rcbxApplyCompany_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindApplyWarehouses();
        }

        protected void rcbxApplyClientUser_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindApplyClientCompanies();
        }

        protected void btnSearchClientRefunds_Click(object sender, EventArgs e)
        {
            BindClientRefunds(true);
        }

        protected void btnResetClientRefunds_Click(object sender, EventArgs e)
        {
            rcbxApplyCompany.ClearSelection();

            rcbxApplyWarehouse.ClearSelection();
            rcbxApplyWarehouse.Items.Clear();

            rcbxApplyClientUser.ClearSelection();

            rcbxApplyClientCompany.ClearSelection();
            rcbxApplyClientCompany.Items.Clear();

            BindClientRefunds(true);
        }

        protected void rgClientRefunds_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindClientRefunds(false);
        }

        protected void rgClientRefunds_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                var uiEntity = (UIClientRefundApplication)gridDataItem.DataItem;

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
                                        linkHtml += "编辑";
                                    else
                                        linkHtml += "查看";
                                    isShowDeleteLink = true;
                                    break;

                                case EWorkflowStatus.Submit:
                                case EWorkflowStatus.ApprovedByTreasurers:
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



                    var editColumn = rgClientRefunds.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT);

                    if (editColumn != null)
                    {
                        var editCell = gridDataItem.Cells[editColumn.OrderIndex];

                        if (editCell != null)
                            editCell.Text = linkHtml;
                    }

                    var deleteColumn = rgClientRefunds.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE);

                    if (deleteColumn != null)
                    {
                        var deleteCell = gridDataItem.Cells[deleteColumn.OrderIndex];

                        if (deleteCell != null && !isShowDeleteLink)
                            deleteCell.Text = string.Empty;
                    }
                }
            }
        }

        protected void rgClientRefunds_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (this.CanAddUserIDs.Contains(CurrentUser.UserID) || CanEditUserIDs.Contains(CurrentUser.UserID))
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = true;
            else
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = false;
        }

        #endregion
    }
}