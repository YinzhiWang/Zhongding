using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UICertificateType
    {
        public int ID { get; set; }
        public string CertificateTypeName { get; set; }
        public Nullable<int> OwnerTypeID { get; set; }
    }
}
