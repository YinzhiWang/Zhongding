using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIClientAttachedInvoiceSettlementDetail : UIBase
    {
        public int ClientInvoiceDetailID { get; set; }
        public int StockOutDetailID { get; set; }
        public int ClientCompanyID { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceNumber { get; set; }
        /// <summary>
        /// 发票类型（高价，低价，平价）
        /// </summary>
        public int InvoiceTypeID { get; set; }
        /// <summary>
        /// 发票结算的税率
        /// </summary>
        public decimal? InvoiceSettlementRatio { get; set; }
        
        public string ProductName { get; set; }
        public string Specification { get; set; }

        public decimal SalesPrice { get; set; }
        public decimal InvoicePrice { get; set; }

        public decimal TotalInvoiceAmount { get; set; }

        /// <summary>
        /// 发票数量
        /// </summary>
        public int InvoiceQty { get; set; }
        /// <summary>
        /// 已结算数量
        /// </summary>
        public int SettledQty { get; set; }
        /// <summary>
        /// 未结算数量
        /// </summary>
        public int ToBeSettlementQty { get; set; }
        /// <summary>
        /// 本次结算数量
        /// </summary>
        public int SettlementQty { get; set; }

        /// <summary>
        /// 本次结算金额
        /// </summary>
        public decimal? SettlementAmount { get; set; }

        /// <summary>
        /// 销售金额
        /// </summary>
        //public decimal? SalesAmount { get; set; }

        /// <summary>
        /// 是否被选中
        /// </summary>
        /// <value><c>true</c> if this instance is checked; otherwise, <c>false</c>.</value>
        public bool IsChecked { get; set; }
    }
}
