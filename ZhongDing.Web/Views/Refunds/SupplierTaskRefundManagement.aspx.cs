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
    public partial class SupplierTaskRefundManagement : WorkflowBasePage
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
                    _CanEditUserIDs = PageWorkflowStepRepository.GetCanAccessUserIDsByID((int)EWorkflowStep.EditSupplierTaskRefund);

                return _CanEditUserIDs;
            }
        }

        #endregion

        protected override int GetCurrentWorkFlowID()
        {
            return (int)EWorkflow.SupplierTaskRefunds;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.SupplierTaskRefundsManage;

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

        private void BindSuppliers()
        {
            rcbxSupplier.ClearSelection();

            IList<UIDropdownItem> suppliers = new List<UIDropdownItem>();

            if (!string.IsNullOrEmpty(rcbxCompany.SelectedValue))
            {
                int companyID;

                if (int.TryParse(rcbxCompany.SelectedValue, out companyID))
                {
                    suppliers = PageSupplierRepository.GetDropdownItems(new UISearchDropdownItem
                    {
                        Extension = new UISearchExtension
                        {
                            CompanyID = companyID
                        }
                    });
                }
            }

            rcbxSupplier.DataSource = suppliers;
            rcbxSupplier.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxSupplier.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxSupplier.DataBind();

            rcbxSupplier.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindProducts()
        {
            rcbxProduct.ClearSelection();

            IList<UIDropdownItem> products = new List<UIDropdownItem>();

            var uiSearchObj = new UISearchDropdownItem { Extension = new UISearchExtension() };

            int companyID = 0;
            int supplierID = 0;

            int.TryParse(rcbxCompany.SelectedValue, out companyID);
            int.TryParse(rcbxSupplier.SelectedValue, out supplierID);

            if (companyID > 0 || supplierID > 0)
            {
                if (companyID > 0)
                    uiSearchObj.Extension.CompanyID = companyID;

                if (supplierID > 0)
                    uiSearchObj.Extension.SupplierID = supplierID;

                products = PageProductRepository.GetDropdownItems(uiSearchObj);
            }

            rcbxProduct.DataSource = products;
            rcbxProduct.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxProduct.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxProduct.DataBind();

            rcbxProduct.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindEntities(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchSupplierRefundApp
            {
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate
            };

            if (!string.IsNullOrEmpty(rcbxCompany.SelectedValue))
            {
                int companyID;
                if (int.TryParse(rcbxCompany.SelectedValue, out companyID))
                    uiSearchObj.CompanyID = companyID;
            }

            if (!string.IsNullOrEmpty(rcbxSupplier.SelectedValue))
            {
                int supplierID;
                if (int.TryParse(rcbxSupplier.SelectedValue, out supplierID))
                    uiSearchObj.SupplierID = supplierID;
            }

            if (!string.IsNullOrEmpty(rcbxProduct.SelectedValue))
            {
                int productID;
                if (int.TryParse(rcbxProduct.SelectedValue, out productID))
                    uiSearchObj.ProductID = productID;
            }

            int totalRecords;

            var entities = PageSupplierRefundAppRepository.GetTaskRefunds(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;

            rgEntities.DataSource = entities;

            if (isNeedRebind)
                rgEntities.Rebind();
        }

        #endregion

        protected void rcbxCompany_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindSuppliers();
            BindProducts();
        }

        protected void rcbxSupplier_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindProducts();
        }

        protected void rgEntities_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindEntities(false);
        }

        protected void rgEntities_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridCommandItem)
            {
                GridCommandItem commandItem = e.Item as GridCommandItem;
                Panel plAddCommand = commandItem.FindControl("plAddCommand") as Panel;

                if (plAddCommand != null)
                {
                    if (this.CanEditUserIDs.Contains(CurrentUser.UserID))
                        plAddCommand.Visible = true;
                    else
                        plAddCommand.Visible = false;
                }
            }
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
                    string linkHtml = string.Empty;

                    if (uiEntity.RefundDate.HasValue)
                    {
                        if (this.CanEditUserIDs.Contains(CurrentUser.UserID))
                            linkHtml += "<a href=\"javascript:void(0);\" onclick=\"redirectToMaintenancePage(" + uiEntity.ID + ")\">"
                                + uiEntity.RefundDate.ToString("yyyy/MM/dd") + "</a>";
                        else
                            linkHtml += uiEntity.RefundDate.ToString("yyyy/MM/dd");
                    }

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

            rcbxSupplier.ClearSelection();
            rcbxSupplier.Items.Clear();

            rcbxProduct.ClearSelection();
            rcbxProduct.Items.Clear();

            rdpBeginDate.Clear();
            rdpEndDate.Clear();

            BindEntities(true);
        }


    }
}