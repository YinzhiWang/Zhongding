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

namespace ZhongDing.Web.Views.Basics
{
    public partial class DeptProductSalesPlanAndBonusManagement : BasePage
    {
        #region Members

        private IDeptProductSalesPlanRepository _PageDeptProductSalesPlanRepository;
        private IDeptProductSalesPlanRepository PageDeptProductSalesPlanRepository
        {
            get
            {
                if (_PageDeptProductSalesPlanRepository == null)
                    _PageDeptProductSalesPlanRepository = new DeptProductSalesPlanRepository();

                return _PageDeptProductSalesPlanRepository;
            }
        }

        private IDepartmentRepository _PageDepartmentRepository;
        private IDepartmentRepository PageDepartmentRepository
        {
            get
            {
                if (_PageDepartmentRepository == null)
                    _PageDepartmentRepository = new DepartmentRepository();

                return _PageDepartmentRepository;
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

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.DeptProductSalesPlanAndBonusManage;

            if (!IsPostBack)
            {
                BindDepartments();
                BindProducts();
            }
        }

        #region Private Methods

        private void BindDepartments()
        {
            var departments = PageDepartmentRepository.GetDropdownItems();

            rcbxDepartment.DataSource = departments;
            rcbxDepartment.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDepartment.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDepartment.DataBind();

            rcbxDepartment.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindProducts()
        {
            var products = PageProductRepository.GetDropdownItems();

            rcbxProduct.DataSource = products;
            rcbxProduct.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxProduct.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxProduct.DataBind();

            rcbxProduct.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindProducts(int departmentID)
        {
            rcbxProduct.ClearSelection();

            var uiSearchObj = new UISearchDropdownItem()
            {
                Extension = new UISearchExtension()
                {
                    DepartmentID = departmentID
                }
            };

            var products = PageProductRepository.GetDropdownItems(uiSearchObj);

            rcbxProduct.DataSource = products;
            rcbxProduct.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxProduct.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxProduct.DataBind();

            rcbxProduct.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindEntities(bool isNeedRebind)
        {
            UISearchDeptProductSalesPlan uiSearchObj = new UISearchDeptProductSalesPlan();

            if (!string.IsNullOrEmpty(rcbxDepartment.SelectedValue))
            {
                int departmentID;
                if (int.TryParse(rcbxDepartment.SelectedValue, out departmentID))
                    uiSearchObj.DepartmentID = departmentID;
            }

            if (!string.IsNullOrEmpty(rcbxProduct.SelectedValue))
            {
                int productID;
                if (int.TryParse(rcbxProduct.SelectedValue, out productID))
                    uiSearchObj.ProductID = productID;
            }

            int totalRecords;

            var entities = PageDeptProductSalesPlanRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

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
            var editableItem = ((GridEditableItem)e.Item);
            String sid = editableItem.GetDataKeyValue("ID").ToString();

            int id = 0;
            if (int.TryParse(sid, out id))
            {
                var currentEntity = PageDeptProductSalesPlanRepository.GetByID(id);

                if (currentEntity != null)
                {
                    foreach (var salesBonus in currentEntity.DepartmentProductSalesBonus)
                    {
                        salesBonus.IsDeleted = true;
                    }

                    currentEntity.IsDeleted = true;

                    PageDeptProductSalesPlanRepository.Save();
                }

                rgEntities.Rebind();
            }
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

        protected void rcbxDepartment_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value))
            {
                int departmentID;

                if (int.TryParse(e.Value, out departmentID))
                    BindProducts(departmentID);
                else
                    BindProducts();
            }
            else
                BindProducts();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindEntities(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rcbxDepartment.ClearSelection();
            rcbxProduct.ClearSelection();

            BindEntities(true);
        }

    }
}