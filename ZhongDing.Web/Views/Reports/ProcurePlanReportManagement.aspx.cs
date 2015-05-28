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
    public partial class ProcurePlanReportManagement : BasePage
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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            this.Master.MenuItemID = (int)EMenuItem.ProcurePlanReportManage;
            if (!IsPostBack)
            {
                BindWarehouses();
                base.PermissionOptionCheckButtonExport(btnExport);
            }

        }
        private void BindProcureOrderReport(bool isNeedRebind)
        {
            UISearchProcurePlanReport uiSearchObj = new UISearchProcurePlanReport()
            {
                ProductName = txtProductName.Text.Trim(),
                WarehouseID = rcbxWarehouse.SelectedValue.ToIntOrNull()

            };

            int totalRecords = 0;

            var uiProcureOrderReports = PageReportRepository.GetProcurePlanReport(uiSearchObj, rgProcureOrderReports.CurrentPageIndex, rgProcureOrderReports.PageSize, out totalRecords);

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

            rcbxWarehouse.SelectedValue = string.Empty;
            rcbxWarehouse.Text = txtProductName.Text = string.Empty;
            BindProcureOrderReport(true);
        }

        private void BindWarehouses()
        {
            var uiSearchObj = new UISearchDropdownItem
            {
                Extension = new UISearchExtension
                {
                    CompanyID = CurrentUser.CompanyID
                }
            };

            var warehouses = PageWarehouseRepository.GetDropdownItems(uiSearchObj);

            rcbxWarehouse.DataSource = warehouses;
            rcbxWarehouse.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxWarehouse.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxWarehouse.DataBind();

            rcbxWarehouse.Items.Insert(0, new RadComboBoxItem("", ""));
        }


        protected void btnExport_Click(object sender, EventArgs e)
        {
            /*
             微软为Response对象提供了一个新的方法TransmitFile来解决使用Response.BinaryWrite
            下载超过400mb的文件时导致Aspnet_wp.exe进程回收而无法成功下载的问题。
            代码如下：
             */
            UISearchProcurePlanReport uiSearchObj = new UISearchProcurePlanReport()
            {
                ProductName = txtProductName.Text.Trim(),
                WarehouseID = rcbxWarehouse.SelectedValue.ToIntOrNull()

            };

            var uiProcurePlanReports = PageReportRepository.GetProcurePlanReport(uiSearchObj);
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
            UIProcurePlanReport model = new UIProcurePlanReport();

            List<ExcelHeader> excelHeaders = new List<ExcelHeader>() { 
               
                new ExcelHeader(model.GetName(() => model.ProductName),"货品名称"),
                new ExcelHeader(model.GetName(() => model.Specification),"规格"),
                new ExcelHeader(model.GetName(() => model.UnitName),"基本单位"),
                new ExcelHeader(model.GetName(() => model.WarehouseName),"仓库"),
                new ExcelHeader(model.GetName(() => model.ToBeOutNumberOfPackages),"件数"),
                new ExcelHeader(model.GetName(() => model.TotalToBeOutQty),"基本数量"),
                new ExcelHeader(model.GetName(() => model.WarehouseNumberOfPackages),"件数"),
                new ExcelHeader(model.GetName(() => model.WarehouseQty),"基本数量"),
             
            };
            Queue<ExcelHeader> excelHeadersQueue = new Queue<ExcelHeader>(excelHeaders);
            Root excelRoot = new Root()
            {
                root = new HeadInfo()
                {
                    rowspan = 2,
                    sheetname = "采购计划报表",
                    defaultheight = null,
                    defaultwidth = 20,
                    head = new List<AttributeList>(){
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,0,0"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,1,1"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,2,2"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,3,3"},

                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,4,4"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,5,5"},

                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,6,6"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,7,7"},
         

                    new AttributeList(){ title="未发数量", cellregion="0,0,4,5"},
                    new AttributeList(){ title="库存数量", cellregion="0,0,6,7"},

                    }
                }
            };

            List<Func<UIProcurePlanReport, string>> fieldFuncs = new List<Func<UIProcurePlanReport, string>>();


           
            fieldFuncs.Add(x => x.ProductName);
            fieldFuncs.Add(x => x.Specification);
            fieldFuncs.Add(x => x.UnitName);
            fieldFuncs.Add(x => x.WarehouseName);
            fieldFuncs.Add(x => x.ToBeOutNumberOfPackages.ToString());
            fieldFuncs.Add(x => x.TotalToBeOutQty.ToString());
            fieldFuncs.Add(x => x.WarehouseNumberOfPackages.ToString());
            fieldFuncs.Add(x => x.WarehouseQty.ToString());


            nPOIHelper.ExportToExcel<UIProcurePlanReport>(
                (List<UIProcurePlanReport>)uiProcurePlanReports,
                excelPath,
                excelHeaders.Select(x => x.Key).ToArray(),
                excelRoot,
                fieldFuncs.ToArray());


            Response.ContentType = "application/x-zip-compressed";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + "采购计划报表".UrlEncode() + ".xls");
            string filename = excelPath;
            Response.TransmitFile(filename);
        }



        protected override EPermission PagePermissionID()
        {
            return EPermission.ProcurePlanReportManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.View;
        }
    }
}