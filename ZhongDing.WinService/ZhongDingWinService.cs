using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using ZhongDing.Common;
using ZhongDing.Common.Enums;

namespace ZhongDing.WinService
{
    public partial class ZhongDingWinService : ServiceBase
    {
        #region Const

        private const string WIN_SERVICE_NAME = "ZhongDing MIS Service";
        private const string WIN_SERVICE_CLASS_NAME = "ZhongDingWinService";

        #endregion

        /// <summary>
        /// The init interval
        /// </summary>
        private int initInterval;

        /// <summary>
        /// The is first run process calculate inventory
        /// </summary>
        private bool isFirstRunProcessCalculateInventory = true;

        /// <summary>
        /// The is run process calculate inventory
        /// </summary>
        private bool isRunProcessCalculateInventory = false;


        public ZhongDingWinService()
        {
            //Initialize Logging Logger
            IConfigurationSource configurationSource = ConfigurationSourceFactory.Create();
            LogWriterFactory logWriterFactory = new LogWriterFactory(configurationSource);
            Microsoft.Practices.EnterpriseLibrary.Logging.Logger.SetLogWriter(logWriterFactory.Create());

            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            initInterval = 60000;

            Utility.WriteTrace("Execute " + WIN_SERVICE_CLASS_NAME + "：OnStart.");

            this.tmCalculateInventory.Enabled = true;
            this.tmCalculateInventory.Interval = initInterval;
            this.tmCalculateInventory.Start();
            Utility.WriteTrace(WIN_SERVICE_NAME + ": tmCalculateInventory.Start.");

        }

        protected override void OnStop()
        {
            Utility.WriteTrace("Execute " + WIN_SERVICE_CLASS_NAME + "：OnStop.");

            this.tmCalculateInventory.Stop();
            this.tmCalculateInventory.Enabled = false;
            this.tmCalculateInventory.Dispose();

            Utility.WriteTrace(WIN_SERVICE_NAME + ": tmCalculateInventory.Stop.");
        }

        /// <summary>
        /// 计算每日库存服务
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        private void tmCalculateInventory_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.tmCalculateInventory.Stop();

            Utility.WriteTrace("Start tmCalculateInventory_Elapsed at：" + DateTime.Now);

            try
            {
                if (isFirstRunProcessCalculateInventory)
                {
                    DateTime dtNextRunTime = CaculateNextRunTime(WebConfig.CalculateInventoryServiceStartTime, WebConfig.CalculateInventoryServiceInterval, DateInterval.Hour);
                    if (dtNextRunTime <= DateTime.Now)
                    {
                        isFirstRunProcessCalculateInventory = false;

                        if (tmCalculateInventory.Interval == initInterval)
                            tmCalculateInventory.Interval = WebConfig.CalculateInventoryServiceInterval * 1000 * 60 * 60;

                        if (isRunProcessCalculateInventory == false)
                        {
                            isRunProcessCalculateInventory = true;

                            CalculateInventoryService.ProcessWork();

                            Utility.WriteTrace(WIN_SERVICE_NAME + ": tmCalculateInventory_Elapsed Processing has finished");

                            isRunProcessCalculateInventory = false;
                        }
                    }
                }
                else
                {
                    if (isRunProcessCalculateInventory == false)
                    {
                        isRunProcessCalculateInventory = true;

                        CalculateInventoryService.ProcessWork();

                        Utility.WriteTrace(WIN_SERVICE_NAME + ": tmCalculateInventory_Elapsed Processing has finished");

                        isRunProcessCalculateInventory = false;
                    }
                }
            }
            catch (Exception ex)
            {
                isRunProcessCalculateInventory = false;

                Utility.WriteTrace(WIN_SERVICE_NAME + ": tmCalculateInventory_Elapsed Error:" + ex.Message);

                Utility.WriteExceptionLog(ex);
            }
            finally
            {

                tmCalculateInventory.Start();

                Utility.WriteTrace("End tmCalculateInventory_Elapsed at：" + DateTime.Now);
            }
        }


        #region Private Methods

        /// <summary>
        /// Caculates the next run time.
        /// </summary>
        /// <param name="startTime">The start time.</param>
        /// <param name="interval">The interval.</param>
        /// <param name="dateInterval">The date interval.</param>
        /// <returns>DateTime.</returns>
        private DateTime CaculateNextRunTime(DateTime startTime, int interval, DateInterval dateInterval = DateInterval.Hour)
        {
            System.DateTime dtStart = startTime;
            if (dtStart >= DateTime.Now)
            {
                return dtStart.AddMinutes(-1);
            }
            else
            {
                double dDiff = 0;

                dDiff = DateAndTime.DateDiff(dateInterval, dtStart, DateTime.Now.AddMinutes(-1)) / interval;

                System.DateTime sNext;

                switch (dateInterval)
                {
                    case DateInterval.Day:
                        sNext = dtStart.AddDays((dDiff + 1) * interval);
                        break;
                    case DateInterval.Hour:
                        sNext = dtStart.AddHours((dDiff + 1) * interval);
                        break;
                    case DateInterval.Minute:
                        sNext = dtStart.AddMinutes((dDiff + 1) * interval);
                        break;
                    case DateInterval.Month:
                        sNext = dtStart.AddMonths(Convert.ToInt32((dDiff + 1)) * interval);
                        break;
                    case DateInterval.Second:
                        sNext = dtStart.AddSeconds((dDiff + 1) * interval);
                        break;
                    case DateInterval.Year:
                        sNext = dtStart.AddYears(Convert.ToInt32((dDiff + 1)) * interval);
                        break;
                    default:
                        sNext = dtStart.AddHours((dDiff + 1) * interval);
                        break;
                }

                return sNext;
            }
        }


        #endregion
    }
}
