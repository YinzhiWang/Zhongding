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
    public partial class ClientSaleAppReportManagement : BasePage
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


        private IClientCompanyRepository _PageClientCompanyRepository;
        private IClientCompanyRepository PageClientCompanyRepository
        {
            get
            {
                if (_PageClientCompanyRepository == null)
                    _PageClientCompanyRepository = new ClientCompanyRepository();

                return _PageClientCompanyRepository;
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

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

            this.Master.MenuItemID = (int)EMenuItem.ClientSaleAppReportManage;
            if (!IsPostBack)
            {
                BindClientUsers();
                BindProducts();
                BindClientCompanies();
            }

        }
        private void BindClientUsers()
        {
            var clientUsers = PageClientUserRepository.GetDropdownItems();
            rcbxClientUser.DataSource = clientUsers;
            rcbxClientUser.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientUser.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientUser.DataBind();

            rcbxClientUser.Items.Insert(0, new RadComboBoxItem("", ""));
        }
        private void BindClientCompanies()
        {
            rcbxClientCompany.ClearSelection();
            rcbxClientCompany.Items.Clear();

            var uiSearchObj = new UISearchDropdownItem();

            if (!string.IsNullOrEmpty(rcbxClientUser.SelectedValue))
            {
                int clientUserID;
                if (int.TryParse(rcbxClientUser.SelectedValue, out clientUserID))
                    uiSearchObj.Extension = new UISearchExtension { ClientUserID = clientUserID };
            }

            var clientCompanies = PageClientCompanyRepository.GetDropdownItems(uiSearchObj);
            rcbxClientCompany.DataSource = clientCompanies;
            rcbxClientCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxClientCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxClientCompany.DataBind();

            rcbxClientCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }
        protected void rcbxClientUser_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindClientCompanies();
        }
        private void BindClientSaleAppReport(bool isNeedRebind)
        {
            UISearchClientSaleAppReport uiSearchObj = new UISearchClientSaleAppReport()
            {
                BeginDate = rdpBeginDate.SelectedDate.HasValue ? rdpBeginDate.SelectedDate : GlobalConst.DATETIME_NULL_VALUE,
                EndDate = rdpEndDate.SelectedDate.HasValue ? rdpEndDate.SelectedDate.Value.AddDays(1) : GlobalConst.DATETIME_NULL_VALUE,
                ClientUserId = rcbxClientUser.SelectedValue.ToIntOrNull(),
                ClientCompanyId = rcbxClientCompany.SelectedValue.ToIntOrNull(),
                ProductId = rcbxProduct.SelectedValue.ToIntOrNull()

            };

            int totalRecords = 0;

            var uiClientSaleAppReports = PageReportRepository.GetClientSaleAppReport(uiSearchObj, rgClientSaleAppReports.CurrentPageIndex, rgClientSaleAppReports.PageSize, out totalRecords);

            rgClientSaleAppReports.VirtualItemCount = totalRecords;

            rgClientSaleAppReports.DataSource = uiClientSaleAppReports;

            if (isNeedRebind)
                rgClientSaleAppReports.Rebind();
        }


        protected void rgClientSaleAppReports_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindClientSaleAppReport(false);
        }

        protected void rgClientSaleAppReports_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            GridEditableItem editableItem = e.Item as GridEditableItem;

            String sid = editableItem.GetDataKeyValue("ID").ToString();


            rgClientSaleAppReports.Rebind();
        }

        protected void rgClientSaleAppReports_ItemCreated(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void rgClientSaleAppReports_ColumnCreated(object sender, Telerik.Web.UI.GridColumnCreatedEventArgs e)
        {

        }

        protected void rgClientSaleAppReports_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindClientSaleAppReport(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {

            rdpBeginDate.SelectedDate = rdpEndDate.SelectedDate = null;
            rcbxClientCompany.SelectedValue = rcbxClientUser.SelectedValue = rcbxProduct.SelectedValue = string.Empty;
            rcbxClientCompany.Text = rcbxClientUser.Text = rcbxProduct.Text = string.Empty;

            BindClientSaleAppReport(true);
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
            UISearchClientSaleAppReport uiSearchObj = new UISearchClientSaleAppReport()
            {
                BeginDate = rdpBeginDate.SelectedDate.HasValue ? rdpBeginDate.SelectedDate : GlobalConst.DATETIME_NULL_VALUE,
                EndDate = rdpEndDate.SelectedDate.HasValue ? rdpEndDate.SelectedDate.Value.AddDays(1) : GlobalConst.DATETIME_NULL_VALUE,
                ClientUserId = rcbxClientUser.SelectedValue.ToIntOrNull(),
                ClientCompanyId = rcbxClientCompany.SelectedValue.ToIntOrNull(),
                ProductId = rcbxProduct.SelectedValue.ToIntOrNull()

            };

            var uiClientSaleAppReports = PageReportRepository.GetClientSaleAppReport(uiSearchObj);
            var excelPath = Server.MapPath("~/App_Data/") + "TempExcel.xls";
            //ExcelHelper.RenderToExcel<UIClientSaleAppReport>(uiClientSaleAppReports,
            //    new List<ExcelHeader>() {
            //        new ExcelHeader() { Key = "OrderDate", Name = "订单日期" },
            //        new ExcelHeader(){ Key="OrderCode", Name="订单号"},
            //        new ExcelHeader(){ Key="ClientName", Name="客户"},
            //        new ExcelHeader(){ Key="ClientCompanyName", Name="商业单位"},
            //        new ExcelHeader(){ Key="CategoryName", Name="货品类别"},
            //        new ExcelHeader(){ Key="ProductCode", Name="货品编号"},
            //        new ExcelHeader(){ Key="ProductName", Name="货品名称"},
            //        new ExcelHeader(){ Key="Specification", Name="规格"},
            //        new ExcelHeader(){ Key="UnitName", Name="基本单位"},
            //        new ExcelHeader(){ Key="SalesPrice", Name="数量"},
            //        new ExcelHeader(){ Key="Count", Name="销售单价"},
            //        new ExcelHeader(){ Key="TotalSalesAmount", Name="金额"},
            //    }, excelPath);
            NPOIHelper nPOIHelper = new Common.NPOIHelper.Excel.NPOIHelper();
            UIClientSaleAppReport model = new UIClientSaleAppReport();

            List<ExcelHeader> excelHeaders = new List<ExcelHeader>() { 
                new ExcelHeader(model.GetName(() => model.OrderDate),"订单日期"), 
                new ExcelHeader(model.GetName(() => model.OrderCode),"订单号"),
                new ExcelHeader(model.GetName(() => model.ClientName),"客户"),
                new ExcelHeader(model.GetName(() => model.ClientCompanyName),"商业单位"),
                new ExcelHeader(model.GetName(() => model.CategoryName),"货品类别"),
                new ExcelHeader(model.GetName(() => model.ProductCode),"货品编号"),
                new ExcelHeader(model.GetName(() => model.ProductName),"货品名称"),
                new ExcelHeader(model.GetName(() => model.Specification),"规格"),
                new ExcelHeader(model.GetName(() => model.UnitName),"基本单位"),
                new ExcelHeader(model.GetName(() => model.SalesPrice),"销售单价"),
                new ExcelHeader(model.GetName(() => model.Count),"数量"),
                new ExcelHeader(model.GetName(() => model.TotalSalesAmount),"金额"),

                new ExcelHeader(model.GetName(() => model.AlreadyOutQty),"基本数量"),
                new ExcelHeader(model.GetName(() => model.AlreadyOutNumberOfPackages),"件数"),
                new ExcelHeader(model.GetName(() => model.AlreadyOutQtySalesPricePrice),"金额"),
                new ExcelHeader(model.GetName(() => model.StopOutQty),"基本数量"),
                new ExcelHeader(model.GetName(() => model.StopOutNumberOfPackages),"件数"),
                new ExcelHeader(model.GetName(() => model.StopOutQtySalesPricePrice),"金额"),
                new ExcelHeader(model.GetName(() => model.NotOutQty),"基本数量"),
                new ExcelHeader(model.GetName(() => model.NotOutNumberOfPackages),"件数"),
                new ExcelHeader(model.GetName(() => model.NotOutQtySalesPricePrice),"金额")
            };
            Queue<ExcelHeader> excelHeadersQueue = new Queue<ExcelHeader>(excelHeaders);
            Root excelRoot = new Root()
            {
                root = new HeadInfo()
                {
                    rowspan = 2,
                    sheetname = "销售订单报表",
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

            List<Func<UIClientSaleAppReport, string>> fieldFuncs = new List<Func<UIClientSaleAppReport, string>>();

            fieldFuncs.Add(x => x.OrderDate.ToString("yyyy/MM/dd"));
            fieldFuncs.Add(x => x.OrderCode);
            fieldFuncs.Add(x => x.ClientName);
            fieldFuncs.Add(x => x.ClientCompanyName);
            fieldFuncs.Add(x => x.CategoryName);
            fieldFuncs.Add(x => x.ProductCode);
            fieldFuncs.Add(x => x.ProductName);
            fieldFuncs.Add(x => x.Specification);
            fieldFuncs.Add(x => x.UnitName);
            fieldFuncs.Add(x => x.SalesPrice.ToString("f2"));
            fieldFuncs.Add(x => x.Count.ToString());
            fieldFuncs.Add(x => x.TotalSalesAmount.ToString("f2"));

            fieldFuncs.Add(x => x.AlreadyOutQty.ToString());
            fieldFuncs.Add(x => x.AlreadyOutNumberOfPackages.ToString("f2"));
            fieldFuncs.Add(x => x.AlreadyOutQtySalesPricePrice.ToString("f2"));

            fieldFuncs.Add(x => x.StopOutQty.ToString());
            fieldFuncs.Add(x => x.StopOutNumberOfPackages.ToString("f2"));
            fieldFuncs.Add(x => x.StopOutQtySalesPricePrice.ToString("f2"));

            fieldFuncs.Add(x => x.NotOutQty.ToString());
            fieldFuncs.Add(x => x.NotOutNumberOfPackages.ToString("f2"));
            fieldFuncs.Add(x => x.NotOutQtySalesPricePrice.ToString("f2"));


            nPOIHelper.ExportToExcel<UIClientSaleAppReport>(
                (List<UIClientSaleAppReport>)uiClientSaleAppReports,
                excelPath,
                excelHeaders.Select(x => x.Key).ToArray(),
                excelRoot,
                fieldFuncs.ToArray());
            Response.ContentType = "application/x-zip-compressed";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + "销售订单报表".UrlEncode() + ".xls");
            string filename = excelPath;
            Response.TransmitFile(filename);
        }




    }
}