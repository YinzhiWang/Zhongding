using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UISalesOrderAppDetail : UIBase
    {
        public int SalesOrderApplicationID { get; set; }

        public int ProductID { get; set; }

        public int ProductSpecificationID { get; set; }

        public int WarehouseID { get; set; }

        public string OrderCode { get; set; }

        public string Warehouse { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string Specification { get; set; }

        public string UnitOfMeasurement { get; set; }

        public string FactoryName { get; set; }

        public decimal SalesPrice { get; set; }

        public decimal? InvoicePrice { get; set; }

        /// <summary>
        /// 销售数量（需发货数量）
        /// </summary>
        /// <value>The sales qty.</value>
        public int SalesQty { get; set; }

        /// <summary>
        /// 已出仓数量（或出库数量）
        /// </summary>
        public int OutQty { get; set; }

        /// <summary>
        /// 未出仓数量
        /// </summary>
        public int ToBeOutQty { get; set; }

        /// <summary>
        /// 库存余量
        /// </summary>
        public int BalanceQty { get; set; }

        /// <summary>
        /// 货款
        /// </summary>
        public decimal TotalSalesAmount { get; set; }

        public decimal NumberOfPackages { get; set; }

        public string BatchNumber { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string LicenseNumber { get; set; }

        public int? NumberInLargePackage { get; set; }

        public int? GiftCount { get; set; }
    }
}
