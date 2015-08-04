using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIHospitalCode : UIBase
    {
        public int HospitalID { get; set; }
        public string Code { get; set; }
        public string Names { get; set; }

        public IEnumerable<string> NameList { get; set; }
    }
}
