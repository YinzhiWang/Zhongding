using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.Common.Sms
{
    public interface IBaseSmsSenderParameter
    {
        string PhoneNumber { get; set; }
        string Code { get; set; }
    }
}
