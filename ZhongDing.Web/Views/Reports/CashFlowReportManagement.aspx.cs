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
    public partial class CashFlowReportManagement : BasePage
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

        private ICashFlowHistoryRepository _PageCashFlowHistoryRepository;
        private ICashFlowHistoryRepository PageCashFlowHistoryRepository
        {
            get
            {
                if (_PageCashFlowHistoryRepository == null)
                    _PageCashFlowHistoryRepository = new CashFlowHistoryRepository();

                return _PageCashFlowHistoryRepository;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            this.Master.MenuItemID = (int)EMenuItem.CashFlowReportManage;
            if (!IsPostBack)
            {

            }

        }
        private void BindCashFlowReport(bool isNeedRebind)
        {
            UISearchCashFlowReport uiSearchObj = new UISearchCashFlowReport()
            {
                BeginDate = rdpBeginDate.SelectedDate.ToFirstDayOfMonthDateOrNull(),
                EndDate = rdpEndDate.SelectedDate.ToFirstDayOfMonthDateOrNull(),
            };

            int totalRecords = 0;

            var uiCashFlowReports = PageReportRepository.GetCashFlowReport(uiSearchObj, rgCashFlowReports.CurrentPageIndex, rgCashFlowReports.PageSize, out totalRecords);

            rgCashFlowReports.VirtualItemCount = totalRecords;

            rgCashFlowReports.DataSource = uiCashFlowReports;

            if (isNeedRebind)
                rgCashFlowReports.Rebind();
        }


        protected void rgCashFlowReports_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindCashFlowReport(false);
        }

        protected void rgCashFlowReports_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();


            rgCashFlowReports.Rebind();
        }

        protected void rgCashFlowReports_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgCashFlowReports_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgCashFlowReports_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindCashFlowReport(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpBeginDate.SelectedDate = rdpEndDate.SelectedDate = null;

            BindCashFlowReport(true);
        }



        protected void rcbxSupplier_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Value))
            {
                var supplierID = e.Value.ToIntOrNull();
                if (supplierID.BiggerThanZero())
                {


                }
            }
        }





        protected override EPermission PagePermissionID()
        {
            return EPermission.CashFlowReportManagement;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.View;
        }

        protected void rgCashFlowReports_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "Download")
            {
                GridEditableItem editableItem = e.Item as GridEditableItem;
                var id = editableItem.GetDataKeyValue("ID").ToIntOrNull();

                var cashFlowHistoryRepository = PageCashFlowHistoryRepository.GetByID(id);
                if (cashFlowHistoryRepository != null)
                {
                    string excelPath = Server.MapPath(cashFlowHistoryRepository.FilePath);

                    Response.ContentType = "application/x-zip-compressed";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + "现金流量报表".UrlEncode() + ".xls");
                    string filename = excelPath;
                    Response.TransmitFile(filename);
                }

            }
        }
    }
}