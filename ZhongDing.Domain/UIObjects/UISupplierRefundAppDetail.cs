using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UISupplierRefundAppDetail : UIBase
    {
        public string SupplierName { get; set; }

        /// <summary>
        /// 采购日期
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// 采购订单编号
        /// </summary>
        /// <value>The order code.</value>
        public string OrderCode { get; set; }

        /// <summary>
        /// 货品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 货品规格
        /// </summary>
        public string Specification { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 采购数量
        /// </summary>
        public int ProcureCount { get; set; }

        /// <summary>
        /// 采购单价
        /// </summary>
        /// <value>The procure price.</value>
        public decimal ProcurePrice { get; set; }

        /// <summary>
        /// 采购金额
        /// </summary>
        /// <value>The total amount.</value>
        public decimal? TotalAmount { get; set; }

        /// <summary>
        /// 税率
        /// </summary>
        public decimal? SupplierTaxRatio { get; set; }

        /// <summary>
        /// 应返款
        /// </summary>
        public decimal? TotalNeedRefundAmount { get; set; }

    }
}
