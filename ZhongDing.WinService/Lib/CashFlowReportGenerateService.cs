using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Business.IRepositories;
using ZhongDing.Business.Repositories;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.Domain.Models;
using ZhongDing.Common.Extension;
using System.Diagnostics;
using ZhongDing.Common.NPOIHelper.Excel;
using System.IO;
using ZhongDing.Domain.UIObjects;

namespace ZhongDing.WinService.Lib
{
    public class CashFlowReportGenerateService
    {/// <summary>
        /// Processes the work.
        /// </summary>
        public static void ProcessWork()
        {
            Utility.WriteTrace("Process CashFlowReportGenerateService Work begin at:" + DateTime.Now);
            //首次运行，直接计算前一天的库存
            using (IUnitOfWork unitOfWork = new UnitOfWork())
            {
                var db = unitOfWork.GetDbModel();
                ICashFlowHistoryRepository cashFlowHistoryRepository = new CashFlowHistoryRepository();
                cashFlowHistoryRepository.SetDbModel(db);

                DateTime cashFlowReportGenerateServiceStartTime = new DateTime(DateTime.Now.Year, 1, 1);
                DateTime? lastCashFlowDate = null;
                var lastCashFlowHistory = cashFlowHistoryRepository.GetList().OrderByDescending(x => x.CashFlowDate).FirstOrDefault();
                if (lastCashFlowHistory != null)
                {
                    lastCashFlowDate = lastCashFlowHistory.CashFlowDate;
                }
                if (lastCashFlowDate == null)
                {
                    lastCashFlowDate = cashFlowReportGenerateServiceStartTime.AddMonths(-1);
                }
                DateTime currentDate = DateTime.Now.ToFirstDayOfMonthDate();
                //这个地方需要讨论，到底是每次仅仅生成当月的还是 生成全部月份的？
                List<DateTime> needGenerateDate = new List<DateTime>();
                //如果没有数据 或者+1 之后 < 当前月 则生成，否则不生成
                if (lastCashFlowDate.Value.AddMonths(1) < currentDate)
                {
                    while (lastCashFlowDate.Value.AddMonths(1) < currentDate)
                    {
                        lastCashFlowDate = lastCashFlowDate.Value.AddMonths(1);
                        needGenerateDate.Add(lastCashFlowDate.Value);
                    }
                    needGenerateDate.ForEach(generateDate =>
                    {
                        GenerateCashFlowReport(unitOfWork, generateDate);
                    });
                    unitOfWork.SaveChanges();

                    //
                    DateTime cashFlowDate = needGenerateDate.LastOrDefault();
                    string filePath = GenerateCashFlowReportExcel(unitOfWork, cashFlowDate);
                    //保存CashFlowHistory
                    CashFlowHistory cashFlowHistory = new CashFlowHistory()
                    {
                        CashFlowDate = cashFlowDate,
                        CashFlowFileName = filePath.FileName(),
                        FilePath = WebConfig.UploadFilePathCashFlow + filePath.FileName(),
                    };
                    db.CashFlowHistory.Add(cashFlowHistory);
                    unitOfWork.SaveChanges();
                }

            }

            Utility.WriteTrace("Process CashFlowReportGenerateService Work end at:" + DateTime.Now);
        }

