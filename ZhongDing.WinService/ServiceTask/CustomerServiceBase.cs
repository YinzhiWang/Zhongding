using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ZhongDing.WinService.ServiceTask
{
    public class CustomerServiceBase : System.ServiceProcess.ServiceBase
    {
        public CustomerServiceBase()
        {

            
          
        }

        private void CheckName()
        {
            if (this.MyServiceName.Contains("."))
                throw new Exception("not contains '.'");
            if (this.MyServiceDisplayName.Contains("."))
                throw new Exception("not contains '.'");
        }
        protected override void OnStart(string[] args)
        {
            Start();
        }

        public virtual void Start() { }

        protected override void OnStop()
        {
        }
        /// <summary>
        /// ServiceName is identity of a service ，please give a identity string.
        /// </summary>
        public virtual string MyServiceName { get; set; }
        /// <summary>
        /// ServiceDisplayName is name will show in Windows OS's Service Management .
        /// </summary>
        public virtual string MyServiceDisplayName { get; set; }
        /// <summary>
        /// ServiceDescription is a description for a service ,will show in Windows OS's Service Management .
        /// </summary>
        public virtual string MyServiceDescription { get; set; }

        public string MyServiceStatus
        {
            get
            {
                CheckName();
                bool? status = IsServiceInstalled(this.MyServiceName);
                if (status == null)
                    return "Unknow.";
                return status.Value ? "已安装" : "未安装";
            }
        }
        /// <summary>是否已安装</summary>
        public static Boolean? IsServiceInstalled(String name)
        {
            System.ServiceProcess.ServiceController control = null;
            try
            {
                // 取的时候就抛异常，是不知道是否安装的
                control = GetService(name);
                if (control == null) return false;
                try
                {
                    //尝试访问一下才知道是否已安装
                    Boolean b = control.CanShutdown;
                    return true;
                }
                catch { return false; }
            }
            catch { return null; }
            finally { if (control != null)control.Dispose(); }
        }
        public static ServiceController GetService(String name)
        {
            var list = new List<ServiceController>(ServiceController.GetServices());
            if (list == null || list.Count < 1) return null;

            //return list.Find(delegate(ServiceController item) { return item.ServiceName == name; });
            foreach (ServiceController item in list)
            {
                if (item.ServiceName == name) return item;
            }
            return null;
        }
    }
}
