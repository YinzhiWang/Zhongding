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
using ZhongDing.Common.Extension;
using ZhongDing.Common.NPOIHelper.Excel;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Web.Views.Reports
{
    public partial class DBClientQuarterlyAssessmentReport : BasePage
    {
        #region Members

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
            this.Master.MenuItemID = (int)EMenuItem.DBClientQuarterlyAssessmentReport;

            if (!IsPostBack)
            {
                BindYears();

                BindClientUsers();
            }
        }

        #region Private Methods
        private void BindYears()
        {
            int curYear = DateTime.Now.Year;

            for (int i = 0; i < 10; i++)
            {
                string strYear = (curYear - i).ToString();

                rcbxYear.Items.Add(new RadComboBoxItem(strYear, strYear));
            }

            rcbxYear.Items.Insert(0, new RadComboBoxItem("", ""));
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

        private void BindEntities(bool isNeedRebind)
        {
            IList<UIDBClientQuarterlyAssessmentReport> uiEntities = new List<UIDBClientQuarterlyAssessmentReport>();

            int totalRecords = 0;

            if (!string.IsNullOrEmpty(rcbxYear.SelectedValue)
                && !string.IsNullOrEmpty(rcbxQuarter.SelectedValue))
            {
                var uiSearchObj = new UISearchDBClientQuarterlyAssessmentReport()
                {
                    Year = Convert.ToInt32(rcbxYear.SelectedValue),
                    Quarter = Convert.ToInt32(rcbxQuarter.SelectedValue),
                };

                if (!string.IsNullOrEmpty(rcbxClientUser.SelectedValue))
                    uiSearchObj.ClientUserID = Convert.ToInt32(rcbxClientUser.SelectedValue);

                uiEntities = PageReportRepository.GetDBClientQuarterlyAssessmentReport(uiSearchObj, rgEntities.CurrentPageIndex, rgEntities.PageSize, out totalRecords);
            }

            rgEntities.DataSource = uiEntities;
            rgEntities.VirtualItemCount = totalRecords;

            if (isNeedRebind)
                rgEntities.Rebind();

            if (totalRecords > 0)
                btnExport.Visible = true;
            else
                btnExport.Visible = false;
        }


        #endregion

        protected void rgEntities_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            BindEntities(false);
        }

        protected void rgEntities_ColumnCreated(object sender, GridColumnCreatedEventArgs e)
        {
            if (!string.IsNullOrEmpty(rcbxYear.SelectedValue)
                && !string.IsNullOrEmpty(rcbxQuarter.SelectedValue))
            {
                int iQuarter = Convert.ToInt32(rcbxQuarter.SelectedValue);

                int beginMonth = 1;

                EQuarter quarter = (EQuarter)iQuarter;

                switch (quarter)
                {
                    case EQuarter.Quarter1:
                        beginMonth = (int)EMonthOfYear.January;
                        break;
                    case EQuarter.Quarter2:
                        beginMonth = (int)EMonthOfYear.April;
                        break;
                    case EQuarter.Quarter3:
                        beginMonth = (int)EMonthOfYear.July;
                        break;
                    case EQuarter.Quarter4:
                        beginMonth = (int)EMonthOfYear.October;
                        break;
                }

                e.OwnerTableView.Columns.FindByUniqueName("FirstMonthSalesQty").HeaderText = beginMonth + "月";
                e.OwnerTableView.Columns.FindByUniqueName("SecondMonthSalesQty").HeaderText = (beginMonth + 1) + "月";
                e.OwnerTableView.Columns.FindByUniqueName("ThirdMonthSalesQty").HeaderText = (beginMonth + 2) + "月";
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindEntities(true);
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            rcbxYear.ClearSelection();
            rcbxQuarter.ClearSelection();
            rcbxClientUser.ClearSelection();

            BindEntities(true);
        }

        protected void btnExportHidden_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(rcbxYear.SelectedValue)
                && !string.IsNullOrEmpty(rcbxQuarter.SelectedValue))
            {
                int iQuarter = Convert.ToInt32(rcbxQuarter.SelectedValue);

                var uiSearchObj = new UISearchDBClientQuarterlyAssessmentReport()
                {
                    Year = Convert.ToInt32(rcbxYear.SelectedValue),
                    Quarter = iQuarter,
                };

                if (!string.IsNullOrEmpty(rcbxClientUser.SelectedValue))
                    uiSearchObj.ClientUserID = Convert.ToInt32(rcbxClientUser.SelectedValue);

                var uiEntities = PageReportRepository.GetDBClientQuarterlyAssessmentReport(uiSearchObj);

                var excelPath = Server.MapPath("~/App_Data/") + "TempDBClientQuarterlyAssessmentReportExcel.xls";

                int beginMonth = 1;

                EQuarter quarter = (EQuarter)iQuarter;

                switch (quarter)
                {
                    case EQuarter.Quarter1:
                        beginMonth = (int)EMonthOfYear.January;
                        break;
                    case EQuarter.Quarter2:
                        beginMonth = (int)EMonthOfYear.April;
                        break;
                    case EQuarter.Quarter3:
                        beginMonth = (int)EMonthOfYear.July;
                        break;
                    case EQuarter.Quarter4:
                        beginMonth = (int)EMonthOfYear.October;
                        break;
                }

                NPOIHelper nPOIHelper = new Common.NPOIHelper.Excel.NPOIHelper();
                UIDBClientQuarterlyAssessmentReport model = new UIDBClientQuarterlyAssessmentReport();

                List<ExcelHeader> excelHeaders = new List<ExcelHeader>() { 
                    new ExcelHeader(model.GetName(() => model.ClientUserName),"客户"),
                    new ExcelHeader(model.GetName(() => model.HospitalType),"医院性质"),
                    new ExcelHeader(model.GetName(() => model.ProductName),"货品名称"),
                    new ExcelHeader(model.GetName(() => model.Specification),"规格"),
                    new ExcelHeader(model.GetName(() => model.HospitalName),"医院"),
                    new ExcelHeader(model.GetName(() => model.PromotionExpense),"推广费"),
                    new ExcelHeader(model.GetName(() => model.QuarterTaskAssignment),"季度任务量"),
                    new ExcelHeader(model.GetName(() => model.FirstMonthSalesQty), beginMonth + "月"),
                    new ExcelHeader(model.GetName(() => model.SecondMonthSalesQty),(beginMonth + 1) + "月"),
                    new ExcelHeader(model.GetName(() => model.ThirdMonthSalesQty),(beginMonth + 2) + "月"),
                    new ExcelHeader(model.GetName(() => model.QuarterAmount),"季度金额"),
                    new ExcelHeader(model.GetName(() => model.RewardRate),"奖罚率"),
                    new ExcelHeader(model.GetName(() => model.RewardAmount),"奖罚金额"),
                };

                Queue<ExcelHeader> excelHeadersQueue = new Queue<ExcelHeader>(excelHeaders);
                Root excelRoot = new Root()
                {
                    root = new HeadInfo()
                    {
                        rowspan = 2,
                        sheetname = "大包客户季度考核表",
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
                            new AttributeList(){ title=excelHeadersQueue.Dequeue().Name, cellregion="0,1,12,12"},
                    }
                    }
                };

                var fieldFuncs = new List<Func<UIDBClientQuarterlyAssessmentReport, string>>();

                fieldFuncs.Add(x => x.ClientUserName);
                fieldFuncs.Add(x => x.HospitalType);
                fieldFuncs.Add(x => x.ProductName);
                fieldFuncs.Add(x => x.Specification);
                fieldFuncs.Add(x => x.HospitalName);
                fieldFuncs.Add(x => x.PromotionExpense.HasValue ? x.PromotionExpense.ToString("C2") : "");
                fieldFuncs.Add(x => x.QuarterTaskAssignment.ToString());
                fieldFuncs.Add(x => x.FirstMonthSalesQty.ToString());
                fieldFuncs.Add(x => x.SecondMonthSalesQty.ToString());
                fieldFuncs.Add(x => x.ThirdMonthSalesQty.ToString());
                fieldFuncs.Add(x => x.QuarterAmount.ToString("C2"));
                fieldFuncs.Add(x => x.RewardRate.ToString("P2"));
                fieldFuncs.Add(x => x.RewardAmount.ToString("C2"));

                nPOIHelper.ExportToExcel<UIDBClientQuarterlyAssessmentReport>(
                    (List<UIDBClientQuarterlyAssessmentReport>)uiEntities,
                    excelPath,
                    excelHeaders.Select(x => x.Key).ToArray(),
                    excelRoot,
                    fieldFuncs.ToArray());
                Response.ContentType = "application/x-zip-compressed";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + ("大包客户季度考核表_(").UrlEncode()
                    + (uiSearchObj.Year + "年" + rcbxQuarter.SelectedItem.Text).UrlEncode() + ").xls");

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

    }
}