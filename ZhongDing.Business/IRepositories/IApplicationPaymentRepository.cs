using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;
using ZhongDing.Domain.UIObjects;
using ZhongDing.Domain.UISearchObjects;

namespace ZhongDing.Business.IRepositories
{
    public interface IApplicationPaymentRepository : IBaseRepository<ApplicationPayment>
    {
        /// <summary>
        /// 获取UI List，不分页
        /// </summary>
        /// <param name="uiSearchObj">The UI search obj.</param>
        /// <returns>IList{UIApplicationPayment}.</returns>
        IList<UIApplicationPayment> GetUIList(UISearchApplicationPayment uiSearchObj = null);

        /// <summary>
        /// 获取UI List，分页
        /// </summary>
        /// <param name="uiSearchObj">查询参数对象.</param>
        /// <param name="pageIndex">当前页.</param>
        /// <param name="pageSize">每页条数.</param>
        /// <param name="totalRecords">总记录数.</param>
        /// <returns>IList{UIApplicationPayment}.</returns>
        IList<UIApplicationPayment> GetUIList(UISearchApplicationPayment uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        /// <summary>
        /// 现金管理
        /// </summary>
        /// <param name="uiSearchObj"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalRecords"></param>
        /// <returns></returns>
        IList<UIApplicationPayment> GetUIListForMoneyManagement(UISearchApplicationPayment uiSearchObj, int pageIndex, int pageSize, out int totalRecords);

        /// <summary>
        /// 获取 指定账户的 指定日期的余额
        /// </summary>
        /// <param name="bankAccountID"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        decimal GetRealTimeBalance(int bankAccountID, DateTime month);
        /// <summary>
        /// 获取 指定帐套，指定年月 销售收入 （招商模式本月的入账）
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        decimal GetSaleIncomeAttractBusinessMode(int companyID, DateTime generateDate);
        /// <summary>
        /// 厂家返款（高开+奖励）-英特康	供应商在本月的返款 （不包括抵扣）
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        decimal GetSupplierRefund(int companyID, DateTime generateDate);
        /// <summary>
        /// 配送公司-四个配送公司	大包在本月的入账	 打包配送模式收了多少钱
        /// </summary>
        /// <param name="distributionCompanyID"></param>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        decimal GetSaleIncomeDaBaoMode(int distributionCompanyID, DateTime generateDate);
        /// <summary>
        /// 挂靠回款	挂靠在本月的入账	 挂靠模式的销售收了多少钱（日期计算按照收款的那个日期）
        /// </summary>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        decimal GetSaleIncomeAttachedMode(DateTime generateDate);
        /// <summary>
        /// 进项票收入-英特康	 	 供应商发票结算的 “结算金额”
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        decimal GetInvoiceIncomeSupplierInvoice(int companyID, DateTime generateDate);
        /// <summary>
        /// 采购金额-万国康	 	 采购订单的金额
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        decimal GetPurchaseAmount(int companyID, DateTime generateDate);
        /// <summary>
        /// 高开客户返款-英特康  	 	 工作流中的 客户返款  和  客户任务奖励返款
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        decimal GetClientRefund(int companyID, DateTime generateDate);
        /// <summary>
        /// 基本工资
        /// </summary>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        decimal GetSalary(DateTime generateDate);
        /// <summary>
        /// 费用报销	不含发货费，不含托管配送费	物流的 排除，托管配送费 排除
        /// </summary>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        decimal GetReimbursement(DateTime generateDate);
        /// <summary>
        /// 借款支出
        /// </summary>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        decimal GetBorrowMoneyExpend(DateTime generateDate);
        /// <summary>
        /// 保证金收入-厂家保证金	 	 供应商保证金 （退回的）
        /// </summary>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        decimal GetSupplierCautionMoneyIncome(DateTime generateDate);
        /// <summary>
        /// 保证金收入-客户保证金	 	 客户保证金（收来的）
        /// </summary>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        decimal GetClientCautionMoneyIncome(DateTime generateDate);
        /// <summary>
        /// 收回借款	 	 收回的借款
        /// </summary>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        decimal GetBorrowMoneyIncome(DateTime generateDate);
        /// <summary>
        /// 支付保证金-退客户保证金	 	 退回给客户的保证金
        /// </summary>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        decimal GetCautionMoneyReturnToClient(DateTime generateDate);
        /// <summary>
        /// 支付保证金-厂家保证金	 	 付给供应商的保证金
        /// </summary>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        decimal GetCautionMoneyPayToSupplier(DateTime generateDate);
        /// <summary>
        /// 销项票税支出-万国康	 	 发票模块的： 客户发票结算管理 和 挂靠发票结算管理
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        decimal GetInvoiceExpend(int companyID, DateTime generateDate);
        /// <summary>
        /// 发货费-英特康	 	 费用报销中的物流费用
        /// </summary>
        /// <param name="companyID"></param>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        decimal GetShippingFee(int companyID, DateTime generateDate);
        /// <summary>
        /// 托管配送费	费用报销中的托管配送费	 费用报销中的 添加托管配送费类型
        /// </summary>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        decimal GetManagedDistributionFee(DateTime generateDate);
        /// <summary>
        /// 其他特殊支出	费用报销中的杂项	 费用报销中的 添加杂项类型
        /// </summary>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        decimal GetOther(DateTime generateDate);
        /// <summary>
        /// 厂家经理返款	付给厂家经理	 厂家经理返款管理
        /// </summary>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        decimal GetFMRefund(DateTime generateDate);
        /// <summary>
        /// 大包客户返款-四个配送公司	 	 工作流中的 大包客户提成结算
        /// </summary>
        /// <param name="generateDate"></param>
        /// <returns></returns>
        decimal GetDaBaoRefund(DateTime generateDate);
    }
}
