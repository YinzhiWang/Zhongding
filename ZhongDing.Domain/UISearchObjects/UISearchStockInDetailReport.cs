using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchStockInDetailReport : UISearchBase
    {
        public int? SupplierId { get; set; }

        public int? ProductId { get; set; }

        public string BatchNumber { get; set; }
    }
}
