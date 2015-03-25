using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDCFlowDataDetail : UIBase
    {
        public string ContractCode { get; set; }

        public string ClientUserName { get; set; }

        public string InChargeUserFullName { get; set; }

        public string HospitalName { get; set; }

        public string UnitName { get; set; }

        public int SaleQty { get; set; }

        public bool? IsTempContract { get; set; }

        public string Comment { get; set; }
    }
}
