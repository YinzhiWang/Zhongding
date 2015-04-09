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
using ZhongDing.WinService.Lib;

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

        private bool isFirstRunProcessImportDCFlowData = true;
        private bool isRunProcessImportDCFlowData = false;

        private bool isFirstRunProcessSettleDBClientBonus = true;
        private bool isRunProcessSettleDBClientBonus = false;


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

            this.tmImportDCFlowData.Enabled = true;
            this.tmImportDCFlowData.Interval = initInterval;
            this.tmImportDCFlowData.Start();
            Utility.WriteTrace(WIN_SERVICE_NAME + ": tmImportDCFlowData.Start.");

        }

        protected override void OnStop()
        {
            Utility.WriteTrace("Execute " + WIN_SERVICE_CLASS_NAME + "：OnStop.");

            this.tmCalculateInventory.Stop();
            this.tmCalculateInventory.Enabled = false;
            this.tmCalculateInventory.Dispose();

            Utility.WriteTrace(WIN_SERVICE_NAME + ": tmCalculateInventory.Stop.");
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

        #region Events

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

        private void tmImportDCFlowData_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.tmImportDCFlowData.Stop();

            Utility.WriteTrace("Start tmImportDCFlowData_Elapsed at：" + DateTime.Now);

            try
            {
                if (isFirstRunProcessImportDCFlowData)
                {
                    DateTime dtNextRunTime = CaculateNextRunTime(WebConfig.ImportDataServiceStartTime, WebConfig.ImportDataServiceInterval, DateInterval.Minute);
                    if (dtNextRunTime <= DateTime.Now)
                    {
                        isFirstRunProcessImportDCFlowData = false;

                        if (tmImportDCFlowData.Interval == initInterval)
                            tmImportDCFlowData.Interval = WebConfig.ImportDataServiceInterval * 1000 * 60 * 60;

                        if (isRunProcessImportDCFlowData == false)
                        {
                            isRunProcessImportDCFlowData = true;

                            ImportDataService.ProcessWork();

                            Utility.WriteTrace(WIN_SERVICE_NAME + ": tmImportDCFlowData_Elapsed Processing has finished");

                            isRunProcessImportDCFlowData = false;
                        }
                    }
                }
                else
                {
                    if (isRunProcessImportDCFlowData == false)
                    {
                        isRunProcessImportDCFlowData = true;

                        ImportDataService.ProcessWork();

                        Utility.WriteTrace(WIN_SERVICE_NAME + ": tmImportDCFlowData_Elapsed Processing has finished");

                        isRunProcessImportDCFlowData = false;
                    }
                }
            }
            catch (Exception ex)
            {
                isRunProcessImportDCFlowData = false;

                Utility.WriteTrace(WIN_SERVICE_NAME + ": tmImportDCFlowData_Elapsed Error:" + ex.Message);

                Utility.WriteExceptionLog(ex);
            }
            finally
            {

                tmImportDCFlowData.Start();

                Utility.WriteTrace("End tmImportDCFlowData_Elapsed at：" + DateTime.Now);
            }
        }

        private void tmSettleDBClientBonus_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.tmSettleDBClientBonus.Stop();

            Utility.WriteTrace("Start tmSettleDBClientBonus_Elapsed at：" + DateTime.Now);

            try
            {
                if (isFirstRunProcessSettleDBClientBonus)
                {
                    DateTime dtNextRunTime = CaculateNextRunTime(WebConfig.SettleDBClientBonusServiceStartTime, WebConfig.SettleDBClientBonusServiceInterval, DateInterval.Month);
                    if (dtNextRunTime <= DateTime.Now)
                    {
                        isFirstRunProcessSettleDBClientBonus = false;

                        if (tmSettleDBClientBonus.Interval == initInterval)
                            tmSettleDBClientBonus.Interval = WebConfig.SettleDBClientBonusServiceInterval * 1000 * 60 * 60;

                        if (isRunProcessSettleDBClientBonus == false)
                        {
                            isRunProcessSettleDBClientBonus = true;

                            DBClientSettleBonusService.ProcessWork();

                            Utility.WriteTrace(WIN_SERVICE_NAME + ": tmSettleDBClientBonus_Elapsed Processing has finished");

                            isRunProcessSettleDBClientBonus = false;
                        }
                    }
                }
                else
                {
                    if (isRunProcessSettleDBClientBonus == false)
                    {
                        isRunProcessSettleDBClientBonus = true;

                        DBClientSettleBonusService.ProcessWork();

                        Utility.WriteTrace(WIN_SERVICE_NAME + ": tmSettleDBClientBonus_Elapsed Processing has finished");

                        isRunProcessSettleDBClientBonus = false;
                    }
                }
            }
            catch (Exception ex)
            {
                isRunProcessSettleDBClientBonus = false;

                Utility.WriteTrace(WIN_SERVICE_NAME + ": tmSettleDBClientBonus_Elapsed Error:" + ex.Message);

                Utility.WriteExceptionLog(ex);
            }
            finally
            {

                tmSettleDBClientBonus.Start();

                Utility.WriteTrace("End tmSettleDBClientBonus_Elapsed at：" + DateTime.Now);
            }
        }

        #endregion

        

    }
}