        private static string GenerateCashFlowReportExcel(IUnitOfWork unitOfWork, DateTime cashFlowDate)
        {
            DbModelContainer db = unitOfWork.GetDbModel();
            DateTime start = cashFlowDate.ToFirstDayOfYearDate();
            var cashFlowBaseDatas = db.CashFlowBaseData.Where(x => x.CashFlowDate <= cashFlowDate && x.CashFlowDate >= start).OrderBy(x => x.CashFlowDate).ToList();
            int minMonth = cashFlowBaseDatas.First().CashFlowDate.Month;
            int maxMonth = cashFlowBaseDatas.Last().CashFlowDate.Month;
            List<CashFlowRowItem> excelData = new List<CashFlowRowItem>();
            //行 第一行 列头
            CashFlowRowItem excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "类别";
            int month = minMonth, index = 0;
            while (cashFlowBaseDatas.Count > index)
            {
                index++;
                System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                propertyInfo.SetValue(excelRowItem, month + "月");
                month++;
            }
            excelData.Add(excelRowItem);
            //行 资金余额 （包括个人和公司账户）
            excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "资金余额 （包括个人和公司账户）";
            month = minMonth; index = 0;
            while (cashFlowBaseDatas.Count > index)
            {
                index++;
                System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                propertyInfo.SetValue(excelRowItem, cashFlowBaseData.MoneyBalanceAll.ToString());
                month++;
            }
            excelData.Add(excelRowItem);
            //行 个人
            excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "个人";
            month = minMonth; index = 0;
            while (cashFlowBaseDatas.Count > index)
            {
                index++;
                System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                propertyInfo.SetValue(excelRowItem, cashFlowBaseData.MoneyBalancePersonal.ToString());
                month++;
            }
            excelData.Add(excelRowItem);
            //行 公司
            excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "公司";
            month = minMonth; index = 0;
            while (cashFlowBaseDatas.Count > index)
            {
                index++;
                System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                propertyInfo.SetValue(excelRowItem, cashFlowBaseData.MoneyBalanceCompany.ToString());
                month++;
            }
            excelData.Add(excelRowItem);
            //行 收入类别
            excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "收入类别";
            excelData.Add(excelRowItem);

            //行 销售收入
            List<Company> companys = GetCompanys(cashFlowBaseDatas);
            companys.ForEach(company =>
            {
                excelRowItem = new CashFlowRowItem();
                excelRowItem.FirstColName = "销售收入-" + company.CompanyName;
                month = minMonth; index = 0;
                while (cashFlowBaseDatas.Count > index)
                {
                    index++;
                    System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                    var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                    var cashFlowSaleIncomeData = cashFlowBaseData.CashFlowSaleIncomeData.FirstOrDefault(x => x.CompanyID == company.ID);
                    if (cashFlowSaleIncomeData != null)
                        propertyInfo.SetValue(excelRowItem, cashFlowSaleIncomeData.Amount.ToString());
                    month++;
                }
                excelData.Add(excelRowItem);
            });
            //行 销售收入 厂家返款（高开+奖励）
            //List<Company> companys = GetCompanys(cashFlowBaseDatas);
            companys.ForEach(company =>
            {
                excelRowItem = new CashFlowRowItem();
                excelRowItem.FirstColName = "厂家返款（高开+奖励）-" + company.CompanyName;
                month = minMonth; index = 0;
                while (cashFlowBaseDatas.Count > index)
                {
                    index++;
                    System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                    var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                    var cashFlowRefundData = cashFlowBaseData.CashFlowRefundData.FirstOrDefault(x => x.CompanyID == company.ID);
                    if (cashFlowRefundData != null)
                        propertyInfo.SetValue(excelRowItem, cashFlowRefundData.Amount.ToString());
                    month++;
                }
                excelData.Add(excelRowItem);
            });

            //行 销售收入 配送公司-四个配送公司	大包在本月的入账	 打包配送模式收了多少钱
            List<DistributionCompany> distributionCompanys = GetDistributionCompanys(cashFlowBaseDatas);
            distributionCompanys.ForEach(distributionCompany =>
            {
                excelRowItem = new CashFlowRowItem();
                excelRowItem.FirstColName = "厂家返款（高开+奖励）-" + distributionCompany.Name;
                month = minMonth; index = 0;
                while (cashFlowBaseDatas.Count > index)
                {
                    index++;
                    System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                    var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                    var cashFlowDistributionCompanyData = cashFlowBaseData.CashFlowDistributionCompanyData.FirstOrDefault(x => x.DistributionCompanyID == distributionCompany.ID);
                    if (cashFlowDistributionCompanyData != null)
                        propertyInfo.SetValue(excelRowItem, cashFlowDistributionCompanyData.Amount.ToString());
                    month++;
                }
                excelData.Add(excelRowItem);
            });
            //行 挂靠回款
            excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "挂靠回款";
            month = minMonth; index = 0;
            while (cashFlowBaseDatas.Count > index)
            {
                index++;
                System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                propertyInfo.SetValue(excelRowItem, cashFlowBaseData.AttachedModeIncome.ToString());
                month++;
            }
            excelData.Add(excelRowItem);
            //行 保证金收入-厂家保证金
            excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "保证金收入-厂家保证金";
            month = minMonth; index = 0;
            while (cashFlowBaseDatas.Count > index)
            {
                index++;
                System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                propertyInfo.SetValue(excelRowItem, cashFlowBaseData.SupplierCautionMoneyIncome.ToString());
                month++;
            }
            excelData.Add(excelRowItem);
            //行 保证金收入-客户保证金
            excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "保证金收入-客户保证金";
            month = minMonth; index = 0;
            while (cashFlowBaseDatas.Count > index)
            {
                index++;
                System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                propertyInfo.SetValue(excelRowItem, cashFlowBaseData.ClientCautionMoneyIncome.ToString());
                month++;
            }
            excelData.Add(excelRowItem);
            //行 收回借款
            excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "收回借款";
            month = minMonth; index = 0;
            while (cashFlowBaseDatas.Count > index)
            {
                index++;
                System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                propertyInfo.SetValue(excelRowItem, cashFlowBaseData.BorrowMoneyIncome.ToString());
                month++;
            }
            excelData.Add(excelRowItem);

            //行 进项票收入-英特康
            //List<Company> companys = GetCompanys(cashFlowBaseDatas);
            companys.ForEach(company =>
            {
                excelRowItem = new CashFlowRowItem();
                excelRowItem.FirstColName = "进项票收入-" + company.CompanyName;
                month = minMonth; index = 0;
                while (cashFlowBaseDatas.Count > index)
                {
                    index++;
                    System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                    var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                    var cashFlowInvoiceIncomeData = cashFlowBaseData.CashFlowInvoiceIncomeData.FirstOrDefault(x => x.CompanyID == company.ID);
                    if (cashFlowInvoiceIncomeData != null)
                        propertyInfo.SetValue(excelRowItem, cashFlowInvoiceIncomeData.Amount.ToString());
                    month++;
                }
                excelData.Add(excelRowItem);
            });
            //行 支出类别
            excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "支出类别";
            excelData.Add(excelRowItem);
            //采购金额-万国康
            //List<Company> companys = GetCompanys(cashFlowBaseDatas);
            companys.ForEach(company =>
            {
                excelRowItem = new CashFlowRowItem();
                excelRowItem.FirstColName = "采购金额-" + company.CompanyName;
                month = minMonth; index = 0;
                while (cashFlowBaseDatas.Count > index)
                {
                    index++;
                    System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                    var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                    var cashFlowPurchaseAmountData = cashFlowBaseData.CashFlowPurchaseAmountData.FirstOrDefault(x => x.CompanyID == company.ID);
                    if (cashFlowPurchaseAmountData != null)
                        propertyInfo.SetValue(excelRowItem, cashFlowPurchaseAmountData.Amount.ToString());
                    month++;
                }
                excelData.Add(excelRowItem);
            });
            //高开客户返款-英特康
            //List<Company> companys = GetCompanys(cashFlowBaseDatas);
            companys.ForEach(company =>
            {
                excelRowItem = new CashFlowRowItem();
                excelRowItem.FirstColName = "高开客户返款-" + company.CompanyName;
                month = minMonth; index = 0;
                while (cashFlowBaseDatas.Count > index)
                {
                    index++;
                    System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                    var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                    var cashFlowClientRefundData = cashFlowBaseData.CashFlowClientRefundData.FirstOrDefault(x => x.CompanyID == company.ID);
                    if (cashFlowClientRefundData != null)
                        propertyInfo.SetValue(excelRowItem, cashFlowClientRefundData.Amount.ToString());
                    month++;
                }
                excelData.Add(excelRowItem);
            });
            //行 /大包客户返款-四个配送公司	 	 工作流中的 大包客户提成结算 
            excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "大包客户返款";
            month = minMonth; index = 0;
            while (cashFlowBaseDatas.Count > index)
            {
                index++;
                System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                propertyInfo.SetValue(excelRowItem, cashFlowBaseData.DaBaoRefund.ToString());
                month++;
            }
            excelData.Add(excelRowItem);
            //行 基本工资
            excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "基本工资";
            month = minMonth; index = 0;
            while (cashFlowBaseDatas.Count > index)
            {
                index++;
                System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                propertyInfo.SetValue(excelRowItem, cashFlowBaseData.Salary.ToString());
                month++;
            }
            excelData.Add(excelRowItem);
            //行 业务提成
            excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "业务提成";
            excelData.Add(excelRowItem);
            //行 奖金福利
            excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "奖金福利";
            excelData.Add(excelRowItem);
            //行 费用报销
            excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "费用报销";
            month = minMonth; index = 0;
            while (cashFlowBaseDatas.Count > index)
            {
                index++;
                System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                propertyInfo.SetValue(excelRowItem, cashFlowBaseData.Reimbursement.ToString());
                month++;
            }
            excelData.Add(excelRowItem);
            //行 借款支出
            excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "借款支出";
            month = minMonth; index = 0;
            while (cashFlowBaseDatas.Count > index)
            {
                index++;
                System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                propertyInfo.SetValue(excelRowItem, cashFlowBaseData.BorrowMoney.ToString());
                month++;
            }
            excelData.Add(excelRowItem);
            //行 支付保证金-厂家保证金
            excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "支付保证金-厂家保证金";
            month = minMonth; index = 0;
            while (cashFlowBaseDatas.Count > index)
            {
                index++;
                System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                propertyInfo.SetValue(excelRowItem, cashFlowBaseData.CautionMoneyPayToSupplier.ToString());
                month++;
            }
            excelData.Add(excelRowItem);
            //行 支付保证金-退客户保证金
            excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "支付保证金-退客户保证金";
            month = minMonth; index = 0;
            while (cashFlowBaseDatas.Count > index)
            {
                index++;
                System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                propertyInfo.SetValue(excelRowItem, cashFlowBaseData.CautionMoneyReturnToClient.ToString());
                month++;
            }
            excelData.Add(excelRowItem);
            //销项票税支出-万国康
            //List<Company> companys = GetCompanys(cashFlowBaseDatas);
            companys.ForEach(company =>
            {
                excelRowItem = new CashFlowRowItem();
                excelRowItem.FirstColName = "销项票税支出-" + company.CompanyName;
                month = minMonth; index = 0;
                while (cashFlowBaseDatas.Count > index)
                {
                    index++;
                    System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                    var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                    var cashFlowInvoiceExpendData = cashFlowBaseData.CashFlowInvoiceExpendData.FirstOrDefault(x => x.CompanyID == company.ID);
                    if (cashFlowInvoiceExpendData != null)
                        propertyInfo.SetValue(excelRowItem, cashFlowInvoiceExpendData.Amount.ToString());
                    month++;
                }
                excelData.Add(excelRowItem);
            });
            //行 产品促销返利
            excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "产品促销返利";
            excelData.Add(excelRowItem);
            //发货费-万国康
            //List<Company> companys = GetCompanys(cashFlowBaseDatas);
            companys.ForEach(company =>
            {
                excelRowItem = new CashFlowRowItem();
                excelRowItem.FirstColName = "发货费-" + company.CompanyName;
                month = minMonth; index = 0;
                while (cashFlowBaseDatas.Count > index)
                {
                    index++;
                    System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                    var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                    var cashFlowShippingFeeData = cashFlowBaseData.CashFlowShippingFeeData.FirstOrDefault(x => x.CompanyID == company.ID);
                    if (cashFlowShippingFeeData != null)
                        propertyInfo.SetValue(excelRowItem, cashFlowShippingFeeData.Amount.ToString());
                    month++;
                }
                excelData.Add(excelRowItem);
            });
            //行 托管配送费
            excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "托管配送费";
            month = minMonth; index = 0;
            while (cashFlowBaseDatas.Count > index)
            {
                index++;
                System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                propertyInfo.SetValue(excelRowItem, cashFlowBaseData.ManagedDistributionFee.ToString());
                month++;
            }
            excelData.Add(excelRowItem);
            //行 其他特殊支出
            excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "其他特殊支出";
            month = minMonth; index = 0;
            while (cashFlowBaseDatas.Count > index)
            {
                index++;
                System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                propertyInfo.SetValue(excelRowItem, cashFlowBaseData.Other.ToString());
                month++;
            }
            excelData.Add(excelRowItem);
            //行 厂家经理返款
            excelRowItem = new CashFlowRowItem();
            excelRowItem.FirstColName = "厂家经理返款";
            month = minMonth; index = 0;
            while (cashFlowBaseDatas.Count > index)
            {
                index++;
                System.Reflection.PropertyInfo propertyInfo = excelRowItem.GetType().GetProperty("Month" + month);
                var cashFlowBaseData = cashFlowBaseDatas.First(x => x.CashFlowDate.Month == month);
                propertyInfo.SetValue(excelRowItem, cashFlowBaseData.FMRefund.ToString());
                month++;
            }
            excelData.Add(excelRowItem);
            string filePath = ExportToExcel(excelData, cashFlowDate);
            return filePath;
        }

