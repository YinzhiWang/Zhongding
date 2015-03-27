using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Sms
{
    public abstract class BaseSmsSenderParameter : IBaseSmsSenderParameter
    {
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
    }
}
