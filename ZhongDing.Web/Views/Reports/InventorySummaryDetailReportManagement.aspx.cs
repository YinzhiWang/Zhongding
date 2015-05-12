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
    public partial class InventorySummaryDetailReportManagement : BasePage
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
        /// <summary>
        /// WarehouseID, x.ProductID, x.ProductSpecificationID, x.BatchNumber, x.LicenseNumber, x.ExpirationDate
        /// </summary>
        private int? WarehouseID
        {
            get
            {
                return IntParameter("WarehouseID");
            }
        }
        private int? ProductID
        {
            get
            {
                return IntParameter("ProductID");
            }
        }
        private int? ProductSpecificationID
        {
            get
            {
                return IntParameter("ProductSpecificationID");
            }
        }
        private string BatchNumber
        {
            get
            {
                return StrParameter("BatchNumber");
            }
        }
        private string LicenseNumber
        {
            get
            {
                return StrParameter("LicenseNumber");
            }
        }
        private DateTime? ExpirationDate
        {
            get
            {
                return DateTimeParameter("ExpirationDate");
            }
        }
        private DateTime? BeginDate
        {
            get
            {
                return DateTimeParameter("BeginDate");
            }
        }
        private DateTime? EndDate
        {
            get
            {
                return DateTimeParameter("EndDate");
            }
        }
        private void BindInventorySummaryDetailReport(bool isNeedRebind)
        {
            //WarehouseID, x.ProductID, x.ProductSpecificationID, x.BatchNumber, x.LicenseNumber, x.ExpirationDate
            UISearchInventorySummaryDetailReport uiSearchObj = new UISearchInventorySummaryDetailReport()
            {
                //ProductName = txtProductName.Text.Trim(),
                WarehouseID = WarehouseID,
                ProductID = ProductID,
                ProductSpecificationID = ProductSpecificationID,
                BatchNumber = BatchNumber,
                LicenseNumber = LicenseNumber,
                ExpirationDate = ExpirationDate,
                BeginDate = BeginDate,
                EndDate = EndDate
            };

            if (rdpBeginDate.SelectedDate.HasValue)
            {
                uiSearchObj.BeginDate = rdpBeginDate.SelectedDate;
            }
            if (rdpEndDate.SelectedDate.HasValue)
            {
                uiSearchObj.EndDate = rdpEndDate.SelectedDate;
            }
            int totalRecords = 0;

            var uiInventorySummaryDetailReports = PageReportRepository.GetInventorySummaryDetailReport(uiSearchObj, rgInventorySummaryDetailReports.CurrentPageIndex, rgInventorySummaryDetailReports.PageSize, out totalRecords);

            rgInventorySummaryDetailReports.VirtualItemCount = totalRecords;

            rgInventorySummaryDetailReports.DataSource = uiInventorySummaryDetailReports;

            if (isNeedRebind)
                rgInventorySummaryDetailReports.Rebind();
        }


        protected void rgInventorySummaryDetailReports_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindInventorySummaryDetailReport(false);
        }

        protected void rgInventorySummaryDetailReports_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();


            rgInventorySummaryDetailReports.Rebind();
        }

        protected void rgInventorySummaryDetailReports_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgInventorySummaryDetailReports_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgInventorySummaryDetailReports_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindInventorySummaryDetailReport(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

            rdpBeginDate.SelectedDate = rdpEndDate.SelectedDate = null;

            BindInventorySummaryDetailReport(true);
        }



        protected void btnExport_Click(object sender, EventArgs e)
        {
            /*
             微软为Response对象提供了一个新的方法TransmitFile来解决使用Response.BinaryWrite
            下载超过400mb的文件时导致Aspnet_wp.exe进程回收而无法成功下载的问题。
            代码如下：
             */
            UISearchInventorySummaryDetailReport uiSearchObj = new UISearchInventorySummaryDetailReport()
            {
                //ProductName = txtProductName.Text.Trim(),
                WarehouseID = WarehouseID,
                ProductID = ProductID,
                ProductSpecificationID = ProductSpecificationID,
                BatchNumber = BatchNumber,
                LicenseNumber = LicenseNumber,
                ExpirationDate = ExpirationDate,
                BeginDate = BeginDate,
                EndDate = EndDate
            };

            if (rdpBeginDate.SelectedDate.HasValue)
            {
                uiSearchObj.BeginDate = rdpBeginDate.SelectedDate;
            }
            if (rdpEndDate.SelectedDate.HasValue)
            {
                uiSearchObj.EndDate = rdpEndDate.SelectedDate;
            }

            var uiReports = PageReportRepository.GetInventorySummaryDetailReport(uiSearchObj);

            var excelPath = Server.MapPath("~/App_Data/") + "TempExcel.xls";

            //"rowspan": 2, 
            //"sheetname": "按商机类型分类", 
            //"defaultwidth": 12, 
            //"defaultheight": 35, 
            NPOIHelper nPOIHelper = new Common.NPOIHelper.Excel.NPOIHelper();
            UIInventorySummaryDetailReport model = new UIInventorySummaryDetailReport();

            List<ExcelHeader> excelHeaders = new List<ExcelHeader>() { 
               
                new ExcelHeader(model.GetName(() => model.EntryOrOutDate),"出入库日期"),
                new ExcelHeader(model.GetName(() => model.Type),"类型"),
                new ExcelHeader(model.GetName(() => model.OrderCode),"订单号"),
                new ExcelHeader(model.GetName(() => model.StockInOrOutCode),"出入库单号"),
                new ExcelHeader(model.GetName(() => model.ProductCode),"货品编号"),
                new ExcelHeader(model.GetName(() => model.ProductName),"货品名称"),
                new ExcelHeader(model.GetName(() => model.Specification),"规格"),
                new ExcelHeader(model.GetName(() => model.UnitName),"基本单位"),
                new ExcelHeader(model.GetName(() => model.BatchNumber),"批号"),
                new ExcelHeader(model.GetName(() => model.ExpirationDate),"有效期"),
                new ExcelHeader(model.GetName(() => model.Price),"单价"),

                new ExcelHeader(model.GetName(() => model.InQty),"基本数量"),
                new ExcelHeader(model.GetName(() => model.InNumberOfPackages),"件数"),

                new ExcelHeader(model.GetName(() => model.OutQty),"基本数量"),
                new ExcelHeader(model.GetName(() => model.OutNumberOfPackages),"件数"),
             
            };
            Queue<ExcelHeader> excelHeadersQueue = new Queue<ExcelHeader>(excelHeaders);
            Root excelRoot = new Root()
            {
                root = new HeadInfo()
                {
                    rowspan = 2,
                    sheetname = "库存汇总表详情",
                    defaultheight = null,
                    defaultwidth = 20,
                    head = new List<AttributeList>(){
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,0,0"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,1,1"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,2,2"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,3,3"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,4,4"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,5,5"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,6,6"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,7,7"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,8,8"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,9,9"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,10,10"},
         
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,11,11"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,12,12"},
         
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,13,13"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="1,1,14,14"},
         

                    new AttributeList(){ title="本期收入", cellregion="0,0,11,12"},
                    new AttributeList(){ title="本期发出", cellregion="0,0,13,14"},

                    }
                }
            };

            List<Func<UIInventorySummaryDetailReport, string>> fieldFuncs = new List<Func<UIInventorySummaryDetailReport, string>>();



            fieldFuncs.Add(x => x.EntryOrOutDate.ToString("yyyy/MM/dd"));
            fieldFuncs.Add(x => x.Type);
            fieldFuncs.Add(x => x.OrderCode);
            fieldFuncs.Add(x => x.StockInOrOutCode);
            fieldFuncs.Add(x => x.ProductCode);
            fieldFuncs.Add(x => x.ProductName);
            fieldFuncs.Add(x => x.Specification);
            fieldFuncs.Add(x => x.UnitName);
            fieldFuncs.Add(x => x.BatchNumber);
            fieldFuncs.Add(x => x.ExpirationDate.ToString("yyyy/MM/dd"));
            fieldFuncs.Add(x => x.Price.ToString("f2"));
            fieldFuncs.Add(x => x.InQty.ToString());
            fieldFuncs.Add(x => x.InNumberOfPackages.ToString("f2"));
            fieldFuncs.Add(x => x.OutQty.ToString());
            fieldFuncs.Add(x => x.OutNumberOfPackages.ToString("f2"));



            nPOIHelper.ExportToExcel<UIInventorySummaryDetailReport>(
                (List<UIInventorySummaryDetailReport>)uiReports,
                excelPath,
                excelHeaders.Select(x => x.Key).ToArray(),
                excelRoot,
                fieldFuncs.ToArray());


            Response.ContentType = "application/x-zip-compressed";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + "库存汇总表详情".UrlEncode() + ".xls");
            string filename = excelPath;
            Response.TransmitFile(filename);
        }




    }
}