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
    public partial class ProcureOrderApplicationPaymentReportManagement : BasePage
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

            this.Master.MenuItemID = (int)EMenuItem.ProcureOrderPaymentReportManage;
            if (!IsPostBack)
            {
                BindSuppliers();

            }

        }
        private void BindProcureOrderApplicationPaymentReport(bool isNeedRebind)
        {
            UISearchProcureOrderApplicationPaymentReport uiSearchObj = new UISearchProcureOrderApplicationPaymentReport()
            {
                BeginDate = rdpBeginDate.SelectedDate.HasValue ? rdpBeginDate.SelectedDate : GlobalConst.DATETIME_NULL_VALUE,
                EndDate = rdpEndDate.SelectedDate.HasValue ? rdpEndDate.SelectedDate.Value.AddDays(1) : GlobalConst.DATETIME_NULL_VALUE,
                SupplierId = rcbxSupplier.SelectedValue.ToIntOrNull(),


            };

            int totalRecords = 0;

            var uiProcureOrderApplicationPaymentReports = PageReportRepository.GetProcureOrderApplicationPaymentReport(uiSearchObj, rgProcureOrderApplicationPaymentReports.CurrentPageIndex, rgProcureOrderApplicationPaymentReports.PageSize, out totalRecords);

            rgProcureOrderApplicationPaymentReports.VirtualItemCount = totalRecords;

            rgProcureOrderApplicationPaymentReports.DataSource = uiProcureOrderApplicationPaymentReports;

            if (isNeedRebind)
                rgProcureOrderApplicationPaymentReports.Rebind();
        }


        protected void rgProcureOrderApplicationPaymentReports_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindProcureOrderApplicationPaymentReport(false);
        }

        protected void rgProcureOrderApplicationPaymentReports_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();


            rgProcureOrderApplicationPaymentReports.Rebind();
        }

        protected void rgProcureOrderApplicationPaymentReports_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgProcureOrderApplicationPaymentReports_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgProcureOrderApplicationPaymentReports_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindProcureOrderApplicationPaymentReport(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpBeginDate.SelectedDate = rdpEndDate.SelectedDate = null;
            rcbxSupplier.SelectedValue = string.Empty;
            rcbxSupplier.Text = string.Empty;


            BindProcureOrderApplicationPaymentReport(true);
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


                }
            }
        }


        protected void btnExport_Click(object sender, EventArgs e)
        {
            /*
             微软为Response对象提供了一个新的方法TransmitFile来解决使用Response.BinaryWrite
            下载超过400mb的文件时导致Aspnet_wp.exe进程回收而无法成功下载的问题。
            代码如下：
             */

            UISearchProcureOrderApplicationPaymentReport uiSearchObj = new UISearchProcureOrderApplicationPaymentReport()
            {
                BeginDate = rdpBeginDate.SelectedDate.HasValue ? rdpBeginDate.SelectedDate : GlobalConst.DATETIME_NULL_VALUE,
                EndDate = rdpEndDate.SelectedDate.HasValue ? rdpEndDate.SelectedDate.Value.AddDays(1) : GlobalConst.DATETIME_NULL_VALUE,
                SupplierId = rcbxSupplier.SelectedValue.ToIntOrNull(),


            };

            var uiProcureOrderApplicationPaymentReports = PageReportRepository.GetProcureOrderApplicationPaymentReport(uiSearchObj);
            var excelPath = Server.MapPath("~/App_Data/") + "TempExcel.xls";
            ExcelHelper.RenderToExcel<UIProcureOrderApplicationPaymentReport>(uiProcureOrderApplicationPaymentReports,
                new List<ExcelHeader>() {
                    new ExcelHeader() { Key = "PayDate", Name = "付款日期" },
                    new ExcelHeader(){ Key="OrderCode", Name="订单号"},
                    new ExcelHeader(){ Key="Amount", Name="金额"},
                    new ExcelHeader(){ Key="FromAccount", Name="支出账号"},
                    new ExcelHeader(){ Key="ToAccount", Name="收款户名"},
               
                }, excelPath);

            Response.ContentType = "application/x-zip-compressed";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + "采购付款明细表".UrlEncode() + ".xls");
            string filename = excelPath;
            Response.TransmitFile(filename);
        }




    }
}