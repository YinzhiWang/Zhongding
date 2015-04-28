﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIProcurePlanReport
    {
        public int WarehouseID { get; set; }
        public string WarehouseName { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int ProductSpecificationID { get; set; }
        public string Specification { get; set; }
        public string UnitName { get; set; }
        public int NumberInLargePackage { get; set; }
        public int TotalToBeOutQty { get; set; }

        public int ToBeOutNumberOfPackages { get; set; }

        public int WarehouseQty { get; set; }
        public int WarehouseNumberOfPackages { get; set; }
    }
}