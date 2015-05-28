using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Products
{
    public partial class InventoryHistoryManagement : BasePage
    {
        #region Members

        private IInventoryHistoryRepository _PageInventoryHistoryRepository;
        private IInventoryHistoryRepository PageInventoryHistoryRepository
        {
            get
            {
                if (_PageInventoryHistoryRepository == null)
                    _PageInventoryHistoryRepository = new InventoryHistoryRepository();

                return _PageInventoryHistoryRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.InventoryHistoryManage;
        }

        #region Private Methods

        private void BindInventoryHistorys(bool isNeedRebind)
        {
            UISearchInventoryHistory uiSearchObj = new UISearchInventoryHistory()
            {
                WarehouseName = txtWarehouseName.Text.Trim(),
                ProductName = txtProductName.Text.Trim()
            };

            int totalRecords;

            var entities = PageInventoryHistoryRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;

            rgEntities.DataSource = entities;

            if (isNeedRebind)
                rgEntities.Rebind();
        }

        #endregion

        protected void rgEntities_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindInventoryHistorys(false);
        }

        protected void rgEntities_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;

            rgEntities.Rebind();
        }

        protected void rgEntities_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgEntities_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgEntities_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindInventoryHistorys(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtWarehouseName.Text = txtProductName.Text = string.Empty;

            BindInventoryHistorys(true);
        }


        protected override EPermission PagePermissionID()
        {
            return EPermission.InventoryHistoryManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.View;
        }
    }
}