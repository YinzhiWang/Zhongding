using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Domain.UIObjects
{
    public class UICashFlowReport : UIBase
    {
        public System.DateTime CashFlowDate { get; set; }
        public string CashFlowFileName { get; set; }
        public string FilePath { get; set; }
    }
}
