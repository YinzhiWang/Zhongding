using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchStockOutDetailReport : UISearchBase
    {
        public int? ProductId { get; set; }

        public int? ClientUserId { get; set; }

        public int? ClientCompanyId { get; set; }
    }
}