        private static string ExportToExcel(List<CashFlowRowItem> excelData, DateTime cashFlowDate)
        {
            string excelPath = WebConfig.WebsiteAbsoluteRootPath + WebConfig.UploadFilePathCashFlow.TrimStart(new char[] { '~', '/' });
            excelPath = excelPath.Replace("/", "\\");
            if (!Directory.Exists(excelPath))
                Directory.CreateDirectory(excelPath);
            excelPath += cashFlowDate.ToString("yyyy-MM") + ".xls";
            if (File.Exists(excelPath))
                File.Delete(excelPath);

            NPOIHelper nPOIHelper = new Common.NPOIHelper.Excel.NPOIHelper();
            CashFlowRowItem model = new CashFlowRowItem();

            List<ExcelHeader> excelHeaders = new List<ExcelHeader>() { 
                new ExcelHeader(model.GetName(() => model.FirstColName),excelData[0].FirstColName),
            };
            var firstRow = excelData[0];
            for (int monthIndex = 0; monthIndex < 12; monthIndex++)
            {
                int monthValue = monthIndex + 1;
                System.Reflection.PropertyInfo pro = firstRow.GetType().GetProperty("Month" + monthValue);
                object value = pro.GetValue(firstRow, null);
                if (value != null && value.ToString().IsNotNullOrEmpty())
                {
                    excelHeaders.Add(new ExcelHeader("Month" + monthValue, value.ToString()));
                }
            }
            Queue<ExcelHeader> excelHeadersQueue = new Queue<ExcelHeader>(excelHeaders);
            Root excelRoot = new Root()
            {
                root = new HeadInfo()
                {
                    rowspan = 2,
                    sheetname = "报表",
                    defaultheight = null,
                    defaultwidth = 20,
                    head = new List<AttributeList>()
                    {
                    }
                }
            };
            for (int index = 0; index < excelHeaders.Count; index++)
            {
                excelRoot.root.head.Add(new AttributeList() { title = excelHeadersQueue.Dequeue().Name, cellregion = "0,1," + index + "," + index + "" });
            }
            excelData.RemoveAt(0);//去除首行
            List<Func<CashFlowRowItem, string>> fieldFuncs = new List<Func<CashFlowRowItem, string>>();

            if (firstRow.FirstColName.IsNotNullOrEmpty()) fieldFuncs.Add(x => x.FirstColName);
            if (firstRow.Month1.IsNotNullOrEmpty()) fieldFuncs.Add(x => x.Month1.IsNotNullOrEmpty() ? x.Month1.ToDecimal().ToString("f2") : string.Empty);
            if (firstRow.Month2.IsNotNullOrEmpty()) fieldFuncs.Add(x => x.Month2.IsNotNullOrEmpty() ? x.Month2.ToDecimal().ToString("f2") : string.Empty);
            if (firstRow.Month3.IsNotNullOrEmpty()) fieldFuncs.Add(x => x.Month3.IsNotNullOrEmpty() ? x.Month3.ToDecimal().ToString("f2") : string.Empty);
            if (firstRow.Month4.IsNotNullOrEmpty()) fieldFuncs.Add(x => x.Month4.IsNotNullOrEmpty() ? x.Month4.ToDecimal().ToString("f2") : string.Empty);
            if (firstRow.Month5.IsNotNullOrEmpty()) fieldFuncs.Add(x => x.Month5.IsNotNullOrEmpty() ? x.Month5.ToDecimal().ToString("f2") : string.Empty);
            if (firstRow.Month6.IsNotNullOrEmpty()) fieldFuncs.Add(x => x.Month6.IsNotNullOrEmpty() ? x.Month6.ToDecimal().ToString("f2") : string.Empty);
            if (firstRow.Month7.IsNotNullOrEmpty()) fieldFuncs.Add(x => x.Month7.IsNotNullOrEmpty() ? x.Month7.ToDecimal().ToString("f2") : string.Empty);
            if (firstRow.Month8.IsNotNullOrEmpty()) fieldFuncs.Add(x => x.Month8.IsNotNullOrEmpty() ? x.Month8.ToDecimal().ToString("f2") : string.Empty);
            if (firstRow.Month9.IsNotNullOrEmpty()) fieldFuncs.Add(x => x.Month9.IsNotNullOrEmpty() ? x.Month9.ToDecimal().ToString("f2") : string.Empty);
            if (firstRow.Month10.IsNotNullOrEmpty()) fieldFuncs.Add(x => x.Month10.IsNotNullOrEmpty() ? x.Month10.ToDecimal().ToString("f2") : string.Empty);
            if (firstRow.Month11.IsNotNullOrEmpty()) fieldFuncs.Add(x => x.Month11.IsNotNullOrEmpty() ? x.Month11.ToDecimal().ToString("f2") : string.Empty);
            if (firstRow.Month12.IsNotNullOrEmpty()) fieldFuncs.Add(x => x.Month12.IsNotNullOrEmpty() ? x.Month12.ToDecimal().ToString("f2") : string.Empty);
            nPOIHelper.ExportToExcel<CashFlowRowItem>(
                (List<CashFlowRowItem>)excelData,
                excelPath,
                excelHeaders.Select(x => x.Key).ToArray(),
                excelRoot,
                fieldFuncs.ToArray());
            return excelPath;
        }

