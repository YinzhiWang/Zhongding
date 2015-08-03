
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using ZhongDing.Common;
using ZhongDing.Common.Enums;
using ZhongDing.WinService.ServiceTask;

namespace ZhongDing.WinService.ServiceTask
{
    /// <summary>
    /// 任务 基类
    /// </summary>
    public abstract class ServiceTaskBase
    {
        private static bool? _IsConsole;
        /// <summary>
        /// 是否是 控制台环境
        /// </summary>
        public static bool IsConsole
        {
            get
            {
                if (!_IsConsole.HasValue)
                {
                    _IsConsole = RuntimeHelper.IsConsole;
                }
                return _IsConsole.Value;
            }
        }
        /// <summary>
        /// 错误异常 统一处理 事件
        /// </summary>
        public event Action<ServiceTaskBase, Exception> OnException;
        /// <summary>
        /// 任务 容器 统一管理所有Task
        /// </summary>
        public ServiceTaskContainer ServiceTaskContainer;
        public DateTime StartDate { get; set; }
        public DateTime FirstStartDate { get; set; }
        public DateTime StopDate { get; set; }
        public int ElapsedTimes { get; set; }
        public int ElapsedHitTimes { get; set; }
        private System.Timers.Timer timer;
        private System.Threading.Thread thread;
        public ServiceTaskParameter ServiceTaskParameter;
        public ServiceTaskBase()
        {
            Init();
        }
        public ServiceTaskBase(ServiceTaskParameter serviceTaskParameter)
        {
            ServiceTaskParameter = serviceTaskParameter;
            Init();
        }
        /// <summary>
        /// 初始化 Task
        /// </summary>
        private void Init()
        {
            this.ValidateParameters();
            if (this.ServiceTaskParameter.ThreadType == EThreadType.Timer)
            {
                timer = new System.Timers.Timer();
                timer.Interval = ServiceTaskParameter.InitInterval;
                timer.Start();
                timer.Elapsed += timer_Elapsed;
            }
            else if (this.ServiceTaskParameter.ThreadType == EThreadType.Thread)
            {
                thread = new Thread(new ParameterizedThreadStart(thread_Elapsed)) { Name = "" };
                thread.Start();
            }
        }
        /// <summary>
        /// 验证 参数
        /// </summary>
        private void ValidateParameters()
        {
            if (string.IsNullOrEmpty(this.ServiceTaskParameter.ServiceTaskName))
            {
                throw new Exception("ServicesParameter.ServiceName is not null.");
            }
        }
        /// <summary>
        /// 启动Task
        /// </summary>
        public void Start()
        {
            this.timer.Start();
            this.FirstStartDate = DateTime.Now;
            this.OnStart();
        }
        /// <summary>
        /// 停止Task
        /// </summary>
        public void Stop()
        {
            this.timer.Stop();
            this.StopDate = DateTime.Now;
            this.OnStop();
        }
        void thread_Elapsed(object sender)
        {

        }
        /// <summary>
        /// 定时器 经过的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            if (ServiceTaskContainer.EnableTrace)
            {
                this.ElapsedTimes++;
            }
            this.timer.Stop();
            StringBuilder sb = new StringBuilder();
            sb.Append(ServiceTaskName + " timer_Elapsed ");
            Stopwatch stopwatch = new Stopwatch();
            try
            {
                stopwatch.Start();
                this.OnLoadEveryTimeStart();
                if (ServiceTaskParameter.IsFirstRun)
                {

                    DateTime dtNextRunTime = CaculateNextRunTime(ServiceTaskParameter.ServiceStartTime, ServiceTaskParameter.ProcessInterval, ServiceTaskParameter.DateIntervalType);
                    if (dtNextRunTime <= DateTime.Now)
                    {
                        sb.Append(" 首次运行");
                        ServiceTaskParameter.IsFirstRun = false;
                        if (this.timer.Interval == ServiceTaskParameter.InitInterval)
                        {
                            this.timer.Interval = ServiceTaskParameter.ProcessInterval * DateAndTime.ConvertDateInterval(this.ServiceTaskParameter.DateIntervalType);
                        }
                        Utility.WriteTrace("Start " + this.ServiceTaskParameter.ServiceTaskName + " at：" + DateTime.Now);
                        this.OnloadFirst();
                        Utility.WriteTrace("End " + this.ServiceTaskParameter.ServiceTaskName + " at：" + DateTime.Now);
                        sb.Append(" 下次运行时间：" + DateTime.Now.AddMilliseconds(this.timer.Interval).ToString());
                        this.OnEnd();
                    }
                    else
                    {
                        sb.Append(" 探测中 下次运行时间：" + dtNextRunTime.ToString());
                    }
                }
                else
                {
                    sb.Append(" 二次运行");
                    Utility.WriteTrace("Start " + this.ServiceTaskParameter.ServiceTaskName + " at：" + DateTime.Now);
                    this.OnloadSecond();
                    Utility.WriteTrace("End " + this.ServiceTaskParameter.ServiceTaskName + " at：" + DateTime.Now);
                    sb.Append(" 下次运行时间：" + DateTime.Now.AddMilliseconds(this.timer.Interval).ToString());
                    this.OnEnd();
                }
                this.OnLoadEveryTimeEnd();
            }
            catch (Exception ex)
            {
                this.OnException(this, ex);
                LogConsole(ex.ToString());
            }
            finally
            {
                stopwatch.Stop();
                sb.Append(" 耗时：");
                sb.Append(stopwatch.ElapsedMilliseconds + "Ms");
                LogConsole(sb.ToString());
                this.timer.Start();
            }
        }
        /// <summary>
        /// 输出Debug信息
        /// </summary>
        /// <param name="log"></param>
        public static void LogConsole(string log)
        {
            if (IsConsole)
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss fff") + " " + log);
            }
            Debug.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss fff") + " " + log);
            //LogHelper.WriteLog(new LogBaseEntity() { ActionDetail = "LogConsole", Message = log });
        }
        public virtual void OnStart()
        { }
        public virtual void OnLoadEveryTimeStart()
        { }
        public virtual void OnLoadEveryTimeEnd()
        { }
        /// <summary>
        /// Timer 自身的首次 任务执行
        /// </summary>
        public virtual void OnloadFirst()
        {
        }
        /// <summary>
        /// Timer 自身的 第二次以及 以后的任务运行
        /// </summary>
        public virtual void OnloadSecond()
        { }
        public virtual void OnStop()
        { }
        public virtual void OnEnd()
        {
            GC.Collect();
        }
        public virtual void OnError(Exception ex)
        { }
        /// <summary>
        /// 任务名称
        /// </summary>
        public string ServiceTaskName
        {
            get
            {
                return this.ServiceTaskParameter.ServiceTaskName;
            }
        }
        /// <summary>
        /// 时间差计算
        /// </summary>
        /// <param name="startTime"></param>
        /// <param name="interval"></param>
        /// <param name="dateInterval"></param>
        /// <returns></returns>
        public virtual DateTime CaculateNextRunTime(DateTime startTime, int interval, DateInterval dateInterval = DateInterval.Hour)
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
    }
}
