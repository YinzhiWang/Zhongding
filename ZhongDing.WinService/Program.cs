using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.WinService.ServiceTask;

namespace ZhongDing.WinService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            if (ZhongDing.Common.RuntimeHelper.IsConsole)
            {
                serviceManager = new ServiceManager<ZhongDingWinService>();
                serviceManager.Run();
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { 
                new ZhongDingWinService() 
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
        private static ZhongDing.WinService.ServiceTask.ServiceManager<ZhongDingWinService> serviceManager;
    }
}
