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
    public partial class ClientInfoInvoiceManagement : BasePage
    {
        #region Members

        private IClientInvoiceRepository _PageClientInvoiceRepository;
        private IClientInvoiceRepository PageClientInvoiceRepository
        {
            get
            {
                if (_PageClientInvoiceRepository == null)
                    _PageClientInvoiceRepository = new ClientInvoiceRepository();

                return _PageClientInvoiceRepository;
            }
        }
        private IClientInvoiceDetailRepository _PageClientInvoiceDetailRepository;
        private IClientInvoiceDetailRepository PageClientInvoiceDetailRepository
        {
            get
            {
                if (_PageClientInvoiceDetailRepository == null)
                    _PageClientInvoiceDetailRepository = new ClientInvoiceDetailRepository();

                return _PageClientInvoiceDetailRepository;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.ClientInvoiceManage;
            if (!IsPostBack)
            {
                BindClientCompanys();
            }
        }
        private void BindClientCompanys()
        {
            var uiSearchObj = new UISearchDropdownItem();

            var clientCompanies = PageClientCompanyRepository.GetDropdownItems(uiSearchObj);
            rcbxClientCompany.DataSource = clientCompanies;
            rcbxClientCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientCompany.DataBind();

            rcbxClientCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }
        private void BindClientInvoice(bool isNeedRebind)
        {
            UISearchClientInvoice uiSearchObj = new UISearchClientInvoice()
            {
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
                InvoiceNumber = txtInvoiceNumber.Text.Trim(),
                CompanyID = SiteUser.GetCurrentSiteUser().CompanyID,
            };

            int totalRecords = 0;

            var uiClientInvoices = PageClientInvoiceRepository.GetUIList(uiSearchObj, rgClientInvoices.CurrentPageIndex, rgClientInvoices.PageSize, out totalRecords);

            rgClientInvoices.VirtualItemCount = totalRecords;

            rgClientInvoices.DataSource = uiClientInvoices;


            if (isNeedRebind)
                rgClientInvoices.Rebind();
        }


        protected void rgClientInvoices_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindClientInvoice(false);
        }

        protected void rgClientInvoices_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;
            var ClientInvoiceID = editableItem.GetDataKeyValue("ID").ToIntOrNull();

            if (ClientInvoiceID.BiggerThanZero())
            {
                var ClientInvoice = PageClientInvoiceRepository.GetByID(ClientInvoiceID);
                var ClientInvoiceDetails = ClientInvoice.ClientInvoiceDetail.ToList();
                ClientInvoiceDetails.ForEach(x => { PageClientInvoiceDetailRepository.DeleteByID(x.ID); });
                PageClientInvoiceRepository.DeleteByID(ClientInvoiceID);
                PageClientInvoiceDetailRepository.Save();
                PageClientInvoiceRepository.Save();
                rgClientInvoices.Rebind();
            }


        }

        protected void rgClientInvoices_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgClientInvoices_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgClientInvoices_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindClientInvoice(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpEndDate.SelectedDate = rdpBeginDate.SelectedDate = null;

            txtInvoiceNumber.Text = rcbxClientCompany.Text = rcbxClientCompany.SelectedValue = string.Empty;
           
            BindClientInvoice(true);
        }
    }
}