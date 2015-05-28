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
    public partial class InventorySummaryReportManagement : BasePage
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

            this.Master.MenuItemID = (int)EMenuItem.InventorySummaryReportManage;
            if (!IsPostBack)
            {
                BindProducts();
                base.PermissionOptionCheckButtonExport(btnExport);
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
        private void BindInventorySummaryReport(bool isNeedRebind)
        {
            UISearchInventorySummaryReport uiSearchObj = new UISearchInventorySummaryReport()
            {
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
                ProductID = rcbxProduct.SelectedValue.ToIntOrNull()

            };

            int totalRecords = 0;

            var reports = PageReportRepository.GetInventorySummaryReport(uiSearchObj, rgInventorySummaryReports.CurrentPageIndex, rgInventorySummaryReports.PageSize, out totalRecords);

            rgInventorySummaryReports.VirtualItemCount = totalRecords;

            rgInventorySummaryReports.DataSource = reports;

            if (isNeedRebind)
                rgInventorySummaryReports.Rebind();
        }


        protected void rgInventorySummaryReports_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindInventorySummaryReport(false);
        }

        protected void rgInventorySummaryReports_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();


            rgInventorySummaryReports.Rebind();
        }

        protected void rgInventorySummaryReports_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgInventorySummaryReports_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgInventorySummaryReports_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindInventorySummaryReport(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

            rcbxProduct.SelectedValue = string.Empty;
            rcbxProduct.Text = string.Empty;
            BindInventorySummaryReport(true);
        }


        protected void btnExport_Click(object sender, EventArgs e)
        {
            /*
             微软为Response对象提供了一个新的方法TransmitFile来解决使用Response.BinaryWrite
            下载超过400mb的文件时导致Aspnet_wp.exe进程回收而无法成功下载的问题。
            代码如下：
             */
            UISearchInventorySummaryReport uiSearchObj = new UISearchInventorySummaryReport()
            {
                BeginDate = rdpBeginDate.SelectedDate,
                EndDate = rdpEndDate.SelectedDate,
                ProductID = rcbxProduct.SelectedValue.ToIntOrNull()

            };

            var reports = PageReportRepository.GetInventorySummaryReport(uiSearchObj);
            var excelPath = Server.MapPath("~/App_Data/") + "TempExcel.xls";
            //"rowspan": 2, 
            //"sheetname": "按商机类型分类", 
            //"defaultwidth": 12, 
            //"defaultheight": 35, 
            NPOIHelper nPOIHelper = new Common.NPOIHelper.Excel.NPOIHelper();
            UIInventorySummaryReport model = new UIInventorySummaryReport();

            List<ExcelHeader> excelHeaders = new List<ExcelHeader>() { 
                new ExcelHeader(model.GetName(() => model.ProductCode),"货品编码"),
                new ExcelHeader(model.GetName(() => model.ProductName),"货品名称"),
                new ExcelHeader(model.GetName(() => model.Specification),"规格"),
                new ExcelHeader(model.GetName(() => model.UnitName),"基本单位"),
                new ExcelHeader(model.GetName(() => model.BatchNumber),"批号"),
                new ExcelHeader(model.GetName(() => model.ExpirationDate),"有效期"),

                new ExcelHeader(model.GetName(() => model.PreBalanceQty),"基本数量"),
                new ExcelHeader(model.GetName(() => model.PreBalanceQtyPackages),"件数"),

                new ExcelHeader(model.GetName(() => model.InQty),"基本数量"),
                new ExcelHeader(model.GetName(() => model.InQtyPackages),"件数"),

                new ExcelHeader(model.GetName(() => model.OutQty),"基本数量"),
                new ExcelHeader(model.GetName(() => model.OutQtyPackages),"件数"),
                
                new ExcelHeader(model.GetName(() => model.CurrentBalanceQty),"基本数量"),
                new ExcelHeader(model.GetName(() => model.CurrentBalanceQtyPackages),"件数"),
                new ExcelHeader(model.GetName(() => model.Amount),"金额")
            };
            Queue<ExcelHeader> excelHeadersQueue = new Queue<ExcelHeader>(excelHeaders);
            Root excelRoot = new Root()
            {
                root = new HeadInfo()
                {
                    rowspan = 2,
                    sheetname = "库存汇总表",
                    defaultheight = null,
                    defaultwidth = 20,
                    head = new List<AttributeList>(){
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,0,0"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,1,1"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,2,2"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,3,3"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,4,4"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,5,5"},

                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,6,6"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,7,7"},

                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,8,8"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,9,9"},

                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,10,10"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,11,11"},

                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,12,12"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,13,13"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,14,14"},

                    new AttributeList(){ title="上期结存", cellregion="0,0,6,7"},
                    new AttributeList(){ title="本期收入", cellregion="0,0,8,9"},
                    new AttributeList(){ title="本期发出", cellregion="0,0,10,11"},
                    new AttributeList(){ title="本期结存", cellregion="0,0,12,14"},

                    }
                }
            };

            List<Func<UIInventorySummaryReport, string>> fieldFuncs = new List<Func<UIInventorySummaryReport, string>>();



            fieldFuncs.Add(x => x.ProductCode);
            fieldFuncs.Add(x => x.ProductName);
            fieldFuncs.Add(x => x.Specification);
            fieldFuncs.Add(x => x.UnitName);
            fieldFuncs.Add(x => x.BatchNumber);
            fieldFuncs.Add(x => x.ExpirationDate.ToString("yyyy/MM/dd"));
            fieldFuncs.Add(x => x.PreBalanceQty.ToString());
            fieldFuncs.Add(x => x.PreBalanceQtyPackages.ToString("f2"));
            fieldFuncs.Add(x => x.InQty.ToString());
            fieldFuncs.Add(x => x.InQtyPackages.ToString("f2"));
            fieldFuncs.Add(x => x.OutQty.ToString());
            fieldFuncs.Add(x => x.OutQtyPackages.ToString("f2"));
            fieldFuncs.Add(x => x.CurrentBalanceQty.ToString());
            fieldFuncs.Add(x => x.CurrentBalanceQtyPackages.ToString("f2"));
            fieldFuncs.Add(x => x.Amount.ToString("f2"));


            nPOIHelper.ExportToExcel<UIInventorySummaryReport>(
                (List<UIInventorySummaryReport>)reports,
                excelPath,
                excelHeaders.Select(x => x.Key).ToArray(),
                excelRoot,
                fieldFuncs.ToArray());


            Response.ContentType = "application/x-zip-compressed";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + "库存汇总表".UrlEncode() + ".xls");
            string filename = excelPath;
            Response.TransmitFile(filename);
        }



        protected override EPermission PagePermissionID()
        {
            return EPermission.InventorySummaryReportManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.View;
        }
    }
}