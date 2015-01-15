using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIStockOutDetail : UIBase
    {
        public int StockOutID { get; set; }

        public int SalesOrderApplicationID { get; set; }

        public int SalesOrderAppDetailID { get; set; }

        public int ProductID { get; set; }

        public int ProductSpecificationID { get; set; }

        public int WarehouseID { get; set; }

        public string OrderCode { get; set; }

        public string Warehouse { get; set; }

        public string ProductName { get; set; }

        public string Specification { get; set; }

        public string UnitOfMeasurement { get; set; }

        public string FactoryName { get; set; }

        public decimal SalesPrice { get; set; }

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

        public int? NumberInLargePackage { get; set; }

        public decimal NumberOfPackages { get; set; }

        public string BatchNumber { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string LicenseNumber { get; set; }

        public bool IsDeleted { get; set; }
    }
}
