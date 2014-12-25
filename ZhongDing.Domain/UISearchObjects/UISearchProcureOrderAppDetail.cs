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
        /// 是否需要计算入库数量和未入库数量
        /// </summary>
        /// <value><c>true</c> if this instance is need cal in qty; otherwise, <c>false</c>.</value>
        public bool IsNeedCalInQty { get; set; }

        /// <summary>
        /// 需排除的IDs
        /// </summary>
        /// <value>The exclude I ds.</value>
        public IEnumerable<int> ExcludeIDs { get; set; }

    }
}
