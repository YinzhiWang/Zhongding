using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UISupplierRefundApp : UIBase
    {
        public int CompanyID { get; set; }

        public int SupplierID { get; set; }

        public int ProductID { get; set; }

        public int ProductSpecificationID { get; set; }

        public string CompanyName { get; set; }

        public string SupplierName { get; set; }

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
        /// 总采购数量
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// 总采购金额
        /// </summary>
        /// <value>The total amount.</value>
        public decimal? TotalAmount { get; set; }

        /// <summary>
        /// 应返款
        /// </summary>
        public decimal? TotalNeedRefundAmount { get; set; }

        /// <summary>
        /// 已返款
        /// </summary>
        public decimal? TotalRefundedAmount { get; set; }

        /// <summary>
        /// 未返款
        /// </summary>
        public decimal? TotalToBeRefundAmount { get; set; }

        /// <summary>
        /// 收款时间
        /// </summary>
        public DateTime? RefundDate { get; set; }

        /// <summary>
        /// 收款类型
        /// </summary>
        public string PaymentMethod { get; set; }

        /// <summary>
        /// 起始时间
        /// </summary>
        public DateTime? BeginDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 收款金额
        /// </summary>
        public decimal RefundAmount { get; set; }
    }
}
