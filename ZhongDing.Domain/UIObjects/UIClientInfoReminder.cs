using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIClientInfoReminder : UIBase
    {
        public int ClientInfoID { get; set; }

        public string ClientCode { get; set; }

        public string ClientName { get; set; }

        public string CertificateType { get; set; }

        public DateTime? EffectiveTo { get; set; }

        public int? AlertBeforeDays { get; set; }

        public string ClientCompanyName { get; set; }

        public int ClientCompanyID { get; set; }
    }
}
