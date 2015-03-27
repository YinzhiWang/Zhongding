using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Sms
{

    public class SmsSenderResult
    {
        public bool Result { get; set; }
        public string Info { get; set; }
        public string Code { get; set; }

        public string ErrorInfo { get; set; }
    }
}
