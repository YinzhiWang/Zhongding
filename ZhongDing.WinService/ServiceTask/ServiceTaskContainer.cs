using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZhongDing.WinService.ServiceTask
{
    public class ServiceTaskContainer
    {
        public ServiceTaskContainer()
        {
        }
     
        protected List<ServiceTaskBase> ServiceTaskList
        {
            get
            {
                if (_WindowsServicesList == null)
                {
                    _WindowsServicesList = new List<ServiceTaskBase>();
                }
                return _WindowsServicesList;
            }
            set
            {
                _WindowsServicesList = value;
            }
        }
        private List<ServiceTaskBase> _WindowsServicesList;

        public event Action<ServiceTaskBase, Exception> OnException;
        public void AddServiceTask(ServiceTaskBase serviceTask)
        {
            if (IsExist(serviceTask))
            {
                throw new Exception("Windows Service Named " + serviceTask.ServiceTaskName + " is already exist.");
            }
            serviceTask.ServiceTaskContainer = this;
            serviceTask.OnException += serviceTask_OnException;
            this.ServiceTaskList.Add(serviceTask);
        }

        void serviceTask_OnException(ServiceTaskBase service, Exception ex)
        {
            if (this.OnException != null)
            {
                this.OnException(service, ex);
            }
        }

        private bool IsExist(ServiceTaskBase serviceTask)
        {
            bool result = false;
            foreach (var service in this.ServiceTaskList)
            {
                if (service.ServiceTaskName.ToLower() == serviceTask.ServiceTaskName.ToLower())
                {
                    return true;
                }
            }
            return result;
        }
        public void Start()
        {
            foreach (var service in this.ServiceTaskList)
            {
                service.Start();
            }
        }
        public void Stop()
        {
            foreach (var task in this.ServiceTaskList)
            {
                task.Stop();
            }
        }

        public bool EnableTrace
        {
            get
            {
                return _EnableTrace;
            }
            set
            {
                _EnableTrace = value;
            }
        }
        private bool _EnableTrace = false;


        public ServiceTaskBase GetServiceTask(string serviceTaskName)
        {
            return this.ServiceTaskList.First(x => x.ServiceTaskName == serviceTaskName);
        }
    }
}
