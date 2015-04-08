using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Common.Extension;
using Telerik.Web.UI;
using ZhongDing.Web.Extensions;
using ZhongDing.Common;

namespace ZhongDing.Web.Views.Invoices
{
    public partial class SupplierInvoiceManagement : BasePage
    {
        #region Members

        private ISupplierInvoiceRepository _PageSupplierInvoiceRepository;
        private ISupplierInvoiceRepository PageSupplierInvoiceRepository
        {
            get
            {
                if (_PageSupplierInvoiceRepository == null)
                    _PageSupplierInvoiceRepository = new SupplierInvoiceRepository();

                return _PageSupplierInvoiceRepository;
            }
        }
        private ISupplierInvoiceDetailRepository _PageSupplierInvoiceDetailRepository;
        private ISupplierInvoiceDetailRepository PageSupplierInvoiceDetailRepository
        {
            get
            {
                if (_PageSupplierInvoiceDetailRepository == null)
                    _PageSupplierInvoiceDetailRepository = new SupplierInvoiceDetailRepository();

                return _PageSupplierInvoiceDetailRepository;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.SupplierInvoiceManage;
            if (!IsPostBack)
            {
                BindSuppliers();
            }
        }
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
        private void BindSupplierInvoice(bool isNeedRebind)
        {
            UISearchSupplierInvoice uiSearchObj = new UISearchSupplierInvoice()
            {
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
                InvoiceNumber = txtInvoiceNumber.Text.Trim(),
                SupplierID = rcbxSupplier.SelectedValue.ToIntOrNull(),
                CompanyID = SiteUser.GetCurrentSiteUser().CompanyID,
                IsGroupByProduct = cbxIsGroupByProduct.Checked
            };

            int totalRecords = 0;

            var uiSupplierInvoices = PageSupplierInvoiceRepository.GetUIList(uiSearchObj, rgSupplierInvoices.CurrentPageIndex, rgSupplierInvoices.PageSize, out totalRecords);

            rgSupplierInvoices.VirtualItemCount = totalRecords;

            rgSupplierInvoices.DataSource = uiSupplierInvoices;

            if (uiSearchObj.IsGroupByProduct)
            {
                rgSupplierInvoices.Columns[4].Visible = false;
                rgSupplierInvoices.Columns[5].Visible = false;
            }
            else
            {
                rgSupplierInvoices.Columns[4].Visible = true;
                rgSupplierInvoices.Columns[5].Visible = true;
            }

            if (isNeedRebind)
                rgSupplierInvoices.Rebind();
        }


        protected void rgSupplierInvoices_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindSupplierInvoice(false);
        }

        protected void rgSupplierInvoices_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;
            var supplierInvoiceID = editableItem.GetDataKeyValue("ID").ToIntOrNull();

            if (supplierInvoiceID.BiggerThanZero())
            {
                var supplierInvoice = PageSupplierInvoiceRepository.GetByID(supplierInvoiceID);
                var supplierInvoiceDetails = supplierInvoice.SupplierInvoiceDetail.ToList();
                supplierInvoiceDetails.ForEach(x => { PageSupplierInvoiceDetailRepository.DeleteByID(x.ID); });
                PageSupplierInvoiceRepository.DeleteByID(supplierInvoiceID);
                PageSupplierInvoiceDetailRepository.Save();
                PageSupplierInvoiceRepository.Save();
                rgSupplierInvoices.Rebind();
            }


        }

        protected void rgSupplierInvoices_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgSupplierInvoices_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgSupplierInvoices_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindSupplierInvoice(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpEndDate.SelectedDate = rdpBeginDate.SelectedDate = null;

            txtInvoiceNumber.Text = rcbxSupplier.Text = rcbxSupplier.SelectedValue = string.Empty;
            cbxIsGroupByProduct.Checked = false;
            BindSupplierInvoice(true);
        }
    }
}