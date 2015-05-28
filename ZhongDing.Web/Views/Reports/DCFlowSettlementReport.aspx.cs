using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories.Reports;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Common.Extension;
using ZhongDing.Common.NPOIHelper.Excel;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Reports
{
    public partial class DCFlowSettlementReport : BasePage
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.DCFlowSettlementReport;

            if (!IsPostBack)
            {
                base.PermissionOptionCheckButtonExport(btnExport);
            }
        }

        #region Private Methods

        private void BindEntities(bool isNeedRebind)
        {
            IList<UIDCFlowSettlementReport> uiEntities = new List<UIDCFlowSettlementReport>();

            int totalRecords = 0;

            var uiSearchObj = new UISearchDCFlowSettlementReport();

            if (rmypSaleDate.SelectedDate.HasValue)
            {
                var tempDate = rmypSaleDate.SelectedDate.Value;

                uiSearchObj.SaleDate = new DateTime(tempDate.Year, tempDate.Month, 1);
            }

            uiEntities = PageReportRepository.GetDCFlowSettlementReport(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.DataSource = uiEntities;
            rgEntities.VirtualItemCount = totalRecords;

            if (isNeedRebind)
                rgEntities.Rebind();

        }


        #endregion

        protected void rgEntities_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindEntities(false);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindEntities(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rmypSaleDate.Clear();

            BindEntities(true);
        }

        protected void btnExportHidden_Click(object sender, EventArgs e)
        {
            var uiSearchObj = new UISearchDCFlowSettlementReport();

            if (rmypSaleDate.SelectedDate.HasValue)
            {
                var tempDate = rmypSaleDate.SelectedDate.Value;

                uiSearchObj.SaleDate = new DateTime(tempDate.Year, tempDate.Month, 1);
            }

            var uiEntities = PageReportRepository.GetDCFlowSettlementReport(uiSearchObj);

            var tempFilePath = Server.MapPath("~/App_Data/");

            if (!System.IO.Directory.Exists(tempFilePath))
                System.IO.Directory.CreateDirectory(tempFilePath);

            var excelPath = tempFilePath + "TempDCFlowSettlementReportExcel.xls";

            NPOIHelper nPOIHelper = new Common.NPOIHelper.Excel.NPOIHelper();
            UIDCFlowSettlementReport model = new UIDCFlowSettlementReport();

            List<ExcelHeader> excelHeaders = new List<ExcelHeader>() { 
                    new ExcelHeader(model.GetName(() => model.ClientUserName),"客户"),
                    new ExcelHeader(model.GetName(() => model.HospitalTypeName),"医院性质"),
                    new ExcelHeader(model.GetName(() => model.ProductName),"货品名称"),
                    new ExcelHeader(model.GetName(() => model.Specification),"规格"),
                    new ExcelHeader(model.GetName(() => model.HospitalName),"医院"),
                    new ExcelHeader(model.GetName(() => model.SaleQty),"销售数量"),
                    new ExcelHeader(model.GetName(() => model.SaleDate),"销售日期"),
                    new ExcelHeader(model.GetName(() => model.DistributionCompanyName),"配送公司"),
                    new ExcelHeader(model.GetName(() => model.TotalPromotionExpense),"推广费"),
                };

            Queue<ExcelHeader> excelHeadersQueue = new Queue<ExcelHeader>(excelHeaders);
            Root excelRoot = new Root()
            {
                root = new HeadInfo()
                {
                    rowspan = 2,
                    sheetname = "配送公司流向结算表",
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
                    }
                }
            };

            var fieldFuncs = new List<Func<UIDCFlowSettlementReport, string>>();

            fieldFuncs.Add(x => x.ClientUserName);
            fieldFuncs.Add(x => x.HospitalTypeName);
            fieldFuncs.Add(x => x.ProductName);
            fieldFuncs.Add(x => x.Specification);
            fieldFuncs.Add(x => x.HospitalName);
            fieldFuncs.Add(x => x.SaleQty.ToString("N0"));
            fieldFuncs.Add(x => x.SaleDate.ToString("yyyy/MM/dd"));
            fieldFuncs.Add(x => x.DistributionCompanyName);
            fieldFuncs.Add(x => x.TotalPromotionExpense.ToString("C2"));


            nPOIHelper.ExportToExcel<UIDCFlowSettlementReport>(
                (List<UIDCFlowSettlementReport>)uiEntities,
                excelPath,
                excelHeaders.Select(x => x.Key).ToArray(),
                excelRoot,
                fieldFuncs.ToArray());
            Response.ContentType = "application/x-zip-compressed";

            if (rmypSaleDate.SelectedDate.HasValue)
            {
                var selectedDate = rmypSaleDate.SelectedDate.Value;
                Response.AddHeader("Content-Disposition", "attachment;filename=" + ("配送公司流向结算表_(").UrlEncode()
                           + (selectedDate.Year + "年" + selectedDate.Month + "月").UrlEncode() + ").xls");
            }
            else
            {
                Response.AddHeader("Content-Disposition", "attachment;filename=" + ("配送公司流向结算表").UrlEncode() + ".xls");
            }

            string filename = excelPath;
            Response.TransmitFile(filename);
        }

        protected override EPermission PagePermissionID()
        {
            return EPermission.DCFlowSettlementReport;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.View;
        }

    }
}