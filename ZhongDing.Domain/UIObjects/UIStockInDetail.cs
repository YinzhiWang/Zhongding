using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIStockInDetail : UIBase
    {
        public int StockInID { get; set; }

        public int ProcureOrderAppID { get; set; }

        public int ProcureOrderAppDetailID { get; set; }

        public int ProductID { get; set; }

        public int ProductSpecificationID { get; set; }

        public int WarehouseID { get; set; }

        public string OrderCode { get; set; }

        public string Warehouse { get; set; }

        public string ProductName { get; set; }

        public string Specification { get; set; }

        public string UnitOfMeasurement { get; set; }

        public string FactoryName { get; set; }

        public decimal ProcurePrice { get; set; }

        /// <summary>
        /// 采购数量
        /// </summary>
        public int ProcureCount { get; set; }

        /// <summary>
        /// 已入仓的数量
        /// </summary>
        public int InQty { get; set; }

        /// <summary>
        /// 未入仓的数量
        /// </summary>
        public int ToBeInQty { get; set; }

        public int? NumberInLargePackage { get; set; }

        public decimal NumberOfPackages { get; set; }

        public string BatchNumber { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public string LicenseNumber { get; set; }

        public bool? IsMortgagedProduct { get; set; }

        /// <summary>
        /// 剩余库存
        /// </summary>
        /// <value>The balance qty.</value>
        public int BalanceQty { get; set; }

        public bool IsDeleted { get; set; }

    }
}