        private static List<DistributionCompany> GetDistributionCompanys(List<CashFlowBaseData> cashFlowBaseDatas)
        {
            List<DistributionCompany> distributionCompanys = new List<DistributionCompany>();
            var cashFlowDistributionCompanyDatas = cashFlowBaseDatas.Last().CashFlowDistributionCompanyData.OrderBy(x => x.DistributionCompanyID).ToList();
            cashFlowDistributionCompanyDatas.ForEach(cashFlowDistributionCompanyData =>
            {
                distributionCompanys.Add(cashFlowDistributionCompanyData.DistributionCompany);
            });
            return distributionCompanys;
        }

        private static List<Company> GetCompanys(List<CashFlowBaseData> cashFlowBaseDatas)
        {
            List<Company> companys = new List<Company>();
            var cashFlowSaleIncomeDatas = cashFlowBaseDatas.Last().CashFlowSaleIncomeData.OrderBy(x => x.CompanyID).ToList();
            cashFlowSaleIncomeDatas.ForEach(cashFlowSaleIncomeData =>
            {
                companys.Add(cashFlowSaleIncomeData.Company);
            });
            return companys;
        }

        private static void GenerateCashFlowReport(IUnitOfWork unitOfWork, DateTime generateDate)
        {
            DbModelContainer db = unitOfWork.GetDbModel();
            IBankAccountRepository bankAccountRepository = new BankAccountRepository();
            ICompanyRepository companyRepository = new CompanyRepository();
            IApplicationPaymentRepository applicationPaymentRepository = new ApplicationPaymentRepository();
            IDistributionCompanyRepository distributionCompanyRepository = new DistributionCompanyRepository();
            bankAccountRepository.SetDbModel(db);
            companyRepository.SetDbModel(db);
            applicationPaymentRepository.SetDbModel(db);
            distributionCompanyRepository.SetDbModel(db);

            CashFlowBaseData cashFlowBaseData = new CashFlowBaseData();
            cashFlowBaseData.CashFlowDate = generateDate;
            cashFlowBaseData.MoneyBalanceAll = bankAccountRepository.GetMoneyBalanceAll(generateDate);
            cashFlowBaseData.MoneyBalanceCompany = bankAccountRepository.GetMoneyBalanceAll(generateDate, EAccountType.Company);
            cashFlowBaseData.MoneyBalancePersonal = bankAccountRepository.GetMoneyBalanceAll(generateDate, EAccountType.Personal);
            //获取所有帐套
            var companyIDs = companyRepository.GetList().Select(x => x.ID).ToList();
            //销售收入-英特康	招商模式本月的入账
            companyIDs.ForEach(companyID =>
            {
                CashFlowSaleIncomeData cashFlowSaleIncomeData = new CashFlowSaleIncomeData()
                {
                    CompanyID = companyID,
                    CashFlowBaseData = cashFlowBaseData,
                    Amount = applicationPaymentRepository.GetSaleIncomeAttractBusinessMode(companyID, generateDate)
                };
                db.CashFlowSaleIncomeData.Add(cashFlowSaleIncomeData);
            });
            //厂家返款（高开+奖励）-英特康	供应商在本月的返款 （不包括抵扣）
            companyIDs.ForEach(companyID =>
            {
                CashFlowRefundData cashFlowRefundData = new CashFlowRefundData()
                {
                    CompanyID = companyID,
                    CashFlowBaseData = cashFlowBaseData,
                    Amount = applicationPaymentRepository.GetSupplierRefund(companyID, generateDate)
                };
                db.CashFlowRefundData.Add(cashFlowRefundData);
            });
            //获取所有配送公司
            var distributionCompanyIDs = distributionCompanyRepository.GetList().Select(x => x.ID).ToList();
            //配送公司-四个配送公司	大包在本月的入账	 打包配送模式收了多少钱
            distributionCompanyIDs.ForEach(distributionCompanyID =>
            {
                CashFlowDistributionCompanyData cashFlowDistributionCompanyData = new CashFlowDistributionCompanyData()
                {
                    DistributionCompanyID = distributionCompanyID,
                    CashFlowBaseData = cashFlowBaseData,
                    Amount = applicationPaymentRepository.GetSaleIncomeDaBaoMode(distributionCompanyID, generateDate)
                };
                db.CashFlowDistributionCompanyData.Add(cashFlowDistributionCompanyData);

            });
            //挂靠回款	挂靠在本月的入账	 挂靠模式的销售收了多少钱（日期计算按照收款的那个日期）
            cashFlowBaseData.AttachedModeIncome = applicationPaymentRepository.GetSaleIncomeAttachedMode(generateDate);
            // 保证金收入-厂家保证金	 	 供应商保证金 （退回的）
            cashFlowBaseData.SupplierCautionMoneyIncome = applicationPaymentRepository.GetSupplierCautionMoneyIncome(generateDate);
            // 保证金收入-客户保证金	 	 客户保证金（收来的）
            cashFlowBaseData.ClientCautionMoneyIncome = applicationPaymentRepository.GetClientCautionMoneyIncome(generateDate);
            //收回借款	 	 收回的借款
            cashFlowBaseData.BorrowMoneyIncome = applicationPaymentRepository.GetBorrowMoneyIncome(generateDate);
            //进项票收入-英特康	 	 供应商发票结算的 “结算金额”
            companyIDs.ForEach(companyID =>
            {
                CashFlowInvoiceIncomeData cashFlowInvoiceIncomeData = new CashFlowInvoiceIncomeData()
                {
                    CompanyID = companyID,
                    CashFlowBaseData = cashFlowBaseData,
                    Amount = applicationPaymentRepository.GetInvoiceIncomeSupplierInvoice(companyID, generateDate)
                };
                db.CashFlowInvoiceIncomeData.Add(cashFlowInvoiceIncomeData);
            });
            //采购金额-万国康	 	 采购订单的金额
            companyIDs.ForEach(companyID =>
            {
                CashFlowPurchaseAmountData cashFlowPurchaseAmountData = new CashFlowPurchaseAmountData()
                {
                    CompanyID = companyID,
                    CashFlowBaseData = cashFlowBaseData,
                    Amount = applicationPaymentRepository.GetPurchaseAmount(companyID, generateDate)
                };
                db.CashFlowPurchaseAmountData.Add(cashFlowPurchaseAmountData);
            });

            //高开客户返款-英特康  	 	 工作流中的 客户返款 和  客户任务奖励返款
            companyIDs.ForEach(companyID =>
            {
                CashFlowClientRefundData cashFlowClientRefundData = new CashFlowClientRefundData()
                {
                    CompanyID = companyID,
                    CashFlowBaseData = cashFlowBaseData,
                    Amount = applicationPaymentRepository.GetClientRefund(companyID, generateDate)
                };
                db.CashFlowClientRefundData.Add(cashFlowClientRefundData);
            });

            //大包客户返款-四个配送公司	 	 工作流中的 大包客户提成结算 
            cashFlowBaseData.DaBaoRefund = applicationPaymentRepository.GetDaBaoRefund(generateDate);
            //distributionCompanyIDs.ForEach(distributionCompanyID =>
            //{
            //    CashFlowDaBaoRefundData cashFlowDaBaoRefundData = new CashFlowDaBaoRefundData()
            //    {
            //         CompanyID = distributionCompanyID,
            //        CashFlowBaseData = cashFlowBaseData,
            //        Amount = applicationPaymentRepository.GetSaleIncomeDaBaoMode(distributionCompanyID, generateDate)
            //    };
            //    db.CashFlowDaBaoRefundData.Add(cashFlowDaBaoRefundData);

            //});

            //基本工资
            cashFlowBaseData.Salary = applicationPaymentRepository.GetSalary(generateDate);
            //业务提成
            //ToDo
            //奖金福利
            //ToDo
            //费用报销	不含发货费，不含托管配送费	物流的 排除，托管配送费 排除
            cashFlowBaseData.Reimbursement = applicationPaymentRepository.GetReimbursement(generateDate);
            //借款支出
            cashFlowBaseData.BorrowMoney = applicationPaymentRepository.GetBorrowMoneyExpend(generateDate);
            //支付保证金-厂家保证金	 	 付给供应商的保证金
            cashFlowBaseData.CautionMoneyPayToSupplier = applicationPaymentRepository.GetCautionMoneyPayToSupplier(generateDate);
            // 支付保证金-退客户保证金	 	 退回给客户的保证金
            cashFlowBaseData.CautionMoneyReturnToClient = applicationPaymentRepository.GetCautionMoneyReturnToClient(generateDate);
            //销项票税支出-万国康	 	 发票模块的： 客户发票结算管理 和 挂靠发票结算管理  
            companyIDs.ForEach(companyID =>
            {
                CashFlowInvoiceExpendData cashFlowInvoiceExpendData = new CashFlowInvoiceExpendData()
                {
                    CompanyID = companyID,
                    CashFlowBaseData = cashFlowBaseData,
                    Amount = applicationPaymentRepository.GetInvoiceExpend(companyID, generateDate)
                };
                db.CashFlowInvoiceExpendData.Add(cashFlowInvoiceExpendData);
            });
            //产品促销返利	客户奖励返款	 留空
            //ToDo
            //发货费-英特康	 	 费用报销中的物流费用
            companyIDs.ForEach(companyID =>
            {
                CashFlowShippingFeeData cashFlowShippingFeeData = new CashFlowShippingFeeData()
                {
                    CompanyID = companyID,
                    CashFlowBaseData = cashFlowBaseData,
                    Amount = applicationPaymentRepository.GetShippingFee(companyID, generateDate)
                };
                db.CashFlowShippingFeeData.Add(cashFlowShippingFeeData);
            });
            //托管配送费	费用报销中的托管配送费	 费用报销中的 添加托管配送费类型
            cashFlowBaseData.ManagedDistributionFee = applicationPaymentRepository.GetManagedDistributionFee(generateDate);
            //其他特殊支出	费用报销中的杂项	 费用报销中的 添加杂项类型
            cashFlowBaseData.Other = applicationPaymentRepository.GetOther(generateDate);
            //厂家经理返款	付给厂家经理	 厂家经理返款管理
            cashFlowBaseData.FMRefund = applicationPaymentRepository.GetFMRefund(generateDate);

            db.CashFlowBaseData.Add(cashFlowBaseData);
        }
    }
}
