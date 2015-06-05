using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZhongDing.Common.Enums;

namespace ZhongDing.WinService.ServiceTask
{
    public class ServiceTaskParameter
    {
        private int initInterval = 60 * 1000;//1 min
        public int InitInterval
        {
            get
            {
                return initInterval;
            }
            set
            {
                initInterval = value;
            }
        }
        private bool isFirstRun = true;
        public bool IsFirstRun
        {
            get
            {
                return isFirstRun;
            }
            set
            {
                isFirstRun = value;
            }
        }
        private DateInterval dateIntervalType = DateInterval.Minute;//default min
        public DateInterval DateIntervalType
        {
            get
            {
                return dateIntervalType;
            }
            set
            {
                dateIntervalType = value;
            }
        }
        private int processInterval = 60000;//1 Min
        public int ProcessInterval
        {
            get
            {
                return processInterval;
            }
            set
            {
                processInterval = value;
            }
        }
        private DateTime serviceStartTime = new DateTime();
        public DateTime ServiceStartTime
        {
            get
            {
                return serviceStartTime;
            }
            set
            {
                serviceStartTime = value;
            }
        }
        private string serviceTaskName;
        public string ServiceTaskName
        {
            get
            {
                return serviceTaskName;
            }
            set
            {
                serviceTaskName = value;
            }
        }


        private EThreadType _ThreadType = EThreadType.Timer;

        public EThreadType ThreadType
        {
            get { return _ThreadType; }
            set { _ThreadType = value; }
        }



    }

    public enum EThreadType
    {
        Timer,
        Thread
    }
}
