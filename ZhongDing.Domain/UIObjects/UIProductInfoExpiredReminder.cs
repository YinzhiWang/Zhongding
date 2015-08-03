using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIProductInfoExpiredReminder : UIBase
    {
        public int ProductID { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public string CertificateType { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public int? AlertBeforeDays { get; set; }
    }
}
