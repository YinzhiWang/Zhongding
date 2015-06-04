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
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Imports
{
    public partial class ProcureOrderDataManagement : BasePage
    {
        #region Members

        private IProcureOrderApplicationRepository _PageProcureOrderAppRepository;
        private IProcureOrderApplicationRepository PageProcureOrderAppRepository
        {
            get
            {
                if (_PageProcureOrderAppRepository == null)
                    _PageProcureOrderAppRepository = new ProcureOrderApplicationRepository();

                return _PageProcureOrderAppRepository;
            }
        }

        private ISupplierRepository _PageSupplierRepository;
        private ISupplierRepository PageSupplierRepository
        {
            get
            {
                if (_PageSupplierRepository == null)
                    _PageSupplierRepository = new SupplierRepository();

                return _PageSupplierRepository;
            }
        }


        #endregion

        //protected override int GetCurrentWorkFlowID()
        //{
        //    return (int)EWorkflow.ProcureOrder;
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ProcureOrderData;

            if (!IsPostBack)
            {
                BindSuppliers();
            }
        }

        #region Private Methods

        private void BindSuppliers()
        {
            var suppliers = PageSupplierRepository.GetDropdownItems(new UISearchDropdownItem
            {
                Extension = new UISearchExtension
                {
                    CompanyID = CurrentUser.CompanyID
                }
            });

            rcbxSupplier.DataSource = suppliers;
            rcbxSupplier.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxSupplier.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxSupplier.DataBind();

            rcbxSupplier.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindEntities(bool isNeedRebind)
        {
            UISearchProcureOrderApplication uiSearchObj = new UISearchProcureOrderApplication()
            {
                CompanyID = CurrentUser.CompanyID,
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
                IsImport = true
            };

            if (!string.IsNullOrEmpty(rcbxSupplier.SelectedValue))
            {
                int supplierID;
                if (int.TryParse(rcbxSupplier.SelectedValue, out supplierID))
                    uiSearchObj.SupplierID = supplierID;
            }

            int totalRecords;

            var entities = PageProcureOrderAppRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;

            rgEntities.DataSource = entities;

            if (isNeedRebind)
                rgEntities.Rebind();
        }

        #endregion


        protected void rgEntities_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindEntities(false);
        }

        protected void rgEntities_DeleteCommand(object sender, GridCommandEventArgs e)
        {

        }

        protected void rgEntities_ItemCreated(object sender, GridItemEventArgs e)
        {

        }

        protected void rgEntities_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {

        }

        protected void rgEntities_ItemDataBound(object sender, GridItemEventArgs e)
        {

        }

        protected void rgEntities_ItemCommand(object sender, GridCommandEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindEntities(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpBeginDate.Clear();
            rdpEndDate.Clear();

            rcbxSupplier.ClearSelection();

            BindEntities(true);
        }

        protected override EPermission PagePermissionID()
        {
            return EPermission.DataImport;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.Create;
        }
    }
}