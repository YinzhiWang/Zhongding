using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.WinService.Lib;
using ZhongDing.WinService.ServiceTask;

namespace ZhongDing.WinService.CustomerTask
{
    public class CalculateBankAccountBalanceTask : ServiceTaskBase
    {
        public CalculateBankAccountBalanceTask(ServiceTaskParameter serviceTaskParameter)
            : base(serviceTaskParameter)
        {

        }
        public override void OnloadFirst()
        {
            CalculateBankAccountBalanceService.ProcessWork();
        }
        public override void OnloadSecond()
        {
            CalculateBankAccountBalanceService.ProcessWork();
        }
    }
}
