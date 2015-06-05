using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using ZhongDing.Common;

namespace ZhongDing.WinService.ServiceTask
{

    public class ServiceManager<T> where T : CustomerServiceBase, new()
    {
        protected string ServiceName
        {
            get { return t.MyServiceName; }
        }
        protected string DisplayName
        {
            get { return t.MyServiceDisplayName; }
        }
        protected string ServiceDescription
        {
            get { return t.MyServiceDescription; }
        }
        private T t = null;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        public virtual void Run()
        {
            if (RuntimeHelper.IsConsole)
            {
                Console.SetWindowSize(180, 50);
                Console.SetWindowPosition(0, 0);
                t = new T();
            lbl:
                ShowStatus();
                WriteLine("按 1 创建 服务.");
                WriteLine("按 2 启动 服务.");
                WriteLine("按 3 停止 服务.");
                WriteLine("按 4 删除 服务.");
                WriteLine("按 5 测试 服务（相当于控制台下即可模拟Service真实运行效果）.");
                WriteLine("按 其他任意键退出.");

                string cmd = Console.ReadLine();
                int iCmd = 0;
                int.TryParse(cmd, out iCmd);
                if (iCmd == 1)
                {
                    CreateService();
                    goto lbl;
                }
                else if (iCmd == 2)
                {
                    StartService();
                    goto lbl;
                }
                else if (iCmd == 3)
                {
                    StopService();
                    goto lbl;
                }
                else if (iCmd == 4)
                {
                    DeleteService();
                    goto lbl;
                }
                else if (iCmd == 5)
                {
                    t.Start();
                    WriteLine("启动中...");
                    Console.ReadLine();
                }
            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[] { t };
                ServiceBase.Run(ServicesToRun);
            }
        }

        protected virtual void StartService()
        {
            RunCmd("net start " + ServiceName, false, true);
        }

        protected virtual void DeleteService()
        {
            StopService();
            RunSC("Delete " + ServiceName);
        }

        protected virtual void StopService()
        {
            RunCmd("net stop " + ServiceName, false, true);
        }

        protected virtual void CreateService()
        {
            RunSC("create " + ServiceName + " BinPath= \"" + Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ExeName) + " -s\" start= auto DisplayName= \"" + DisplayName + "\"");
            RunSC("description " + ServiceName + " \"" + ServiceDescription + "\"");
        }
        /// <summary>执行SC命令</summary>
        /// <param name="cmd"></param>
        static void RunSC(String cmd)
        {
            String path = Environment.SystemDirectory;
            path = Path.Combine(path, @"sc.exe");
            if (!File.Exists(path)) path = "sc.exe";
            if (!File.Exists(path)) return;
            RunCmd(path + " " + cmd, false, true);
        }
        /// <summary>执行一个命令</summary>
        /// <param name="cmd"></param>
        /// <param name="showWindow"></param>
        /// <param name="waitForExit"></param>
        static void RunCmd(String cmd, Boolean showWindow, Boolean waitForExit)
        {
            WriteLine("RunCmd " + cmd);

            Process p = new Process();
            ProcessStartInfo si = new ProcessStartInfo();
            String path = Environment.SystemDirectory;
            path = Path.Combine(path, @"cmd.exe");
            si.FileName = path;
            if (!cmd.StartsWith(@"/")) cmd = @"/c " + cmd;
            si.Arguments = cmd;
            si.UseShellExecute = false;
            si.CreateNoWindow = !showWindow;
            si.RedirectStandardOutput = true;
            si.RedirectStandardError = true;
            p.StartInfo = si;

            p.Start();
            if (waitForExit)
            {
                p.WaitForExit();

                String str = p.StandardOutput.ReadToEnd();
                if (!String.IsNullOrEmpty(str)) WriteLine(str.Trim(new Char[] { '\r', '\n', '\t' }).Trim());
                str = p.StandardError.ReadToEnd();
                if (!String.IsNullOrEmpty(str)) WriteLine(str.Trim(new Char[] { '\r', '\n', '\t' }).Trim());
            }
        }

        /// <summary>写日志</summary>
        /// <param name="msg"></param>
        protected static void WriteLine(String msg)
        {
            Console.WriteLine(msg);
        }
        /// <summary>是否已安装</summary>
        protected static Boolean? IsInstalled(string serviceName)
        {
            return IsServiceInstalled(serviceName);
        }

        /// <summary>是否已启动</summary>
        protected static Boolean? IsRunning(string serviceName)
        {
            return IsServiceRunning(serviceName);
        }

        #region 辅助
        /// <summary>取得服务</summary>
        /// <param name="name">名称</param>
        /// <returns></returns>
        protected static ServiceController GetService(String name)
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

        /// <summary>是否已安装</summary>
        protected static Boolean? IsServiceInstalled(String name)
        {
            ServiceController control = null;
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

        /// <summary>是否已启动</summary>
        protected static Boolean? IsServiceRunning(String name)
        {
            ServiceController control = null;
            try
            {
                control = GetService(name);
                if (control == null) return false;
                try
                {
                    //尝试访问一下才知道是否已安装
                    Boolean b = control.CanShutdown;
                }
                catch { return false; }

                control.Refresh();
                if (control.Status == ServiceControllerStatus.Running) return true;
                if (control.Status == ServiceControllerStatus.Stopped) return false;
                return null;
            }
            catch { return null; }
            finally { if (control != null)control.Dispose(); }
        }
        #endregion
        /// <summary>显示状态</summary>
        protected virtual void ShowStatus()
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;

            WriteLine("Service Info：");
            if (ServiceName != DisplayName)
                Console.WriteLine("服务：{0}({1})", DisplayName, ServiceName);
            else
                Console.WriteLine("服务：{0}", ServiceName);
            Console.WriteLine("描述：{0}", ServiceDescription);
            Console.Write("状态：");
            if (IsInstalled(ServiceName) == null)
                Console.WriteLine("未知");
            else if (IsInstalled(ServiceName) == false)
                Console.WriteLine("未安装");
            else
            {
                if (IsRunning(ServiceName) == null)
                    Console.WriteLine("未知");
                else
                {
                    if (IsRunning(ServiceName) == false)
                        Console.WriteLine("未启动");
                    else
                        Console.WriteLine("运行中");
                }
            }

            var asm = Assembly.GetExecutingAssembly();
            Console.WriteLine();
            Console.WriteLine("核心：{0}", asm.GetName().Version);
            Console.ForegroundColor = color;
        }
        /// <summary>Exe程序名</summary>
        protected static String ExeName
        {
            get
            {
                Process p = Process.GetCurrentProcess();
                String filename = p.MainModule.FileName;
                filename = Path.GetFileName(filename);
                filename = filename.Replace(".vshost.", ".");
                return filename;
            }
        }
    }
}
