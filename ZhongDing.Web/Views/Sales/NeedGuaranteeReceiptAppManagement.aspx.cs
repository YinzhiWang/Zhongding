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
using ZhongDing.Common.Extension;

namespace ZhongDing.Web.Views.Sales
{
    public partial class NeedGuaranteeReceiptAppManagement : WorkflowBasePage
    {
        protected override EPermission PagePermissionID()
        {
            return EPermission.GuaranteeReceiptManagement;
        }


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




        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.GuaranteeReceiptManagement;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.GuaranteeReceiptManage;

            if (!IsPostBack)
            {
                BindClientUsers();
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





        #endregion

        protected void rcbxClientUser_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {

        }


        private void BindEntities(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchClientSaleApplication
            {
                CompanyID = CurrentUser.CompanyID,
                GuaranteeExpirationDateBeginDate = rdpBeginDate.SelectedDate,
                GuaranteeExpirationDateEndDate = rdpEndDate.SelectedDate,
                OnlyNotReceipted = true,
                NeedGuaranteeAmount = true,
                IsGuaranteed = true
            };

            if (!string.IsNullOrEmpty(rcbxClientUser.SelectedValue))
            {
                int clientUserID;
                if (int.TryParse(rcbxClientUser.SelectedValue, out clientUserID))
                    uiSearchObj.ClientUserID = clientUserID;
            }
            uiSearchObj.UnIncludeIDs = GetSelectedIDs;
            uiSearchObj.IncludeWorkflowStatusIDs = new List<int>() { (int)EWorkflowStatus.Completed, (int)EWorkflowStatus.Shipping };
            int totalRecords;

            var uiEntities = PageClientSalesAppRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;
            rgEntities.DataSource = uiEntities;

            if (isNeedRebind)
                rgEntities.Rebind();
        }

        protected void rgEntities_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindEntities(false);
        }

        protected void rgEntities_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                //GridCommandItem commandItem = e.Item as GridCommandItem;
                //Panel plAddCommand = commandItem.FindControl("plAddCommand") as Panel;

                //if (plAddCommand != null)
                //{
                //    if (this.CanAddUserIDs.Contains(CurrentUser.UserID))
                //        plAddCommand.Visible = true;
                //    else
                //        plAddCommand.Visible = false;
                //}
            }
        }

        protected void rgEntities_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            //if (this.CanAddUserIDs.Contains(CurrentUser.UserID) || CanEditUserIDs.Contains(CurrentUser.UserID))
            //    e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = true;
            //else
            //    e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = false;

            //if (this.CanStopUserIDs.Contains(CurrentUser.UserID))
            //    e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_STOP).Visible = true;
            //else
            //    e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_STOP).Visible = false;
        }

        protected void rgEntities_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                //GridDataItem gridDataItem = e.Item as GridDataItem;
                //var uiEntity = (UIClientSaleApplication)gridDataItem.DataItem;


            }
        }

        protected void rgEntities_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            var id = editableItem.GetDataKeyValue("ID").ToIntOrNull();
            if (id.BiggerThanZero())
            {
                AddSelectedIDs(id.Value);
                BindEntities(true);
                BindEntitiesSelected(true);
                ShowSuccessMessage("已选择");
            }
        }

        protected void rgEntities_ItemCommand(object sender, GridCommandEventArgs e)
        {
        }

        public List<int> GetSelectedIDs
        {
            get
            {
                return (List<int>)Session[GlobalConst.SessionKeys.SELECTED_ClientSaleApplication_IDS];
            }
        }

        public void AddSelectedIDs(int id)
        {
            List<int> ids = (List<int>)Session[GlobalConst.SessionKeys.SELECTED_ClientSaleApplication_IDS];
            if (ids == null) ids = new List<int>();
            if (!ids.Contains(id))
            {
                ids.Add(id);
                Session[GlobalConst.SessionKeys.SELECTED_ClientSaleApplication_IDS] = ids;
            }
        }

        public void DeleteSelectedIDs(int id)
        {
            List<int> ids = (List<int>)Session[GlobalConst.SessionKeys.SELECTED_ClientSaleApplication_IDS];
            if (ids == null) ids = new List<int>();
            if (ids.Contains(id))
            {
                ids.Remove(id);
                Session[GlobalConst.SessionKeys.SELECTED_ClientSaleApplication_IDS] = ids;
            }
        }

        private void BindEntitiesSelected(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchClientSaleApplication
            {
                CompanyID = CurrentUser.CompanyID,
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
                OnlyFilterID = true,
            };


            uiSearchObj.IncludeIDs = GetSelectedIDs;

            int totalRecords;

            var uiEntities = PageClientSalesAppRepository.GetUIList(uiSearchObj, rgEntitiesSelected.CurrentPageIndex, rgEntitiesSelected.PageSize, out totalRecords);

            rgEntitiesSelected.VirtualItemCount = totalRecords;
            rgEntitiesSelected.DataSource = uiEntities;
            btnSettle.Visible = uiEntities.Count != 0;
            if (isNeedRebind)
                rgEntitiesSelected.Rebind();
        }

        protected void rgEntitiesSelected_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindEntitiesSelected(false);
        }

        protected void rgEntitiesSelected_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                //GridCommandItem commandItem = e.Item as GridCommandItem;
                //Panel plAddCommand = commandItem.FindControl("plAddCommand") as Panel;

                //if (plAddCommand != null)
                //{
                //    if (this.CanAddUserIDs.Contains(CurrentUser.UserID))
                //        plAddCommand.Visible = true;
                //    else
                //        plAddCommand.Visible = false;
                //}
            }
        }

        protected void rgEntitiesSelected_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {
            //if (this.CanAddUserIDs.Contains(CurrentUser.UserID) || CanEditUserIDs.Contains(CurrentUser.UserID))
            //    e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = true;
            //else
            //    e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_DELETE).Visible = false;

            //if (this.CanStopUserIDs.Contains(CurrentUser.UserID))
            //    e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_STOP).Visible = true;
            //else
            //    e.OwnerTableView.Columns.FindByUniqueName(GlobalConst.GridColumnUniqueNames.COLUMN_STOP).Visible = false;
        }

        protected void rgEntitiesSelected_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                //GridDataItem gridDataItem = e.Item as GridDataItem;
                //var uiEntity = (UIClientSaleApplication)gridDataItem.DataItem;


            }
        }

        protected void rgEntitiesSelected_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;
            var id = editableItem.GetDataKeyValue("ID").ToIntOrNull();
            if (id.BiggerThanZero())
            {
                DeleteSelectedIDs(id.Value);
                BindEntities(true);
                BindEntitiesSelected(true);
                ShowSuccessMessage("已删除");
            }
        }

        protected void rgEntitiesSelected_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == GlobalConst.GridColumnUniqueNames.COLUMN_STOP)
            {
                GridEditableItem editableItem = e.Item as GridEditableItem;

                //String sid = editableItem.GetDataKeyValue("ID").ToString();

                //int id = 0;
                //if (int.TryParse(sid, out id))
                //{
                //    var currentEntity = PageClientSalesAppRepository.GetByID(id);

                //    if (currentEntity != null)
                //    {
                //        currentEntity.SalesOrderApplication.IsStop = true;
                //        currentEntity.SalesOrderApplication.StoppedBy = CurrentUser.UserID;
                //        currentEntity.SalesOrderApplication.StoppedOn = DateTime.Now;

                //        PageClientSalesAppRepository.Save();
                //    }
                //}

                rgEntitiesSelected.Rebind();
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
            BindEntities(true);
        }
    }
}