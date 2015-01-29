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

namespace ZhongDing.Web.Views.Sales
{
    public partial class ClientSaleAppStockOutManagement : WorkflowBasePage
    {
        #region Members

        private IStockOutRepository _PageStockOutRepository;
        private IStockOutRepository PageStockOutRepository
        {
            get
            {
                if (_PageStockOutRepository == null)
                    _PageStockOutRepository = new StockOutRepository();

                return _PageStockOutRepository;
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
                    _CanAddUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewClientStockOut);

                return _CanAddUserIDs;
            }
        }

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditClientStockOut);

                return _CanEditUserIDs;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ClientOrderStockOutManage;
            this.CurrentWorkFlowID = (int)EWorkflow.ClientOrderStockOut;

            if (!IsPostBack)
            {
                BindClientUsers();

                BindWorkflowStatus();
            }
        }

        #region Private Methods

        private void BindClientUsers()
        {
            var clientUsers = PageClientUserRepository.GetDropdownItems();
            rcbxClientUser.DataSource = clientUsers;
            rcbxClientUser.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientUser.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientUser.DataBind();

            rcbxClientUser.Items.Insert(0, new RadComboBoxItem("", ""));
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

        private void BindWorkflowStatus()
        {
            var uiSearchObj = new UISearchDropdownItem();

            IList<int> includeItemValues = new List<int>();
            includeItemValues.Add((int)EWorkflowStatus.TemporarySave);
            includeItemValues.Add((int)EWorkflowStatus.ToBeOutWarehouse);
            includeItemValues.Add((int)EWorkflowStatus.OutWarehouse);

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
            var uiSearchObj = new UISearchStockOut()
            {
                //CompanyID = CurrentUser.CompanyID,
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
                ReceiverTypeID = (int)EReceiverType.ClientUser
            };

            IList<int> includeWorkflowStatusIDs = PageWorkflowStatusRepository
                .GetCanAccessIDsByUserID(CurrentWorkFlowID, CurrentUser.UserID);

            if (includeWorkflowStatusIDs == null
                || includeWorkflowStatusIDs.Count == 0)
            {
                includeWorkflowStatusIDs = new List<int>();
                includeWorkflowStatusIDs.Add((int)EWorkflowStatus.OutWarehouse);
            }
            else
            {
                if (this.CanAddUserIDs.Contains(CurrentUser.UserID) || this.CanEditUserIDs.Contains(CurrentUser.UserID))
                {
                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.TemporarySave))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.TemporarySave);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ToBeOutWarehouse))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ToBeOutWarehouse);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.OutWarehouse))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.OutWarehouse);
                }
            }

            uiSearchObj.IncludeWorkflowStatusIDs = includeWorkflowStatusIDs;

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

            if (!string.IsNullOrEmpty(rcbxWorkflowStatus.SelectedValue))
            {
                int workflowStatusID;
                if (int.TryParse(rcbxWorkflowStatus.SelectedValue, out workflowStatusID))
                    uiSearchObj.WorkflowStatusID = workflowStatusID;
            }

            int totalRecords;

            var entities = PageStockOutRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;

            rgEntities.DataSource = entities;

            if (isNeedRebind)
                rgEntities.Rebind();
        }

        #endregion

        protected void rcbxClientUser_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindClientCompanies();

        }

        protected void rgEntities_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindEntities(false);
        }

        protected void rgEntities_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                using (IUnitOfWork unitOfWork = new UnitOfWork())
                {
                    var db = unitOfWork.GetDbModel();

                    IStockOutRepository stockOutRepository = new StockOutRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    stockOutRepository.SetDbModel(db);
                    appNoteRepository.SetDbModel(db);

                    var currentEntity = stockOutRepository.GetByID(id);

                    if (currentEntity != null)
                    {
                        foreach (var item in currentEntity.StockOutDetail)
                        {
                            item.IsDeleted = true;
                        }

                        var appNotes = appNoteRepository.GetList(x => x.ApplicationID == currentEntity.ID);
                        foreach (var item in appNotes)
                        {
                            appNoteRepository.Delete(item);
                        }

                        unitOfWork.SaveChanges();
                    }
                }

                rgEntities.Rebind();
            }
        }

        protected void rgEntities_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
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

        protected void rgEntities_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            if (this.CanAddUserIDs.Contains(CurrentUser.UserID) || CanEditUserIDs.Contains(CurrentUser.UserID))
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = true;
            else
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = false;
        }

        protected void rgEntities_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                UIStockOut uiEntity = (UIStockOut)gridDataItem.DataItem;

                if (uiEntity != null)
                {
                    string columnEditLinkHtml = "<a href=\"javascript:void(0);\" onclick=\"redirectToMaintenancePage(" + uiEntity.ID + ")\">";

                    var canAccessUserIDs = PageWorkflowStatusRepository.GetCanAccessUserIDsByID(this.CurrentWorkFlowID, uiEntity.WorkflowStatusID);

                    bool isCanAccessUser = false;
                    if (canAccessUserIDs.Contains(CurrentUser.UserID))
                        isCanAccessUser = true;

                    bool isCanEditUser = false;
                    if (CanEditUserIDs.Contains(CurrentUser.UserID)
                        || uiEntity.CreatedByUserID == CurrentUser.UserID)
                        isCanEditUser = true;

                    bool isShowPrintLink = false;
                    bool isShowDeleteLink = false;

                    EWorkflowStatus workflowStatus = (EWorkflowStatus)uiEntity.WorkflowStatusID;

                    switch (workflowStatus)
                    {
                        case EWorkflowStatus.ToBeOutWarehouse:
                        case EWorkflowStatus.OutWarehouse:
                            isShowPrintLink = true;
                            break;
                    }

                    if (CanEditUserIDs.Contains(CurrentUser.UserID))
                    {
                        columnEditLinkHtml += "编辑";

                        switch (workflowStatus)
                        {
                            case EWorkflowStatus.TemporarySave:
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
                                    if (isCanEditUser)
                                    {
                                        columnEditLinkHtml += "编辑";

                                        isShowDeleteLink = true;
                                    }
                                    else
                                        columnEditLinkHtml += "查看";

                                    break;

                                case EWorkflowStatus.ToBeOutWarehouse:
                                    columnEditLinkHtml += "出库";
                                    isShowPrintLink = true;
                                    break;

                                case EWorkflowStatus.OutWarehouse:
                                    isShowPrintLink = true;
                                    columnEditLinkHtml += "查看";
                                    break;
                            }
                        }
                        else
                            columnEditLinkHtml += "查看";
                    }

                    columnEditLinkHtml += "</a>";

                    var editColumn = rgEntities.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT);

                    if (editColumn != null)
                    {
                        var editCell = gridDataItem.Cells[editColumn.OrderIndex];

                        if (editCell != null)
                            editCell.Text = columnEditLinkHtml;
                    }

                    var printColumn = rgEntities.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_PRINT);

                    if (printColumn != null)
                    {
                        var printCell = gridDataItem.Cells[printColumn.OrderIndex];

                        if (printCell != null && !isShowPrintLink)
                            printCell.Text = string.Empty;
                    }

                    var deleteColumn = rgEntities.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE);

                    if (deleteColumn != null)
                    {
                        var deleteCell = gridDataItem.Cells[deleteColumn.OrderIndex];

                        if (deleteCell != null && !isShowDeleteLink)
                            deleteCell.Text = string.Empty;
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
            rdpBeginDate.Clear();
            rdpEndDate.Clear();

            rcbxClientUser.ClearSelection();
            rcbxClientCompany.ClearSelection();
            rcbxWorkflowStatus.ClearSelection();

            BindEntities(true);
        }
    }
}