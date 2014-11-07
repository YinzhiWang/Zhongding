using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Basics
{
    public partial class SupplierManagement : BasePage
    {
        #region Members

        private ISupplierRepository _PageSupplierRepository;
        private ISupplierRepository PageSupplierRepository
        {
            get
            {
                if (_PageSupplierRepository == null)
                {
                    _PageSupplierRepository = new SupplierRepository();
                }

                return _PageSupplierRepository;
            }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = 5;
        }

        #region Private Methods

        private void BindSuppliers(bool isNeedRebind)
        {
            UISearchSupplier uiSearchObj = new UISearchSupplier()
            {
                SupplierCode = txtSupplierCode.Text.Trim(),
                SupplierName = txtSupplierName.Text.Trim()
            };

            int totalRecords;

            var companies = PageSupplierRepository.GetUIList(uiSearchObj, rgSuppliers.CurrentPageIndex, rgSuppliers.PageSize, out totalRecords);

            rgSuppliers.VirtualItemCount = totalRecords;

            rgSuppliers.DataSource = companies;

            if (isNeedRebind)
                rgSuppliers.Rebind();
        }

        #endregion

        protected void rgSuppliers_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindSuppliers(false);
        }

        protected void rgSuppliers_DeleteCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {

        }

        protected void rgSuppliers_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgSuppliers_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgSuppliers_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindSuppliers(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtSupplierCode.Text = string.Empty;
            txtSupplierName.Text = string.Empty;

            BindSuppliers(true);
        }
    }
}