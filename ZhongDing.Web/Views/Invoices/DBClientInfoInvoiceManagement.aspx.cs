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
    public partial class DBClientInfoInvoiceManagement : BasePage
    {
        #region Members

        private IDBClientInvoiceRepository _PageDBClientInvoiceRepository;
        private IDBClientInvoiceRepository PageDBClientInvoiceRepository
        {
            get
            {
                if (_PageDBClientInvoiceRepository == null)
                    _PageDBClientInvoiceRepository = new DBClientInvoiceRepository();

                return _PageDBClientInvoiceRepository;
            }
        }
        private IDBClientInvoiceDetailRepository _PageDBClientInvoiceDetailRepository;
        private IDBClientInvoiceDetailRepository PageDBClientInvoiceDetailRepository
        {
            get
            {
                if (_PageDBClientInvoiceDetailRepository == null)
                    _PageDBClientInvoiceDetailRepository = new DBClientInvoiceDetailRepository();

                return _PageDBClientInvoiceDetailRepository;
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
        private IDistributionCompanyRepository _PageDistributionCompanyRepository;
        private IDistributionCompanyRepository PageDistributionCompanyRepository
        {
            get
            {
                if (_PageDistributionCompanyRepository == null)
                    _PageDistributionCompanyRepository = new DistributionCompanyRepository();

                return _PageDistributionCompanyRepository;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.DBClientInvoiceManage;
            if (!IsPostBack)
            {
                BindDistributionCompanies();
            }
        }

        private void BindDistributionCompanies()
        {
            var distributionCompanies = PageDistributionCompanyRepository.GetDropdownItems();

            rcbxDistributionCompany.DataSource = distributionCompanies;
            rcbxDistributionCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDistributionCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDistributionCompany.DataBind();

            rcbxDistributionCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }
        private void BindDBClientInvoice(bool isNeedRebind)
        {
            UISearchDBClientInvoice uiSearchObj = new UISearchDBClientInvoice()
            {
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
                InvoiceNumber = txtInvoiceNumber.Text.Trim(),
                CompanyID = SiteUser.GetCurrentSiteUser().CompanyID,
                DistributionCompanyID = rcbxDistributionCompany.SelectedValue.ToIntOrNull()
            };

            int totalRecords = 0;

            var uiDBClientInvoices = PageDBClientInvoiceRepository.GetUIList(uiSearchObj, rgDBClientInvoices.CurrentPageIndex, rgDBClientInvoices.PageSize, out totalRecords);

            rgDBClientInvoices.VirtualItemCount = totalRecords;

            rgDBClientInvoices.DataSource = uiDBClientInvoices;


            if (isNeedRebind)
                rgDBClientInvoices.Rebind();
        }


        protected void rgDBClientInvoices_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindDBClientInvoice(false);
        }

        protected void rgDBClientInvoices_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;
            var DBClientInvoiceID = editableItem.GetDataKeyValue("ID").ToIntOrNull();

            if (DBClientInvoiceID.BiggerThanZero())
            {
                var DBClientInvoice = PageDBClientInvoiceRepository.GetByID(DBClientInvoiceID);
                var DBClientInvoiceDetails = DBClientInvoice.DBClientInvoiceDetail.ToList();
                DBClientInvoiceDetails.ForEach(x => { PageDBClientInvoiceDetailRepository.DeleteByID(x.ID); });
                PageDBClientInvoiceRepository.DeleteByID(DBClientInvoiceID);
                PageDBClientInvoiceDetailRepository.Save();
                PageDBClientInvoiceRepository.Save();
                rgDBClientInvoices.Rebind();
            }


        }

        protected void rgDBClientInvoices_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgDBClientInvoices_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgDBClientInvoices_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindDBClientInvoice(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpEndDate.SelectedDate = rdpBeginDate.SelectedDate = null;

            txtInvoiceNumber.Text = rcbxDistributionCompany.Text = rcbxDistributionCompany.SelectedValue = string.Empty;
           
            BindDBClientInvoice(true);
        }
    }
}