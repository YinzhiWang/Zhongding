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
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Basics
{
    public partial class WarehouseManagement : BasePage
    {
        #region Members

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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.WarehouseManage;

        }

        private void BindWarehouses(bool isNeedRebind)
        {
            UISearchWarehouse uiSearchObj = new UISearchWarehouse()
            {
                CompanyID = CurrentUser.CompanyID,
                WarehouseCode = txtSerialNo.Text.Trim(),
                Name = txtName.Text.Trim()
            };

            int totalRecords;

            var uiWarehouses = PageWarehouseRepository.GetUIList(uiSearchObj, rgWarehouses.CurrentPageIndex, rgWarehouses.PageSize, out totalRecords);

            rgWarehouses.VirtualItemCount = totalRecords;

            rgWarehouses.DataSource = uiWarehouses;

            if (isNeedRebind)
                rgWarehouses.Rebind();
        }


        protected void rgWarehouses_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindWarehouses(false);
        }

        protected void rgWarehouses_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {

                PageWarehouseRepository.DeleteByID(id);
                PageWarehouseRepository.Save();
            }

            rgWarehouses.Rebind();
        }

        protected void rgWarehouses_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgWarehouses_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgWarehouses_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindWarehouses(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSerialNo.Text = string.Empty;
            txtName.Text = string.Empty;

            BindWarehouses(true);
        }
    }
}