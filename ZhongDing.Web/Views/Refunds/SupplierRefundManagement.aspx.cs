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
    public partial class SupplierRefundManagement : WorkflowBasePage
    {
        #region Members

        private ISupplierRefundApplicationRepository _PageSupplierRefundAppRepository;
        private ISupplierRefundApplicationRepository PageSupplierRefundAppRepository
        {
            get
            {
                if (_PageSupplierRefundAppRepository == null)
                    _PageSupplierRefundAppRepository = new SupplierRefundApplicationRepository();

                return _PageSupplierRefundAppRepository;
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

        private IList<int> _CanEditUserIDs;
        private IList<int> CanEditUserIDs
        {
            get
            {
                if (_CanEditUserIDs == null)
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditSupplierRefund);

                return _CanEditUserIDs;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.SupplierRefundsManage;
            this.CurrentWorkFlowID = (int)EWorkflow.SupplierRefunds;

            if (!IsPostBack)
            {
                BindCompany();
            }
        }

        #region Private Methods

        private void BindCompany()
        {
            var companies = PageCompanyRepository.GetDropdownItems();
            rcbxCompany.DataSource = companies;
            rcbxCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxCompany.DataBind();

            rcbxCompany.Items.Insert(0, new RadComboBoxItem("", ""));
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

        private void BindProducts()
        {
            rcbxProduct.ClearSelection();
            rcbxProduct.Items.Clear();

            int companyID;

            if (int.TryParse(rcbxCompany.SelectedValue, out companyID))
            {
                var products = PageProductRepository.GetDropdownItems(new UISearchDropdownItem()
                {
                    Extension = new UISearchExtension
                    {
                        CompanyID = companyID
                    }
                });

                rcbxProduct.DataSource = products;
                rcbxProduct.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
                rcbxProduct.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
                rcbxProduct.DataBind();

                rcbxProduct.Items.Insert(0, new RadComboBoxItem("", ""));
            }
        }

        private void BindEntities(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchSupplierRefundApp();

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

            if (!string.IsNullOrEmpty(rcbxProduct.SelectedValue))
            {
                int productID;
                if (int.TryParse(rcbxProduct.SelectedValue, out productID))
                    uiSearchObj.ProductID = productID;
            }

            int totalRecords;

            var entities = PageSupplierRefundAppRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;

            rgEntities.DataSource = entities;

            if (isNeedRebind)
                rgEntities.Rebind();
        }

        #endregion

        protected void rcbxCompany_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindWarehouses();

            BindProducts();
        }

        protected void rgEntities_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindEntities(false);
        }

        protected void rgEntities_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                var uiEntity = (UISupplierRefundApp)gridDataItem.DataItem;

                if (uiEntity != null)
                {
                    string linkHtml = "<a href=\"javascript:void(0);\" onclick=\"redirectToMaintenancePage(" + uiEntity.ID + ","
                        + uiEntity.CompanyID + "," + uiEntity.SupplierID + "," + uiEntity.ProductID + "," + uiEntity.ProductSpecificationID + ")\">";

                    if (CanEditUserIDs.Contains(CurrentUser.UserID))
                        linkHtml += "编辑";
                    else
                        linkHtml += "查看";

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
            rcbxCompany.ClearSelection();
            rcbxWarehouse.ClearSelection();
            rcbxProduct.ClearSelection();

            BindEntities(true);
        }
    }
}