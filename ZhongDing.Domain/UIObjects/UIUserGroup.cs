using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIUserGroup : UIBase
    {
        public string GroupName { get; set; }

        public string UserNames { get; set; }

        public IEnumerable<int> UserIDs { get; set; }
    }
}
