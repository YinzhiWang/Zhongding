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
using ZhongDing.Common.NPOIHelper.Excel;

namespace ZhongDing.Web.Views.Reports
{
    public partial class ProcureOrderReportManagement : BasePage
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

            this.Master.MenuItemID = (int)EMenuItem.ProcureOrderReportManage;
            if (!IsPostBack)
            {
                BindSuppliers();
                BindProducts();
                base.PermissionOptionCheckButtonExport(btnExport);
            }

        }
        private void BindProcureOrderReport(bool isNeedRebind)
        {
            UISearchProcureOrderReport uiSearchObj = new UISearchProcureOrderReport()
            {
                BeginDate = rdpBeginDate.SelectedDate.HasValue ? rdpBeginDate.SelectedDate : GlobalConst.DATETIME_NULL_VALUE,
                EndDate = rdpEndDate.SelectedDate.HasValue ? rdpEndDate.SelectedDate.Value.AddDays(1) : GlobalConst.DATETIME_NULL_VALUE,
                SupplierId = rcbxSupplier.SelectedValue.ToIntOrNull(),
                ProductId = rcbxProduct.SelectedValue.ToIntOrNull()

            };

            int totalRecords = 0;

            var uiProcureOrderReports = PageReportRepository.GetProcureOrderReport(uiSearchObj, rgProcureOrderReports.CurrentPageIndex, rgProcureOrderReports.PageSize, out totalRecords);

            rgProcureOrderReports.VirtualItemCount = totalRecords;

            rgProcureOrderReports.DataSource = uiProcureOrderReports;

            if (isNeedRebind)
                rgProcureOrderReports.Rebind();
        }


        protected void rgProcureOrderReports_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindProcureOrderReport(false);
        }

        protected void rgProcureOrderReports_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();


            rgProcureOrderReports.Rebind();
        }

        protected void rgProcureOrderReports_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgProcureOrderReports_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgProcureOrderReports_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindProcureOrderReport(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpBeginDate.SelectedDate = rdpEndDate.SelectedDate = null;
            rcbxProduct.SelectedValue = rcbxSupplier.SelectedValue = string.Empty;
            rcbxProduct.Text = rcbxSupplier.Text = string.Empty;
            BindProcureOrderReport(true);
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
            UISearchProcureOrderReport uiSearchObj = new UISearchProcureOrderReport()
            {
                BeginDate = rdpBeginDate.SelectedDate.HasValue ? rdpBeginDate.SelectedDate : GlobalConst.DATETIME_NULL_VALUE,
                EndDate = rdpEndDate.SelectedDate.HasValue ? rdpEndDate.SelectedDate.Value.AddDays(1) : GlobalConst.DATETIME_NULL_VALUE,
                SupplierId = rcbxSupplier.SelectedValue.ToIntOrNull(),
                ProductId = rcbxProduct.SelectedValue.ToIntOrNull()

            };

            var uiProcureOrderReports = PageReportRepository.GetProcureOrderReport(uiSearchObj);
            var excelPath = Server.MapPath("~/App_Data/") + "TempExcel.xls";
            //ExcelHelper.RenderToExcel<UIProcureOrderReport>(uiProcureOrderReports,
            //    new List<ExcelHeader>() {
            //        new ExcelHeader() { Key = "OrderDate", Name = "订单日期" },
            //        new ExcelHeader(){ Key="OrderCode", Name="订单号"},
            //        new ExcelHeader(){ Key="SupplierName", Name="供应商"},
            //        new ExcelHeader(){ Key="WarehouseName", Name="仓库"},
            //        new ExcelHeader(){ Key="ProductCode", Name="货品编号"},
            //        new ExcelHeader(){ Key="CategoryName", Name="货品类别"},
            //        new ExcelHeader(){ Key="ProductName", Name="货品名称"},
            //        new ExcelHeader(){ Key="Specification", Name="规格"},
            //        new ExcelHeader(){ Key="UnitName", Name="基本单位"},
            //        new ExcelHeader(){ Key="ProcurePrice", Name="采购单价"},
            //        new ExcelHeader(){ Key="ProcureCount", Name="数量"},
            //        new ExcelHeader(){ Key="TotalAmount", Name="金额"},
            //        new ExcelHeader(){ Key="AlreadyInQty", Name="基本数量"},
            //        new ExcelHeader(){ Key="AlreadyInNumberOfPackages", Name="件数"},
            //        new ExcelHeader(){ Key="AlreadyInQtyProcurePrice", Name="金额"},
            //        new ExcelHeader(){ Key="StopInQty", Name="基本数量"},
            //        new ExcelHeader(){ Key="StopInNumberOfPackages", Name="件数"},
            //        new ExcelHeader(){ Key="StopInQtyProcurePrice", Name="金额"},
            //        new ExcelHeader(){ Key="NotInQty", Name="基本数量"},
            //        new ExcelHeader(){ Key="NotInNumberOfPackages", Name="件数"},
            //        new ExcelHeader(){ Key="NotInQtyProcurePrice", Name="金额"}
            //    }, excelPath);



            //"rowspan": 2, 
            //"sheetname": "按商机类型分类", 
            //"defaultwidth": 12, 
            //"defaultheight": 35, 
            NPOIHelper nPOIHelper = new Common.NPOIHelper.Excel.NPOIHelper();
            UIProcureOrderReport model = new UIProcureOrderReport();

            List<ExcelHeader> excelHeaders = new List<ExcelHeader>() { 
                new ExcelHeader(model.GetName(() => model.OrderDate),"订单日期"), 
                new ExcelHeader(model.GetName(() => model.OrderCode),"订单号"),
                new ExcelHeader(model.GetName(() => model.SupplierName),"供应商"),
                new ExcelHeader(model.GetName(() => model.WarehouseName),"仓库"),
                new ExcelHeader(model.GetName(() => model.ProductCode),"货品编号"),
                new ExcelHeader(model.GetName(() => model.CategoryName),"货品类别"),
                new ExcelHeader(model.GetName(() => model.ProductName),"货品名称"),
                new ExcelHeader(model.GetName(() => model.Specification),"规格"),
                new ExcelHeader(model.GetName(() => model.UnitName),"基本单位"),
                new ExcelHeader(model.GetName(() => model.ProcurePrice),"采购单价"),
                new ExcelHeader(model.GetName(() => model.ProcureCount),"数量"),
                new ExcelHeader(model.GetName(() => model.TotalAmount),"金额"),
                new ExcelHeader(model.GetName(() => model.AlreadyInQty),"基本数量"),
                new ExcelHeader(model.GetName(() => model.AlreadyInNumberOfPackages),"件数"),
                new ExcelHeader(model.GetName(() => model.AlreadyInQtyProcurePrice),"金额"),
                new ExcelHeader(model.GetName(() => model.StopInQty),"基本数量"),
                new ExcelHeader(model.GetName(() => model.StopInNumberOfPackages),"件数"),
                new ExcelHeader(model.GetName(() => model.StopInQtyProcurePrice),"金额"),
                new ExcelHeader(model.GetName(() => model.NotInQty),"基本数量"),
                new ExcelHeader(model.GetName(() => model.NotInNumberOfPackages),"件数"),
                new ExcelHeader(model.GetName(() => model.NotInQtyProcurePrice),"金额")
            };
            Queue<ExcelHeader> excelHeadersQueue = new Queue<ExcelHeader>(excelHeaders);
            Root excelRoot = new Root()
            {
                root = new HeadInfo()
                {
                    rowspan = 2,
                    sheetname = "采购订单报表",
                    defaultheight = null,
                    defaultwidth = 12,
                    head = new List<AttributeList>(){
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,0,0"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,1,1", width=20},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,2,2", width=20},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,3,3"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,4,4"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,5,5"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,6,6", width=20},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,7,7"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,8,8"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,9,9"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,10,10"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,11,11"},

                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,12,12"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,13,13"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,14,14"},

                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,15,15"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,16,16"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,17,17"},

                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,18,18"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,19,19"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,20,20"},

                    new AttributeList(){ title="已执行数量", cellregion="0,0,12,14"},
                    new AttributeList(){ title="中止数量", cellregion="0,0,15,17"},
                    new AttributeList(){ title="未执行数量", cellregion="0,0,18,20"},

                    }
                }
            };

            List<Func<UIProcureOrderReport, string>> fieldFuncs = new List<Func<UIProcureOrderReport, string>>();

            fieldFuncs.Add(x => x.OrderDate.ToString("yyyy/MM/dd"));
            fieldFuncs.Add(x => x.OrderCode);
            fieldFuncs.Add(x => x.SupplierName);
            fieldFuncs.Add(x => x.WarehouseName);
            fieldFuncs.Add(x => x.ProductCode);
            fieldFuncs.Add(x => x.CategoryName);
            fieldFuncs.Add(x => x.ProductName);
            fieldFuncs.Add(x => x.Specification);
            fieldFuncs.Add(x => x.UnitName);
            fieldFuncs.Add(x => x.ProcurePrice.ToString("f2"));
            fieldFuncs.Add(x => x.ProcureCount.ToString());
            fieldFuncs.Add(x => x.TotalAmount.ToString("f2"));

            fieldFuncs.Add(x => x.AlreadyInQty.ToString());
            fieldFuncs.Add(x => x.AlreadyInNumberOfPackages.ToString("f2"));
            fieldFuncs.Add(x => x.AlreadyInQtyProcurePrice.ToString("f2"));

            fieldFuncs.Add(x => x.StopInQty.ToString());
            fieldFuncs.Add(x => x.StopInNumberOfPackages.ToString("f2"));
            fieldFuncs.Add(x => x.StopInQtyProcurePrice.ToString("f2"));

            fieldFuncs.Add(x => x.NotInQty.ToString());
            fieldFuncs.Add(x => x.NotInNumberOfPackages.ToString("f2"));
            fieldFuncs.Add(x => x.NotInQtyProcurePrice.ToString("f2"));


            nPOIHelper.ExportToExcel<UIProcureOrderReport>(
                (List<UIProcureOrderReport>)uiProcureOrderReports,
                excelPath,
                excelHeaders.Select(x => x.Key).ToArray(),
                excelRoot,
                fieldFuncs.ToArray());


            Response.ContentType = "application/x-zip-compressed";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + "采购订单报表".UrlEncode() + ".xls");
            string filename = excelPath;
            Response.TransmitFile(filename);
        }



        protected override EPermission PagePermissionID()
        {
            return EPermission.ProcureOrderReportManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.View;
        }
    }
}