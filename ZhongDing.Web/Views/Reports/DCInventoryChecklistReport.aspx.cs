using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Common.Extension;
using ZhongDing.Common.NPOIHelper.Excel;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Reports
{
    public partial class DCInventoryChecklistReport : BasePage
    {
        #region Members

        private IDCInventoryDataRepository _PageDCInventoryDataRepository;
        private IDCInventoryDataRepository PageDCInventoryDataRepository
        {
            get
            {
                if (_PageDCInventoryDataRepository == null)
                    _PageDCInventoryDataRepository = new DCInventoryDataRepository();

                return _PageDCInventoryDataRepository;
            }
        }

        private IDistributionCompanyRepository _PageDistributionCompanyRepository;
        private IDistributionCompanyRepository PageDistributionCompanyRepository
        {
            get
            {
                if (_PageDistributionCompanyRepository == null)
                    _PageDistributionCompanyRepository = new DistributionCompanyRepository();

                return _PageDistributionCompanyRepository;
            }
        }

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            this.Master.MenuItemID = (int)EMenuItem.DCInventoryChecklistReport;

            if (!IsPostBack)
            {
                BindDistributionCompanies();
                base.PermissionOptionCheckButtonExport(btnExport);
            }
        }

        #region Private Methods

        private void BindDistributionCompanies()
        {
            var distributionCompanies = PageDistributionCompanyRepository.GetDropdownItems();

            rcbxDistributionCompany.DataSource = distributionCompanies;
            rcbxDistributionCompany.DataTextField = GlobalConst.DEFAULT_DROPDOWN_DATATEXTFIELD;
            rcbxDistributionCompany.DataValueField = GlobalConst.DEFAULT_DROPDOWN_DATAVALUEFIELD;
            rcbxDistributionCompany.DataBind();

            rcbxDistributionCompany.Items.Insert(0, new RadComboBoxItem("", ""));
        }

        private void BindEntities(bool isNeedRebind)
        {
            var uiSearchObj = new UISearchDCInventoryData();

            if (rdpSettlementDate.SelectedDate.HasValue)
                uiSearchObj.SettlementDate = rdpSettlementDate.SelectedDate;
            else
                uiSearchObj.SettlementDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;

            if (!string.IsNullOrEmpty(rcbxDistributionCompany.SelectedValue))
            {
                int distributionCompanyID;
                if (int.TryParse(rcbxDistributionCompany.SelectedValue, out distributionCompanyID))
                    uiSearchObj.DistributionCompanyID = distributionCompanyID;
            }

            int totalRecords;

            var entities = PageDCInventoryDataRepository.GetUIList(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);

            rgEntities.VirtualItemCount = totalRecords;

            rgEntities.DataSource = entities;

            if (isNeedRebind)
                rgEntities.Rebind();

        }

        #endregion

        protected void rgEntities_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            BindEntities(false);
        }

        protected void rgEntities_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item.ItemType == GridItemType.Item
                || e.Item.ItemType == GridItemType.AlternatingItem)
            {
                GridDataItem gridDataItem = e.Item as GridDataItem;
                var uiEntity = (UIDCInventoryData)gridDataItem.DataItem;

                if (uiEntity != null)
                {
                    if (uiEntity.BookBalanceQty != uiEntity.DCBalanceQty)
                    {
                        var bookBalanceQtyColumn = rgEntities.MasterTableView.GetColumn("BookBalanceQty");

                        if (bookBalanceQtyColumn != null)
                        {
                            var bookBalanceQtyCell = gridDataItem.Cells[bookBalanceQtyColumn.OrderIndex];

                            if (bookBalanceQtyCell != null)
                                bookBalanceQtyCell.Attributes.CssStyle.Add("color", "red");
                        }

                        var dcBalanceQtyColumn = rgEntities.MasterTableView.GetColumn("DCBalanceQty");

                        if (dcBalanceQtyColumn != null)
                        {
                            var dcBalanceQtyCell = gridDataItem.Cells[dcBalanceQtyColumn.OrderIndex];

                            if (dcBalanceQtyCell != null)
                                dcBalanceQtyCell.Attributes.CssStyle.Add("color", "red");
                        }
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindEntities(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rdpSettlementDate.Clear();
            rcbxDistributionCompany.ClearSelection();

            BindEntities(true);
        }

        protected void btnExportHidden_Click(object sender, EventArgs e)
        {
            if (rdpSettlementDate.SelectedDate.HasValue
                && !string.IsNullOrEmpty(rcbxDistributionCompany.SelectedValue))
            {
                var uiSearchObj = new UISearchDCInventoryData();

                if (rdpSettlementDate.SelectedDate.HasValue)
                    uiSearchObj.SettlementDate = rdpSettlementDate.SelectedDate;
                else
                    uiSearchObj.SettlementDate = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;

                if (!string.IsNullOrEmpty(rcbxDistributionCompany.SelectedValue))
                {
                    int distributionCompanyID;
                    if (int.TryParse(rcbxDistributionCompany.SelectedValue, out distributionCompanyID))
                        uiSearchObj.DistributionCompanyID = distributionCompanyID;
                }

                var uiEntities = PageDCInventoryDataRepository.GetUIList(uiSearchObj);

                var excelPath = Server.MapPath("~/App_Data/") + "TempDCInventoryChecklistReportExcel.xls";

                NPOIHelper nPOIHelper = new Common.NPOIHelper.Excel.NPOIHelper();
                UIDCInventoryData model = new UIDCInventoryData();

                List<ExcelHeader> excelHeaders = new List<ExcelHeader>() { 
                new ExcelHeader(model.GetName(() => model.DistributionCompanyName),"配送公司"), 
                new ExcelHeader(model.GetName(() => model.ProductName),"货品名称"),
                new ExcelHeader(model.GetName(() => model.Specification),"规格"),
                new ExcelHeader(model.GetName(() => model.BookBalanceQty),"公司账面库存"),
                new ExcelHeader(model.GetName(() => model.DCBalanceQty),"配送公司库存")
            };

                Queue<ExcelHeader> excelHeadersQueue = new Queue<ExcelHeader>(excelHeaders);
                Root excelRoot = new Root()
                {
                    root = new HeadInfo()
                    {
                        rowspan = 2,
                        sheetname = "配送公司库存核对表",
                        defaultheight = null,
                        defaultwidth = 20,
                        head = new List<AttributeList>(){
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,0,0"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,1,1"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,2,2"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,3,3"},
                    new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,4,4"}
                    }
                    }
                };

                List<Func<UIDCInventoryData, string>> fieldFuncs = new List<Func<UIDCInventoryData, string>>();

                fieldFuncs.Add(x => x.DistributionCompanyName);
                fieldFuncs.Add(x => x.ProductName);
                fieldFuncs.Add(x => x.Specification);
                fieldFuncs.Add(x => x.BookBalanceQty.HasValue ? x.BookBalanceQty.ToString() : "");
                fieldFuncs.Add(x => x.DCBalanceQty.HasValue ? x.DCBalanceQty.ToString() : "");

                nPOIHelper.ExportToExcel<UIDCInventoryData>(
                    (List<UIDCInventoryData>)uiEntities,
                    excelPath,
                    excelHeaders.Select(x => x.Key).ToArray(),
                    excelRoot,
                    fieldFuncs.ToArray());
                Response.ContentType = "application/x-zip-compressed";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + ("大包客户提成结算表_(库存日期").UrlEncode()
                    + "_" + uiSearchObj.SettlementDate.ToString("yyyy年MM月dd日").UrlEncode() + ").xls");

                string filename = excelPath;
                Response.TransmitFile(filename);
            }
            else
            {

                this.Master.BaseNotification.ContentIcon = GlobalConst.NotificationSettings.CONTENT_ICON_ERROR;
                this.Master.BaseNotification.AutoCloseDelay = 1000;
                this.Master.BaseNotification.Show("请先选择查询条件，再导出数据");
            }

        }

        protected override EPermission PagePermissionID()
        {
            return EPermission.DCInventoryChecklistReport;
        }

        protected override EPermissionOption PageAccessEPermissionOption()
        {
            return EPermissionOption.View;
        }

    }
}