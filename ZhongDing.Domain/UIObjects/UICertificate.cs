using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UICertificate : UIBase
    {

        /// <summary>
        /// 所有者实体ID
        /// </summary>
        public int OwnerEntityID { get; set; }
        public int CertificateID { get; set; }
        public Nullable<int> CertificateTypeID { get; set; }
        public Nullable<int> OwnerTypeID { get; set; }
        public string CertificateTypeName { get; set; }
        public Nullable<bool> IsGotten { get; set; }
        public string GottenDescription { get; set; }
        public Nullable<DateTime> EffectiveFrom { get; set; }
        public Nullable<DateTime> EffectiveTo { get; set; }
        public string EffectiveDateDescription { get; set; }
        public Nullable<bool> IsNeedAlert { get; set; }
        public Nullable<int> AlertBeforeDays { get; set; }
    }
}
