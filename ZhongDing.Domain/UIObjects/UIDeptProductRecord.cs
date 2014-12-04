using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UIDeptProductRecord : UIBase
    {
        public int Year { get; set; }

        public int? Task { get; set; }

        public int? Actual { get; set; }
    }
}
