using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UISupplierInfoReminder : UIBase
    {
        public int SupplierID { get; set; }

        public string SupplierCode { get; set; }

        public string SupplierName { get; set; }

        public string CertificateType { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public int? AlertBeforeDays { get; set; }
    }
}
