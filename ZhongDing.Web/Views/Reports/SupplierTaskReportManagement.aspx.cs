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
    public partial class SupplierTaskReportManagement : BasePage
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
        private IDBClientSettleBonusRepository _PageDBClientSettleBonusRepository;
        private IDBClientSettleBonusRepository PageDBClientSettleBonusRepository
        {
            get
            {
                if (_PageDBClientSettleBonusRepository == null)
                    _PageDBClientSettleBonusRepository = new DBClientSettleBonusRepository();

                return _PageDBClientSettleBonusRepository;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            this.Master.MenuItemID = (int)EMenuItem.SupplierTaskReportManage;
            if (!IsPostBack)
            {

                BindSuppliers();
                rmypSettlementDate.SelectedDate = DateTime.Now;
                base.PermissionOptionCheckButtonExport(btnExport);
            }

        }
        private void BindProcureOrderReport(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchSupplierTaskReport
            {
                Date = rmypSettlementDate.SelectedDate.HasValue ? rmypSettlementDate.SelectedDate.Value.AddDays(-1) : DateTime.Now,
                SupplierID = rcbxSupplier.SelectedValue.ToIntOrNull(),
                CompanyID = CurrentUser.CompanyID
            };


            int totalRecords = 0;
            var uiProcureOrderReports = PageReportRepository.GetSupplierTaskReport(uiSearchObj, rgProcureOrderReports.CurrentPageIndex, rgProcureOrderReports.PageSize, out totalRecords);

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
            rmypSettlementDate.SelectedDate = null;
            rcbxSupplier.SelectedValue = string.Empty;

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
        protected void btnExport_Click(object sender, EventArgs e)
        {
            /*
             微软为Response对象提供了一个新的方法TransmitFile来解决使用Response.BinaryWrite
            下载超过400mb的文件时导致Aspnet_wp.exe进程回收而无法成功下载的问题。
            代码如下：
             */
            var uiSearchObj = new UISearchSupplierTaskReport
            {
                Date = rmypSettlementDate.SelectedDate.HasValue ? rmypSettlementDate.SelectedDate.Value.AddDays(-1) : DateTime.Now,
                SupplierID = rcbxSupplier.SelectedValue.ToIntOrNull(),
                CompanyID = CurrentUser.CompanyID
            };

            var uiReports = PageReportRepository.GetSupplierTaskReport(uiSearchObj);
            var excelPath = Server.MapPath("~/App_Data/") + "TempExcel.xls";


            //"rowspan": 2, 
            //"sheetname": "按商机类型分类", 
            //"defaultwidth": 12, 
            //"defaultheight": 35, 
            NPOIHelper nPOIHelper = new Common.NPOIHelper.Excel.NPOIHelper();
            UISupplierTaskReport model = new UISupplierTaskReport();

            List<ExcelHeader> excelHeaders = new List<ExcelHeader>() { 
                new ExcelHeader(model.GetName(() => model.Date),"月份"), 
                new ExcelHeader(model.GetName(() => model.SupplierName),"供应商"),
                new ExcelHeader(model.GetName(() => model.ProductName),"货品名称"),
                new ExcelHeader(model.GetName(() => model.Specification),"规格"),
                new ExcelHeader(model.GetName(() => model.TaskQuantity),"月任务量"),
                new ExcelHeader(model.GetName(() => model.AlreadyProcureCount),"已采购数量"),
            };
            Queue<ExcelHeader> excelHeadersQueue = new Queue<ExcelHeader>(excelHeaders);
            Root excelRoot = new Root()
            {
                root = new HeadInfo()
                {
                    rowspan = 2,
                    sheetname = "供应商任务统计表",
                    defaultheight = null,
                    defaultwidth = 12,
                    head = new List<AttributeList>(){
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,0,0"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,1,1", width=20},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,2,2", width=20},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,3,3"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,4,4"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,5,5"},

                    }
                }
            };

            List<Func<UISupplierTaskReport, string>> fieldFuncs = new List<Func<UISupplierTaskReport, string>>();

            fieldFuncs.Add(x => x.Date.ToString("yyyy/MM"));
            fieldFuncs.Add(x => x.SupplierName);
            fieldFuncs.Add(x => x.ProductName);
            fieldFuncs.Add(x => x.Specification);
            fieldFuncs.Add(x => x.TaskQuantity.ToString());
            fieldFuncs.Add(x => x.AlreadyProcureCount.ToString());

            nPOIHelper.ExportToExcel<UISupplierTaskReport>(
                (List<UISupplierTaskReport>)uiReports,
                excelPath,
                excelHeaders.Select(x => x.Key).ToArray(),
                excelRoot,
                fieldFuncs.ToArray());


            Response.ContentType = "application/x-zip-compressed";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + "供应商任务统计表".UrlEncode() + ".xls");
            string filename = excelPath;
            Response.TransmitFile(filename);
        }



        protected override EPermission PagePermissionID()
        {
            return EPermission.SupplierTaskReportManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.View;
        }
    }
}