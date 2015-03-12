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
    public partial class FMRefundAppManagement : WorkflowBasePage
    {
        #region Members

        private IFactoryManagerRefundAppRepository _PageFactoryManagerRefundAppRepository;
        private IFactoryManagerRefundAppRepository PageFactoryManagerRefundAppRepository
        {
            get
            {
                if (_PageFactoryManagerRefundAppRepository == null)
                    _PageFactoryManagerRefundAppRepository = new FactoryManagerRefundAppRepository();

                return _PageFactoryManagerRefundAppRepository;
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

        private ICompanyRepository _PageCompanyRepository;
        private ICompanyRepository PageCompanyRepository
        {
            get
            {
                if (_PageCompanyRepository == null)
                    _PageCompanyRepository = new CompanyRepository();

                return _PageCompanyRepository;
            }
        }

        private IProductRepository _PageProductRepository;
        private IProductRepository PageProductRepository
        {
            get
            {
                if (_PageProductRepository == null)
                    _PageProductRepository = new ProductRepository();

                return _PageProductRepository;
            }
        }

        private IList<int> _CanAddUserIDs;
        private IList<int> CanAddUserIDs
        {
            get
            {
                if (_CanAddUserIDs == null)
                    _CanAddUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.NewFMRefund);

                return _CanAddUserIDs;
            }
        }

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditFMRefund);

                return _CanEditUserIDs;
            }
        }

        private IList<int> _CanAuditByTreasurersUserIDs;
        private IList<int> CanAuditByTreasurersUserIDs
        {
            get
            {
                if (_CanAuditByTreasurersUserIDs == null)
                    _CanAuditByTreasurersUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.AuditFMRefundByTreasurers);

                return _CanAuditByTreasurersUserIDs;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.FactoryManagerRefunds;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.FactoryManagerRefundsManage;

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
        }

        private void BindClientUsers()
        {
            var clientUsers = PageClientUserRepository.GetDropdownItems();
            rcbxClientUser.DataSource = clientUsers;
            rcbxClientUser.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientUser.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientUser.DataBind();

            rcbxClientUser.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindProducts()
        {
            rcbxProduct.ClearSelection();
            rcbxProduct.Items.Clear();

            int companyID;

            if (int.TryParse(rcbxCompany.SelectedValue, out companyID))
            {
                var products = PageProductRepository.GetDropdownItems(new UISearchDropdownItem()
                {
                    Extension = new UISearchExtension { CompanyID = companyID }
                });

                rcbxProduct.DataSource = products;
                rcbxProduct.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                rcbxProduct.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                rcbxProduct.DataBind();

                rcbxProduct.Items.Insert(0, new RadComboBoxItem("", ""));
            }
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

        private void BindEntities(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchFactoryManagerRefundApp
            {
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
            };

            if (!string.IsNullOrEmpty(rcbxCompany.SelectedValue))
            {
                int companyID;
                if (int.TryParse(rcbxCompany.SelectedValue, out companyID))
                    uiSearchObj.CompanyID = companyID;
            }

            if (!string.IsNullOrEmpty(rcbxClientUser.SelectedValue))
            {
                int clientUserID;
                if (int.TryParse(rcbxClientUser.SelectedValue, out clientUserID))
                    uiSearchObj.ClientUserID = clientUserID;
            }

            if (!string.IsNullOrEmpty(rcbxProduct.SelectedValue))
            {
                int productID;
                if (int.TryParse(rcbxProduct.SelectedValue, out productID))
                    uiSearchObj.ProductID = productID;
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

                if (CanAuditByTreasurersUserIDs.Contains(CurrentUser.UserID))
                {
                    if (!includeWorkflowStatusIDs.Contains((int)EWorkflowStatus.Submit))
                        includeWorkflowStatusIDs.Add((int)EWorkflowStatus.Submit);

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

            var uiEntities = PageFactoryManagerRefundAppRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;
            rgEntities.DataSource = uiEntities;

            if (isNeedRebind)
                rgEntities.Rebind();
        }

        #endregion

        protected void rcbxCompany_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindProducts();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindEntities(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rcbxCompany.ClearSelection();

            rcbxClientUser.ClearSelection();

            rcbxProduct.ClearSelection();
            rcbxProduct.Items.Clear();

            rcbxWorkflowStatus.ClearSelection();

            BindEntities(true);
        }

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
                var uiEntity = (UIFactoryManagerRefundApp)gridDataItem.DataItem;

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

                    bool isShowPrintLink = false;
                    bool isShowDeleteLink = false;

                    EWorkflowStatus workflowStatus = (EWorkflowStatus)uiEntity.WorkflowStatusID;

                    switch (workflowStatus)
                    {
                        case EWorkflowStatus.Paid:
                            isShowPrintLink = true;
                            break;
                    }

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

                    var editColumn = rgEntities.MasterTableView.GetColumn(GlobalConst.GridColumnUniqueNames.COLUMN_EDIT);

                    if (editColumn != null)
                    {
                        var editCell = gridDataItem.Cells[editColumn.OrderIndex];

                        if (editCell != null)
                            editCell.Text = linkHtml;
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

        protected void rgEntities_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            if (this.CanAddUserIDs.Contains(CurrentUser.UserID) || CanEditUserIDs.Contains(CurrentUser.UserID))
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = true;
            else
                e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = false;
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

                    IFactoryManagerRefundAppRepository fmRefundAppRepository = new FactoryManagerRefundAppRepository();
                    IApplicationNoteRepository appNoteRepository = new ApplicationNoteRepository();

                    fmRefundAppRepository.SetDbModel(db);
                    appNoteRepository.SetDbModel(db);

                    var currentEntity = fmRefundAppRepository.GetByID(id);

                    if (currentEntity != null)
                    {
                        fmRefundAppRepository.Delete(currentEntity);

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
    }
}