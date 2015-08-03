using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.WinService.Lib;
using ZhongDing.WinService.ServiceTask;

namespace ZhongDing.WinService.CustomerTask
{
    public class CashFlowReportGenerateTask : ServiceTaskBase
    {
        public CashFlowReportGenerateTask(ServiceTaskParameter serviceTaskParameter)
            : base(serviceTaskParameter)
        {

        }
        public override void OnloadFirst()
        {
            CashFlowReportGenerateService.ProcessWork();
        }
        public override void OnloadSecond()
        {
            CashFlowReportGenerateService.ProcessWork();
        }
    }
}
