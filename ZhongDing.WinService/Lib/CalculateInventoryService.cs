using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Common;

namespace ZhongDing.WinService
{
    public class CalculateInventoryService
    {
        /// <summary>
        /// Processes the work.
        /// </summary>
        public static void ProcessWork()
        {
            Utility.WriteTrace("Process Calculate Inventory Work begin at:" + DateTime.Now);

            System.Threading.Thread.Sleep(60000);

            Utility.WriteTrace("Process Calculate Inventory Work end at:" + DateTime.Now);
        }
    }
}
