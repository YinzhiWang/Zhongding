using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchHospital : UISearchBase
    {
        public int DBContractID { get; set; }

        public string HospitalName { get; set; }
    }
}
