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
    public partial class CashFlowReportDetail : BasePage
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

            this.Master.MenuItemID = (int)EMenuItem.CashFlowReportManage;
            if (!IsPostBack)
            {

            }

        }
        private void BindCashFlowReport(bool isNeedRebind)
        {
            UISearchCashFlowReportDetail uiSearchObj = new UISearchCashFlowReportDetail()
            {
                //BeginDate = rdpBeginDate.SelectedDate.ToFirstDayOfMonthDateOrNull(),
                //EndDate = rdpEndDate.SelectedDate.ToFirstDayOfMonthDateOrNull(),
                CashFlowHistoryID = CurrentEntityID
            };
            //int totalRecords = 0;
            var uiCashFlowReports = PageReportRepository.GetCashFlowReportDetail(uiSearchObj);
            if (uiCashFlowReports != null && uiCashFlowReports.Count > 0)
            {
                for (int i = 0; i < uiCashFlowReports.Count; i++)
                {
                    if (i != 0)
                    {
                        if (uiCashFlowReports[i].Month1.IsNotNullOrEmpty()) uiCashFlowReports[i].Month1 = "￥" + uiCashFlowReports[i].Month1.ToDecimal().ToString("f2");
                        if (uiCashFlowReports[i].Month2.IsNotNullOrEmpty()) uiCashFlowReports[i].Month2 = "￥" + uiCashFlowReports[i].Month2.ToDecimal().ToString("f2");
                        if (uiCashFlowReports[i].Month3.IsNotNullOrEmpty()) uiCashFlowReports[i].Month3 = "￥" + uiCashFlowReports[i].Month3.ToDecimal().ToString("f2");
                        if (uiCashFlowReports[i].Month4.IsNotNullOrEmpty()) uiCashFlowReports[i].Month4 = "￥" + uiCashFlowReports[i].Month4.ToDecimal().ToString("f2");
                        if (uiCashFlowReports[i].Month5.IsNotNullOrEmpty()) uiCashFlowReports[i].Month5 = "￥" + uiCashFlowReports[i].Month5.ToDecimal().ToString("f2");
                        if (uiCashFlowReports[i].Month6.IsNotNullOrEmpty()) uiCashFlowReports[i].Month6 = "￥" + uiCashFlowReports[i].Month6.ToDecimal().ToString("f2");
                        if (uiCashFlowReports[i].Month7.IsNotNullOrEmpty()) uiCashFlowReports[i].Month7 = "￥" + uiCashFlowReports[i].Month7.ToDecimal().ToString("f2");
                        if (uiCashFlowReports[i].Month8.IsNotNullOrEmpty()) uiCashFlowReports[i].Month8 = "￥" + uiCashFlowReports[i].Month8.ToDecimal().ToString("f2");
                        if (uiCashFlowReports[i].Month9.IsNotNullOrEmpty()) uiCashFlowReports[i].Month9 = "￥" + uiCashFlowReports[i].Month9.ToDecimal().ToString("f2");
                        if (uiCashFlowReports[i].Month10.IsNotNullOrEmpty()) uiCashFlowReports[i].Month10 = "￥" + uiCashFlowReports[i].Month10.ToDecimal().ToString("f2");
                        if (uiCashFlowReports[i].Month11.IsNotNullOrEmpty()) uiCashFlowReports[i].Month11 = "￥" + uiCashFlowReports[i].Month11.ToDecimal().ToString("f2");
                        if (uiCashFlowReports[i].Month12.IsNotNullOrEmpty()) uiCashFlowReports[i].Month12 = "￥" + uiCashFlowReports[i].Month12.ToDecimal().ToString("f2");
                    }
                }
                var item = uiCashFlowReports[0];
                //<telerik:GridBoundColumn UniqueName="FirstColName" HeaderText="报表名称" DataField="FirstColName">
                //            </telerik:GridBoundColumn>
                //if (item.FirstColName.IsNotNullOrEmpty())
                //{

                //}
                rgCashFlowReports.Height = uiCashFlowReports.Count * 30 + 100;
            }
            //rgCashFlowReports.VirtualItemCount = totalRecords;
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
            //rdpBeginDate.SelectedDate = rdpEndDate.SelectedDate = null;

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
    }
}