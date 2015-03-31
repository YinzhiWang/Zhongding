using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchProcureOrderReport : UISearchBase
    {
        public int? ProductId { get; set; }
        public int? SupplierId { get; set; }
    }
}
