﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface IReportRepository
    {
        IList<UICompany> GetCompanyReport();
        IList<UIProcureOrderReport> GetProcureOrderReport(Domain.UISearchObjects.UISearchProcureOrderReport uiSearchObj);

        IList<UIProcureOrderReport> GetProcureOrderReport(Domain.UISearchObjects.UISearchProcureOrderReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords);


        IList<UIProcureOrderApplicationPaymentReport> GetProcureOrderApplicationPaymentReport(Domain.UISearchObjects.UISearchProcureOrderApplicationPaymentReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        IList<UIProcureOrderApplicationPaymentReport> GetProcureOrderApplicationPaymentReport(Domain.UISearchObjects.UISearchProcureOrderApplicationPaymentReport uiSearchObj);

        IList<UIClientSaleAppReport> GetClientSaleAppReport(UISearchClientSaleAppReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        IList<UIClientSaleAppReport> GetClientSaleAppReport(UISearchClientSaleAppReport uiSearchObj);

        IList<UIStockOutDetailReport> GetStockOutDetailReport(UISearchStockOutDetailReport uiSearchObj);

        IList<UIStockOutDetailReport> GetStockOutDetailReport(UISearchStockOutDetailReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        IList<UIStockInDetailReport> GetStockInDetailReport(UISearchStockInDetailReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        IList<UIStockInDetailReport> GetStockInDetailReport(UISearchStockInDetailReport uiSearchObj);

        IList<UIProcurePlanReport> GetProcurePlanReport(UISearchProcurePlanReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
        
        IList<UIProcurePlanReport> GetProcurePlanReport(UISearchProcurePlanReport uiSearchObj);

        /// <summary>
        /// 大包客户季度考核表
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <returns>IList{UIDBClientQuarterlyAssessmentReport}.</returns>
        IList<UIDBClientQuarterlyAssessmentReport> GetDBClientQuarterlyAssessmentReport(UISearchDBClientQuarterlyAssessmentReport uiSearchObj);

        /// <summary>
        /// 大包客户季度考核表
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalRecords">The total records.</param>
        /// <returns>IList{UIDBClientQuarterlyAssessmentReport}.</returns>
        IList<UIDBClientQuarterlyAssessmentReport> GetDBClientQuarterlyAssessmentReport(UISearchDBClientQuarterlyAssessmentReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        IList<UIInventorySummaryReport> GetInventorySummaryReport(UISearchInventorySummaryReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        IList<UIInventorySummaryDetailReport> GetInventorySummaryDetailReport(UISearchInventorySummaryDetailReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        IList<UIInventorySummaryReport> GetInventorySummaryReport(UISearchInventorySummaryReport uiSearchObj);

        IList<UIInventorySummaryDetailReport> GetInventorySummaryDetailReport(UISearchInventorySummaryDetailReport uiSearchObj);

        /// <summary>
        /// 配送公司流向结算表
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <returns>IList{UIDCFlowSettlementReport}.</returns>
        IList<UIDCFlowSettlementReport> GetDCFlowSettlementReport(UISearchDCFlowSettlementReport uiSearchObj);

        /// <summary>
        /// 配送公司流向结算表
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalRecords">The total records.</param>
        /// <returns>IList{UIDCFlowSettlementReport}.</returns>
        IList<UIDCFlowSettlementReport> GetDCFlowSettlementReport(UISearchDCFlowSettlementReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        /// <summary>
        /// 大包客户结算
        /// </summary>
        /// <param name="uiSearchObj"></param>
        /// <returns></returns>
        IList<UIDBClientSettlementReport> GetDBClientSettlementReport(UISearchDBClientSettlementReport uiSearchObj);
        IList<UIDBClientSettlementReport> GetDBClientSettlementReport(UISearchDBClientSettlementReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords);


        IList<UISupplierTaskReport> GetSupplierTaskReport(UISearchSupplierTaskReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
        IList<UISupplierTaskReport> GetSupplierTaskReport(UISearchSupplierTaskReport uiSearchObj);

        /// <summary>
        /// 每月现金流量表
        /// </summary>
        /// <param name="uiSearchObj"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        IList<UICashFlowReport> GetCashFlowReport(UISearchCashFlowReport uiSearchObj, int pageIndex, int pageSize, out int totalRecords);
        /// <summary>
        /// 获取现金流表 详情
        /// </summary>
        /// <param name="uiSearchObj"></param>
        /// <returns></returns>
        IList<CashFlowRowItem> GetCashFlowReportDetail(UISearchCashFlowReportDetail uiSearchObj);
    }
}
