using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZhongDing.Common.Sms
{
    public interface ISmsSender
    {
        SmsSenderResult Send(IBaseSmsSenderParameter para);
    }
}
