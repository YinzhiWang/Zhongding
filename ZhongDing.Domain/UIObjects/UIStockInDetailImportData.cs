using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Domain.Models;

namespace ZhongDing.Domain.UIObjects
{
    public class UIStockInDetailImportData : UIBase
    {
        public int StockInImportFileLogID { get; set; }
        public string Code { get; set; }
        public Nullable<System.DateTime> EntryDate { get; set; }
        public string SupplierName { get; set; }
        public int StockInImportDataID { get; set; }
        public int ProcureOrderAppID { get; set; }
        public int ProcureOrderAppDetailID { get; set; }
        public string ProductName { get; set; }
        public string ProductSpecification { get; set; }
        public string WarehouseName { get; set; }
        public decimal ProcurePrice { get; set; }
        public int InQty { get; set; }
        public string BatchNumber { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public string LicenseNumber { get; set; }
        public Nullable<bool> IsMortgagedProduct { get; set; }

    }
}
