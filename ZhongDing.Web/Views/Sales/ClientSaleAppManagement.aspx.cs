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
    public partial class ClientSaleAppManagement : WorkflowBasePage
    {
        #region Members

        private IClientSaleApplicationRepository _PageClientSalesAppRepository;
        private IClientSaleApplicationRepository PageClientSalesAppRepository
        {
            get
            {
                if (_PageClientSalesAppRepository == null)
                    _PageClientSalesAppRepository = new ClientSaleApplicationRepository();

                return _PageClientSalesAppRepository;
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
                    _CanAddUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewClientOrder);

                return _CanAddUserIDs;
            }
        }

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditClientOrder);

                return _CanEditUserIDs;
            }
        }

        private IList<int> _CanStopUserIDs;
        private IList<int> CanStopUserIDs
        {
            get
            {
                if (_CanStopUserIDs == null)
                    _CanStopUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.StopClientOrder);

                return _CanStopUserIDs;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.ClientOrder;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ClientOrderManage;

            if (!IsPostBack)
            {
                BindClientUsers();

                BindClientCompanies();

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
            includeItemValues.Add((int)EWorkflowStatus.Submit);
            includeItemValues.Add((int)EWorkflowStatus.ReturnBasicInfo);
            includeItemValues.Add((int)EWorkflowStatus.ApprovedBasicInfo);
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
            var uiSearchObj = new UISearchClientSaleApplication
            {
                CompanyID = CurrentUser.CompanyID,
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
            };

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
                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.TemporarySave))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.TemporarySave);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.Submit))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.Submit);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ReturnBasicInfo))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ReturnBasicInfo);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.ApprovedBasicInfo))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.ApprovedBasicInfo);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.Shipping))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.Shipping);

                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.Completed))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.Completed);
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

            var uiEntities = PageClientSalesAppRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;
            rgEntities.DataSource = uiEntities;

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

            if (this.CanStopUserIDs.Contains(CurrentUser.UserID))
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_STOP).Visible = true;
            else
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_STOP).Visible = false;
        }

        protected void rgEntities_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                var uiEntity = (UIClientSaleApplication)gridDataItem.DataItem;

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
                                    linkHtml += "审核";
                                    break;

                                case EWorkflowStatus.ApprovedBasicInfo:
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
                            case EWorkflowStatus.ApprovedBasicInfo:
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

                    IClientSaleApplicationRepository clientSalesAppRepository = new ClientSaleApplicationRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    clientSalesAppRepository.SetDbModel(db);
                    appNoteRepository.SetDbModel(db);

                    var currentEntity = clientSalesAppRepository.GetByID(id);

                    if (currentEntity != null)
                    {
                        if (currentEntity.SalesOrderApplication != null)
                        {
                            foreach (var item in currentEntity.SalesOrderApplication.SalesOrderAppDetail)
                            {
                                item.IsDeleted = true;
                            }

                            currentEntity.SalesOrderApplication.IsDeleted = true;
                        }

                        clientSalesAppRepository.Delete(currentEntity);

                        var appNotes = appNoteRepository.GetList(x =>x.WorkflowID == CurrentWorkFlowID && x.ApplicationID == currentEntity.ID);
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

        protected void rgEntities_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == GlobalConst.GridColumnUniqueNames.COLUMN_STOP)
            {
                GridEditableItem editableItem = e.Item as GridEditableItem;

                String sid = editableItem.GetDataKeyValue("ID").ToString();

                int id = 0;
                if (int.TryParse(sid, out id))
                {
                    var currentEntity = PageClientSalesAppRepository.GetByID(id);

                    if (currentEntity != null)
                    {
                        currentEntity.SalesOrderApplication.IsStop = true;
                        currentEntity.SalesOrderApplication.StoppedBy = CurrentUser.UserID;
                        currentEntity.SalesOrderApplication.StoppedOn = DateTime.Now;

                        PageClientSalesAppRepository.Save();
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

            rcbxClientUser.ClearSelection();
            rcbxClientCompany.ClearSelection();
            rcbxWorkflowStatus.ClearSelection();

            BindEntities(true);
        }


        protected override EWorkflow PagePermissionWorkflowID()
        {
            return EWorkflow.ClientOrder;
        }
    }
}