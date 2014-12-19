using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchProcureOrderAppDetail : UISearchBase
    {
        public int ProcureOrderApplicationID { get; set; }

        public int WarehouseID { get; set; }
    }
}
