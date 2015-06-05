using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UISearchObjects
{
    public class UISearchReimbursementType : UISearchBase
    {
        public string Name { get; set; }

        public int? ParentID { get; set; }

        public bool OnlyParent { get; set; }
    }
}
