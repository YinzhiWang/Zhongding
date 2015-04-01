using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Business.Repositories.Reports;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.UISearchObjects;
using ZhongDing.Common.Extension;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.Web.Views.Reports
{
    public partial class ClientSaleAppReportManagement : BasePage
    {
        #region Members
        private IReportRepository _PageReportRepository;
        private IReportRepository PageReportRepository
        {
            get
            {
                if (_PageReportRepository == null)
                    _PageReportRepository = new ReportRepository();

                return _PageReportRepository;
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
        private IClientUserRepository _PageClientUserRepository;
        private IClientUserRepository PageClientUserRepository
        {
            get
            {
                if (_PageClientUserRepository == null)
                    _PageClientUserRepository = new ClientUserRepository();

                return _PageClientUserRepository;
            }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            this.Master.MenuItemID = (int)EMenuItem.ClientSaleAppReportManage;
            if (!IsPostBack)
            {
                BindClientUsers();
                BindProducts();
                BindClientCompanies();
            }

        }
        private void BindClientUsers()
        {
            var clientUsers = PageClientUserRepository.GetDropdownItems();
            rcbxClientUser.DataSource = clientUsers;
            rcbxClientUser.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientUser.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientUser.DataBind();

            rcbxClientUser.Items.Insert(0, new RadComboBoxItem("", ""));
        }
        private void BindClientCompanies()
        {
            rcbxClientCompany.ClearSelection();
            rcbxClientCompany.Items.Clear();

            var uiSearchObj = new UISearchDropdownItem();

            if (!string.IsNullOrEmpty(rcbxClientUser.SelectedValue))
            {
                int clientUserID;
                if (int.TryParse(rcbxClientUser.SelectedValue, out clientUserID))
                    uiSearchObj.Extension = new UISearchExtension { ClientUserID = clientUserID };
            }

            var clientCompanies = PageClientCompanyRepository.GetDropdownItems(uiSearchObj);
            rcbxClientCompany.DataSource = clientCompanies;
            rcbxClientCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientCompany.DataBind();

            rcbxClientCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }
        protected void rcbxClientUser_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindClientCompanies();
        }
        private void BindClientSaleAppReport(bool isNeedRebind)
        {
            UISearchClientSaleAppReport uiSearchObj = new UISearchClientSaleAppReport()
            {
                BeginDate = rdpBeginDate.SelectedDate.HasValue ? rdpBeginDate.SelectedDate : GlobalConst.DATETIME_NULL_VALUE,
                EndDate = rdpEndDate.SelectedDate.HasValue ? rdpEndDate.SelectedDate.Value.AddDays(1) : GlobalConst.DATETIME_NULL_VALUE,
                ClientUserId = rcbxClientUser.SelectedValue.ToIntOrNull(),
                ClientCompanyId = rcbxClientCompany.SelectedValue.ToIntOrNull(),
                ProductId = rcbxProduct.SelectedValue.ToIntOrNull()

            };

            int totalRecords = 0;

            var uiClientSaleAppReports = PageReportRepository.GetClientSaleAppReport(uiSearchObj, rgClientSaleAppReports.CurrentPageIndex, rgClientSaleAppReports.PageSize, out totalRecords);

            rgClientSaleAppReports.VirtualItemCount = totalRecords;

            rgClientSaleAppReports.DataSource = uiClientSaleAppReports;

            if (isNeedRebind)
                rgClientSaleAppReports.Rebind();
        }


        protected void rgClientSaleAppReports_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindClientSaleAppReport(false);
        }

        protected void rgClientSaleAppReports_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();


            rgClientSaleAppReports.Rebind();
        }

        protected void rgClientSaleAppReports_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgClientSaleAppReports_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgClientSaleAppReports_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindClientSaleAppReport(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

            rdpBeginDate.SelectedDate = rdpEndDate.SelectedDate = null;
            rcbxClientCompany.SelectedValue = rcbxClientUser.SelectedValue = rcbxProduct.SelectedValue = string.Empty;
            rcbxClientCompany.Text = rcbxClientUser.Text = rcbxProduct.Text = string.Empty;

            BindClientSaleAppReport(true);
        }



        private void BindProducts(int supplierID = 0)
        {
            var products = PageProductRepository.GetDropdownItems(new UISearchDropdownItem()
            {
                Extension = new UISearchExtension
                {
                    SupplierID = supplierID,
                }
            });

            rcbxProduct.DataSource = products;
            rcbxProduct.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxProduct.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxProduct.DataBind();

            rcbxProduct.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            /*
             微软为Response对象提供了一个新的方法TransmitFile来解决使用Response.BinaryWrite
            下载超过400mb的文件时导致Aspnet_wp.exe进程回收而无法成功下载的问题。
            代码如下：
             */
            UISearchClientSaleAppReport uiSearchObj = new UISearchClientSaleAppReport()
            {
                BeginDate = rdpBeginDate.SelectedDate.HasValue ? rdpBeginDate.SelectedDate : GlobalConst.DATETIME_NULL_VALUE,
                EndDate = rdpEndDate.SelectedDate.HasValue ? rdpEndDate.SelectedDate.Value.AddDays(1) : GlobalConst.DATETIME_NULL_VALUE,
                ClientUserId = rcbxClientUser.SelectedValue.ToIntOrNull(),
                ClientCompanyId = rcbxClientCompany.SelectedValue.ToIntOrNull(),
                ProductId = rcbxProduct.SelectedValue.ToIntOrNull()

            };

            var uiClientSaleAppReports = PageReportRepository.GetClientSaleAppReport(uiSearchObj);
            var excelPath = Server.MapPath("~/App_Data/") + "Excel.xls";
            ExcelHelper.RenderToExcel<UIClientSaleAppReport>(uiClientSaleAppReports,
                new List<ExcelHeader>() {
                    new ExcelHeader() { Key = "OrderDate", Name = "订单日期" },
                    new ExcelHeader(){ Key="OrderCode", Name="订单号"},
                    new ExcelHeader(){ Key="ClientName", Name="客户"},
                    new ExcelHeader(){ Key="ClientCompanyName", Name="商业单位"},
                    new ExcelHeader(){ Key="CategoryName", Name="货品类别"},
                    new ExcelHeader(){ Key="ProductCode", Name="货品编号"},
                    new ExcelHeader(){ Key="ProductName", Name="货品名称"},
                    new ExcelHeader(){ Key="Specification", Name="规格"},
                    new ExcelHeader(){ Key="UnitName", Name="基本单位"},
                    new ExcelHeader(){ Key="SalesPrice", Name="数量"},
                    new ExcelHeader(){ Key="Count", Name="销售单价"},
                    new ExcelHeader(){ Key="TotalSalesAmount", Name="金额"},
                }, excelPath);

            Response.ContentType = "application/x-zip-compressed";
            Response.AddHeader("Content-Disposition", "attachment;filename=Excel.xls");
            string filename = excelPath;
            Response.TransmitFile(filename);
        }




    }
}