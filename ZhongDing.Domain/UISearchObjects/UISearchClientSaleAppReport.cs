using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchClientSaleAppReport : UISearchBase
    {
        public int? ClientUserId { get; set; }

        public int? ClientCompanyId { get; set; }

        public int? ProductId { get; set; }
    }
}
