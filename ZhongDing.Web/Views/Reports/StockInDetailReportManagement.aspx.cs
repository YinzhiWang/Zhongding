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
    public partial class StockInDetailReportManagement : BasePage
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            this.Master.MenuItemID = (int)EMenuItem.StockInDetailReportManage;
            if (!IsPostBack)
            {
                BindSuppliers();
                BindProducts();
                base.PermissionOptionCheckButtonExport(btnExport);
            }

        }
        private void BindStockInDetailReport(bool isNeedRebind)
        {
            UISearchStockInDetailReport uiSearchObj = new UISearchStockInDetailReport()
            {
                BeginDate = rdpBeginDate.SelectedDate.HasValue ? rdpBeginDate.SelectedDate : GlobalConst.DATETIME_NULL_VALUE,
                EndDate = rdpEndDate.SelectedDate.HasValue ? rdpEndDate.SelectedDate.Value.AddDays(1) : GlobalConst.DATETIME_NULL_VALUE,
                SupplierId = rcbxSupplier.SelectedValue.ToIntOrNull(),
                ProductId = rcbxProduct.SelectedValue.ToIntOrNull(),
                BatchNumber = txtBatchNumber.Text.Trim()

            };

            int totalRecords = 0;

            var uiStockInDetailReports = PageReportRepository.GetStockInDetailReport(uiSearchObj, rgStockInDetailReports.CurrentPageIndex, rgStockInDetailReports.PageSize, out totalRecords);

            rgStockInDetailReports.VirtualItemCount = totalRecords;

            rgStockInDetailReports.DataSource = uiStockInDetailReports;

            if (isNeedRebind)
                rgStockInDetailReports.Rebind();
        }


        protected void rgStockInDetailReports_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindStockInDetailReport(false);
        }

        protected void rgStockInDetailReports_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();


            rgStockInDetailReports.Rebind();
        }

        protected void rgStockInDetailReports_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgStockInDetailReports_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgStockInDetailReports_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindStockInDetailReport(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpBeginDate.SelectedDate = rdpEndDate.SelectedDate = null;
            rcbxProduct.SelectedValue = rcbxSupplier.SelectedValue = string.Empty;
            rcbxProduct.Text = rcbxSupplier.Text = string.Empty;
            txtBatchNumber.Text = string.Empty;
            BindStockInDetailReport(true);
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
            UISearchStockInDetailReport uiSearchObj = new UISearchStockInDetailReport()
            {
                BeginDate = rdpBeginDate.SelectedDate.HasValue ? rdpBeginDate.SelectedDate : GlobalConst.DATETIME_NULL_VALUE,
                EndDate = rdpEndDate.SelectedDate.HasValue ? rdpEndDate.SelectedDate.Value.AddDays(1) : GlobalConst.DATETIME_NULL_VALUE,
                SupplierId = rcbxSupplier.SelectedValue.ToIntOrNull(),
                ProductId = rcbxProduct.SelectedValue.ToIntOrNull(),
                BatchNumber = txtBatchNumber.Text.Trim()
            };

            var uiStockInDetailReports = PageReportRepository.GetStockInDetailReport(uiSearchObj);
            var excelPath = Server.MapPath("~/App_Data/") + "TempExcel.xls";
            ExcelHelper.RenderToExcel<UIStockInDetailReport>(uiStockInDetailReports,
                new List<ExcelHeader>() {
                    new ExcelHeader() { Key = "EntryDate", Name = "订单日期" },
                    new ExcelHeader(){ Key="StockInCode", Name="入库单号"},
                    new ExcelHeader(){ Key="OrderCode", Name="订单号"},
                    new ExcelHeader(){ Key="SupplierName", Name="供应商"},
                    new ExcelHeader(){ Key="WarehouseName", Name="仓库"},
                    new ExcelHeader(){ Key="ProductCode", Name="货品编号"},
                    new ExcelHeader(){ Key="CategoryName", Name="货品类别"},
                    new ExcelHeader(){ Key="ProductName", Name="货品名称"},
                    new ExcelHeader(){ Key="Specification", Name="规格"},
                    new ExcelHeader(){ Key="UnitName", Name="基本单位"},
                    new ExcelHeader(){ Key="ProcurePrice", Name="采购单价"},
                    new ExcelHeader(){ Key="InQty", Name="数量"},
                    new ExcelHeader(){ Key="NumberOfPackages", Name="件数"},
                    new ExcelHeader(){ Key="BatchNumber", Name="批号"},
                    new ExcelHeader(){ Key="ExpirationDate", Name="有效期"},
                    new ExcelHeader(){ Key="TotalAmount", Name="进货货款"},
                }, excelPath);

            Response.ContentType = "application/x-zip-compressed";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + "入库明细报表".UrlEncode() + ".xls");
            string filename = excelPath;
            Response.TransmitFile(filename);
        }



        protected override EPermission PagePermissionID()
        {
            return EPermission.StockInDetailReportManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.View;
        }
    }
}