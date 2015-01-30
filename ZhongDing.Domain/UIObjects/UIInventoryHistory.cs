﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIInventoryHistory : UIBase
    {
        public int WarehouseID { get; set; }
        public int ProductID { get; set; }
        public int ProductSpecificationID { get; set; }
        public string LicenseNumber { get; set; }
        public string BatchNumber { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
        public decimal ProcurePrice { get; set; }
        public int InQty { get; set; }
        public int OutQty { get; set; }
        public int BalanceQty { get; set; }
        public DateTime StatDate { get; set; }
    }
}