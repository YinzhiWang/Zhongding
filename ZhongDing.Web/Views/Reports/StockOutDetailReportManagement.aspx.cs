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
    public partial class StockOutDetailReportManagement : BasePage
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

            this.Master.MenuItemID = (int)EMenuItem.StockOutDetailReportManage;
            if (!IsPostBack)
            {
                BindClientUsers();
                BindProducts();
                BindClientCompanies();
                base.PermissionOptionCheckButtonExport(btnExport);
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

        private void BindStockOutDetailReport(bool isNeedRebind)
        {
            UISearchStockOutDetailReport uiSearchObj = new UISearchStockOutDetailReport()
            {
                BeginDate = rdpBeginDate.SelectedDate.HasValue ? rdpBeginDate.SelectedDate : GlobalConst.DATETIME_NULL_VALUE,
                EndDate = rdpEndDate.SelectedDate.HasValue ? rdpEndDate.SelectedDate.Value.AddDays(1) : GlobalConst.DATETIME_NULL_VALUE,
                ClientUserId = rcbxClientUser.SelectedValue.ToIntOrNull(),
                ClientCompanyId = rcbxClientCompany.SelectedValue.ToIntOrNull(),
                ProductId = rcbxProduct.SelectedValue.ToIntOrNull()

            };

            int totalRecords = 0;

            var uiStockOutDetailReports = PageReportRepository.GetStockOutDetailReport(uiSearchObj, rgStockOutDetailReports.CurrentPageIndex, rgStockOutDetailReports.PageSize, out totalRecords);

            rgStockOutDetailReports.VirtualItemCount = totalRecords;

            rgStockOutDetailReports.DataSource = uiStockOutDetailReports;

            if (isNeedRebind)
                rgStockOutDetailReports.Rebind();
        }


        protected void rgStockOutDetailReports_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindStockOutDetailReport(false);
        }

        protected void rgStockOutDetailReports_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();


            rgStockOutDetailReports.Rebind();
        }

        protected void rgStockOutDetailReports_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgStockOutDetailReports_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgStockOutDetailReports_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindStockOutDetailReport(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpBeginDate.SelectedDate = rdpEndDate.SelectedDate = null;
            rcbxClientCompany.SelectedValue = rcbxClientUser.SelectedValue = rcbxProduct.SelectedValue = string.Empty;
            rcbxClientCompany.Text = rcbxClientUser.Text = rcbxProduct.Text = string.Empty;


            BindStockOutDetailReport(true);
        }

        protected void rcbxSupplier_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value))
            {
                var supplierID = e.Value.ToIntOrNull();
                if (supplierID.BiggerThanZero())
                {
                    BindProducts(supplierID.Value);
                    rcbxProduct.SelectedIndex = 0;
                    rcbxProduct.Text = string.Empty;
                }
            }
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
            UISearchStockOutDetailReport uiSearchObj = new UISearchStockOutDetailReport()
            {
                BeginDate = rdpBeginDate.SelectedDate.HasValue ? rdpBeginDate.SelectedDate : GlobalConst.DATETIME_NULL_VALUE,
                EndDate = rdpEndDate.SelectedDate.HasValue ? rdpEndDate.SelectedDate.Value.AddDays(1) : GlobalConst.DATETIME_NULL_VALUE,
                ClientUserId = rcbxClientUser.SelectedValue.ToIntOrNull(),
                ClientCompanyId = rcbxClientCompany.SelectedValue.ToIntOrNull(),
                ProductId = rcbxProduct.SelectedValue.ToIntOrNull()

            };

            var uiStockOutDetailReports = PageReportRepository.GetStockOutDetailReport(uiSearchObj);
            var excelPath = Server.MapPath("~/App_Data/") + "TempExcel.xls";
            ExcelHelper.RenderToExcel<UIStockOutDetailReport>(uiStockOutDetailReports,
                new List<ExcelHeader>() {
                    new ExcelHeader() { Key = "OutDate", Name = "出库日期" },
                    new ExcelHeader(){ Key="StockOutCode", Name="出库单号"},
                    new ExcelHeader(){ Key="SalesOrderApplicationOrderCode", Name="销售订单号"},
                    new ExcelHeader(){ Key="ClientName", Name="客户"},
                    new ExcelHeader(){ Key="ClientCompanyName", Name="商业单位"},
                    new ExcelHeader(){ Key="WarehouseName", Name="仓库"},
                    new ExcelHeader(){ Key="ProductCode", Name="货品编号"},
                    new ExcelHeader(){ Key="CategoryName", Name="货品类别"},
                    new ExcelHeader(){ Key="ProductName", Name="货品名称"},
                    new ExcelHeader(){ Key="Specification", Name="规格"},
                    new ExcelHeader(){ Key="UnitName", Name="基本单位"},
                    new ExcelHeader(){ Key="OutQty", Name="出库数量"},
                    new ExcelHeader(){ Key="NumberOfPackages", Name="件数"},
                    new ExcelHeader(){ Key="BatchNumber", Name="批号"},
                    new ExcelHeader(){ Key="ExpirationDate", Name="有效期"},
                    new ExcelHeader(){ Key="ProcurePrice", Name="采购单价"},
                    new ExcelHeader(){ Key="SalesPrice", Name="销售单价"},
                    new ExcelHeader(){ Key="TotalSalesAmount", Name="销售货款"}
                }, excelPath);

            Response.ContentType = "application/x-zip-compressed";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + "出库明细报表".UrlEncode() + ".xls");
            string filename = excelPath;
            Response.TransmitFile(filename);
        }



        protected override EPermission PagePermissionID()
        {
            return EPermission.StockOutDetailReportManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.View;
        }
    }
}