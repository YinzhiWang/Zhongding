using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDBContractHospital : UIHospital
    {
        public int DBContractID { get; set; }

        public int HospitalCodeID { get; set; }
    }
}
