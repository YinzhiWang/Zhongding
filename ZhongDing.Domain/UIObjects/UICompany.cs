using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    /// <summary>
    /// 类：账套UI对象
    /// </summary>
    public class UICompany : UIBase
    {
        /// <summary>
        /// 账套编号
        /// </summary>
        /// <value>The company code.</value>
        public string CompanyCode { get; set; }

        /// <summary>
        /// 账套名称
        /// </summary>
        /// <value>The name of the company.</value>
        public string CompanyName { get; set; }

        /// <summary>
        /// 供应商发票返点
        /// </summary>
        /// <value>The provider tex ratio.</value>
        public decimal? ProviderTexRatio { get; set; }

        /// <summary>
        /// 客户发票返点(高开)
        /// </summary>
        /// <value>The client tax high ratio.</value>
        public decimal? ClientTaxHighRatio { get; set; }

        /// <summary>
        /// 客户发票返点(低开)
        /// </summary>
        /// <value>The client tax low ratio.</value>
        public decimal? ClientTaxLowRatio { get; set; }

        /// <summary>
        /// 是否启用平进平出
        /// </summary>
        /// <value><c>null</c> if [enable tax deduction] contains no value, <c>true</c> if [enable tax deduction]; otherwise, <c>false</c>.</value>
        public bool? EnableTaxDeduction { get; set; }

        /// <summary>
        /// 平进平出税率返点
        /// </summary>
        /// <value>The client tax deduction ratio.</value>
        public decimal? ClientTaxDeductionRatio { get; set; }

    }
}
