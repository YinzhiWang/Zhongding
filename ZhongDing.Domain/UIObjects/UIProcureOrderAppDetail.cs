using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIProcureOrderAppDetail : UIBase
    {
        public int ProcureOrderAppID { get; set; }

        public int ProductID { get; set; }

        public int ProductSpecificationID { get; set; }

        public int WarehouseID { get; set; }

        public string OrderCode { get; set; }

        public string Warehouse { get; set; }

        public string ProductName { get; set; }

        public string Specification { get; set; }

        public string FactoryName { get; set; }

        public string UnitOfMeasurement { get; set; }

        public int ProcureCount { get; set; }

        /// <summary>
        /// 已入仓的数量
        /// </summary>
        public int InQty { get; set; }

        /// <summary>
        /// 未入仓的数量
        /// </summary>
        public int ToBeInQty { get; set; }

        public decimal NumberOfPackages { get; set; }

        public decimal ProcurePrice { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal? TaxAmount { get; set; }

        public string LicenseNumber { get; set; }

        public string ProcureOrderApplicationOrderCode { get; set; }

        public DateTime ProcureOrderApplicationOrderDate { get; set; }

        public decimal NotTaxAmount { get; set; }


        public decimal SupplierInvoiceDetailTotalAmount { get; set; }
    }
}
