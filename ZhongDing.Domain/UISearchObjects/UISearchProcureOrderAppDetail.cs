using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchProcureOrderAppDetail : UISearchBase
    {
        public int ProcureOrderAppID { get; set; }

        public int WarehouseID { get; set; }

        public int SupplierID { get; set; }

        /// <summary>
        /// 需排除的IDs
        /// </summary>
        public IEnumerable<int> ExcludeIDs { get; set; }

    }
}
